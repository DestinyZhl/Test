using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Model
{
    public class FlowChartMgDataDTO : EntityDTOBase
    {
        public int FlowChart_MgData_UID { get; set; }
        public int FlowChart_Detail_UID { get; set; }
        public DateTime Product_Date { get; set; }
        public int Product_Plan { get; set; }
        public double Target_Yield { get; set; }

    }
}
