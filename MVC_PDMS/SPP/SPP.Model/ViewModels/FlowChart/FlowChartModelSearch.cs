using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SPP.Common.Helpers;

namespace SPP.Model.ViewModels
{
    #region 往DB发送搜索请求
    public class FlowChartModelSearch : BaseModel
    {
        public string BU_D_Name { get; set; }

        public string Project_Name { get; set; }

        public string Part_Types { get; set; }

        public string Product_Phase { get; set; }

        public int Is_Closed { get; set; }

        public int Is_Latest { get; set; }

        public DateTime? Modified_Date_From { get; set; }

        public DateTime? Modified_Date_End { get; set; }

        public string Modified_By { get; set; }
    }
    #endregion

    #region 从DB中检索出数据加载
    public class FlowChartGet : BaseModel
    {
        public FlowChartMasterDTO FlowChartMasterDTO { get; set; }

        public SystemProjectDTO SystemProjectDTO { get; set; }

        public SystemUserDTO SystemUserDTO { get; set; }

        public string BU_D_Name { get; set; }
    }

    public class FlowChartModelGet : BaseModel
    {
        public int FlowChart_Master_UID { get; set; }

        public string Project_Name { get; set; }

        public string BU_D_Name { get; set; }

        public string Part_Types { get; set; }

        public string Product_Phase { get; set; }

        public bool Is_Closed { get; set; }

        public bool Is_Latest { get; set; }

        public int FlowChart_Version { get; set; }

        public string FlowChart_Version_Comment { get; set; }

        public string User_Name { get; set; }

        [JsonConverter(typeof(SPPDateTimeConverter))]
        public DateTime Modified_Date { get; set; }

        public string User_NTID { get; set; }

        public bool IsTemp { get; set; }
    }
    #endregion

    #region
    public class FlowChartImport:BaseModel
    {
        public FlowChartMasterDTO FlowChartMasterDTO { get; set; }

        public List<FlowChartImportDetailDTO> FlowChartImportDetailDTOList { get; set; }

    }

    public class FlowChartImportDetailDTO
    {
        public FlowChartDetailDTO FlowChartDetailDTO { get; set; }

        public FlowChartMgDataDTO FlowChartMgDataDTO { get; set; }

    }
    #endregion


    #region 从DB中获取
    public class FlowChartDetailGet
    {
        public FlowChartDetailDTO FlowChartDetailDTO { get; set; }

        public FlowChartDetailTempDTO FlowChartDetailTempDTO { get; set; }

        public FlowChartMgDataDTO FlowChartMgDataDTO { get; set; }

        public FlowChartMgDataTempDTO FlowChartMgDataTempDTO { get; set; }

        public SystemUserDTO SystemUserDTO { get; set; }

        public string FunPlant { get; set; }
    }

    public class FlowChartDetailGetByMasterInfo
    {
        public string BU_D_Name { get; set; }

        public string Project_Name { get; set; }

        public string Part_Types { get; set; }

        public string Product_Phase { get; set; }

        public List<SystemFunctionPlantDTO> SystemFunctionPlantDTOList { get; set; }
    }

    #endregion

    #region 查看历史版本从DB中获取数据
    public class FlowChartHistoryGet
    {
        public int FlowChart_Master_UID { get; set; }

        public string BU_D_Name { get; set; }

        public string Project_Name { get; set; }

        public string Product_Phase { get; set; }

        public string Part_Types { get; set; }

        public int FlowChart_Version { get; set; }

        public string FlowChart_Version_Comment { get; set; }

        public string User_Name { get; set; }

        [JsonConverter(typeof(SPPDateTimeConverter))]
        public DateTime Modified_Date { get; set; }
    }
    #endregion

    #region 传参
    public class FlowChartExcelImportParas : BaseModel
    {
        public string BU_D_Name { get; set; }
        public string Project_Name { get; set; }
        public string Part_Types { get; set; }
        public string Product_Phase { get; set; }
        public int FlowChart_Master_UID { get; set; }
        public bool isEdit { get; set; }

    }
    #endregion

    #region 从DB中获取数据Export
    public class FlowChartExcelExport : BaseModel
    {
        public string BU_D_Name { get; set; }
        public string Project_Name { get; set; }
        public string Part_Types { get; set; }
        public string Product_Phase { get; set; }

        public SystemUserDTO SystemUserDTO { get; set; }

        public List<FlowChartDetailAndMGDataDTO> FlowChartDetailAndMGDataDTOList { get; set; }
    }

    public class FlowChartDetailAndMGDataDTO
    {
        public string DRI { get; set; }

        public string Place { get; set; }

        public string Process { get; set; }

        public string PlantName { get; set; }

        public string Product_Stage { get; set; }

        public string Color { get; set; }

        public string Process_Desc { get; set; }

        public double Target_Yield { get; set; }

        public int Product_Plan { get; set; }
    }
    #endregion

    #region 从DBDetail中批量获取页面的数据
    public class FlowChartDetailAndMGDataInputDTO : BaseModel
    {
        public int FlowChart_Detail_UID { get; set; }

        public int Product_Plan { get; set; }

        public string Target_Yield { get; set; }
    }
    #endregion

    #region 获取下一周的日期
    public class Week
    {
        public DateTime Monday { get; set; }

        public DateTime Tuesday { get; set; }

        public DateTime Wednesday { get; set; }

        public DateTime Thursday { get; set; }

        public DateTime Friday { get; set; }

        public DateTime Saturday { get; set; }

        public DateTime Sunday { get; set; }
    }
    #endregion
}
