using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Common.WebApi;
using Newtonsoft.Json.Serialization;

namespace Common.WebApi
{
    public class StartUp 
    {
        public static void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\bin\\log4net.config"));

            //默认返回json格式
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            //序列化json忽略Serializable的影响
            var serializerSettings =
  GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
            var contractResolver =
              (DefaultContractResolver)serializerSettings.ContractResolver;
            contractResolver.IgnoreSerializableAttribute = true;
        }
    }
}
