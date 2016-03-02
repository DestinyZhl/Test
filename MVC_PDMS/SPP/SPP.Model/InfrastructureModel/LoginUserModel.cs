using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Model
{
    public class LoginUserMoel: BaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
