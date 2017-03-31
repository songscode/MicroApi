using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Common.WebApi.Filters
{
    /// <summary>
    /// 不授权验证AuthorizationAttribute
    /// </summary>
    public class NoAuthorizationAttribute : ActionFilterAttribute
    {

    }
}
