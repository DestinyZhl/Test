using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Model
{
    public class FlowChartDetailDTO : EntityDTOBase
    {
        public int FlowChart_Detail_UID { get; set; }
        public int FlowChart_Master_UID { get; set; }
        public int System_FunPlant_UID { get; set; }
        public int Process_Seq { get; set; }
        public string DRI { get; set; }
        public string Place { get; set; }
        public string Process { get; set; }
        public int Product_Stage { get; set; }
        public string Color { get; set; }
        public string Process_Desc { get; set; }
        public string Material_No { get; set; }
        public int FlowChart_Version { get; set; }
        public string FlowChart_Version_Comment { get; set; }
    }
}
