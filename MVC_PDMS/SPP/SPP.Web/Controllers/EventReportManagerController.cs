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
using System.Linq;
using OfficeOpenXml.Style;

namespace SPP.Web.Controllers
{
    public class EventReportManagerController : WebControllerBase
    {
        // GET: EventReportManager
        public ActionResult Index()
        {

            return View();
        }

        #region WarningList----------------------------------Destiny 2015/12/9

        public ActionResult WarningList()
        {
            ViewBag.PageTitle = null;
            return View();
        }

        public ActionResult GetWarningList()
        {
            string user_account_uid = this.CurrentUser.AccountUId.ToString();
            string apiUrl = string.Format("EventReportManager/GetWarningListAPI?User_account_uid={0}", user_account_uid);
            HttpResponseMessage responseMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responseMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        [HttpGet]
        public ActionResult PrepareForEditWarningListData(int Warning_UID)
        {
            string apiUrl = string.Format("EventReportManager/GetWarningDataByWarningUidAPI?Warning_UID={0}", Warning_UID);
            HttpResponseMessage responseMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responseMessage.Content.ReadAsStringAsync().Result;

            var entity = JsonConvert.DeserializeObject<ProcessDataSearch>(result);
            TempData["ProcessDataSearch"] = entity;

            return RedirectToAction("QuestProductDatas", "ProductInput");
        }

        #endregion
        #region PPCheckData----------------------------------Sidney 2015/12/4
        /// <summary>
        /// PPCheckData
        /// </summary>
        /// <returns></returns>
        public ActionResult PPCheckData()
        {
            return View();
        }
        /// <summary>
        /// QueryPPCheckDatas
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult QueryPPCheckDatas(PPCheckDataSearch search, Page page)
        {
            var apiUrl = string.Format("EventReportManager/QueryPPCheckDatasAPI");
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }
        /// <summary>
        /// 修改WIP
        /// </summary>
        /// <param name="jsonPPCheckList"></param>
        /// <returns></returns>
        public ActionResult EditWIPView(int product_uid, int wip_qty, int wip_old, int wip_add, string comment)
        {
            var apiUrl = string.Format("EventReportManager/EditWIPViewAPI?product_uid={0}&&wip_qty={1}&&wip_old={2}&&wip_add={3}&&comment={4}&&modifiedUser={5}", product_uid, wip_qty, wip_old, wip_add, comment, this.CurrentUser.AccountUId);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }
        /// <summary>
        /// 修改WIP
        /// </summary>
        /// <param name="jsonPPCheckList"></param>
        /// <returns></returns>
        public ActionResult EditWIP(string jsonPPCheckList)
        {
            var apiUrl = "EventReportManager/EditWIPAPI";
            var entity = JsonConvert.DeserializeObject<PPEditWIP>(jsonPPCheckList);
            entity.Modified_UID=this.CurrentUser.AccountUId;
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }
        /// <summary>
        /// 导出EXCEL
        /// </summary>
        /// <param name="jsonExportList"></param>
        /// <returns></returns>
        public ActionResult DoExportPPCheckData(string jsonExportList, string submitType, string exportName)
        {
            //创建EXL文档
            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("PP检核资料数据");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new List<string> { "序号", "制程序号", "场地", "功能厂", "制程", "颜色", "负责主管", "目标良率", "计划" };
            var stringSubs = new List<string> { "滚动计划", "领料", "仓库领料", "良品", "试制调机", "入库", "NG", "滚动达成率", "最终良率", "WIP" };
            int columnIndex = 0;
            using (var excelPackage = new ExcelPackage(stream))
            {
                //定义变量
                var fail_NoData = "";
                //获取当前时段及日期
                var apiUrl = string.Format("EventReportManager/GetIntervalInfoAPI?opType={0}", "OP1");
                HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
                var intervalResult = responMessage.Content.ReadAsStringAsync().Result;
                var intervalList = JsonConvert.DeserializeObject<IntervalEnum>(intervalResult);
                var nowDate = intervalList.NowDate;
                var nowInterval = intervalList.Time_Interval;
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add(nowDate);
                var entity = JsonConvert.DeserializeObject<ExportPPCheckData>(jsonExportList);
                worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                if (!entity.TabList.Any())
                {
                    return Content("导出EXL数据前，请先查询数据！");
                }
                foreach (var item in entity.TabList)
                {
                    //获取到数据源
                    var QueryData = new PPCheckDataSearch();
                    QueryData.Customer = entity.Customer;
                    QueryData.Color = entity.Color;
                    QueryData.Part_Types = entity.Part_Types;
                    QueryData.Interval_Time = entity.Time_InterVal;
                    QueryData.Project = entity.Project;
                    QueryData.Reference_Date = entity.Reference_Date;
                    QueryData.Tab_Select_Text = item.Time_InterVal;
                    QueryData.Product_Phase = entity.Product_Phase;
                    var page = new Page();
                    page.PageSize = 10;
                    var apiUrl1 = "";
                    if (exportName == "Report")
                        apiUrl1 = string.Format("EventReportManager/QueryReportDatasAPI");
                    else
                        apiUrl1 = string.Format("EventReportManager/QueryPPCheckDatasAPI");
                    HttpResponseMessage responMessage1 = APIHelper.APIPostAsync(QueryData, page, apiUrl1);
                    var result1 = responMessage1.Content.ReadAsStringAsync().Result;
                    var list = JsonConvert.DeserializeObject<ExportPPCheckDataResult>(result1);
                    if (!list.Items.Any())
                    {
                        fail_NoData = "时段：" + item.Time_InterVal + "没有数据，请重新选择查询条件！";
                        return Content(fail_NoData);
                    }

                    //第一笔资料需要加上汇总的数据
                    if (columnIndex == 0)
                    {
                        stringHeads.AddRange(stringSubs);
                        columnIndex = stringHeads.Count();
                        //创建EXL头部
                        worksheet.Cells[1, 1, 1, 9].Merge = true;
                        worksheet.Cells[1, 1].Value = "客户:" + entity.Customer + " 专案:" + entity.Project + " 部件:" + entity.Part_Types + " 日期:" + entity.Reference_Date + " 2H战情报表";
                        worksheet.Cells[2, 1, 2, 9].Merge = true;
                        worksheet.Cells[2, 1].Value = "Update:" + nowDate;
                        worksheet.Cells[3, 1, 3, 9].Merge = true;
                        worksheet.Cells[3, 10, 3, 19].Merge = true;
                        worksheet.Cells[3, 10].Value = "当日生产达成状况";
                        //创建标题
                        for (int colIndex = 0; colIndex < columnIndex; colIndex++)
                        {
                            worksheet.Cells[4, colIndex + 1].Value = stringHeads[colIndex];
                        }
                        //set cell value
                        for (int index = 0; index < list.Items.Count; index++)
                        {

                            var currentRecord = list.Items[index];
                            //seq                       
                            worksheet.Cells[index + 5, 1].Value = index + 1;
                            worksheet.Cells[index + 5, 2].Value = currentRecord.Process_Seq;
                            worksheet.Cells[index + 5, 3].Value = currentRecord.Place;
                            worksheet.Cells[index + 5, 4].Value = currentRecord.FunPlant;
                            worksheet.Cells[index + 5, 5].Value = currentRecord.Process;
                            worksheet.Cells[index + 5, 6].Value = currentRecord.Color;
                            worksheet.Cells[index + 5, 7].Value = currentRecord.FunPlant_Manager;
                            worksheet.Cells[index + 5, 8].Value = currentRecord.Target_Yield + "%";
                            worksheet.Cells[index + 5, 9].Value = currentRecord.Product_Plan;
                            worksheet.Cells[index + 5, 10].Value = currentRecord.Product_Plan_Sum;
                            worksheet.Cells[index + 5, 11].Value = currentRecord.Picking_QTY;
                            worksheet.Cells[index + 5, 12].Value = currentRecord.WH_Picking_QTY;
                            worksheet.Cells[index + 5, 13].Value = currentRecord.Good_QTY;
                            worksheet.Cells[index + 5, 14].Value = currentRecord.Adjust_QTY;
                            worksheet.Cells[index + 5, 15].Value = currentRecord.WH_QTY;
                            worksheet.Cells[index + 5, 16].Value = currentRecord.NG_QTY;
                            worksheet.Cells[index + 5, 17].Value = currentRecord.Rolling_Yield_Rate + "%";
                            worksheet.Cells[index + 5, 18].Value = currentRecord.Finally_Field + "%";
                            worksheet.Cells[index + 5, 19].Value = currentRecord.WIP_QTY;
                        }

                    }
                    else
                    {
                        stringHeads.AddRange(stringSubs);
                        columnIndex = stringHeads.Count();
                        var stringLength = (stringHeads.Count() - stringSubs.Count());

                        worksheet.Cells[3, stringLength + 1, 3, stringHeads.Count()].Merge = true;
                        worksheet.Cells[3, stringLength + 1].Value = "时段：" + item.Time_InterVal;
                        //创建其他时段的标题
                        for (int colIndex = stringLength; colIndex < stringHeads.Count(); colIndex++)
                        {
                            worksheet.Cells[4, colIndex + 1].Value = stringHeads[colIndex];
                        }
                        //set cell value
                        for (int index = 0; index < list.Items.Count; index++)
                        {
                            var currentRecord = list.Items[index];
                            //seq                       
                            worksheet.Cells[index + 5, stringLength + 1].Value = (currentRecord.Product_Plan_Sum == null) ? currentRecord.Product_Plan_Sum : 0;
                            worksheet.Cells[index + 5, stringLength + 2].Value = currentRecord.Picking_QTY;
                            worksheet.Cells[index + 5, stringLength + 3].Value = currentRecord.WH_Picking_QTY;
                            worksheet.Cells[index + 5, stringLength + 4].Value = currentRecord.Good_QTY;
                            worksheet.Cells[index + 5, stringLength + 5].Value = currentRecord.Adjust_QTY;
                            worksheet.Cells[index + 5, stringLength + 6].Value = currentRecord.WH_QTY;
                            worksheet.Cells[index + 5, stringLength + 7].Value = currentRecord.NG_QTY;
                            worksheet.Cells[index + 5, stringLength + 8].Value = currentRecord.Rolling_Yield_Rate + "%";
                            worksheet.Cells[index + 5, stringLength + 9].Value = currentRecord.Finally_Field + "%";
                            worksheet.Cells[index + 5, stringLength + 10].Value = currentRecord.WIP_QTY;
                        }
                    }
                }


                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }
            if (submitType == "AJAX")
            {
                return Content("SUCCESS");
            }
            else
            {
                return new FileContentResult(stream.ToArray(), "application/octet-stream") { FileDownloadName = Server.UrlEncode(fileName) };
            }

        }
        /// <summary>
        /// 获取Comfirm标志
        /// </summary>
        /// <param name="ComfirmJson"></param>
        /// <returns></returns>
        public ActionResult GetComfirmValue(string ComfirmJson)
        {
            var ComfirmResult = "";
            var entity = JsonConvert.DeserializeObject<ExportPPCheckData>(ComfirmJson);
            foreach (var item in entity.TabList)
            {
                //获取到数据源
                var QueryData = new PPCheckDataSearch();
                QueryData.Customer = entity.Customer;
                QueryData.Color = entity.Color;
                QueryData.Part_Types = entity.Part_Types;
                QueryData.Interval_Time = entity.Time_InterVal;
                QueryData.Project = entity.Project;
                QueryData.Reference_Date = entity.Reference_Date;
                QueryData.Tab_Select_Text = item.Time_InterVal;
                QueryData.Product_Phase = entity.Product_Phase;
                var page = new Page();
                page.PageSize = 10;
                var apiUrl1 = string.Format("EventReportManager/QueryPPCheckDatasAPI");
                HttpResponseMessage responMessage1 = APIHelper.APIPostAsync(QueryData, page, apiUrl1);
                var result1 = responMessage1.Content.ReadAsStringAsync().Result;
                var list = JsonConvert.DeserializeObject<ExportPPCheckDataResult>(result1);
                foreach (var listItem in list.Items)
                {
                    if (listItem.Is_Comfirm == 1)
                        ComfirmResult = "Fail";
                    break;
                }
                if (ComfirmResult == "Fail")
                    break;
            }
            return Content("Fail", "application/json");
        }
        /// <summary>
        /// PP检核时，检查是否所有功能厂都有提供数据
        /// </summary>
        /// <param name="jsonList"></param>
        /// <returns></returns>
        public ActionResult CheckFunPlantDataIsFull(string jsonList)
        {
            var entity = JsonConvert.DeserializeObject<ExportPPCheckData>(jsonList);
            var pp = new PPCheckDataSearch
            {
                Customer = entity.Customer,
                Color = entity.Color,
                Interval_Time = entity.Time_InterVal,
                Part_Types = entity.Part_Types,
                Product_Phase = entity.Product_Phase,
                Project = entity.Project,
                Reference_Date = entity.Reference_Date
            };
            foreach (var en in entity.TabList)
            {
                //仅检查当前时段，当日汇总不检查
                if (en.Time_InterVal != "ALL")
                    pp.Tab_Select_Text = en.Time_InterVal;
            }
            var apiUrl = string.Format("EventReportManager/CheckFunPlantDataIsFullAPI");
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(pp, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }
        #endregion
        #region ProductReportDisplay----------------------------------Sidney 2015/12/4
        public ActionResult ProductReportDisplay()
        {
            return View();
        }
        /// <summary>
        /// 查询日报表数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult QueryReportDatas(ReportDataSearch search, Page page)
        {
            var apiUrl = string.Format("EventReportManager/QueryReportDatasAPI");
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }
        /// <summary>
        /// 导出EXCEL
        /// </summary>
        /// <param name="jsonExportList"></param>
        /// <returns></returns>
         public ActionResult DoExportIntervalData(string jsonexportlist, string submitType, string exportName)
        {
        
            var entity = JsonConvert.DeserializeObject<ReportDataSearch>(jsonexportlist);
        //get Export datas
        var apiUrl = string.Format("ProductInput/QueryTimeSpanReport");
        HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
        var result = responMessage.Content.ReadAsStringAsync().Result;

        var list = JsonConvert.DeserializeObject<ExportIntervalReportDataResult>(result);


        var stream = new MemoryStream();
        var fileName = PathHelper.SetGridExportExcelName(exportName);
        var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
        var stringHeads = new string[] { "制程", "Total 计划", "Total 实际", "Total 达成率"};


            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add("周报");

                //set Title
                for (int colIndex = 0; colIndex<stringHeads.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = stringHeads[colIndex];
                }

                //set cell value
                for (int index = 0; index<list.Items.Count; index++)
                {
                    var currentRecord = list.Items[index];
//seq

                    worksheet.Cells[index + 2, 1].Value = currentRecord.Process;
                    worksheet.Cells[index + 2, 2].Value = currentRecord.SumPlan;
                    worksheet.Cells[index + 2, 3].Value = currentRecord.SumGoodQty;
                    worksheet.Cells[index + 2, 4].Value = currentRecord.SumYieldRate;
                  
                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }

            if (submitType == "AJAX")
            {
                return Content("SUCCESS");
            }
            else
            {
                return new FileContentResult(stream.ToArray(), "application/octet-stream") { FileDownloadName = Server.UrlEncode(fileName) };
            }
        }
        public ActionResult DoExportWeeklyData(string jsonexportlist, string submitType, string exportName)
        {
        
            var entity = JsonConvert.DeserializeObject<ReportDataSearch>(jsonexportlist);
            //get Export datas
            var apiUrl = string.Format("ProductInput/QueryWeekReport");
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            var list = JsonConvert.DeserializeObject<ExportWeeklyReportDataResult>(result);


            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName(exportName);
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new string[] { "制程", "Total 计划", "Total 实际", "Total 达成率", "(周一) 计划", "(周一) 实际", "(周一) 达成率",
                "(周二) 计划", "(周二) 实际", "(周二) 达成率",
                  "(周三) 计划", "(周三) 实际", "(周三) 达成率",
                    "(周四) 计划", "(周四) 实际", "(周四) 达成率",
                      "(周五) 计划", "(周五) 实际", "(周五) 达成率",
                        "(周六) 计划", "(周六) 实际", "(周六) 达成率",
                          "(周日) 计划", "(周日) 实际", "(周日) 达成率"};


            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add("周报");

                //set Title
                for (int colIndex = 0; colIndex < stringHeads.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = stringHeads[colIndex];
                }

                //set cell value
                for (int index = 0; index < list.Items.Count; index++)
                {
                    var currentRecord = list.Items[index];
                    //seq

                    worksheet.Cells[index + 2, 1].Value = currentRecord.Process;
                    worksheet.Cells[index + 2, 2].Value = currentRecord.SumPlan;
                    worksheet.Cells[index + 2, 3].Value = currentRecord.SumGoodQty;
                    worksheet.Cells[index + 2, 4].Value = currentRecord.SumYieldRate;
                    worksheet.Cells[index + 2, 5].Value = currentRecord.MondayPlan;
                    worksheet.Cells[index + 2, 6].Value = currentRecord.MondayGoodQty;
                    worksheet.Cells[index + 2, 7].Value = currentRecord.MondayYieldRate;


                    worksheet.Cells[index + 2, 8].Value = currentRecord.TuesdayPlan;
                    worksheet.Cells[index + 2, 9].Value = currentRecord.TuesdayGoodQty;
                    worksheet.Cells[index + 2, 10].Value = currentRecord.TuesdayYieldRate;

                    worksheet.Cells[index + 2, 11].Value = currentRecord.WednesdayPlan;
                    worksheet.Cells[index + 2, 12].Value = currentRecord.WednesdayGoodQty;
                    worksheet.Cells[index + 2, 13].Value = currentRecord.WednesdayYieldRate;

                    worksheet.Cells[index + 2, 14].Value = currentRecord.ThursdayPlan;
                    worksheet.Cells[index + 2, 15].Value = currentRecord.ThursdayGoodQty;
                    worksheet.Cells[index + 2, 16].Value = currentRecord.ThursdayYieldRate;

                    worksheet.Cells[index + 2, 17].Value = currentRecord.FridayPlan;
                    worksheet.Cells[index + 2, 18].Value = currentRecord.FridayGoodQty;
                    worksheet.Cells[index + 2, 19].Value = currentRecord.FridayYieldRate;


                    worksheet.Cells[index + 2, 20].Value = currentRecord.SaterdayPlan;
                    worksheet.Cells[index + 2, 21].Value = currentRecord.SaterdayGoodQty;
                    worksheet.Cells[index + 2, 22].Value = currentRecord.SaterdayYieldRate;

                    worksheet.Cells[index + 2, 23].Value = currentRecord.SundayPlan;
                    worksheet.Cells[index + 2, 24].Value = currentRecord.SundayGoodQty;
                    worksheet.Cells[index + 2, 25].Value = currentRecord.SundayYieldRate;

                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }

          
            if (submitType == "AJAX")
            {
                return Content("SUCCESS");
            }
            else
            {
                return new FileContentResult(stream.ToArray(), "application/octet-stream") { FileDownloadName = Server.UrlEncode(fileName) };
            }
        }

        public ActionResult DoExportProductReport(string jsonExportList, string submitType, string exportName)
        {
            //创建EXL文档
            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("PP检核资料数据");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new List<string> { "序号", "制程序号", "场地", "功能厂", "制程", "颜色", "负责主管", "目标良率", "总计划" };
            var stringSum = new List<string> { "汇总计划", "总领料", "总仓库领料", "总良品", "总调制试机", "总入库", "总NG", "总达成率", "总良率" };
            var stringAll = new List<string> { "滚动计划", "总领料", "总仓库领料", "总良品", "总调制试机", "总入库", "总NG", "滚动达成率", "最终良率" };
            var stringSubs = new List<string> { "计划", "领料", "仓库领料", "良品", "试制调机", "入库", "NG", "达成率", "良率", "WIP" };
            int columnIndex = 0;
            using (var excelPackage = new ExcelPackage(stream))
            {
                //定义变量
                var fail_NoData = "";
                //获取当前时段及日期
                var apiUrl = string.Format("EventReportManager/GetIntervalInfoAPI?opType={0}", "OP1");
                HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
                var intervalResult = responMessage.Content.ReadAsStringAsync().Result;
                var intervalList = JsonConvert.DeserializeObject<IntervalEnum>(intervalResult);
                var nowDate = intervalList.NowDate;
                var nowInterval = intervalList.Time_Interval;

                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add(nowDate);
                var entity = JsonConvert.DeserializeObject<ExportPPCheckData>(jsonExportList);
                worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                var finallyList = entity.TabList.Count();
                if (!entity.TabList.Any())
                {
                    return Content("导出EXL数据前，请先查询数据！");
                }
                foreach (var item in entity.TabList)
                {
                    //获取到数据源
                    var QueryData = new ReportDataSearch();
                    QueryData.Customer = entity.Customer;
                    QueryData.Color = entity.Color;
                    QueryData.Part_Types = entity.Part_Types;
                    QueryData.Interval_Time = entity.Time_InterVal;
                    QueryData.Project = entity.Project;
                    QueryData.Reference_Date = entity.Reference_Date;
                    QueryData.Tab_Select_Text = item.Time_InterVal;
                    QueryData.Product_Phase = entity.Product_Phase;
                    var page = new Page();
                    page.PageSize = 10;
                    var apiUrl1 = "";
                    apiUrl1 = string.Format("EventReportManager/QueryReportDatasAPI");
                    HttpResponseMessage responMessage1 = APIHelper.APIPostAsync(QueryData, page, apiUrl1);
                    var result1 = responMessage1.Content.ReadAsStringAsync().Result;
                    var list = JsonConvert.DeserializeObject<ExportReportDataResult>(result1);
                    if (!list.Items.Any())
                    {
                        fail_NoData = "时段：" + item.Time_InterVal + "没有数据，请重新选择查询条件！";
                        return Content(fail_NoData);
                    }
                    //创建EXL头部
                    worksheet.Cells[1, 1, 1, 9].Merge = true;
                    worksheet.Cells[1, 1].Value = "客户:" + entity.Customer + " 专案:" + entity.Project + " 部件:" + entity.Part_Types + " 日期:" + entity.Reference_Date + " 2H战情报表";
                    worksheet.Cells[2, 1, 2, 9].Merge = true;
                    worksheet.Cells[2, 1].Value = "Update:" + nowDate;
                    worksheet.Cells[3, 1, 3, 9].Merge = true;
                    #region Phase1 计算Heads内容
                    //创建标题
                    for (int i = 0; i < stringHeads.Count(); i++)
                    {
                        worksheet.Cells[4, i + 1].Value = stringHeads[i];
                    }
                    //添加值
                    for (int index = 0; index < list.Items.Count; index++)
                    {
                        var currentRecord = list.Items[index];
                        //seq                       
                        worksheet.Cells[index + 5, 1].Value = index + 1;
                        worksheet.Cells[index + 5, 2].Value = currentRecord.Process_Seq;
                        worksheet.Cells[index + 5, 3].Value = currentRecord.Place;
                        worksheet.Cells[index + 5, 4].Value = currentRecord.FunPlant;
                        worksheet.Cells[index + 5, 5].Value = currentRecord.Process;
                        worksheet.Cells[index + 5, 6].Value = currentRecord.Color;
                        worksheet.Cells[index + 5, 7].Value = currentRecord.DRI;
                        worksheet.Cells[index + 5, 8].Value = currentRecord.Target_Yield + "%";
                        worksheet.Cells[index + 5, 9].Value = currentRecord.All_Product_Plan;
                    }
                    #endregion
                    #region Phase2 如果查询条件中有白班/夜班小计，则不计算Subs,但包含WIP

                    if (item.Time_InterVal == "白班小计" || item.Time_InterVal == "夜班小计"|| item.Time_InterVal == "全天")
                    {
                        stringHeads.AddRange(stringSum);
                        stringHeads.Add("WIP");
                        var stringIndex = stringHeads.Count() - stringSum.Count() - 1;
                        //创建时段标题
                        worksheet.Cells[3, stringIndex + 1, 3, stringHeads.Count()].Merge = true;
                        worksheet.Cells[3, stringIndex + 1].Value = "时段：" + item.Time_InterVal;
                        //创建标题

                        for (int i = stringIndex; i < stringHeads.Count(); i++)
                        {
                            worksheet.Cells[4, i + 1].Value = stringHeads[i];
                        }
                        //填充数据
                        for (int index = 0; index < list.Items.Count; index++)
                        {
                            var currentRecord = list.Items[index];
                            worksheet.Cells[index + 5, stringIndex + 1].Value = currentRecord.All_Product_Plan_Sum;
                            worksheet.Cells[index + 5, stringIndex + 2].Value = currentRecord.All_Picking_QTY;
                            worksheet.Cells[index + 5, stringIndex + 3].Value = currentRecord.All_WH_Picking_QTY;
                            worksheet.Cells[index + 5, stringIndex + 4].Value = currentRecord.All_Good_QTY;
                            worksheet.Cells[index + 5, stringIndex + 5].Value = currentRecord.All_Adjust_QTY;
                            worksheet.Cells[index + 5, stringIndex + 6].Value = currentRecord.All_WH_QTY;
                            worksheet.Cells[index + 5, stringIndex + 7].Value = currentRecord.All_NG_QTY;
                            worksheet.Cells[index + 5, stringIndex + 8].Value = currentRecord.All_Rolling_Yield_Rate + "%";
                            worksheet.Cells[index + 5, stringIndex + 9].Value = currentRecord.All_Finally_Field + "%";

                            worksheet.Cells[index + 5, stringIndex + 10].Value = currentRecord.WIP_QTY;
                        }
                    }
                    #endregion
                    else
                    {
                        #region Phase3 如果查询最后1时段的数据,则需要将ALL的数据放在前面，如果不是最后1个时段的数据，则只计算Subs
                        if (item == entity.TabList[finallyList - 1])
                        {
                            stringHeads.AddRange(stringAll);
                            stringHeads.AddRange(stringSubs);
                            //创建时段标题
                            var stringIndex = stringHeads.Count() - stringAll.Count() - stringSubs.Count();
                            worksheet.Cells[3, stringIndex + 1, 3, stringHeads.Count()].Merge = true;
                            worksheet.Cells[3, stringIndex + 1].Value = "时段：" + item.Time_InterVal;
                            //创建标题
                            for (int i = stringIndex; i < stringHeads.Count(); i++)
                            {
                                worksheet.Cells[4, i + 1].Value = stringHeads[i];
                            }
                            //填充数据
                            for (int index = 0; index < list.Items.Count; index++)
                            {
                                var currentRecord = list.Items[index];
                                worksheet.Cells[index + 5, stringIndex + 1].Value = currentRecord.All_Product_Plan_Sum;
                                worksheet.Cells[index + 5, stringIndex + 2].Value = currentRecord.All_Picking_QTY;
                                worksheet.Cells[index + 5, stringIndex + 3].Value = currentRecord.All_WH_Picking_QTY;
                                worksheet.Cells[index + 5, stringIndex + 4].Value = currentRecord.All_Good_QTY;
                                worksheet.Cells[index + 5, stringIndex + 5].Value = currentRecord.All_Adjust_QTY;
                                worksheet.Cells[index + 5, stringIndex + 6].Value = currentRecord.All_WH_QTY;
                                worksheet.Cells[index + 5, stringIndex + 7].Value = currentRecord.All_NG_QTY;
                                worksheet.Cells[index + 5, stringIndex + 8].Value = currentRecord.All_Rolling_Yield_Rate + "%";
                                worksheet.Cells[index + 5, stringIndex + 9].Value = currentRecord.All_Finally_Field + "%";

                                worksheet.Cells[index + 5, stringIndex + 10].Value = currentRecord.Product_Plan;
                                worksheet.Cells[index + 5, stringIndex + 11].Value = currentRecord.Picking_QTY;
                                worksheet.Cells[index + 5, stringIndex + 12].Value = currentRecord.WH_Picking_QTY;
                                worksheet.Cells[index + 5, stringIndex + 13].Value = currentRecord.Good_QTY;
                                worksheet.Cells[index + 5, stringIndex + 14].Value = currentRecord.Adjust_QTY;
                                worksheet.Cells[index + 5, stringIndex + 15].Value = currentRecord.WH_QTY;
                                worksheet.Cells[index + 5, stringIndex + 16].Value = currentRecord.NG_QTY;
                                worksheet.Cells[index + 5, stringIndex + 17].Value = currentRecord.Rolling_Yield_Rate + "%";
                                worksheet.Cells[index + 5, stringIndex + 18].Value = currentRecord.Finally_Field + "%";
                                worksheet.Cells[index + 5, stringIndex + 19].Value = currentRecord.WIP_QTY;
                            }
                        }
                        else
                        {
                            stringHeads.AddRange(stringSubs);
                            //创建时段标题
                            var stringIndex = stringHeads.Count() - stringSubs.Count();
                            worksheet.Cells[3, stringIndex + 1, 3, stringHeads.Count()].Merge = true;
                            worksheet.Cells[3, stringIndex + 1].Value = "时段：" + item.Time_InterVal;
                            //创建标题
                            for (int i = stringIndex; i < stringHeads.Count(); i++)
                            {
                                worksheet.Cells[4, stringIndex + 1].Value = stringHeads[i];
                            }
                            //填充数据
                            for (int index = 0; index < list.Items.Count; index++)
                            {
                                var currentRecord = list.Items[index];
                                worksheet.Cells[index + 5, stringIndex + 1].Value = currentRecord.Product_Plan;
                                worksheet.Cells[index + 5, stringIndex + 2].Value = currentRecord.Picking_QTY;
                                worksheet.Cells[index + 5, stringIndex + 3].Value = currentRecord.WH_Picking_QTY;
                                worksheet.Cells[index + 5, stringIndex + 4].Value = currentRecord.Good_QTY;
                                worksheet.Cells[index + 5, stringIndex + 5].Value = currentRecord.Adjust_QTY;
                                worksheet.Cells[index + 5, stringIndex + 6].Value = currentRecord.WH_QTY;
                                worksheet.Cells[index + 5, stringIndex + 7].Value = currentRecord.NG_QTY;
                                worksheet.Cells[index + 5, stringIndex + 8].Value = currentRecord.Rolling_Yield_Rate + "%";
                                worksheet.Cells[index + 5, stringIndex + 9].Value = currentRecord.Finally_Field + "%";
                                worksheet.Cells[index + 5, stringIndex + 10].Value = currentRecord.WIP_QTY;
                            }
                        }
                        #endregion
                    }
                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }
            if (submitType == "AJAX")
            {
                return Content("SUCCESS");
            }
            else
            {
                return new FileContentResult(stream.ToArray(), "application/octet-stream") { FileDownloadName = Server.UrlEncode(fileName) };
            }

        }

        #endregion
        #region Product_Input And Product_Input_History Common Function-------Sidney 2015/12/20
        /// <summary>
        /// GetCustomerSource
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCustomerSource()
        {
            var apiUrl = "EventReportManager/GetCustomerSourceAPI";
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }
        /// <summary>
        /// GetProjectSource
        /// </summary>
        /// <param name="CustomerName"></param>
        /// <returns></returns>
        public ActionResult GetProjectSource(string CustomerName)
        {
            var apiUrl = string.Format("EventReportManager/GetProjectSourceAPI?customer={0}", CustomerName);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }
        /// <summary>
        /// GetProductPhaseSource
        /// </summary>
        /// <param name="CustomerName"></param>
        /// <param name="ProjectName"></param>
        /// <returns></returns>
        public ActionResult GetProductPhaseSource(string CustomerName, string ProjectName)
        {
            var apiUrl = string.Format("EventReportManager/GetProductPhaseSourceAPI?customer={0}&&project={1}", CustomerName, ProjectName);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }
        /// <summary>
        /// GetPartTypesSource
        /// </summary>
        /// <param name="CustomerName"></param>
        /// <param name="ProjectName"></param>
        /// <param name="ProductPhaseName"></param>
        /// <returns></returns>
        public ActionResult GetPartTypesSource(string CustomerName, string ProjectName, string ProductPhaseName)
        {
            var apiUrl = string.Format("EventReportManager/GetPartTypesSourceAPI?customer={0}&&project={1}&&productphase={2}", CustomerName, ProjectName, ProductPhaseName);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }
        /// <summary>
        /// GetColorSource
        /// </summary>
        /// <param name="CustomerName"></param>
        /// <param name="ProjectName"></param>
        /// <param name="ProductPhaseName"></param>
        /// <param name="PartTypesName"></param>
        /// <returns></returns>
        public ActionResult GetColorSource(string CustomerName, string ProjectName, string ProductPhaseName, string PartTypesName)
        {
            var apiUrl = string.Format("EventReportManager/GetColorSourceAPI?customer={0}&&project={1}&&productphase={2}&&parttypes={3}", CustomerName, ProjectName, ProductPhaseName, PartTypesName);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }
        /// <summary>
        /// GetIntervalTime
        /// </summary>
        /// <param name="nowOrAllInterval">可传入两个参数：当前时段=PPCheckData，所有时段=DataReport </param>
        /// <returns></returns>
        public ActionResult GetIntervalTime(string nowOrAllInterval)
        {
            var apiUrl = string.Format("EventReportManager/GetIntervalTimeAPI?PageName={0}", nowOrAllInterval);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }
        #endregion
        #region Day Week Month Report Function------------------------Sidney 2016/01/28
        public ActionResult GetWeekVersionSource(string CustomerName, string ProjectName, string ProductPhaseName, string PartTypesName, DateTime myDate)
        {
            DateTime beginTime; DateTime endTime;
            DayOfWeek weekDay = myDate.DayOfWeek;
            beginTime = myDate.AddDays(DayOfWeek.Monday - weekDay);
            endTime = myDate.AddDays(DayOfWeek.Sunday - weekDay + 7);
            var apiUrl = string.Format("EventReportManager/GetVersionSourceAPI?customer={0}&&project={1}&&productphase={2}&&parttypes={3}&&beginTime={4}&&endTime={5}", CustomerName, ProjectName, ProductPhaseName, PartTypesName, beginTime, endTime);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }
        public ActionResult GetVersionSource(string CustomerName, string ProjectName, string ProductPhaseName, string PartTypesName, DateTime beginTime, DateTime endTime)
        {

            var apiUrl = string.Format("EventReportManager/GetVersionSourceAPI?customer={0}&&project={1}&&productphase={2}&&parttypes={3}&&beginTime={4}&&endTime={5}", CustomerName, ProjectName, ProductPhaseName, PartTypesName, beginTime, endTime);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult GetVersionBeginEndDate(string CustomerName, string ProjectName, string ProductPhaseName, string PartTypesName, string Version, DateTime startDay, DateTime endDay)
        {
            int InteverVersion = 0;
            bool trys = int.TryParse(Version, out InteverVersion);
            var apiUrl = string.Format("EventReportManager/GetVersionBeginEndDateAPI?customer={0}&&project={1}&&productphase={2}&&parttypes={3}&&version={4}&&startDay={5}&&endDay={6}", CustomerName, ProjectName, ProductPhaseName, PartTypesName, InteverVersion, startDay, endDay);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        #endregion
        public ActionResult GetDateTime(string mydate)
        {
            try
            {
                DateTime myDate = DateTime.Parse(mydate);

                string startDate = string.Empty;
                string endDate = string.Empty;

                DayOfWeek weekDay = myDate.DayOfWeek;

                if (weekDay.ToString() == "Sunday")
                {
                    endDate = myDate.AddDays(DayOfWeek.Sunday - weekDay).ToShortDateString();
                    startDate = myDate.AddDays(DayOfWeek.Sunday - weekDay - 6).ToShortDateString();

                }
                else
                {
                    startDate = myDate.AddDays(DayOfWeek.Monday - weekDay).ToShortDateString();
                    endDate = myDate.AddDays(DayOfWeek.Sunday - weekDay + 7).ToShortDateString();
                }


                string result = "从 " + startDate + " 到 " + endDate;
                return Content(result);
            }
            catch
            {
                return null;
            }

        }

        public ActionResult QueryTimeSpanReport(ReportDataSearch search, Page page)
        {
            var apiUrl = string.Format("ProductInput/QueryTimeSpanReport");
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }
        public ActionResult QueryWeekReport(ReportDataSearch search, Page page)
        {
            var apiUrl = string.Format("ProductInput/QueryWeekReport");
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        #region  导出周 月 时间段报表----------------------Justin 2016/02/19

        #endregion
    }
}