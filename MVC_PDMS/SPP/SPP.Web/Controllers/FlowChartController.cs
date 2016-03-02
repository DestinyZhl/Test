using Newtonsoft.Json;
using OfficeOpenXml;
using SPP.Common.Constants;
using SPP.Common.Helpers;
using SPP.Core;
using SPP.Core.BaseController;
using SPP.Model;
using SPP.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web.Mvc;
using SPP.Model.ViewModels.Settings;
using System.Web.Helpers;
using System.Web;
using System.Linq;
using System.Text;
using OfficeOpenXml.Style;
using System.Drawing;

namespace SPP.Web.Controllers
{
    public class FlowChartController : WebControllerBase
    {

        public ActionResult FlowChartList(string errorInfo)
        {
            if (!string.IsNullOrEmpty(errorInfo))
            {
                ViewBag.errorInfo = errorInfo;
            }
            //绑定状态下拉框
            Dictionary<int, string> statusDir = new Dictionary<int, string>();
            statusDir.Add(StructConstants.IsClosedStatus.AllKey, StructConstants.IsClosedStatus.AllValue);
            statusDir.Add(StructConstants.IsClosedStatus.ClosedKey, StructConstants.IsClosedStatus.ClosedValue);
            statusDir.Add(StructConstants.IsClosedStatus.ProcessKey, StructConstants.IsClosedStatus.ProcessValue);
            statusDir.Add(StructConstants.IsClosedStatus.ApproveKey, StructConstants.IsClosedStatus.ApproveValue);
            ViewBag.Status = statusDir;

            //绑定最新版下拉框
            Dictionary<int, string> lastestDir = new Dictionary<int, string>();
            lastestDir.Add(StructConstants.IsLastestStatus.AllKey, StructConstants.IsLastestStatus.AllValue);
            lastestDir.Add(StructConstants.IsLastestStatus.LastestKey, StructConstants.IsLastestStatus.LastestValue);
            ViewBag.Lastest = lastestDir;

            return View();
        }

        #region FlowChart List -----add by Destiny Zhang 2015-12-16 

        public ActionResult ProjectList()
        {
            ViewBag.PageTitle = null;
            return View();
        }

