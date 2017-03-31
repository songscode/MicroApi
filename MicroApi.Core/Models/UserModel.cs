using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace MicroApi.Core.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "必填")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "msmdmm")]
        public string Password { get; set; }
    }
}
