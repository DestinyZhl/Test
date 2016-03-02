using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Model.ViewModels
{
    public class TimeSpanReport
    {
        public string Process { set; get; }
        public int SumPlan { set; get; }
        public int SumGoodQty { set; get; }
        public decimal SumYieldRate { set; get; }
    }
    public class ExportWeeklyReportDataResult : BaseModel
    {
        public int TotalItemCount { get; set; }
        public List<WeekReportVM> Items { get; set; }
    }
    public class ExportIntervalReportDataResult : BaseModel
    {
        public int TotalItemCount { get; set; }
        public List<TimeSpanReportVM> Items { get; set; }
    }
    public class WeekReport
    {
        public string Process { set; get; }
        public int SumPlan { set; get; }
        public int SumGoodQty { set; get; }
        public decimal SumYieldRate { set; get; }

        public int MondayPlan { set; get; }
        public int MondayGoodQty { set; get; }
        public decimal MondayYieldRate { set; get; }

        public int TuesdayPlan { set; get; }
        public int TuesdayGoodQty { set; get; }
        public decimal TuesdayYieldRate { set; get; }

        public int WednesdayPlan { set; get; }
        public int WednesdayGoodQty { set; get; }
        public decimal WednesdayYieldRate { set; get; }

        public int ThursdayPlan { set; get; }
        public int ThursdayGoodQty { set; get; }
        public decimal ThursdayYieldRate { set; get; }

        public int FridayPlan { set; get; }
        public int FridayGoodQty { set; get; }
        public decimal FridayYieldRate { set; get; }

        public int SaterdayPlan { set; get; }
        public int SaterdayGoodQty { set; get; }
        public decimal SaterdayYieldRate { set; get; }

        public int SundayPlan { set; get; }
        public int SundayGoodQty { set; get; }
        public decimal SundayYieldRate { set; get; }
    }


    public class TimeSpanReportVM
    {
        public string Process { set; get; }
        public int SumPlan { set; get; }
        public int SumGoodQty { set; get; }
        public string SumYieldRate { set; get; }
    }

    public class WeekReportVM
    {
        public string Process { set; get; }
        public int SumPlan { set; get; }
        public int SumGoodQty { set; get; }
        public string SumYieldRate { set; get; }

        public int MondayPlan { set; get; }
        public int MondayGoodQty { set; get; }
        public string MondayYieldRate { set; get; }

        public int TuesdayPlan { set; get; }
        public int TuesdayGoodQty { set; get; }
        public string TuesdayYieldRate { set; get; }

        public int WednesdayPlan { set; get; }
        public int WednesdayGoodQty { set; get; }
        public string WednesdayYieldRate { set; get; }

        public int ThursdayPlan { set; get; }
        public int ThursdayGoodQty { set; get; }
        public string ThursdayYieldRate { set; get; }

        public int FridayPlan { set; get; }
        public int FridayGoodQty { set; get; }
        public string FridayYieldRate { set; get; }

        public int SaterdayPlan { set; get; }
        public int SaterdayGoodQty { set; get; }
        public string SaterdayYieldRate { set; get; }

        public int SundayPlan { set; get; }
        public int SundayGoodQty { set; get; }
        public string SundayYieldRate { set; get; }
    }



}