        public ActionResult QueryFlowChartsListData()
        {
            string user_account_uid = this.CurrentUser.AccountUId.ToString();
            string apiUrl = string.Format("FlowChart/QueryFlowChartDataAPI?user_account_uid={0}", user_account_uid);
            HttpResponseMessage responseMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responseMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        [HttpGet]
        public ActionResult PrepareForEditeData(int flowChartMaster_Uid)
        {
            string apiUrl = string.Format("FlowChart/QueryFlowChartDataByMasterUid?flowChartMaster_uid={0}", flowChartMaster_Uid);
            HttpResponseMessage responseMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responseMessage.Content.ReadAsStringAsync().Result;
            var entity = JsonConvert.DeserializeObject<ProcessDataSearch>(result);

            apiUrl = string.Format("EventReportManager/GetIntervalInfoAPI?opType={0}", "OP1");
            responseMessage = APIHelper.APIGetAsync(apiUrl);
            result = responseMessage.Content.ReadAsStringAsync().Result;
            var time = JsonConvert.DeserializeObject<IntervalEnum>(result);

            int uid = this.CurrentUser.AccountUId;
            apiUrl = string.Format("ProductInput/getCurrentPlantNameAPI?uid={0}", uid);
            responseMessage = APIHelper.APIGetAsync(apiUrl);
            result = responseMessage.Content.ReadAsStringAsync().Result;
            var funcPlant = JsonConvert.DeserializeObject<string>(result);

            entity.Date = DateTime.Parse(time.NowDate);
            entity.Time = time.Time_Interval;
            entity.QuertFlag = "ProjectList";
            entity.Func_Plant = funcPlant;
            TempData["ProcessDataSearch"] = entity;

            return RedirectToAction("QuestProductDatas", "ProductInput");
        }

        #endregion

        #region FlowChart Plan -----add by Destiny Zhang 2016-1-14 

        public ActionResult FlowChartPlanManager(int MasterUID)
        {
            ViewBag.MasterUID = MasterUID;

            var ddlapiUrl = string.Format("FlowChart/QueryFunPlantAPI?id={0}", MasterUID);
            HttpResponseMessage responddlMessage = APIHelper.APIGetAsync(ddlapiUrl);
            var ddlResult = responddlMessage.Content.ReadAsStringAsync().Result;
            var item = JsonConvert.DeserializeObject<FlowChartDetailGetByMasterInfo>(ddlResult);
            ViewBag.CustomerName = item.BU_D_Name;
            ViewBag.ProjectName = item.Project_Name;
            ViewBag.PartTypes = item.Part_Types;
            ViewBag.ProductPhase = item.Product_Phase;

            return View();
        }

        public ActionResult GetDateTime(bool IsThisWork)
        {
            string startDate = string.Empty;
            string endDate = string.Empty;

            DayOfWeek weekDay = DateTime.Now.DayOfWeek;

            if (IsThisWork)
            {
                startDate = DateTime.Now.AddDays(DayOfWeek.Monday - weekDay).ToShortDateString();
                endDate = DateTime.Now.AddDays(DayOfWeek.Sunday - weekDay+7).ToShortDateString();
            }
            else
            {
                int changeDay = DayOfWeek.Sunday - weekDay + 8;
                startDate = DateTime.Now.AddDays(changeDay).ToShortDateString();

                changeDay = DayOfWeek.Sunday - weekDay + 15;
                endDate = DateTime.Now.AddDays(changeDay).ToShortDateString();
            }

            string result = "从"+startDate + "至" + endDate;
            return Content(result);
        }

        /////justin************************************************************/////////

        public ActionResult QueryProcessMGData(int MasterUID, bool IsThisWork)
        {
            string date = string.Empty;
            DayOfWeek weekDay = DateTime.Now.DayOfWeek;

            if (IsThisWork)
            {
                date = DateTime.Now.AddDays(DayOfWeek.Monday - weekDay).ToShortDateString();
            }
            else
            {
                date = DateTime.Now.AddDays(DayOfWeek.Sunday - weekDay + 8).ToShortDateString();
            }
            string api = "FlowChart/QueryProcessMGDataAPI?masterUID=" + MasterUID + "&date=" + date;
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(api);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult ModifyProcessMGData(string jsonWithData)
        {
            var apiUrl = "FlowChart/ModifyProcessMGDataAPI";
            var entity = JsonConvert.DeserializeObject<FlowChartPlanManagerDTO>(jsonWithData);

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult QueryProcessMGDataSingle(int detailUID, bool IsThisWork)
        {
            string date = string.Empty;
            DayOfWeek weekDay = DateTime.Now.DayOfWeek;

            if (IsThisWork)
            {
                date = DateTime.Now.AddDays(DayOfWeek.Monday - weekDay).ToShortDateString();
            }
            else
            {
                date = DateTime.Now.AddDays(DayOfWeek.Sunday - weekDay + 8).ToShortDateString();
            }
            string api = "FlowChart/QueryProcessMGDataSingleAPI?detailUID=" + detailUID + "&date=" + date;
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(api);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        #endregion

        public ActionResult QueryFlowCharts(FlowChartModelSearch search, Page page)
        {
            var apiUrl = string.Format("FlowChart/QueryFlowChartsAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }


        public string ImportExcel(HttpPostedFileBase uploadName, string FlowChart_Version_Comment)
        {
            string errorInfo = string.Empty;
            FlowChartImport importItem = new FlowChartImport();

            errorInfo = AddOrUpdateExcel(uploadName, 0, FlowChart_Version_Comment, false, out importItem);

            if (string.IsNullOrEmpty(errorInfo))
            {
                string api = "FlowChart/ImportFlowChartAPI?isEdit=false";
                HttpResponseMessage responMessage = APIHelper.APIPostAsync(importItem, api);
            }
            return errorInfo;
        }

        public string ImportExcelUpdate(HttpPostedFileBase uploadName_update, int FlowChart_Master_UID, string FlowChart_Version_Comment)
        {
            string errorInfo = string.Empty;
            FlowChartImport importItem = new FlowChartImport();

            errorInfo = AddOrUpdateExcel(uploadName_update, FlowChart_Master_UID, FlowChart_Version_Comment, true, out importItem);

            if (string.IsNullOrEmpty(errorInfo))
            {
                string api = "FlowChart/ImportFlowChartAPI?isEdit=true";
                HttpResponseMessage responMessage = APIHelper.APIPostAsync(importItem, api);
            }
            return errorInfo;
        }

        private string AddOrUpdateExcel(HttpPostedFileBase uploadName, int FlowChart_Master_UID, string FlowChart_Version_Comment, bool isEdit, out FlowChartImport importItem)
        {
            string apiUrl = string.Empty;
            string errorInfo = string.Empty;
            FlowChartMasterDTO newMaster = new FlowChartMasterDTO();
            List<FlowChartImportDetailDTO> detailDTOList = new List<FlowChartImportDetailDTO>();
            importItem = new FlowChartImport();
            importItem.FlowChartMasterDTO = newMaster;
            importItem.FlowChartImportDetailDTOList = detailDTOList;
            try
            {
                using (var xlPackage = new ExcelPackage(uploadName.InputStream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                    int totalRows = worksheet.Dimension.End.Row;
                    if (worksheet == null)
                    {
                        errorInfo = "没有worksheet内容";
                        return errorInfo;
                    }
                    //头样式设置
                    var propertiesHead = GetHeadColumn();
                    //内容样式设置
                    var propertiesContent = GetContentColumn();

                    int iRow = 2;

                    bool allColumnsAreEmpty = true;
                    for (var i = 1; i <= propertiesHead.Length; i++)
                    {
                        if (worksheet.Cells[iRow, i].Value != null && !String.IsNullOrEmpty(worksheet.Cells[iRow, i].Value.ToString()))
                        {
                            allColumnsAreEmpty = false;
                            break;
                        }
                    }
                    if (allColumnsAreEmpty)
                    {
                        errorInfo = "Excel格式不正确";
                        return errorInfo;
                    }

                    string BU_D_Name = ExcelHelper.ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(propertiesHead, "客户")].Value);
                    string Project_Name = ExcelHelper.ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(propertiesHead, "专案名称")].Value);
                    string Part_Types = ExcelHelper.ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(propertiesHead, "部件")].Value);
                    string Product_Phase = ExcelHelper.ConvertColumnToString(worksheet.Cells[iRow, GetColumnIndex(propertiesHead, "阶段")].Value);

                    if (string.IsNullOrWhiteSpace(BU_D_Name) || string.IsNullOrWhiteSpace(Project_Name) || string.IsNullOrWhiteSpace(Part_Types) || string.IsNullOrWhiteSpace(Product_Phase))
                    {
                        errorInfo = "客户,专案名称,部件,阶段不能为空Excel格式不正确";
                        return errorInfo;
                    }

                    FlowChartExcelImportParas paraItem = new FlowChartExcelImportParas();
                    paraItem.BU_D_Name = BU_D_Name;
                    paraItem.Project_Name = Project_Name;
                    paraItem.Part_Types = Part_Types;
                    paraItem.Product_Phase = Product_Phase;
                    paraItem.FlowChart_Master_UID = FlowChart_Master_UID;


                    if (isEdit)
                    {
                        paraItem.isEdit = true;
                    }
                    else
                    {
                        paraItem.isEdit = false;
                    }
                    apiUrl = string.Format("FlowChart/CheckFlowChartAPI");
                    HttpResponseMessage responMessage = APIHelper.APIPostAsync(paraItem, apiUrl);
                    var projectUIDOrFLMasterUID = responMessage.Content.ReadAsStringAsync().Result;
                    projectUIDOrFLMasterUID = projectUIDOrFLMasterUID.Replace("\"", "");

                    if (!ValidIsInt(projectUIDOrFLMasterUID, isEdit))
                    {
                        errorInfo = projectUIDOrFLMasterUID;
                        return errorInfo;
                    }


                    if (isEdit)
                    {
                        var idList = projectUIDOrFLMasterUID.Split('_').ToList();
                        newMaster.FlowChart_Master_UID = Convert.ToInt32(idList[0]);
                        newMaster.Project_UID = Convert.ToInt32(idList[1]);
                        newMaster.FlowChart_Version = Convert.ToInt32(idList[2]);
                    }
                    else
                    {
                        newMaster.Project_UID = Convert.ToInt32(projectUIDOrFLMasterUID);
                        newMaster.FlowChart_Version = 1;
                    }
                    newMaster.Part_Types = Part_Types;
                    newMaster.FlowChart_Version_Comment = FlowChart_Version_Comment;
                    newMaster.Is_Latest = true;
                    newMaster.Is_Closed = false;
                    newMaster.Modified_UID = this.CurrentUser.AccountUId;
                    newMaster.Modified_Date = DateTime.Now;


                    //获取所有厂别
                    var plantAPI = "FlowChart/QueryAllFunctionPlantsAPI";
                    //HttpResponseMessage message = APIHelper.APIPostAsync(null,plantAPI);
                    HttpResponseMessage message = APIHelper.APIGetAsync(plantAPI);
                    var jsonPlants = message.Content.ReadAsStringAsync().Result;
                    var functionPlants = JsonConvert.DeserializeObject<List<SystemFunctionPlantDTO>>(jsonPlants);
                    //从第四行开始读取
                    iRow = iRow + 2;
                    //编号
                    int Process_Seq;
                    //DRI
                    string DRI;
                    //场地
                    string Place;
                    //工站名稱
                    string Process;
                    //厂别
                    int System_FunPlant_UID;
                    string plantName;
                    //阶层
                    int Product_Stage;
                    //颜色
                    string Color;
                    //工站說明
                    string Process_Desc;
                    //目标良率
                    //double Target_Yield;
                    //计划目标
                    //int Product_Plan;
                    for (var i = iRow; i <= totalRows; i++)
                    {
                        Process_Seq = Convert.ToInt32(worksheet.Cells[i, GetColumnIndex(propertiesContent, "编号")].Value);
                        //循环结束,防止有空行
                        if (Process_Seq == 0)
                        {
                            break;
                        }
                        DRI = ExcelHelper.ConvertColumnToString(worksheet.Cells[i, GetColumnIndex(propertiesContent, "DRI")].Value);
                        if (string.IsNullOrWhiteSpace(DRI))
                        {
                            //跳出循环
                            errorInfo = string.Format("第[{0}]行DRI不能为空", i);
                            return errorInfo;
                        }
                        Place = ExcelHelper.ConvertColumnToString(worksheet.Cells[i, GetColumnIndex(propertiesContent, "场地")].Value);
                        if (string.IsNullOrWhiteSpace(Place))
                        {
                            //跳出循环
                            errorInfo = string.Format("第[{0}]行场地不能为空", i);
                            return errorInfo;
                        }
                        Process = ExcelHelper.ConvertColumnToString(worksheet.Cells[i, GetColumnIndex(propertiesContent, "工站名稱")].Value);
                        if (string.IsNullOrWhiteSpace(Process))
                        {
                            //跳出循环
                            errorInfo = string.Format("第[{0}]行工站名稱不能为空", i);
                            return errorInfo;
                        }
                        plantName = ExcelHelper.ConvertColumnToString(worksheet.Cells[i, GetColumnIndex(propertiesContent, "厂别")].Value) ?? string.Empty;
                        if (string.IsNullOrWhiteSpace(plantName))
                        {
                            //跳出循环
                            errorInfo = string.Format("第[{0}]行厂别不能为空", i);
                            return errorInfo;
                        }
                        var hasItem = functionPlants.Where(m => m.FunPlant.ToLower() == plantName.ToLower().Trim()).FirstOrDefault();
                        if (hasItem != null)
                        {
                            System_FunPlant_UID = hasItem.System_FunPlant_UID;
                        }
                        else
                        {
                            //跳出循环
                            errorInfo = string.Format("厂别[{0}]输入不正确", plantName);
                            return errorInfo;
                        }
                        Product_Stage = Convert.ToInt32(worksheet.Cells[i, GetColumnIndex(propertiesContent, "阶层")].Value);
                        Color = ExcelHelper.ConvertColumnToString(worksheet.Cells[i, GetColumnIndex(propertiesContent, "颜色")].Value) ?? string.Empty;
                        Process_Desc = ExcelHelper.ConvertColumnToString(worksheet.Cells[i, GetColumnIndex(propertiesContent, "工站說明")].Value);

                        //Target_Yield = Convert.ToDouble(worksheet.Cells[i, GetColumnIndex(propertiesContent, "目标良率")].Value);
                        //Target_Yield = Convert.ToDouble(Target_Yield.ToString("#0.0000"));
                        //Product_Plan = Convert.ToInt32(worksheet.Cells[i, GetColumnIndex(propertiesContent, "计划目标")].Value);

                        var listColor = Color.Split('/').ToList();
                        foreach (var itemColor in listColor)
                        {
                            FlowChartImportDetailDTO detailDTOItem = new FlowChartImportDetailDTO();

                            FlowChartDetailDTO newDetailDtoItem = new FlowChartDetailDTO();
                            newDetailDtoItem.FlowChart_Master_UID = newMaster.FlowChart_Master_UID;
                            newDetailDtoItem.System_FunPlant_UID = System_FunPlant_UID;
                            newDetailDtoItem.Process_Seq = Process_Seq;
                            newDetailDtoItem.DRI = DRI;
                            newDetailDtoItem.Place = Place;
                            newDetailDtoItem.Process = Process;
                            newDetailDtoItem.Product_Stage = Product_Stage;
                            newDetailDtoItem.Color = itemColor;
                            newDetailDtoItem.Process_Desc = Process_Desc;
                            newDetailDtoItem.FlowChart_Version_Comment = FlowChart_Version_Comment;
                            newDetailDtoItem.Modified_UID = newMaster.Modified_UID;
                            newDetailDtoItem.Modified_Date = newMaster.Modified_Date;
                            if (isEdit)
                            {
                                //存到临时表里面的数据所以要加1
                                newDetailDtoItem.FlowChart_Version = newMaster.FlowChart_Version + 1;
                            }
                            else
                            {
                                //存到正式表里面的数据不用加1
                                newDetailDtoItem.FlowChart_Version = newMaster.FlowChart_Version;
                            }

                            //FlowChartMgDataDTO newMgDataDtoItem = new FlowChartMgDataDTO();
                            //newMgDataDtoItem.FlowChart_Detail_UID = newDetailDtoItem.FlowChart_Detail_UID;
                            //newMgDataDtoItem.Product_Plan = Product_Plan;
                            //newMgDataDtoItem.Target_Yield = Target_Yield;
                            //newMgDataDtoItem.Modified_UID = newMaster.Modified_UID;
                            //newMgDataDtoItem.Modified_Date = newMaster.Modified_Date;

                            detailDTOItem.FlowChartDetailDTO = newDetailDtoItem;
                            //detailDTOItem.FlowChartMgDataDTO = newMgDataDtoItem;
                            detailDTOList.Add(detailDTOItem);
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                errorInfo = "上传的文件类型不正确 " + exc.ToString();
            }
            return errorInfo;
        }

        public ActionResult QueryHistoryFlowChart(int FlowChart_Master_UID)
        {
            string api = "FlowChart/QueryHistoryFlowChartAPI?FlowChart_Master_UID=" + FlowChart_Master_UID;
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(api);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        private int GetColumnIndex(string[] properties, string columnName)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            if (columnName == null)
                throw new ArgumentNullException("columnName");

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    //Excel开始的index为1
                    return i + 1;
                }

            }
            return 0;
        }

        public FileResult DownloadExcel()
        {
            var filePath = Server.MapPath("~/ExcelTemplate/");
            var fullFileName = filePath + "FlowChart_Template.xlsx";
            FilePathResult fpr = new FilePathResult(fullFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            return fpr;
        }

        public ActionResult QueryFlowChart(int FlowChart_Master_UID)
        {
            string api = "FlowChart/QueryFlowChartAPI?FlowChart_Master_UID=" + FlowChart_Master_UID;
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(api);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult UpdateExcelInfo(HttpPostedFileBase uploadName_update, int FlowChart_Master_UID, string FlowChart_Version_Comment)
        {
            string errorInfo = string.Empty;
            try
            {
                using (var xlPackage = new ExcelPackage(uploadName_update.InputStream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        errorInfo = "没有worksheet内容";
                        return Content(errorInfo, "application/json");
                    }

                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
            return Content(errorInfo, "application/json");
        }

        private string[] GetHeadColumn()
        {
            var propertiesHead = new[]
            {
                "客户",
                "专案名称",
                "部件",
                "阶段"
            };
            return propertiesHead;
        }

        private string[] GetContentColumn()
        {
            var propertiesContent = new[]
                {
                    "编号",
                    "DRI",
                    "场地",
                    "工站名稱",
                    "厂别",
                    "阶层",
                    "颜色",
                    "工站說明",
                    //"目标良率",
                    //"计划目标"
                };
            return propertiesContent;
        }

        private bool ValidIsInt(string result, bool isEdit)
        {
            var validResult = false;
            if (isEdit)
            {
                var splitList = result.Split('_').ToList();
                if (splitList.Count() > 1)
                {
                    validResult = true;
                }
            }
            else
            {
                int validInt = 0;
                var isInt = int.TryParse(result, out validInt);
                if (isInt)
                {
                    validResult = true;
                }
            }
            return validResult;
        }

        private bool ValidIsDouble(string result)
        {
            var validResult = false;
            double validDouble = 0;
            var isDouble = double.TryParse(result, out validDouble);
            if (isDouble)
            {
                validResult = true;
            }
            return validResult;
        }

        public ActionResult FlowChartDetail(int id, bool IsTemp, int Version)
        {
            ViewBag.ID = id;
            ViewBag.IsTemp = IsTemp;
            ViewBag.Version = Version;

            //绑定功能厂下拉框
            var ddlapiUrl = string.Format("FlowChart/QueryFunPlantAPI?id={0}", id);
            HttpResponseMessage responddlMessage = APIHelper.APIGetAsync(ddlapiUrl);
            var ddlResult = responddlMessage.Content.ReadAsStringAsync().Result;
            var item = JsonConvert.DeserializeObject<FlowChartDetailGetByMasterInfo>(ddlResult);
            ViewBag.FunPlantList = item.SystemFunctionPlantDTOList;
            ViewBag.CustomerName = item.BU_D_Name;
            ViewBag.ProjectName = item.Project_Name;
            ViewBag.PartTypes = item.Part_Types;
            ViewBag.ProductPhase = item.Product_Phase;
            return View();
        }


        public ActionResult QueryFLDetailList(int id, bool IsTemp, int Version)
        {
            ViewBag.DetailUID = id;
            ViewBag.IsTemp = IsTemp;
            var apiUrl = string.Format("FlowChart/QueryFLDetailListAPI?id={0}&IsTemp={1}&Version={2}", id, IsTemp, Version);

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(null, null, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            var item = JsonConvert.DeserializeObject<PagedListModel<FlowChartDetailGet>>(result);
            return Content(result, "application/json");
        }

        public ActionResult DoHistoryExcelExport(int id, bool isTemp)
        {
            var apiUrl = string.Format("FlowChart/DoHistoryExcelExportAPI?id={0}&IsTemp={1}", id, isTemp);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            var item = JsonConvert.DeserializeObject<FlowChartExcelExport>(result);


            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("FlowChart");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var propertiesHead = GetHeadColumn();
            var propertiesContent = GetContentColumn();
            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add("FlowChart");

                //填充Title内容
                for (int colIndex = 0; colIndex < propertiesHead.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = propertiesHead[colIndex];

                    switch (colIndex)
                    {
                        case 0:
                            worksheet.Cells[2, colIndex + 1].Value = item.BU_D_Name;
                            break;
                        case 1:
                            worksheet.Cells[2, colIndex + 1].Value = item.Project_Name;
                            break;
                        case 2:
                            worksheet.Cells[2, colIndex + 1].Value = item.Part_Types;
                            break;
                        case 3:
                            worksheet.Cells[2, colIndex + 1].Value = item.Product_Phase;
                            break;
                    }
                }

                //填充Content内容
                for (int colIndex = 0; colIndex < propertiesContent.Length; colIndex++)
                {
                    worksheet.Cells[3, colIndex + 1].Value = propertiesContent[colIndex];
                }

                for (int index = 0; index < item.FlowChartDetailAndMGDataDTOList.Count(); index++)
                {
                    var currentRecord = item.FlowChartDetailAndMGDataDTOList[index];
                    worksheet.Cells[index + 4, 1].Value = index + 1;
                    //DRI
                    worksheet.Cells[index + 4, 2].Value = currentRecord.DRI;
                    //场地
                    worksheet.Cells[index + 4, 3].Value = currentRecord.Place;
                    //Process
                    worksheet.Cells[index + 4, 4].Value = currentRecord.Process;
                    //厂别
                    worksheet.Cells[index + 4, 5].Value = currentRecord.PlantName;
                    //阶层
                    worksheet.Cells[index + 4, 6].Value = currentRecord.Product_Stage;
                    //颜色
                    worksheet.Cells[index + 4, 7].Value = currentRecord.Color;
                    //工站说明
                    worksheet.Cells[index + 4, 8].Value = currentRecord.Process_Desc;
                    //目标良率
                    //worksheet.Cells[index + 4, 9].Value = currentRecord.Target_Yield;
                    //worksheet.Cells[index + 4, 9].Style.Numberformat.Format = "0.00%";
                    //计划目标
                    //worksheet.Cells[index + 4, 10].Value = currentRecord.Product_Plan;
                }

                excelPackage.Save();
            }
            return new FileContentResult(stream.ToArray(), "application/octet-stream")
            { FileDownloadName = Server.UrlEncode(fileName) };

        }

        public ActionResult ClosedFlowChart(int FlowChart_Master_UID, bool isClosed)
        {
            var apiUrl = string.Format("FlowChart/ClosedFLAPI?FlowChart_Master_UID={0}&isClosed={1}", FlowChart_Master_UID, isClosed);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult DeleteFlowChartTemp(int FlowChart_Master_UID)
        {
            var apiUrl = string.Format("FlowChart/DeleteFLTempAPI?FlowChart_Master_UID={0}", FlowChart_Master_UID);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            return null;
        }

        public ActionResult QueryFLDetailByID(int FlowChart_Detail_UID)
        {
            var apiUrl = string.Format("FlowChart/QueryFLDetailByIDAPI?id={0}", FlowChart_Detail_UID);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult EditFLDetailInfo(int id, bool isTemp, int Version, FlowChartDetailDTO dto)
        {
            var apiUrl = string.Format("FlowChart/SaveFLDetailInfoAPI?AccountID={0}",this.CurrentUser.AccountUId);
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(dto, apiUrl);
            return RedirectToAction("FlowChartDetail", "FlowChart", new { id = id, IsTemp = isTemp, Version = Version });
        }

        public ActionResult SaveAllDetailInfo(string jsonDataTable)
        {
            var apiUrl = string.Format("FlowChart/SaveAllDetailInfoAPI?AccountID={0}", this.CurrentUser.AccountUId);
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(jsonDataTable, apiUrl);
            return null;
        }

        #region FlowChart生产维护

        #region 生产维护模板下载
        public FileResult DownloadPlanExcel(int id, string clintName)
        {
            var apiUrl = string.Format("FlowChart/QueryFLDetailListAPI?id={0}", id);
            //var list = JsonConvert.DeserializeObject<List<FlowChartDetailDTO>()>
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<List<FlowChartDetailDTO>>(result);
            if (list.Count() > 0)
            {
                var stream = new MemoryStream();
                var fileName = PathHelper.SetGridExportExcelName("FlowChartPlanManager");
                string[] propertiesHead = new string[] { };
                switch (clintName)
                {
                    case "js_btn_download_fl":
                        propertiesHead = GetNextWeekPlanHeadColumn();
                        break;
                    case "js_btn_download_currentWK":
                        propertiesHead = GetCurrentWeekPlanHeadColumn();
                        break;
                }

                using (var excelPackage = new ExcelPackage(stream))
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("FlowChartPlanManager");

                    SetExcelStyle(worksheet, propertiesHead);

                    int iRow = 3;
                    foreach (var item in list)
                    {
                        worksheet.Cells[iRow, 1].Value = item.Process_Seq;
                        worksheet.Cells[iRow, 2].Value = item.Process;
                        worksheet.Cells[iRow, 3].Value = item.Color;
                        worksheet.Cells[iRow, 18].Value = item.FlowChart_Detail_UID;
                        iRow++;
                    }
                    //Excel最后一行行号
                    int maxRow = iRow - 1;
                    //设置灰色背景
                    var colorRange = string.Format("A3:C{0}", maxRow);
                    worksheet.Cells[colorRange].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[colorRange].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                    //设置主键列隐藏
                    worksheet.Column(18).Hidden = true;
                    //设置边框
                    worksheet.Cells[string.Format("A1:Q{0}", maxRow)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[string.Format("A1:Q{0}", maxRow)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[string.Format("A1:Q{0}", maxRow)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[string.Format("A1:Q{0}", maxRow)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    excelPackage.Save();
                }
                return new FileContentResult(stream.ToArray(), "application/octet-stream")
                { FileDownloadName = Server.UrlEncode(fileName) };
            }
            else
            {
                return null;
            }
        }

        private void SetExcelStyle(ExcelWorksheet worksheet, string[] propertiesHead)
        {
            //填充Title内容
            worksheet.Cells[1, 1].Value = propertiesHead[0];
            worksheet.Cells[1, 2].Value = propertiesHead[1];
            worksheet.Cells[1, 3].Value = propertiesHead[2];
            worksheet.Cells[1, 4].Value = propertiesHead[3];
            worksheet.Cells[1, 6].Value = propertiesHead[4];
            worksheet.Cells[1, 8].Value = propertiesHead[5];
            worksheet.Cells[1, 10].Value = propertiesHead[6];
            worksheet.Cells[1, 12].Value = propertiesHead[7];
            worksheet.Cells[1, 14].Value = propertiesHead[8];
            worksheet.Cells[1, 16].Value = propertiesHead[9];
            worksheet.Cells[1, 18].Value = propertiesHead[10];
            //合并单元格
            worksheet.Cells["D1:E1"].Merge = true;
            worksheet.Cells["F1:G1"].Merge = true;
            worksheet.Cells["H1:I1"].Merge = true;
            worksheet.Cells["J1:K1"].Merge = true;
            worksheet.Cells["L1:M1"].Merge = true;
            worksheet.Cells["N1:O1"].Merge = true;
            worksheet.Cells["P1:Q1"].Merge = true;

            //填充SubTitle内容
            for (int i = 4; i < 18; i++)
            {
                if (i % 2 == 0)
                {
                    worksheet.Cells[2, i].Value = "生产计划";
                }
                else
                {
                    worksheet.Cells[2, i].Value = "目标良率";
                }
                worksheet.Cells[2, i].Style.Font.Bold = true;
            }

            //设置百分比格式
            worksheet.Column(5).Style.Numberformat.Format = "0.00%";
            worksheet.Column(7).Style.Numberformat.Format = "0.00%";
            worksheet.Column(9).Style.Numberformat.Format = "0.00%";
            worksheet.Column(11).Style.Numberformat.Format = "0.00%";
            worksheet.Column(13).Style.Numberformat.Format = "0.00%";
            worksheet.Column(15).Style.Numberformat.Format = "0.00%";
            worksheet.Column(17).Style.Numberformat.Format = "0.00%";

            //设置列宽
            worksheet.Column(1).Width = 10;
            worksheet.Column(2).Width = 17;
            for (int i = 3; i <= 17; i++)
            {
                worksheet.Column(i).Width = 12;
            }

            worksheet.Cells["A1:Q2"].Style.Font.Bold = true;
            worksheet.Cells["A1:Q2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A1:Q2"].Style.Fill.BackgroundColor.SetColor(Color.Orange);

        }

        private string[] GetNextWeekPlanHeadColumn()
        {
            var nextWeek = GetNextWeek(DateTime.Now);
            var propertiesHead = new[]
            {
                "制程序号",
                "制程名称",
                "颜色",
                string.Format("星期一({0})", nextWeek.Monday.Date.ToString("yyyy-MM-dd")),
                string.Format("星期二({0})", nextWeek.Tuesday.Date.ToString("yyyy-MM-dd")),
                string.Format("星期三({0})", nextWeek.Wednesday.Date.ToString("yyyy-MM-dd")),
                string.Format("星期四({0})", nextWeek.Thursday.Date.ToString("yyyy-MM-dd")),
                string.Format("星期五({0})", nextWeek.Friday.Date.ToString("yyyy-MM-dd")),
                string.Format("星期六({0})", nextWeek.Saturday.Date.ToString("yyyy-MM-dd")),
                string.Format("星期日({0})", nextWeek.Sunday.Date.ToString("yyyy-MM-dd")),
                "FlowChart_Detail_UID"
            };
            return propertiesHead;
        }

        private string[] GetCurrentWeekPlanHeadColumn()
        {
            var currentWeek = GetCurrentWeek(DateTime.Now);
            var propertiesHead = new[]
            {
                "制程序号",
                "制程名称",
                "颜色",
                string.Format("星期一({0})", currentWeek.Monday.Date.ToString("yyyy-MM-dd")),
                string.Format("星期二({0})", currentWeek.Tuesday.Date.ToString("yyyy-MM-dd")),
                string.Format("星期三({0})", currentWeek.Wednesday.Date.ToString("yyyy-MM-dd")),
                string.Format("星期四({0})", currentWeek.Thursday.Date.ToString("yyyy-MM-dd")),
                string.Format("星期五({0})", currentWeek.Friday.Date.ToString("yyyy-MM-dd")),
                string.Format("星期六({0})", currentWeek.Saturday.Date.ToString("yyyy-MM-dd")),
                string.Format("星期日({0})", currentWeek.Sunday.Date.ToString("yyyy-MM-dd")),
                "FlowChart_Detail_UID"
            };
            return propertiesHead;
        }

        private Week GetNextWeek(DateTime dt)
        {
            var strDT = dt.DayOfWeek.ToString();
            Week nextWeek = new Week();
            //获取下周一的日期
            switch (strDT)
            {
                case "Monday":
                    nextWeek.Monday = dt.AddDays(7);
                    break;
                case "Tuesday":
                    nextWeek.Monday = dt.AddDays(6);
                    break;
                case "Wednesday":
                    nextWeek.Monday = dt.AddDays(5);
                    break;
                case "Thursday":
                    nextWeek.Monday = dt.AddDays(4);
                    break;
                case "Friday":
                    nextWeek.Monday = dt.AddDays(3);
                    break;
                case "Saturday":
                    nextWeek.Monday = dt.AddDays(2);
                    break;
                case "Sunday":
                    nextWeek.Monday = dt.AddDays(1);
                    break;
            }
            nextWeek.Tuesday = nextWeek.Monday.AddDays(1);
            nextWeek.Wednesday = nextWeek.Monday.AddDays(2);
            nextWeek.Thursday = nextWeek.Monday.AddDays(3);
            nextWeek.Friday = nextWeek.Monday.AddDays(4);
            nextWeek.Saturday = nextWeek.Monday.AddDays(5);
            nextWeek.Sunday = nextWeek.Monday.AddDays(6);

            return nextWeek;
        }

        private Week GetCurrentWeek(DateTime dt)
        {
            var strDT = dt.DayOfWeek.ToString();
            Week currentWeek = new Week();
            switch (strDT)
            {
                case "Monday":
                    currentWeek.Monday = dt;
                    break;
                case "Tuesday":
                    currentWeek.Monday = dt.AddDays(-1);
                    break;
                case "Wednesday":
                    currentWeek.Monday = dt.AddDays(-2);
                    break;
                case "Thursday":
                    currentWeek.Monday = dt.AddDays(-3);
                    break;
                case "Friday":
                    currentWeek.Monday = dt.AddDays(-4);
                    break;
                case "Saturday":
                    currentWeek.Monday = dt.AddDays(-5);
                    break;
                case "Sunday":
                    currentWeek.Monday = dt.AddDays(-6);
                    break;
            }
            currentWeek.Tuesday = currentWeek.Monday.AddDays(1);
            currentWeek.Wednesday = currentWeek.Monday.AddDays(2);
            currentWeek.Thursday = currentWeek.Monday.AddDays(3);
            currentWeek.Friday = currentWeek.Monday.AddDays(4);
            currentWeek.Saturday = currentWeek.Monday.AddDays(5);
            currentWeek.Sunday = currentWeek.Monday.AddDays(6);

            return currentWeek;
        }
        #endregion


        #region 生产维护模板导入
        public string ImportPlanExcel(HttpPostedFileBase upload_excel, int FlowChart_Master_UID, string hid_currentOrNextWeek)
        {
            string errorInfo = string.Empty;
            using (var xlPackage = new ExcelPackage(upload_excel.InputStream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                int totalRows = worksheet.Dimension.End.Row;
                int totalColumns = worksheet.Dimension.Columns;
                if (worksheet == null)
                {
                    errorInfo = "没有worksheet内容";
                    return errorInfo;
                }

                //获取表头内容
                string[] propertiesHead = new string[] { };
                Week week = new Week();
                switch (hid_currentOrNextWeek)
                {
                    case "nextWeek":
                        propertiesHead = GetNextWeekPlanHeadColumn();
                        week = GetNextWeek(DateTime.Now);
                        break;
                    case "currentWeek":
                        propertiesHead = GetCurrentWeekPlanHeadColumn();
                        week = GetCurrentWeek(DateTime.Now);
                        break;
                }

                bool allColumnsAreError = false;

                //验证Excel的表头是否正确
                allColumnsAreError = validExcelTitleIsErrorTwo(worksheet, propertiesHead, 1, totalColumns);
                if (allColumnsAreError)
                {
                    errorInfo = "Excel格式不正确";
                    return errorInfo;
                }

                List<FlowChartMgDataDTO> mgDataList = new List<FlowChartMgDataDTO>();
                //Excel行号
                int iRow = 3;
                //Excel列号
                int iColumn;
                int j = 4;
                //如果Excel导入的计划是本周的计划，则导入的数据从当天的日期开始到往后导入到数据库中
                if (hid_currentOrNextWeek == "currentWeek")
                {
                    var strDT = DateTime.Now.DayOfWeek.ToString();
                    switch (strDT)
                    {
                        case "Monday":
                            j = 4;
                            break;
                        case "Tuesday":
                            j = 6;
                            break;
                        case "Wednesday":
                            j = 8;
                            break;
                        case "Thursday":
                            j = 10;
                            break;
                        case "Friday":
                            j = 12;
                            break;
                        case "Saturday":
                            j = 14;
                            break;
                        case "Sunday":
                            j = 16;
                            break;
                    }
                }

                for (iRow = 3; iRow <= totalRows; iRow++)
                {
                    if (allColumnsAreError)
                    {
                        break;
                    }
                    for (iColumn = j; iColumn <= 17; iColumn++)
                    {
                        FlowChartMgDataDTO newMGDataItem = new FlowChartMgDataDTO();

                        var FlowChart_Detail_UID = ExcelHelper.ConvertColumnToString(worksheet.Cells[iRow, 18].Value);
                        if (string.IsNullOrWhiteSpace(FlowChart_Detail_UID))
                        {
                            allColumnsAreError = true;
                            errorInfo = string.Format("第{0}行主键没有值", iRow);
                            break;
                        }

                        var Product_Plan = ExcelHelper.ConvertColumnToString(worksheet.Cells[iRow, iColumn].Value);
                        //如果生产计划为空则默认为0
                        if (string.IsNullOrWhiteSpace(Product_Plan))
                        {
                            Product_Plan = "0";
                            //allColumnsAreError = true;
                            //errorInfo = string.Format("第{0}行生产计划没有值", iRow);
                            //break;
                        }

                        if (!ValidIsDouble(Product_Plan))
                        {
                            allColumnsAreError = true;
                            errorInfo = string.Format("第{0}行生产计划输入不正确", iRow);
                            break;
                        }

                        var Target_Yield = ExcelHelper.ConvertColumnToString(worksheet.Cells[iRow, ++iColumn].Value);
                        //如果目标良率为空则默认为0
                        if (string.IsNullOrWhiteSpace(Target_Yield))
                        {
                            Target_Yield = "0";
                            //allColumnsAreError = true;
                            //errorInfo = string.Format("第{0}行目标良率没有值", iRow);
                            //break;
                        }
                        if (!ValidIsDouble(Target_Yield))
                        {
                            allColumnsAreError = true;
                            errorInfo = string.Format("第{0}行目标良率输入不正确", iRow);
                            break;
                        }

                        newMGDataItem.FlowChart_Detail_UID = Convert.ToInt32(FlowChart_Detail_UID);
                        switch (iColumn)
                        {
                            case 5:
                                newMGDataItem.Product_Date = week.Monday;
                                break;
                            case 7:
                                newMGDataItem.Product_Date = week.Tuesday;
                                break;
                            case 9:
                                newMGDataItem.Product_Date = week.Wednesday;
                                break;
                            case 11:
                                newMGDataItem.Product_Date = week.Thursday;
                                break;
                            case 13:
                                newMGDataItem.Product_Date = week.Friday;
                                break;
                            case 15:
                                newMGDataItem.Product_Date = week.Saturday;
                                break;
                            case 17:
                                newMGDataItem.Product_Date = week.Sunday;
                                break;
                        }

                        //四舍五入小数点
                        newMGDataItem.Product_Plan = Convert.ToInt32(Convert.ToDouble(Product_Plan).ToString("#0"));

                        //四舍五入小数点
                        newMGDataItem.Target_Yield = Convert.ToDouble(Target_Yield);
                        newMGDataItem.Target_Yield = Convert.ToDouble(newMGDataItem.Target_Yield.ToString("#0.0000"));

                        newMGDataItem.Modified_UID = this.CurrentUser.AccountUId;
                        newMGDataItem.Modified_Date = DateTime.Now;

                        mgDataList.Add(newMGDataItem);
                    }

                }

                if (allColumnsAreError)
                {
                    return errorInfo;
                }

                var json = JsonConvert.SerializeObject(mgDataList);
                string api = string.Format("FlowChart/ImportFlowChartMGDataAPI?FlowChart_Master_UID={0}", FlowChart_Master_UID);
                HttpResponseMessage responMessage = APIHelper.APIPostAsync(json, api);

            }
            return errorInfo;
        }

        #region /验证Excel的表头是否正确
        private bool validExcelTitleIsErrorTwo(ExcelWorksheet worksheet, string[] propertiesHead, int iRow, int totalColumns)
        {
            bool allColumnsAreError = false;
            for (var i = 1; i <= totalColumns; i++)
            {
                if (allColumnsAreError)
                {
                    break;
                }
                switch (i)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4: //星期一
                        allColumnsAreError = validExcelTitleIsError(worksheet, propertiesHead, iRow, i, i - 1);
                        break;
                    case 6: //星期二
                        allColumnsAreError = validExcelTitleIsError(worksheet, propertiesHead, iRow, i, 4);
                        break;
                    case 8: //星期三
                        allColumnsAreError = validExcelTitleIsError(worksheet, propertiesHead, iRow, i, 5);
                        break;
                    case 10: //星期四
                        allColumnsAreError = validExcelTitleIsError(worksheet, propertiesHead, iRow, i, 6);
                        break;
                    case 12: //星期五
                        allColumnsAreError = validExcelTitleIsError(worksheet, propertiesHead, iRow, i, 7);
                        break;
                    case 14: //星期六
                        allColumnsAreError = validExcelTitleIsError(worksheet, propertiesHead, iRow, i, 8);
                        break;
                    case 16: //星期日
                        allColumnsAreError = validExcelTitleIsError(worksheet, propertiesHead, iRow, i, 9);
                        break;
                    case 18:
                        if (ExcelHelper.ConvertColumnToString(worksheet.Cells[iRow, i].Value) != "FlowChart_Detail_UID")
                        {
                            allColumnsAreError = true;
                        }
                        break;
                    default:
                        continue;
                }
            }
            return allColumnsAreError;
        }

        private bool validExcelTitleIsError(ExcelWorksheet worksheet, string[] propertiesHead, int iRow, int iColumn, int column)
        {
            bool isError = false;
            if (ExcelHelper.ConvertColumnToString(worksheet.Cells[iRow, iColumn].Value) != propertiesHead[column])
            {
                isError = true;
            }
            return isError;
        }
        #endregion

        #endregion


        #endregion


    }


}