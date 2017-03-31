using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Newtonsoft.Json;

namespace Common.WebApi.Filters
{
    public class ParameterValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            if (!actionContext.ModelState.IsValid)
            {
                string msg = "";
                foreach (var item in actionContext.ModelState)
                {
                    var err = string.Join(",", item.Value.Errors.Select(e => e.ErrorMessage));
                    if (!string.IsNullOrEmpty(err))
                        msg += string.Format("参数{0}：{1}；", item.Key, err);
                }
                actionContext.Response = HttpResponseMessageExtension.Xml(new ResponseResult { Code= "error", Msg = msg });
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            if (actionExecutedContext.Exception != null)
            {
                actionExecutedContext.Response = HttpResponseMessageExtension.Xml(new ResponseResult { Code = "error", Msg = actionExecutedContext.Exception.Message });
                actionExecutedContext.Response.StatusCode = HttpStatusCode.InternalServerError;
            }
        }
    }
}
