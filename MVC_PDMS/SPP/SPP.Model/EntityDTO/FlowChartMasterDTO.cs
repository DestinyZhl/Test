using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Model
{
    public class FlowChartMasterDTO : EntityDTOBase
    {
        public int FlowChart_Master_UID { get; set; }
        public int Project_UID { get; set; }
        public string Part_Types { get; set; }
        public int FlowChart_Version { get; set; }
        public string FlowChart_Version_Comment { get; set; }
        public bool Is_Latest { get; set; }
        public bool Is_Closed { get; set; }
    }

    public class FlowChartPlanManagerDTO : EntityDTOBase
    {
        public int Detail_UID { set; get; }
        public int Process_seq { set; get; }
        public string Process { set; get; }
        public DateTime date { get; set; }

        public string Color { get; set; }
        public int MonDayProduct_Plan { set; get; }
        public double MonDayTarget_Yield { set; get; }

        public int TuesDayProduct_Plan { set; get; }
        public double TuesDayTarget_Yield { set; get; }

        public int WednesdayProduct_Plan { set; get; }
        public double WednesdayTarget_Yield { set; get; }

        public int ThursdayProduct_Plan { set; get; }
        public double ThursdayTarget_Yield { set; get; }

        public int FriDayProduct_Plan { set; get; }
        public double FridayTarget_Yield { set; get; }

        public int SaterDayProduct_Plan { set; get; }
        public double SaterDayTarget_Yield { set; get; }

        public int SunDayProduct_Plan { set; get; }
        public double SunDayTarget_Yield { set; get; }
    }
}
