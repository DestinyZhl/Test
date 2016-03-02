using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Model
{
    public class SystemFunctionDTO:EntityDTOBase
    {
        public int Function_UID { get; set; }
        public int Parent_Function_UID { get; set; }
        public string Function_ID { get; set; }
        public string Function_Name { get; set; }
        public string Function_Desc { get; set; }
        public int Order_Index { get; set; }
        public string Icon_ClassName { get; set; }
        public string URL { get; set; }
        public string Mobile_URL { get; set; }
        public bool Is_Show { get; set; }
       
    }
}
