using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SPP.Model.ViewModels
{
    public class ZeroFunPlantInfo
    {

        public List<ZeroProcessDataSearch> ZeroList { get; set; }
    }

    public class ZeroProcessDataSearch : BaseModel
    {
        public int Create_User { get; set; }
        public DateTime Create_Time { get; set; }
        public string Func_Plant { get; set; }
        public string Customer { get; set; }
        public string Project { get; set; }
        public string Part_Types { get; set; }
        public string Product_Phase { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string QuertFlag { get; set; }
    }

    public class ProcessDataSearch : BaseModel
    {
        public string Func_Plant { get; set; }
        public string Customer { get; set; }
        public string Project { get; set; }
        public string Part_Types { get; set; }
        public string Product_Phase { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string QuertFlag { get; set; }
    }

    public class ProcessInfo : BaseModel
    {
        public int Process_Seq { get; set; }
        public string Process { get; set; }
        public string Color { get; set; }
    }
    public class ProductDataView : BaseModel
    {
        public int Process_Seq { get; set; }
        public string Process { get; set; }
        public string Color { get; set; }
        public int Good_QTY { get; set; }

        public int Picking_QTY { get; set; }
        public int WH_Picking_QTY { get; set; }

        public int NG_QTY { get; set; }
        public int WH_QTY { get; set; }
        public int Adjust_QTY { get; set; }
    }
    public class ProductDataList : BaseModel
    {
        public List<ProductDataItem> ProductLists { get; set; }
    }

    public class ProductDataItem : BaseModel
    {
        public int Product_UID { get; set; }
        public bool Is_Comfirm { get; set; }
        public System.DateTime Product_Date { get; set; }
        public string Time_Interval { get; set; }
        public string Customer { get; set; }
        public string Project { get; set; }
        public string Part_Types { get; set; }
        public string FunPlant { get; set; }
        public string FunPlant_Manager { get; set; }
        public string Product_Phase { get; set; }
        public int Process_Seq { get; set; }
        public string Place { get; set; }
        public string Process { get; set; }
        public int FlowChart_Master_UID { get; set; }
        public int FlowChart_Version { get; set; }
        public string Color { get; set; }
        public int Prouct_Plan { get; set; }
        public int Product_Stage { get; set; }
        public double Target_Yield { get; set; }
        public int Good_QTY { get; set; }
        public string Good_MismatchFlag { get; set; }
        public string DRI { get; set; }
        public int ? FlowChart_Detail_UID { get; set; }
        public int Picking_QTY { get; set; }
        public int WH_Picking_QTY { get; set; }
        public string Picking_MismatchFlag { get; set; }
        public int NG_QTY { get; set; }
        public int WH_QTY { get; set; }
        public int WIP_QTY { get; set; }
        public int Adjust_QTY { get; set; }
        public int Creator_UID { get; set; }
        public System.DateTime Create_Date { get; set; }
        public string Material_No { get; set; }
        public int Modified_UID { get; set; }
        public System.DateTime Modified_Date { get; set; }
    }

    public class YieldChart : BaseModel
    {
        public int Prouct_Plan { get; set; }
        public string Process { get; set; }
        public int Good_QTY { get; set; }
        public int WH_QTY { get; set; }
    }

    public class YieldChartSearch : BaseModel
    {
        public string Customer { get; set; }
        public string Project { get; set; }
        public string Part_Types { get; set; }
        public string Product_Phase { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
    }
}
