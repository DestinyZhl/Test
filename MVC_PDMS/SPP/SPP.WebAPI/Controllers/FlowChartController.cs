using SPP.Core;
using SPP.Core.Authentication;
using SPP.Model;
using SPP.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using Newtonsoft.Json;
using SPP.Model.ViewModels;

namespace SPP.WebAPI.Controllers
{
    public class FlowChartController : ApiControllerBase
    {
        IFlowChartService flowChartService;

        public FlowChartController(IFlowChartService flowChartService)
        {
            this.flowChartService = flowChartService;
        }

        [AcceptVerbs("GET")]
        [IgnoreDBAuthorize]
        public IHttpActionResult QueryFlowChartDataAPI(int user_account_uid)
        {
            var result = flowChartService.QueryFlowChartMasterDatas(user_account_uid);
            return Ok(result);
        }

        [AcceptVerbs("GET")]
        public IHttpActionResult QueryFlowChartDataByMasterUid(int flowChartMaster_Uid)
        {
            var result = flowChartService.QueryFlowChartDataByMasterUid(flowChartMaster_Uid);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult QueryFlowChartsAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<FlowChartModelSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);

            var flCharts = flowChartService.QueryFlowCharts(searchModel, page);
            return Ok(flCharts);
        }

        [HttpPost]
        [IgnoreDBAuthorize]
        public string CheckFlowChartAPI(FlowChartExcelImportParas parasItem)
        {
            var result = flowChartService.CheckFlowChart(parasItem);
            return result;
        }

        [AcceptVerbs("GET")]
        [IgnoreDBAuthorize]
        public IHttpActionResult QueryAllFunctionPlantsAPI()
        {
            var result = flowChartService.QueryAllFunctionPlants();
            return Ok(result);
        }

        public void ImportFlowChartAPI(FlowChartImport importItem, bool isEdit)
        {
            if (isEdit)
            {
                flowChartService.ImportFlowUpdateChart(importItem);
            }
            else
            {
                flowChartService.ImportFlowChart(importItem);
            }
        }

        [HttpPost]
        [IgnoreDBAuthorize]
        public void ImportFlowChartMGDataAPI(dynamic json, int FlowChart_Master_UID)
        {
            var jsonData = json.ToString();
            var list = JsonConvert.DeserializeObject<IEnumerable<FlowChartMgDataDTO>>(jsonData);
            flowChartService.ImportFlowChartMGData(list, FlowChart_Master_UID);
        }

        [AcceptVerbs("GET")]
        [IgnoreDBAuthorize]
        public IHttpActionResult QueryFlowChartAPI(int FlowChart_Master_UID)
        {
            var result = flowChartService.QueryFlowChart(FlowChart_Master_UID);
            return Ok(result);
        }

        [AcceptVerbs("GET")]
        public IHttpActionResult QueryHistoryFlowChartAPI(int FlowChart_Master_UID)
        {
            var result = flowChartService.QueryHistoryFlowChart(FlowChart_Master_UID);
            return Ok(result);
        }

        [AcceptVerbs("GET")]
        public IHttpActionResult DoHistoryExcelExportAPI(int id, bool IsTemp)
        {
            var result = flowChartService.ExportFlowChart(id, IsTemp);
            return Ok(result);
        }

        [AcceptVerbs("GET")]
        public bool ClosedFLAPI(int FlowChart_Master_UID, bool isClosed)
        {
            var result = flowChartService.ClosedFL(FlowChart_Master_UID, isClosed);
            return result;
        }

        [HttpPost]
        public IHttpActionResult QueryFLDetailListAPI(int id, bool IsTemp, int Version)
        {
            if (IsTemp)
            {
                var result = flowChartService.QueryFLDetailTempList(id,Version);
                return Ok(result);
            }
            else
            {
                var result = flowChartService.QueryFLDetailList(id,Version);
                return Ok(result);

            }
        }

