using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Model
{
    public class ExportReportDataResult : BaseModel
    {
        public int TotalItemCount { get; set; }
        public List<Daily_ProductReportItem> Items { get; set; }
    }
   
    public class ExportPPCheckDataResult : BaseModel
    {
        public int TotalItemCount { get; set; }
        public List<PPCheckDataItem> Items { get; set; }
    }
    public class ExportPPCheckData : BaseModel
    {
        public string Customer { get; set; }
        public string Project { get; set; }
        public string Product_Phase { get; set; }
        public string Part_Types { get; set; }
        public string Color { get; set; }
        public DateTime Reference_Date { get; set; }
        public string Time_InterVal { get; set; }
        public List<Tab_All_Text> TabList { get; set; }
    }
    public class Tab_All_Text : BaseModel
    {
        public string Time_InterVal { get; set; }
    }
    public class PPEditWIP : EntityDTOBase
    {
        public List<ProductUIDAndWIP> PPEditValue { get; set; }
    }
    public class ProductUIDAndWIP:BaseModel
    {
        public string Product_UID { get; set; }
        public int Wip_Qty { get; set;}
    }
    public class PPCheckDataSearch :BaseModel
    {
        public string Customer { get; set; }
        public string Project { get; set; }
        public string Product_Phase { get; set; }
        public string Part_Types { get; set; }
        public string Color { get; set; }
        public DateTime Reference_Date { get; set; }
        public string Interval_Time { get; set; }
        public string Tab_Select_Text { get; set; }
    }
    public class PPCheckDataItem : BaseModel
    {
        
        public int Product_Input_UID { get; set; }
        public int Product_Stage { get; set; }
        public int Is_Comfirm { get; set; }
        public string Color { get; set; }
        public int Process_Seq { get; set; }
        public string Place { get; set; }
        public string FunPlant { get; set; }
        public string Process { get; set; }
        public string FunPlant_Manager { get; set; }
        public double Target_Yield { get; set; }
        public int Product_Plan { get; set; }
        public int Product_Plan_Sum { get; set; }
        public int Picking_QTY { get; set; }
        public string Picking_MismatchFlag { get; set; }
        public int WH_Picking_QTY { get; set; }
        public int Good_QTY { get; set; }
        public string Good_MismatchFlag { get; set; }
        public int Adjust_QTY { get; set; }
        public int WH_QTY { get; set; }
        public int NG_QTY { get; set; }
        public double Rolling_Yield_Rate { get; set; }
        public double Finally_Field { get; set; }
        public int? WIP_QTY { get; set; }
    }
    public class PPCheckDataItemOrigOriginal : BaseModel
    {
        public int Product_Input_UID { get; set; }
        public int Product_Stage { get; set; }
        public int Is_Comfirm { get; set; }
        public string Color { get; set; }
        public string Time_Interval { get; set; }
        public int Process_Seq { get; set; }
        public string Place { get; set; }
        public string FunPlant { get; set; }
        public string Process { get; set; }
        public string FunPlant_Manager { get; set; }
        public double Target_Yield { get; set; }
        public int Product_Plan { get; set; }
        public int Product_Plan_Sum { get; set; }
        public int Picking_QTY { get; set; }
        public string Picking_MismatchFlag { get; set; }
        public int WH_Picking_QTY { get; set; }
        public int Good_QTY { get; set; }
        public string Good_MismatchFlag { get; set; }
        public int Adjust_QTY { get; set; }
        public int WH_QTY { get; set; }
        public int NG_QTY { get; set; }
        public double Rolling_Yield_Rate { get; set; }
        public double Finally_Field { get; set; }
        public int? WIP_QTY { get; set; }
    }
    /// <summary>
    /// 日报表
    /// </summary>
    public class Daily_ProductReportItem
    {
        public int Product_Input_UID { get; set; }
        public int Product_Stage { get; set; }
        public bool Is_Comfirm { get; set; }
        public int Process_Seq { get; set; }
        public string Place { get; set; }
        public string FunPlant { get; set; }
        public string Process { get; set; }
        public string Color { get; set; }
        public string DRI { get; set; }
        public decimal Target_Yield { get; set; }
        public int All_Product_Plan { get; set; }
        public int All_Product_Plan_Sum { get; set; }
        public int All_Picking_QTY { get; set; }
        public string All_Picking_MismatchFlag { get; set; }
        public int All_WH_Picking_QTY { get; set; }
        public int All_Good_QTY { get; set; }
        public string All_Good_MismatchFlag { get; set; }
        public int All_Adjust_QTY { get; set; }
        public int All_WH_QTY { get; set; }
        public int All_NG_QTY { get; set; }
        public decimal All_Rolling_Yield_Rate { get; set; }
        public decimal All_Finally_Field { get; set; }
        public int Product_Plan { get; set; }
        public int Picking_QTY { get; set; }
        public string Picking_MismatchFlag { get; set; }
        public int WH_Picking_QTY { get; set; }
        public int Good_QTY { get; set; }
        public string Good_MismatchFlag { get; set; }
        public int Adjust_QTY { get; set; }
        public int WH_QTY { get; set; }
        public int NG_QTY { get; set; }
        public decimal Rolling_Yield_Rate { get; set; }
        public decimal Finally_Field { get; set; }
        public int? WIP_QTY { get; set; }
    }

    public class Daily_ProductReport
    {
        public int Product_Stage { get; set; }
        public int Process_Seq { get; set; }
        public string Place { get; set; }
        public string FunPlant { get; set; }
        public string Process { get; set; }
        public string Color { get; set; }
        public string DRI { get; set; }
        public decimal Target_Yield { get; set; }
        public int All_Product_Plan { get; set; }
        public int All_Product_Plan_Sum { get; set; }
        public int All_Picking_QTY { get; set; }
        public string All_Picking_MismatchFlag { get; set; }
        public int All_WH_Picking_QTY { get; set; }
        public int All_Good_QTY { get; set; }
        public string All_Good_MismatchFlag { get; set; }
        public int All_Adjust_QTY { get; set; }
        public int All_WH_QTY { get; set; }
        public int All_NG_QTY { get; set; }
        public decimal All_Rolling_Yield_Rate { get; set; }
        public decimal All_Finally_Field { get; set; }
        public int Product_Plan { get; set; }
        public int Picking_QTY { get; set; }
        public string Picking_MismatchFlag { get; set; }
        public int WH_Picking_QTY { get; set; }
        public int Good_QTY { get; set; }
        public string Good_MismatchFlag { get; set; }
        public int Adjust_QTY { get; set; }
        public int WH_QTY { get; set; }
        public int NG_QTY { get; set; }
        public decimal Rolling_Yield_Rate { get; set; }
        public decimal Finally_Field { get; set; }
        public int? WIP_QTY { get; set; }
    }

    public class List_Daily_ProductReportItem
    {
        public List<Daily_ProductReportItem> Result { get; set; }
    }

    public class ReportDataSearch : BaseModel
    {
        public string Customer { get; set; }
        public string Project { get; set; }
        public string Product_Phase { get; set; }
        public string Part_Types { get; set; }
        public string Color { get; set; }
        public string Select_Type { get; set; }
        public string FunPlant { set; get; }
        //日报表
        public DateTime? Reference_Date { get; set; }
        public string Interval_Time { get; set; }
        public string Tab_Select_Text { get; set; }
        //周报表
        public DateTime ?Week_Date_Start { get; set; }
        public DateTime ?Week_Date_End { get; set; }
        public int ?Week_Version { get; set; }
        //月报表
        public DateTime ?Month_Date_Start { get; set; }
        public DateTime ?Month_Date_End { get; set; }
        public int ?Month_Version { get; set; }
        //时段报表
        public DateTime ?Interval_Date_Start { get; set; }
        public DateTime ?Interval_Date_End { get; set; }
        public int ?Verion_Interval { get; set; }

    }

    public class VersionBeginEndDate : BaseModel
    {
        public DateTime VersionBeginDate { get; set;}
        public DateTime VersionEndDate { get; set; }
        public string Interval { get; set; }
    }
}
