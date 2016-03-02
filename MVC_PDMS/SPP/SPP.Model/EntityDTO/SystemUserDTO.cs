using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Model
{
    public class SystemUserDTO:EntityDTOBase
    {
        public int Account_UID { get; set; }
        public string User_NTID { get; set; }
        public string User_Name { get; set; }
        public bool Enable_Flag { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string LoginToken { get; set; }
    }
}
