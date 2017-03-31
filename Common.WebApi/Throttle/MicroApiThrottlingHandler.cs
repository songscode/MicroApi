using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApiThrottle;

namespace Common.WebApi.Throttle
{
    public class MicroApiThrottlingHandler : ThrottlingHandler
    {
        protected override RequestIdentity SetIndentity(HttpRequestMessage request)
        {
            //授权参数token参数获取
            string token = "";
            var paramters = request.GetQueryNameValuePairs();
            if (paramters != null)
            {
                //token验证
                var keyValuePairs = paramters as KeyValuePair<string, string>[] ?? paramters.ToArray();
                token = keyValuePairs.FirstOrDefault(e => e.Key == "token").Value;
            }

            return new RequestIdentity()
            {
                //ClientKey = request.Headers.Contains("Authorization-Key") ? request.Headers.GetValues("Authorization-Key").First() : "anon",
                ClientKey = !string.IsNullOrEmpty(token) ? token : "default",
                ClientIp = base.GetClientIp(request).ToString(),
                Endpoint = request.RequestUri.AbsolutePath.ToLowerInvariant()
            };
        }
    }
}
