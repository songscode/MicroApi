using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.WebApi
{
    [Serializable]
    public  class ResponseResult
    {
        public string Code { get; set; }
        public string Msg { get; set; }
    }
}