        [AcceptVerbs("GET")]
        public IHttpActionResult QueryFLDetailByIDAPI(int id)
        {
            var result = flowChartService.QueryFLDetailByID(id);
            return Ok(result);
        }

        [AcceptVerbs("GET")]
        public IHttpActionResult QueryFLDetailListAPI(int id)
        {
            var result = flowChartService.QueryFLDetailList(id);
            return Ok(result);
        }

        [AcceptVerbs("GET")]
        [IgnoreDBAuthorize]
        public IHttpActionResult QueryFunPlantAPI(int id)
        {
            var result = flowChartService.QueryFunPlant(id);
            return Ok(result);
        }

        public void SaveFLDetailInfoAPI(FlowChartDetailDTO dto, int AccountID)
        {
            flowChartService.SaveFLDetailInfo(dto, AccountID);
        }

        public void SaveAllDetailInfoAPI(dynamic json, int AccountID)
        {
            var jsonData = json.ToString();
            var list = JsonConvert.DeserializeObject<List<FlowChartDetailAndMGDataInputDTO>>(jsonData);
            flowChartService.SaveAllDetailInfo(list, AccountID);
        }

        [AcceptVerbs("GET")]
        public void DeleteFLTempAPI(int FlowChart_Master_UID)
        {
            flowChartService.DeleteFLTemp(FlowChart_Master_UID);
        }

        [IgnoreDBAuthorize]
        [AcceptVerbs("GET")]
        public IHttpActionResult QueryProcessMGDataAPI(int masterUID ,string date)
        {
            DateTime nowDate = DateTime.Parse(date);
            var result = flowChartService.QueryProcessMGData(masterUID, nowDate);
            return Ok(result);
        }

        [IgnoreDBAuthorize]
        public string ModifyProcessMGDataAPI(FlowChartPlanManagerDTO dto)
        {
           
            var ent = flowChartService.QueryProcessMGDataSingle(dto.Detail_UID, dto.date);
            ent.MondayProduct_Plan = dto.MonDayProduct_Plan;
            ent.MondayTarget_Yield = dto.MonDayTarget_Yield;
            ent.TuesdayProduct_Plan = dto.TuesDayProduct_Plan;
            ent.TuesdayTarget_Yield = dto.TuesDayTarget_Yield;
            ent.WednesdayProduct_Plan = dto.WednesdayProduct_Plan;
            ent.WednesdayTarget_Yield = dto.WednesdayTarget_Yield;
            ent.ThursdayProduct_Plan = dto.ThursdayProduct_Plan;
            ent.ThursdayTarget_Yield = dto.ThursdayTarget_Yield;
            ent.FridayProduct_Plan = dto.FriDayProduct_Plan;
            ent.FridayTarget_Yield = dto.FridayTarget_Yield;
            ent.SaterdayProduct_Plan = dto.SaterDayProduct_Plan;
            ent.SaterdayTarget_Yield = dto.SaterDayTarget_Yield;
            ent.SundayProduct_Plan = dto.SunDayProduct_Plan;
            ent.SundayTarget_Yield = dto.SunDayTarget_Yield;
            string  result= flowChartService.FlowChartPlan(ent);
             return result;
        }

        [IgnoreDBAuthorize]
        [AcceptVerbs("GET")]
        public IHttpActionResult QueryProcessMGDataSingleAPI(int detailUID, string date)
        {
            DateTime nowDate = DateTime.Parse(date);
            var result = flowChartService.QueryProcessMGDataSingle(detailUID, nowDate);
            return Ok(result);
        }

        [AcceptVerbs("GET")]
        public IHttpActionResult QueryProjectTypes()
        {
            var result = flowChartService.QueryProjectTypes();
            return Ok(result);
        }

        [AcceptVerbs("GET")]
        public IHttpActionResult QueryProcessByFlowchartMasterUid(int flowcharMasterUid)
        {
            var result = flowChartService.QueryProcess(flowcharMasterUid);
            return Ok(result);
        }
    }
}