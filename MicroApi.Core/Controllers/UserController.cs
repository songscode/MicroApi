using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using MicroApi.Core.Models;
using Common.WebApi;
using Common.WebApi.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApiThrottle;

namespace MicroApi.Core.Controllers
{
    [EnableThrottling(PerSecond = 2)]//限流
    [RoutePrefix("api/users")]//api路由
    [Authorization]//权限
    public class UserController:ApiController
    {
        [Route("model")]
        [NoAuthorization]
        public SuccessResult<UserModel> Get([ModelBinder]UserModel model)
        {
            throw new ArgumentNullException("异常测试");
            //return model;
            //return JsonConvert.SerializeObject(new SuccessResult {UserInfo = model, Code = "Success", Msg = "成功"});
            return new SuccessResult<UserModel> { Detail = model };
        }
        [Route("detail")]
        [HttpGet]
        public string Detail()
        {
            return "detail_001";
        }
        [Route("{id:nonzero}")]
        public string Get(int id)
        {
            return string.Format("get_id_{0}",id);
        }
        [Route("name/{name}")]
        public string GetName(string name)
        {
            return string.Format("get_name_{0}", name);
        }

        [Route("imp")]
        public string GetImp()
        {
            throw new NotImplementedException("This method is not implemented");
        }
    }

    public class SuccessResult<T>: ResponseResult
    {
        public SuccessResult()
        {
            Code = "Success";
            Msg = "成功1";
        }
        public T Detail { get; set; }
    }
}
