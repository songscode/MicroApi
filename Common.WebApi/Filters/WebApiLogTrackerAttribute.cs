using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Common.WebApi.Log;

namespace Common.WebApi.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class WebApiLogTrackerAttribute : ActionFilterAttribute
    {
        private readonly string Key = "_thisWebApiOnActionMonitorLog_";

        /// <summary>
        /// 执行前
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            MonitorLog monLog = new MonitorLog();
            monLog.ExecuteStartTime = DateTime.Now;
            monLog.HttpRequestHeaders = actionContext.Request.Headers.ToString();
            monLog.HttpMethod = actionContext.Request.Method.Method;

            actionContext.Request.Properties[Key] = monLog;

            var paramters = actionContext.Request.GetQueryNameValuePairs().ToDictionary(e=>e.Key,e=>e.Value);
            monLog.ActionParams = paramters;
        }
        /// <summary>
        /// 执行后
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            MonitorLog monLog = actionExecutedContext.Request.Properties[Key] as MonitorLog;
            var controllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            if (monLog == null)
            {
                LoggerHelper.SystemLog.Warn(string.Format("controller:{0},action:{1},没有添加日志跟踪", controllerName, actionName));
                return;
            }
            monLog.ExecuteEndTime = DateTime.Now;
            monLog.ActionName = actionName;
            monLog.ControllerName = controllerName;
            LoggerHelper<MonitorLog>.Info(monLog);
            if (actionExecutedContext.Exception != null)
            {
                LoggerHelper<MonitorLog>.Error(monLog, actionExecutedContext.Exception);
            }
        }
    }
}
