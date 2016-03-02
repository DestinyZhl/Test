using Newtonsoft.Json;
using SPP.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Model
{
    public class ProductDataDTO : EntityDTOBase
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
        public int Picking_QTY { get; set; }
        public int WH_Picking_QTY { get; set; }
        public string Picking_MismatchFlag { get; set; }
        public int NG_QTY { get; set; }
        public int WH_QTY { get; set; }
        public int WIP_QTY { get; set; }
        public string DRI { get; set; }
        public int Adjust_QTY { get; set; }
        public int Creator_UID { get; set; }
        public System.DateTime Create_Date { get; set; }
        public string Material_No { get; set; }
        public int? FlowChart_Detail_UID { get; set; }

    }
  

    public class ProductDataViewDTO
    {
        public int Product_UID { get; set; }
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
  
    public class ProductDataVM
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
        public string Good_Contact { get; set; }
        public int Picking_QTY { get; set; }
        public int WH_Picking_QTY { get; set; }
        public string Picking_MismatchFlag { get; set; }
        public string Picking_Contact { get; set; }
        public int NG_QTY { get; set; }
        public int WH_QTY { get; set; }
        public int? WIP_QTY { get; set; }
        public int Adjust_QTY { get; set; }
        public int Creator_UID { get; set; }
        public System.DateTime Create_Date { get; set; }
        public string Material_No { get; set; }
        [JsonConverter(typeof(SPPDateTimeConverter))]
        public DateTime Modified_Date { get; set; }
        public int Modified_UID { get; set; }
        public string Modified_UserNTID { get; set; }
      
          
    }
}
