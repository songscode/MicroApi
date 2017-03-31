using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Http.Tracing;
using Common.WebApi.Constraints;
using Common.WebApi.Filters;
using Common.WebApi.Throttle;
using WebApiThrottle;

namespace Common.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            //全局约束
            var constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("nonzero", typeof(NonZeroConstraint));
            config.MapHttpAttributeRoutes(constraintResolver);

            //全局默认路由
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //全局过滤器
            config.Filters.Add(new NotImplExceptionFilterAttribute());//不执行的异常
            config.Filters.Add(new ParameterValidationAttribute());//参数验证
            config.Filters.Add(new WebApiLogTrackerAttribute());//跟踪器

            //可自定义定义接口format返回参数格式
            //https://code.msdn.microsoft.com/Support-format-in-ASPNET-e3785b2a
            //config.Formatters.XmlFormatter.AddQueryStringMapping("format", "xml", "application/xml");
            //config.Formatters.JsonFormatter.AddQueryStringMapping("format", "json", "application/json");

            
            //trace provider
            var traceWriter = new SystemDiagnosticsTraceWriter()
            {
                IsVerbose = true
            };
            config.Services.Replace(typeof(ITraceWriter), traceWriter);
            config.EnableSystemDiagnosticsTracing();

            //节流阀
            config.MessageHandlers.Add(new ThrottlingHandler(
                policy: new ThrottlePolicy(perMinute:1, perHour: 30, perDay: 35, perWeek: 3000)
                {
                    //scope to IPs
                    IpThrottling = true,
                    IpRules = new Dictionary<string, RateLimits>
                    { 
                        { "::1/10", new RateLimits { PerSecond = 2 } },
                        { "192.168.2.1", new RateLimits { PerMinute = 30, PerHour = 30*60, PerDay = 30*60*24 } }
                    },
                    //white list the "::1" IP to disable throttling on localhost for Win8
                    IpWhitelist = new List<string> { "127.0.0.1", "192.168.0.0/24" },

                    //scope to clients (if IP throttling is applied then the scope becomes a combination of IP and client key)
                    ClientThrottling = true,
                    ClientRules = new Dictionary<string, RateLimits>
                    { 
                        { "api-client-key-1", new RateLimits { PerMinute = 60, PerHour = 600 } },
                        { "api-client-key-9", new RateLimits { PerDay = 5000 } }
                    },
                    //white list API keys that don’t require throttling
                    ClientWhitelist = new List<string> { "admin-key" },

                    //scope to endpoints
                    EndpointThrottling = true,
                    EndpointRules = new Dictionary<string, RateLimits>
                    { 
                        { "api/search", new RateLimits { PerSecond = 10, PerMinute = 100, PerHour = 1000 } }
                    }
                },
                policyRepository: new PolicyCacheRepository(),//规则缓存数据
                repository: new CacheRepository(),//缓存数据
                logger: new TracingThrottleLogger(traceWriter)));//日志记录
        }

    }
}
