using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Common.WebApi.Filters
{
    /// <summary>
    /// 授权过滤器
    /// </summary>
    public class AuthorizationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 控制器执行前验证
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            //是否授权验证
            bool disabled = actionContext.ActionDescriptor.GetFilters().Contains(new NoAuthorizationAttribute());
            if (disabled)
            {
                return;
            }
            //参数获取
            var paramters = actionContext.Request.GetQueryNameValuePairs();
            if (paramters == null)
            {
                return;
            }
            //token验证
            var keyValuePairs = paramters as KeyValuePair<string, string>[] ?? paramters.ToArray();
            var token = keyValuePairs.FirstOrDefault(e => e.Key == "token").Value;
            //todo 授权验证
            if (token=="1")
            {
                var responseMessage = new HttpResponseMessage();
                responseMessage.Content = new StringContent("授权成功");
                actionContext.Response = responseMessage;
            }
            else if (token == "2")
            {
                var responseMessage = new HttpResponseMessage();
                responseMessage.Content=new StringContent("授权失败");
                actionContext.Response=responseMessage;
            }
        }
    }
   
}
