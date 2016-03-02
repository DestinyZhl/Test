using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Model
{
    public class SystemProjectDTO : EntityDTOBase
    {
        public int Project_UID { get; set; }
        public string Project_Code { get; set; }
        public int BU_D_UID { get; set; }
        public string Project_Name { get; set; }
        public string Product_Phase { get; set; }
        public Nullable<System.DateTime> Start_Date { get; set; }
        public Nullable<System.DateTime> Closed_Date { get; set; }
    }
}
