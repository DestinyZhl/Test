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
using System.Net;

namespace SPP.Web.Controllers
{
    public class SettingsController : WebControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        #region User Maintenance-------------------Add By Tonny 2015/11/02
        public ActionResult UserMaintenance()
        {
            return View();
        }

        /// <summary>
        /// get paged records of users by query conditions
        /// </summary>
        /// <param name="search">query conditions</param>
        /// <param name="page">page info, auto fill by front-end</param>
        /// <returns>json of paged records</returns>
        public ActionResult QueryUsers(UserModelSearch search, Page page)
        {
            var apiUrl = string.Format("Settings/QueryUsersAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        /// <summary>
        /// get user by account id
        /// </summary>
        /// <param name="uuid">account id</param>
        /// <returns>single user json</returns>
        public ActionResult QueryUser(int uuid)
        {
            var apiUrl = string.Format("Settings/QueryUserAPI?uuid={0}", uuid);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        /// <summary>
        /// delete specific user
        /// </summary>
        /// <param name="dto">entity, or just fill the primary key account id</param>
        /// <returns></returns>
        public ActionResult DeleteUser(SystemUserDTO dto)
        {
            var apiUrl = string.Format("Settings/DeleteUserAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(dto, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        /// <summary>
        /// edit or add new user
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="isEdit">flag, true edit/false add</param>
        /// <returns></returns>
        public ActionResult EditProfile(SystemUserDTO dto, bool isEdit)
        {
            //add new
            var apiUrl = string.Format("Settings/AddUserAPI");
            dto.Modified_UID = this.CurrentUser.AccountUId;
            if (isEdit == true)
            {
                //modify
                apiUrl = string.Format("Settings/ModifyUserAPI");
            }
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(dto, apiUrl);

            return RedirectToAction("UserMaintenance", "Settings");
        }

        /// call by validate 根据用户Account UID判定用户是否存在
        /// </summary>
        /// <param name="Account_UID"></param>
        /// <returns>jsonstring : true [not exist]/ false [exist]</returns>
        public ActionResult SystemUserWithUIdNotExist(int Account_UID)
        {
            var apiUrl = string.Format("Common/GetSystemUserByUId/{0}", Account_UID);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.StatusCode == HttpStatusCode.NotFound ? "true" : "false";

            return Content(result, "application/json");
        }

        /// <summary>
        /// call by validate 根据用户NTID判定用户是否存在
        /// </summary>
        /// <param name="User_NTID"></param>
        /// <returns>jsonstring : true/false</returns>
        public ActionResult SystemUserWithNTIdNotExist(string User_NTID)
        {
            var apiUrl = string.Format("Common/GetSystemUserByNTId/?ntid={0}", User_NTID);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.StatusCode == HttpStatusCode.NotFound ? "true" : "false";

            return Content(result, "application/json");
        }

        /// <summary>
        /// Export data selected in grid
        /// </summary>
        /// <param name="uuids">selected keys</param>
        /// <returns></returns>
        public ActionResult DoExportUser(string uuids)
        {
            //get Export datas
            var apiUrl = string.Format("Settings/DoExportUserAPI?uuids={0}", uuids);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<List<SystemUserDTO>>(result);

            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("SystemUsers");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new string[] { "Seq", "Account UID", "User NTID", "User Name", "Enable", "User Email", "User Tel", "Modified User", "Modified Date" };

            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add("System Users");

                //set Title
                for (int colIndex = 0; colIndex < stringHeads.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = stringHeads[colIndex];
                }

                //set cell value
                for (int index = 0; index < list.Count; index++)
                {
                    var currentRecord = list[index];
                    //seq
                    worksheet.Cells[index + 2, 1].Value = index + 1;
                    worksheet.Cells[index + 2, 2].Value = currentRecord.Account_UID;
                    worksheet.Cells[index + 2, 3].Value = currentRecord.User_NTID;
                    worksheet.Cells[index + 2, 4].Value = currentRecord.User_Name;
                    worksheet.Cells[index + 2, 5].Value = currentRecord.Enable_Flag ? "Y" : "N";
                    worksheet.Cells[index + 2, 6].Value = currentRecord.Email;
                    worksheet.Cells[index + 2, 7].Value = currentRecord.Tel;
                    worksheet.Cells[index + 2, 8].Value = currentRecord.Modified_UserNTID;
                    worksheet.Cells[index + 2, 9].Value = currentRecord.Modified_Date.ToString(FormatConstants.DateTimeFormatString);
                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }

            return new FileContentResult(stream.ToArray(), "application/octet-stream")
            { FileDownloadName = Server.UrlEncode(fileName) };
        }

        #endregion //User Maintenance

        #region Plant Maintenance -------------------Add By Sidney 2015/11/09
        public ActionResult PlantMaintenance()
        {
            return View();
        }
        /// <summary>
        /// get paged records of users by query conditions
        /// </summary>
        /// <param name="search">query conditions</param>
        /// <param name="page">page info, auto fill by front-end</param>
        /// <returns>json of paged records</returns>
        public ActionResult QueryPlants(PlantModelSearch search, Page page)
        {
            var apiUrl = string.Format("Settings/QueryPlantsAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        /// <summary>
        /// get user by account id
        /// </summary>
        /// <param name="uuid">account id</param>
        /// <returns>single user json</returns>
        public ActionResult QueryPlant(int uuid)
        {
            var apiUrl = string.Format("Settings/QueryPlantAPI?uuid={0}", uuid);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        /// <summary>
        /// delete specific user
        /// </summary>
        /// <param name="dto">entity, or just fill the primary key account id</param>
        /// <returns></returns>
        public ActionResult DeletePlant(SystemPlantDTO dto)
        {
            var apiUrl = string.Format("Settings/DeletePlantAPI");
            //MVC调用WebAPI请使用SPP.Core.APIHelper中的方法。此方法内对授权和异常进行了处
            //理。如有特殊复杂需求，可自行封装。
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(dto, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }
        public ActionResult DoExportPlant(string uuids)
        {
            //get Export datas
            var apiUrl = string.Format("Settings/DoExportPlantAPI?uuids={0}", uuids);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<List<SystemPlantDTO>>(result);

            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("Plant Maintenance");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new string[] { "Seq", "Plant", "Location", "Type", "Plant Code", "Plant Name(ZH)", "PlantManager Name", "PlantManager Tel", "Begin Date", "End Date", "Modified User", "Modified Date" };

            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add("Plant Maintenance");

                //set Title
                for (int colIndex = 0; colIndex < stringHeads.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = stringHeads[colIndex];
                }

                //set cell value
                for (int index = 0; index < list.Count; index++)
                {
                    var currentRecord = list[index];
                    //seq
                    worksheet.Cells[index + 2, 1].Value = index + 1;
                    worksheet.Cells[index + 2, 2].Value = currentRecord.Plant;
                    worksheet.Cells[index + 2, 3].Value = currentRecord.Location;
                    worksheet.Cells[index + 2, 4].Value = currentRecord.Type;
                    worksheet.Cells[index + 2, 5].Value = currentRecord.Name_0;
                    worksheet.Cells[index + 2, 6].Value = currentRecord.Name_1;
                    worksheet.Cells[index + 2, 7].Value = currentRecord.PlantManager_Name;
                    worksheet.Cells[index + 2, 8].Value = currentRecord.PlantManager_Tel;
                    worksheet.Cells[index + 2, 9].Value = currentRecord.Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate);
                    worksheet.Cells[index + 2, 10].Value = currentRecord.End_Date == null ? null : ((DateTime)currentRecord.End_Date).ToString(FormatConstants.DateTimeFormatStringByDate);
                    worksheet.Cells[index + 2, 11].Value = currentRecord.ModifiedUser.User_Name;
                    worksheet.Cells[index + 2, 12].Value = currentRecord.Modified_Date.ToString(FormatConstants.DateTimeFormatString);
                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }

            return new FileContentResult(stream.ToArray(), "application/octet-stream")
            { FileDownloadName = Server.UrlEncode(fileName) };
        }

        public ActionResult AddPlant(string jsonAddPlant)
        {

            var apiUrl = "Settings/AddPlantAPI";
            var entity = JsonConvert.DeserializeObject<SystemPlantDTO>(jsonAddPlant);
            entity.Modified_UID = this.CurrentUser.AccountUId;

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult EditPlant(string jsonEditPlant)
        {

            var apiUrl = "Settings/ModifyPlantAPI";
            var entity = JsonConvert.DeserializeObject<SystemPlantDTO>(jsonEditPlant);
            entity.Modified_UID = this.CurrentUser.AccountUId;

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        /// <summary>Plant
        /// edit or add new 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="isEdit">flag, true edit/false add</param>
        /// <returns></returns>
        public ActionResult EditPlantProfile(SystemPlantDTO dto, bool isEdit)
        {
            //add new
            var apiUrl = string.Format("Settings/AddPlantAPI");
            dto.Modified_UID = this.CurrentUser.AccountUId;
            if (isEdit == true)
            {
                //modify
                apiUrl = string.Format("Settings/ModifyPlantAPI");
            }
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(dto, apiUrl);
            //return Content("<script >alert('提交留言成功，谢谢对我们支持，我们会根据您提供联系方式尽快与您取的联系！');</script >", "text/html");
            return RedirectToAction("PlantMaintenance", "Settings");
        }

        /// <summary>
        /// call by validate, check user is exist or not
        /// </summary>
        /// <param name="Account_UID">account id</param>
        /// <returns>json, true/false</returns>
        public ActionResult CheckPlantExistByUId(int System_Plant_UID)
        {
            var apiUrl = string.Format("Settings/CheckPlantExistByUId/?uuid={0}", System_Plant_UID);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            return Content(responMessage.Content.ReadAsStringAsync().Result, "application/json");
        }

        /// <summary>
        /// call by validate, check user is exist or not
        /// </summary>
        /// <param name="Account_UID">account id</param>
        /// <returns>json, true/false</returns>
        public ActionResult CheckPlantExistByPlant(string Plant)
        {
            var apiUrl = string.Format("Settings/CheckPlantExistByPlant/?Plant={0}", Plant);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            return Content(responMessage.Content.ReadAsStringAsync().Result, "application/json");
        }

        public ActionResult GetMaxEnddate4Plant(int uid)
        {
            var apiUrl = string.Format("Settings/GetMaxEnddate4Plant/{0}", uid.ToString());
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            return Content(responMessage.Content.ReadAsStringAsync().Result, "application/json");
        }
        #endregion //Plant Maintenance 

        #region BU Master Maintenance-------------------Add By Rock 2015/11/09
        //SETTINGS/QUERYBUINFOAPI
        public ActionResult BUMasterMaintenance()
        {
            return View();
        }

        /// <summary>
        /// 页面初始化加载
        /// </summary>
        /// <param name="search">query conditions</param>
        /// <param name="page">page info, auto fill by front-end</param>
        /// <returns>json of paged records</returns>
        public ActionResult QueryBUs(BUModelSearch search, Page page)
        {
            var apiUrl = string.Format("Settings/QueryBUsAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        /// <summary>
        /// 检查BU ID是否存在
        /// </summary>
        /// <param name="BU_ID"></param>
        /// <returns></returns>
        public ActionResult CheckBuExistById(string BU_ID)
        {
            var apiUrl = string.Format("Settings/CheckBuExistById/?buid={0}", BU_ID);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            return Content(responMessage.Content.ReadAsStringAsync().Result, "application/json");
        }

        /// <summary>
        /// 检查BU Name是否存在
        /// </summary>
        /// <param name="BU_Name"></param>
        /// <returns></returns>
        //public ActionResult CheckBuExistByName(string BU_Name)
        //{
        //    var apiUrl = string.Format("Settings/CheckBuExistByName/?buname={0}", BU_Name);
        //    HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

        //    return Content(responMessage.Content.ReadAsStringAsync().Result, "application/json");
        //}

        /// <summary>
        /// 新增或修改BU信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public ActionResult AddOrEditBUInfo(SystemBUMDTO dto, bool isEdit)
        {
            //add new
            var apiUrl = string.Format("Settings/AddBUAPI");
            dto.Modified_UID = this.CurrentUser.AccountUId;
            if (isEdit == true)
            {
                //modify
                apiUrl = string.Format("Settings/ModifyBUAPI");
            }
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(dto, apiUrl);

            return RedirectToAction("BUMasterMaintenance", "Settings");
        }

        /// <summary>
        /// 通过BU_M_UID获取BU信息
        /// </summary>
        /// <param name="BU_M_UID"></param>
        /// <returns></returns>
        public ActionResult QueryBU(int BU_M_UID)
        {
            var apiUrl = string.Format("Settings/QueryBUAPI?BU_M_UID={0}", BU_M_UID);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult DeleteBU(SystemBUMDTO dto)
        {
            var apiUrl = string.Format("Settings/DeleteBUAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(dto, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult DoExportBU(string uuids)
        {
            //get Export datas
            var apiUrl = string.Format("Settings/DoExportBUAPI?uuids={0}", uuids);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<List<BUModelGet>>(result);

            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("BU Master Maintenance");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new string[] { "Seq", "BU ID", "BU Name", "BUManager Name", "BUManager Tel", "BUManager Email", "Begin Date", "End Date", "Modified User", "Modified Date" };

            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add("BU Master Maintenance");

                //set Title
                for (int colIndex = 0; colIndex < stringHeads.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = stringHeads[colIndex];
                }

                //set cell value
                for (int index = 0; index < list.Count; index++)
                {
                    var currentRecord = list[index];
                    //seq
                    worksheet.Cells[index + 2, 1].Value = index + 1;
                    worksheet.Cells[index + 2, 2].Value = currentRecord.SystemBUMDTO.BU_ID;
                    worksheet.Cells[index + 2, 3].Value = currentRecord.SystemBUMDTO.BU_Name;
                    worksheet.Cells[index + 2, 4].Value = currentRecord.SystemBUMDTO.BUManager_Name;
                    worksheet.Cells[index + 2, 5].Value = currentRecord.SystemBUMDTO.BUManager_Tel;
                    worksheet.Cells[index + 2, 6].Value = currentRecord.SystemBUMDTO.BUManager_Email;
                    worksheet.Cells[index + 2, 7].Value = currentRecord.SystemBUMDTO.Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate);
                    worksheet.Cells[index + 2, 8].Value = currentRecord.SystemBUMDTO.End_Date != null ? currentRecord.SystemBUMDTO.End_Date.Value.ToString(FormatConstants.DateTimeFormatStringByDate) : string.Empty;
                    worksheet.Cells[index + 2, 9].Value = currentRecord.SystemUserDTO.User_Name;
                    worksheet.Cells[index + 2, 10].Value = currentRecord.SystemBUMDTO.Modified_Date.ToString(FormatConstants.DateTimeFormatString);
                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }
            return new FileContentResult(stream.ToArray(), "application/octet-stream")
            { FileDownloadName = Server.UrlEncode(fileName) };
        }

        #endregion //BU Master Maintenance

        #region BU Detail Maintenance ----------------------------Add by Rock 2015/1109------------
        public ActionResult BUDetailMaintenance()
        {
            return View();
        }

        public ActionResult QueryBUDetails(BUDetailModelSearch search, Page page)
        {
            var apiUrl = string.Format("Settings/QueryBUDetailsAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult CheckBuExistById_Two(string BU_ID)
        {
            var apiUrl = string.Format("Settings/CheckBuExistById_TwoAPI/?buid={0}", BU_ID);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            return Content(responMessage.Content.ReadAsStringAsync().Result, "application/json");
        }

        public ActionResult CheckBeginDateAndEndDate(string BU_ID, string BU_Name, string Begin_Date, string End_Date, bool isEdit)
        {
            var apiUrl = string.Format("Settings/CheckBeginDateAndEndDateAPI/?BU_ID={0}&BU_Name={1}&Begin_Date={2}&End_Date={3}", BU_ID, BU_Name, Begin_Date, End_Date);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult QueryBUDInfoByBU_D_ID(string BU_D_ID)
        {
            var apiUrl = string.Format("Settings/QueryBUDInfoByBU_D_IDAPI/?BU_D_ID={0}", BU_D_ID);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult GetBUIDAndBUNameByBUID(string BU_ID)
        {
            var apiUrl = string.Format("Settings/GetBUIDAndBUNameByBUIDAPI/?BU_ID={0}", BU_ID);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            if (result == "null")
            {
                SystemBUMDTO dto = new SystemBUMDTO();
                result = JsonConvert.SerializeObject(dto);
                //result = Json(dto).ToString();
            }
            return Content(result, "application/json");
        }

        //public ActionResult CheckExistBU_D_ID(string BU_D_ID)
        //{
        //    var apiUrl = string.Format("Settings/CheckExistBU_D_IDAPI/?BU_D_ID={0}&BU_D_UID={1}&isEdit={2}", BU_D_ID,BU_D_UID,isEdit);
        //    HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
        //    var result = responMessage.Content.ReadAsStringAsync().Result;
        //    return Content(result, "application/json");
        //}

        public ActionResult AddOrEditBUDetailInfo(SystemBUDDTO dto, bool isEdit)
        {
            var apiUrl = string.Empty;
            string BU_ID = Request.Form["BU_ID"];
            string BU_Name = Request.Form["BU_Name"];

            if (isEdit)
            {
                apiUrl = string.Format("Settings/EditBUDetailInfoAPI?BU_ID={0}&BU_Name={1}&isEdit={2}", BU_ID, BU_Name, isEdit);
            }
            else
            {
                apiUrl = string.Format("Settings/AddBUDetailInfoAPI?BU_ID={0}&BU_Name={1}&isEdit={2}", BU_ID, BU_Name, isEdit);
            }
            dto.Modified_UID = this.CurrentUser.AccountUId;

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(dto, apiUrl);

            return RedirectToAction("BUDetailMaintenance", "Settings");
        }

        public ActionResult QueryBUDetail(int BU_D_UID)
        {
            var apiUrl = string.Format("Settings/QueryBUDetailAPI?BU_D_UID={0}", BU_D_UID);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult DeleteBUDDetail(int BU_D_UID)
        {
            var apiUrl = string.Format("Settings/DeleteBUDDetailAPI?BU_D_UID={0}", BU_D_UID);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult DoExportBUDetail(string uuids)
        {
            //get Export datas
            var apiUrl = string.Format("Settings/DoExportBUDetailAPI?BU_D_UIDS={0}", uuids);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<List<BUDetailModelGet>>(result);

            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("BU Detail Maintenance");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new string[] { "Seq", "BU ID", "BU Name", "BU Customer ID", "BU Customer Name", "Begin Date", "End Date", "Modified User", "Modified Date" };

            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add("BU Detail Maintenance");

                //set Title
                for (int colIndex = 0; colIndex < stringHeads.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = stringHeads[colIndex];
                }

                //set cell value
                for (int index = 0; index < list.Count; index++)
                {
                    var currentRecord = list[index];
                    //seq
                    worksheet.Cells[index + 2, 1].Value = index + 1;
                    //BU ID
                    worksheet.Cells[index + 2, 2].Value = currentRecord.SystemBUMDTO.BU_ID;
                    //BU Name
                    worksheet.Cells[index + 2, 3].Value = currentRecord.SystemBUMDTO.BU_Name;
                    //BU Customer ID
                    worksheet.Cells[index + 2, 4].Value = currentRecord.SystemBUDDTO.BU_D_ID;
                    //BU Customer Name
                    worksheet.Cells[index + 2, 5].Value = currentRecord.SystemBUDDTO.BU_D_Name;
                    //Begin Date
                    worksheet.Cells[index + 2, 6].Value = currentRecord.SystemBUDDTO.Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate);
                    worksheet.Cells[index + 2, 7].Value = currentRecord.SystemBUDDTO.End_Date != null ? currentRecord.SystemBUDDTO.End_Date.Value.ToString(FormatConstants.DateTimeFormatStringByDate) : string.Empty;
                    worksheet.Cells[index + 2, 8].Value = currentRecord.SystemUserDTO.User_Name;
                    worksheet.Cells[index + 2, 9].Value = currentRecord.SystemBUDDTO.Modified_Date.ToString(FormatConstants.DateTimeFormatString);
                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }
            return new FileContentResult(stream.ToArray(), "application/octet-stream")
            { FileDownloadName = Server.UrlEncode(fileName) };
        }

        #endregion

        #region User BU Setting ------------------Start--------------Add by Rock 2015/11/18---------
        public ActionResult UserBUSetting()
        {
            return View();
        }

        public ActionResult QueryUserBUs(UserBUSettingSearch search, Page page)
        {
            var apiUrl = string.Format("Settings/QueryUserBUsAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");

        }

        public ActionResult QueryUserBU(int System_User_BU_UID)
        {
            var apiUrl = string.Format("Settings/QueryUserBUAPI?System_User_BU_UID={0}", System_User_BU_UID);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult QueryBUAndBUDSByBUID(string BU_ID)
        {
            var apiUrl = string.Format("Settings/QueryBUAndBUDSByBUIDAPI?BU_ID={0}", BU_ID);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult AddOrEditUserBU(string jsonUserBUWithSubs, bool isEdit)
        {
            var apiUrl = string.Empty;
            if (isEdit)
            {
                apiUrl = string.Format("Settings/EditUserBUAPI");
            }
            else
            {
                apiUrl = string.Format("Settings/AddUserBUAPI");
            }
            var entity = JsonConvert.DeserializeObject<UserBUAddOrSave>(jsonUserBUWithSubs);
            entity.Modified_UID = this.CurrentUser.AccountUId;

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult ExportUserBU(string uids)
        {
            //get Export datas
            var apiUrl = string.Format("Settings/DoExportUserBUAPI?KeyIDS={0}", uids);
            var responMessage = APIHelper.APIGetAsync(apiUrl);

            var result = responMessage.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<List<UserBUSettingGet>>(result);

            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("User BU Setting");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new string[] { "Seq", "User NTID", "User Name", "BU ID", "BU Name", "BU Customer ID", "BU Customer Name", "Begin Date", "End Date", "Modified User", "Modified Date" };

            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add("User BU Setting");

                //set Title
                for (int colIndex = 0; colIndex < stringHeads.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = stringHeads[colIndex];
                }

                //set cell value
                for (int index = 0; index < list.Count; index++)
                {
                    var currentRecord = list[index];
                    //seq
                    worksheet.Cells[index + 2, 1].Value = index + 1;
                    //User NTID
                    worksheet.Cells[index + 2, 2].Value = currentRecord.SystemUserDTO.Account_UID;
                    //User Name
                    worksheet.Cells[index + 2, 3].Value = index + 1;
                    //BU ID
                    worksheet.Cells[index + 2, 4].Value = currentRecord.SystemBUMDTO.BU_ID;
                    //BU Name
                    worksheet.Cells[index + 2, 5].Value = currentRecord.SystemBUMDTO.BU_Name;
                    //BU Customer ID
                    worksheet.Cells[index + 2, 6].Value = currentRecord.SystemBUDDTO != null ? currentRecord.SystemBUDDTO.BU_D_ID : string.Empty;
                    //BU Customer Name
                    worksheet.Cells[index + 2, 7].Value = currentRecord.SystemBUDDTO != null ? currentRecord.SystemBUDDTO.BU_D_Name : string.Empty;
                    //Begin Date
                    worksheet.Cells[index + 2, 8].Value = currentRecord.SystemUserBusinessGroupDTO.Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate);
                    //End Date
                    worksheet.Cells[index + 2, 9].Value = currentRecord.SystemUserBusinessGroupDTO.End_Date != null ? currentRecord.SystemUserBusinessGroupDTO.End_Date.Value.ToString(FormatConstants.DateTimeFormatStringByDate) : string.Empty;
                    //Modified User
                    worksheet.Cells[index + 2, 10].Value = currentRecord.SystemUserDTO1.User_Name;
                    //Modified Time
                    worksheet.Cells[index + 2, 11].Value = currentRecord.SystemUserBusinessGroupDTO.Modified_Date.ToString(FormatConstants.DateTimeFormatString);
                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }
            return new FileContentResult(stream.ToArray(), "application/octet-stream")
            { FileDownloadName = Server.UrlEncode(fileName) };
        }

        #endregion User BU Setting ------------------End--------------Add by Rock 2015/11/18---------

        #region Role Maintenance------------------Add By Allen 2015/11/9

        public ActionResult RoleMaintenance()
        {
            return View();
        }

        /// <summary>
        /// get paged records of users by query conditions
        /// </summary>
        /// <param name="search">query conditions</param>
        /// <param name="page">page info, auto fill by front-end</param>
        /// <returns>json of paged records</returns>
        public ActionResult QueryRoles(RoleModelSearch search, Page page)
        {
            var apiUrl = string.Format("Settings/QueryRolesAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        /// <summary>
        /// get user by account id
        /// </summary>
        /// <param name="uuid">account id</param>
        /// <returns>single user json</returns>
        public ActionResult QueryRole(int uuid)
        {
            var apiUrl = string.Format("Settings/QueryRoleAPI?uuid={0}", uuid);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        /// <summary>
        /// delete specific user
        /// </summary>
        /// <param name="dto">entity, or just fill the primary key account id</param>
        /// <returns></returns>
        public ActionResult DeleteRole(SystemRoleDTO dto)
        {
            var apiUrl = string.Format("Settings/DeleteRoleAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(dto, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        /// <summary>
        /// edit or add new user
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="isEdit">flag, true edit/false add</param>
        /// <returns></returns>
        public ActionResult Editfile(SystemRoleDTO dto, bool isEdit)
        {
            //add new
            var apiUrl = string.Format("Settings/AddRoleAPI");
            dto.Modified_UID = this.CurrentUser.AccountUId;
            if (isEdit == true)
            {
                //modify
                apiUrl = string.Format("Settings/ModifyRoleAPI");
            }
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(dto, apiUrl);

            return RedirectToAction("RoleMaintenance", "Settings");
        }

        /// <summary>
        /// call by validate, check user is exist or not
        /// </summary>
        /// <param name="Account_UID">account id</param>
        /// <returns>json, true/false</returns>
        public ActionResult CheckRoleExistById(string Role_ID)
        {
            var apiUrl = string.Format("Settings/CheckRoleExistById/?rid={0}", Role_ID);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            return Content(responMessage.Content.ReadAsStringAsync().Result, "application/json");
        }

        /// <summary>
        /// Export data selected in grid
        /// </summary>
        /// <param name="uuids">selected keys</param>
        /// <returns></returns>
        public ActionResult DoExportRole(string ruids)
        {
            //get Export datas
            var apiUrl = string.Format("Settings/DoExportRoleAPI?ruids={0}", ruids);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<List<SystemRoleDTO>>(result);

            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("SystemRole");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new string[] { "Seq", "Role ID", "Role Name", "Modified User", "Modified Date" };

            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add("System Role");

                //set Title
                for (int colIndex = 0; colIndex < stringHeads.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = stringHeads[colIndex];
                }

                //set cell value
                for (int index = 0; index < list.Count; index++)
                {
                    var currentRecord = list[index];
                    //seq
                    worksheet.Cells[index + 2, 1].Value = index + 1;
                    worksheet.Cells[index + 2, 2].Value = currentRecord.Role_ID;
                    worksheet.Cells[index + 2, 3].Value = currentRecord.Role_Name;
                    worksheet.Cells[index + 2, 4].Value = currentRecord.ModifiedUser.User_Name;
                    worksheet.Cells[index + 2, 5].Value = currentRecord.Modified_Date.ToString(FormatConstants.DateTimeFormatString);
                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }

            return new FileContentResult(stream.ToArray(), "application/octet-stream")
            { FileDownloadName = Server.UrlEncode(fileName) };
        }

        #endregion //Role Maintenance

        #region System Function Maintenance Add By Tonny 2015/11/12

        public ActionResult SystemFunctionMaintenance()
        {
            return View();
        }

        /// <summary>
        /// get paged records of functions by query conditions
        /// </summary>
        /// <param name="search">query conditions</param>
        /// <param name="page">page info, auto fill by front-end</param>
        /// <returns>json of paged records</returns>
        public ActionResult QueryFunctions(FunctionModelSearch search, Page page)
        {
            var apiUrl = "Settings/QueryFunctionsAPI";

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult QueryFunction(int uid)
        {
            var apiUrl = string.Format("Settings/QueryFunctionWithSubsAPI?uid={0}", uid);

            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult AddFunction(string jsonFunctionWithSubs)
        {

            var apiUrl = "Settings/AddFunctionWithSubsAPI";
            var entity = JsonConvert.DeserializeObject<FunctionWithSubs>(jsonFunctionWithSubs);
            entity.Modified_UID = this.CurrentUser.AccountUId;

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult ModifyFunctionWithSubs(string jsonFunctionWithSubs)
        {

            var apiUrl = "Settings/ModifyFunctionWithSubsAPI";
            var entity = JsonConvert.DeserializeObject<FunctionWithSubs>(jsonFunctionWithSubs);
            entity.Modified_UID = this.CurrentUser.AccountUId;

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        [HttpGet]
        public ActionResult GetFunctionByID(string functionId)
        {
            var apiUrl = string.Format("Settings/GetFunctionByIDAPI?functionId={0}", functionId);

            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult DeleteFunction(int uid)
        {
            var apiUrl = string.Format("Settings/DeleteFunctionAPI?uid={0}", uid);

            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult DeleteSubFunction(int subfunction_UId)
        {
            var apiUrl = string.Format("Settings/DeleteSubFunctionAPI?subfunction_UId={0}", subfunction_UId);

            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult DoExportFunction(string uids)
        {
            //get Export datas
            var apiUrl = string.Format("Settings/DoExportFunctionAPI?uids={0}", uids);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<IList<FunctionItem>>(result);

            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("SystemFunction");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new string[] { "Seq", "Parent Function ID", "Parent Function Name", "Function ID"
                , "Function Name", "Order Index", "Is Show","URL","Sub Function", "Modified User", "Modified Date" };

            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add("System Function");

                //set Title
                for (int colIndex = 0; colIndex < stringHeads.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = stringHeads[colIndex];
                }

                //set cell value
                for (int index = 0; index < list.Count; index++)
                {
                    var currentRecord = list[index];
                    //seq
                    worksheet.Cells[index + 2, 1].Value = index + 1;
                    worksheet.Cells[index + 2, 2].Value = currentRecord.Parent_Function_ID;
                    worksheet.Cells[index + 2, 3].Value = currentRecord.Parent_Function_Name;
                    worksheet.Cells[index + 2, 4].Value = currentRecord.Function_ID;
                    worksheet.Cells[index + 2, 5].Value = currentRecord.Function_Name;
                    worksheet.Cells[index + 2, 6].Value = currentRecord.Order_Index;
                    worksheet.Cells[index + 2, 7].Value = currentRecord.Is_Show ? "Y" : "N";
                    worksheet.Cells[index + 2, 8].Value = currentRecord.URL;
                    worksheet.Cells[index + 2, 9].Value = currentRecord.SubFunctionCount;
                    worksheet.Cells[index + 2, 10].Value = currentRecord.Modified_UserName;
                    worksheet.Cells[index + 2, 11].Value = currentRecord.Modified_Date.ToString(FormatConstants.DateTimeFormatString);
                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }

            return new FileContentResult(stream.ToArray(), "application/octet-stream")
            { FileDownloadName = Server.UrlEncode(fileName) };
        }
        #endregion //end System Function Maintenance

        #region User Plant Setting Add By Sidney 2015/11/17

        public ActionResult UserPlantSetting()
        {
            return View();
        }
        #region Add

        [HttpGet]
        public ActionResult GetPlantInfoByPlant(string Plant)
        {
            var apiUrl = string.Format("Settings/GetPlantInfoAPI?Plant={0}", Plant);

            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }
        public ActionResult AddUserPlant(string jsonUserWithPlant)
        {

            var apiUrl = "Settings/AddUserPlantAPI";
            var entity = JsonConvert.DeserializeObject<UserPlantEditModel>(jsonUserWithPlant);
            entity.Modified_UID = this.CurrentUser.AccountUId;

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }
        #endregion
        #region query
        public ActionResult QueryUserPlants(UserPlantModelSearch search, Page page)
        {
            var apiUrl = "Settings/QueryUserPlantsAPI";

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }
        public ActionResult QueryUserPlantByAccountUID(int uuid)
        {
            var apiUrl = string.Format("Settings/QueryUserPlantByAccountUID?uuid={0}", uuid);

            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }
        #endregion
        #region modify
        public ActionResult modifyUserPlant(string jsonUserWithPlant)
        {

            var apiUrl = "Settings/ModifyUserPlantAPI";
            var entity = JsonConvert.DeserializeObject<UserPlantEditModel>(jsonUserWithPlant);
            entity.Modified_UID = this.CurrentUser.AccountUId;

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }


        #endregion
        public ActionResult DoExportUserPlant(string uids)
        {
            //get Export datas
            var apiUrl = string.Format("Settings/DoExportUserPlantAPI?uids={0}", uids);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<IList<UserPlantItem>>(result);

            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("UserPlantSetting");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new string[] { "Seq", "User NTID", "User Name", "Plant"
                , "Location", "Type", "Plant Code","Begin Date","End Date", "Modified User", "Modified Date" };

            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add("UserPlantSetting");

                //set Title
                for (int colIndex = 0; colIndex < stringHeads.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = stringHeads[colIndex];
                }

                //set cell value
                for (int index = 0; index < list.Count; index++)
                {
                    var currentRecord = list[index];
                    //seq
                    worksheet.Cells[index + 2, 1].Value = index + 1;
                    worksheet.Cells[index + 2, 2].Value = currentRecord.User_NTID;
                    worksheet.Cells[index + 2, 3].Value = currentRecord.User_Name;
                    worksheet.Cells[index + 2, 4].Value = currentRecord.Plant;
                    worksheet.Cells[index + 2, 5].Value = currentRecord.Location;
                    worksheet.Cells[index + 2, 6].Value = currentRecord.Type;
                    worksheet.Cells[index + 2, 7].Value = currentRecord.Plant_Code;
                    worksheet.Cells[index + 2, 8].Value = currentRecord.Begin_Date == null ? null : ((DateTime)currentRecord.Begin_Date).ToString(FormatConstants.DateTimeFormatStringByDate);
                    worksheet.Cells[index + 2, 9].Value = currentRecord.End_Date == null ? null : ((DateTime)currentRecord.End_Date).ToString(FormatConstants.DateTimeFormatStringByDate);
                    worksheet.Cells[index + 2, 10].Value = currentRecord.Modified_UserName;
                    worksheet.Cells[index + 2, 11].Value = currentRecord.Modified_Date.ToString(FormatConstants.DateTimeFormatString);
                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }

            return new FileContentResult(stream.ToArray(), "application/octet-stream")
            { FileDownloadName = Server.UrlEncode(fileName) };
        }
        #endregion

        #region Role Function Setting Module -------------- Add by Tonny 2015/11/18

        public ActionResult RoleFunctionSetting()
        {
            var apiUrl = "Common/GetAllRoles";
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            var rolesModel = JsonConvert.DeserializeObject<IEnumerable<SystemRoleDTO>>(result);
            return View(rolesModel);
        }

        /// <summary>
        /// get paged records of role functions by query conditions
        /// </summary>
        /// <param name="search">query conditions</param>
        /// <param name="page">page info, auto fill by front-end</param>
        /// <returns>json of paged records</returns>
        public ActionResult QueryRoleFunctions(RoleFunctionSearchModel search, Page page)
        {
            var apiUrl = "Settings/QueryRoleFunctionsAPI";

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        [HttpGet]
        public ActionResult QueryRoleFunction(int roleUId)
        {
            var apiUrl = "Settings/QueryRoleFunctionAPI/" + roleUId.ToString();

            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        [HttpGet]
        public ActionResult QueryRoleSubFunctions(RoleSubFunctionSearchModel search)
        {
            var apiUrl = string.Format("Settings/QueryRoleSubFunctionsAPI/?Role_UID={0}&Function_UID={1}", search.Role_UID.ToString(), search.Function_UID.ToString());

            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        [HttpGet]
        public ActionResult QuerySubFunctionsByFunctionUID(int uid)
        {
            var apiUrl = string.Format("Settings/QuerySubFunctionsByFunctionUIDAPI/{0}", uid.ToString());

            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        [HttpGet]
        public ActionResult DeleteRoleFunction(int uid)
        {
            var apiUrl = string.Format("Settings/DeleteRoleFunctionAPI/{0}", uid.ToString());

            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult ModifyRoleFunctionWithSubs(string jsonFunctionWithSubs)
        {
            var apiUrl = "Settings/ModifyRoleFunctionWithSubsAPI";

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(jsonFunctionWithSubs, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult AddRoleFunctionWithSubs(string jsonFunctionWithSubs)
        {
            var apiUrl = "Settings/AddRoleFunctionWithSubsAPI";

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(jsonFunctionWithSubs, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult ExportRoleFunctionSetting(string uids)
        {
            //get Export datas
            var apiUrl = string.Format("Settings/DoExportRoleFunctionsAPI?uids={0}", uids);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<IList<RoleFunctionItem>>(result);

            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("RoleFunctionSetting");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new string[] { "Seq", "Role ID", "Function ID", "Function Name", "Order Index",
                "Is Show(Function)","URL","Is Show(Role)","Sub Function Qty", "Modified User", "Modified Date" };

            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add("RoleFunctionSettings");

                //set Title
                for (int colIndex = 0; colIndex < stringHeads.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = stringHeads[colIndex];
                }

                //set cell value
                for (int index = 0; index < list.Count; index++)
                {
                    var currentRecord = list[index];
                    //seq
                    worksheet.Cells[index + 2, 1].Value = index + 1;
                    worksheet.Cells[index + 2, 2].Value = currentRecord.Role_ID;
                    worksheet.Cells[index + 2, 3].Value = currentRecord.Function_ID;
                    worksheet.Cells[index + 2, 4].Value = currentRecord.Function_Name;
                    worksheet.Cells[index + 2, 5].Value = currentRecord.Order_Index;
                    worksheet.Cells[index + 2, 6].Value = currentRecord.Is_Show_Function ? "Y" : "N";
                    worksheet.Cells[index + 2, 7].Value = currentRecord.URL;
                    worksheet.Cells[index + 2, 8].Value = currentRecord.Is_Show_Role ? "Y" : "N";
                    worksheet.Cells[index + 2, 9].Value = currentRecord.SubFunctionCount > 0 ? "Y" : "N";
                    worksheet.Cells[index + 2, 10].Value = currentRecord.Modified_UserName;
                    worksheet.Cells[index + 2, 11].Value = currentRecord.Modified_Date.ToString(FormatConstants.DateTimeFormatString);
                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }

            return new FileContentResult(stream.ToArray(), "application/octet-stream")
            { FileDownloadName = Server.UrlEncode(fileName) };
        }

        #endregion

        #region Organization Maintenance      by  justin

        public ActionResult OrgMaintenance()
        {
            return View();
        }
        /// <summary>
        /// get paged records of users by query conditions
        /// </summary>
        /// <param name="search">query conditions</param>
        /// <param name="page">page info, auto fill by front-end</param>
        /// <returns>json of paged records</returns>
        public ActionResult QueryOrgs(OrgModelSearch search, Page page)
        {
            var apiUrl = string.Format("Settings/QueryOrgsAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        /// <summary>
        /// get user by account id
        /// </summary>
        /// <param name="uuid">account id</param>
        /// <returns>single user json</returns>
        public ActionResult QueryOrg(int uuid)
        {
            var apiUrl = string.Format("Settings/QueryOrgAPI?uuid={0}", uuid);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult AddOrg(string jsonAddOrg)
        {

            var apiUrl = "Settings/AddOrgAPI";
            var entity = JsonConvert.DeserializeObject<SystemOrgDTO>(jsonAddOrg);
            entity.Modified_UID = this.CurrentUser.AccountUId;

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult EditOrg(string jsonEditOrg)
        {

            var apiUrl = "Settings/ModifyOrgAPI";
            var entity = JsonConvert.DeserializeObject<SystemOrgDTO>(jsonEditOrg);
            entity.Modified_UID = this.CurrentUser.AccountUId;

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult CheckOrgExistByIdAndName(string id, string name)
        {
            var apiUrl = string.Format("Settings/CheckOrgExistByIdAndName/?id={0}&name={1}", id, name);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            return Content(responMessage.Content.ReadAsStringAsync().Result, "application/json");
        }

        public ActionResult CheckOrgExistByIdAndNameWithUId(int uid, string id, string name)
        {
            var apiUrl = string.Format("Settings/CheckOrgExistByIdAndNameWithUId/?uid={0}&id={1}&name={2}", uid, id, name);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            return Content(responMessage.Content.ReadAsStringAsync().Result, "application/json");
        }

        public ActionResult GetMaxEnddate4Org(int orgUId)
        {
            var apiUrl = string.Format("Settings/GetMaxEnddate4Org/{0}", orgUId.ToString());
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            return Content(responMessage.Content.ReadAsStringAsync().Result, "application/json");
        }

        public ActionResult DoExportOrg(string uids)
        {
            //get Export datas
            var apiUrl = string.Format("Settings/DoExportOrgAPI?uids={0}", uids);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<List<SystemOrgDTO>>(result);

            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("SystemOrganization");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new string[] { "Seq", "Organization ID", "Organization Name", "Organization_Desc", "OrgManager Name", "OrgManager Tel", "OrgManager Email", "Cost_Center", "Begin Date", "End Date", "Modified User", "Modified Date" };

            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add("System Plants");

                //set Title
                for (int colIndex = 0; colIndex < stringHeads.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = stringHeads[colIndex];
                }

                //set cell value
                for (int index = 0; index < list.Count; index++)
                {
                    var currentRecord = list[index];
                    //seq
                    worksheet.Cells[index + 2, 1].Value = index + 1;
                    worksheet.Cells[index + 2, 2].Value = currentRecord.Organization_ID;
                    worksheet.Cells[index + 2, 3].Value = currentRecord.Organization_Name;
                    worksheet.Cells[index + 2, 4].Value = currentRecord.Organization_Desc;
                    worksheet.Cells[index + 2, 5].Value = currentRecord.OrgManager_Name;
                    worksheet.Cells[index + 2, 6].Value = currentRecord.OrgManager_Tel;
                    worksheet.Cells[index + 2, 7].Value = currentRecord.OrgManager_Email;
                    worksheet.Cells[index + 2, 8].Value = currentRecord.Cost_Center;
                    worksheet.Cells[index + 2, 9].Value = currentRecord.Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate);
                    worksheet.Cells[index + 2, 10].Value =
                        currentRecord.End_Date == null ? string.Empty :
                            ((DateTime)currentRecord.End_Date).ToString(FormatConstants.DateTimeFormatStringByDate);
                    worksheet.Cells[index + 2, 11].Value = currentRecord.Modified_UserName;
                    worksheet.Cells[index + 2, 12].Value = currentRecord.Modified_Date.ToString(FormatConstants.DateTimeFormatString);
                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }

            return new FileContentResult(stream.ToArray(), "application/octet-stream")
            { FileDownloadName = Server.UrlEncode(fileName) };
        }

        #endregion 

        #region OrganizationBom Maintenance    by justin
        public ActionResult OrganizationBomMaintenance()
        {
            return View();
        }
        /// <summary>
        /// get paged records of users by query conditions
        /// </summary>
        /// <param name="search">query conditions</param>
        /// <param name="page">page info, auto fill by front-end</param>
        /// <returns>json of paged records</returns>
        public ActionResult QueryOrgBoms(OrgBomModelSearch search, Page page)
        {
            var apiUrl = string.Format("Settings/QueryOrgBomsAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult QueryOrgBom(int uid)
        {
            var apiUrl = string.Format("Settings/QueryOrgBomAPI?uid={0}", uid);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult AddOrgBom(string jsonAddOrgBom)
        {

            var apiUrl = "Settings/AddOrgBomAPI";
            var entity = JsonConvert.DeserializeObject<SystemOrgBomDTO>(jsonAddOrgBom);
            entity.Modified_UID = this.CurrentUser.AccountUId;

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult EditOrgBom(string jsonEditOrgBom)
        {

            var apiUrl = "Settings/ModifyOrgBomAPI";
            var entity = JsonConvert.DeserializeObject<SystemOrgBomDTO>(jsonEditOrgBom);
            entity.Modified_UID = this.CurrentUser.AccountUId;

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult DoExportOrgBom(string uids)
        {
            //get Export datas
            var apiUrl = string.Format("Settings/DoExportOrgBomAPI?uids={0}", uids);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<List<SystemOrgAndBomDTO>>(result);

            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("SystemOrganizationBom");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new string[] { "Seq", "Parent Organization ID", "Parent Organization Name", "Order Index", "Child Organization ID", "Child Organization Name", "Begin Date", "End Date", "Modified User", "Modified Date" };

            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add("System OranizationBoms");

                //set Title
                for (int colIndex = 0; colIndex < stringHeads.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = stringHeads[colIndex];
                }

                //set cell value
                for (int index = 0; index < list.Count; index++)
                {
                    var currentRecord = list[index];
                    //seq
                    worksheet.Cells[index + 2, 1].Value = index + 1;
                    worksheet.Cells[index + 2, 2].Value = currentRecord.ParentOrg_ID;
                    worksheet.Cells[index + 2, 3].Value = currentRecord.Parent_Organization_Name;
                    worksheet.Cells[index + 2, 4].Value = currentRecord.Order_Index;
                    worksheet.Cells[index + 2, 5].Value = currentRecord.ChildOrg_ID;
                    worksheet.Cells[index + 2, 6].Value = currentRecord.Child_Organization_Name;
                    worksheet.Cells[index + 2, 7].Value = currentRecord.Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate);
                    worksheet.Cells[index + 2, 8].Value =
                        currentRecord.End_Date == null ? string.Empty :
                            ((DateTime)currentRecord.End_Date).ToString(FormatConstants.DateTimeFormatStringByDate);
                    worksheet.Cells[index + 2, 9].Value = currentRecord.Modified_UserName;
                    worksheet.Cells[index + 2, 10].Value = currentRecord.Modified_Date.ToString(FormatConstants.DateTimeFormatString);
                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }

            return new FileContentResult(stream.ToArray(), "application/octet-stream")
            { FileDownloadName = Server.UrlEncode(fileName) };
        }
        public ActionResult CheckOrgBomExist(int uid, int parentUId, int childUId, int index)
        {
            var apiUrl = string.Format("Settings/CheckOrgBomExistAPI/?uid={0}&parentUId={1}&childUId={2}&index={3}",
                                                uid.ToString(), parentUId.ToString(), childUId.ToString(), index.ToString());

            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        #endregion

        #region User Organization Setting Add By Justin

        public ActionResult UserOrgSetting()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetOrgInfoByOrg(string Org)
        {
            var apiUrl = string.Format("Settings/GetOrgInfoAPI?OrgID={0}", Org);

            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        #region query
        public ActionResult QueryUserOrgs(UserOrgModelSearch search, Page page)
        {
            var apiUrl = "Settings/QueryUserOrgsAPI";

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }
        public ActionResult QueryUserOrg(int uuid)
        {
            var apiUrl = string.Format("Settings/QueryUserOrgsByAccountUIDAPI?uuid={0}", uuid);

            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }
        #endregion

        #region Add
        public ActionResult AddUserOrg(string jsonUserWithPlant)
        {

            var apiUrl = "Settings/AddUserOrgAPI";
            var entity = JsonConvert.DeserializeObject<UserOrgEditModel>(jsonUserWithPlant);
            entity.Modified_UID = this.CurrentUser.AccountUId;

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }
        #endregion

        #region modify
        public ActionResult modifyUserOrg(string jsonUserWithPlant)
        {

            var apiUrl = "Settings/ModifyUserOrgAPI";
            var entity = JsonConvert.DeserializeObject<UserOrgEditModel>(jsonUserWithPlant);
            entity.Modified_UID = this.CurrentUser.AccountUId;

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }
        #endregion

        public ActionResult DoExportUserOrg(string uids)
        {
            //get Export datas
            var apiUrl = string.Format("Settings/DoExportUserOrgAPI?uids={0}", uids);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<IList<UserOrgItem>>(result);

            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("UserOrgSetting");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new string[] { "Seq", "User NTID", "User Name", "Organization ID"
                , "Organization Name","Begin Date","End Date", "Modified User", "Modified Date" };

            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add("UserOrgSetting");

                //set Title
                for (int colIndex = 0; colIndex < stringHeads.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = stringHeads[colIndex];
                }

                //set cell value
                for (int index = 0; index < list.Count; index++)
                {
                    var currentRecord = list[index];
                    //seq
                    worksheet.Cells[index + 2, 1].Value = index + 1;
                    worksheet.Cells[index + 2, 2].Value = currentRecord.User_NTID;
                    worksheet.Cells[index + 2, 3].Value = currentRecord.User_Name;
                    worksheet.Cells[index + 2, 4].Value = currentRecord.Organization_ID;
                    worksheet.Cells[index + 2, 5].Value = currentRecord.Organization_Name;
                    worksheet.Cells[index + 2, 6].Value = currentRecord.Begin_Date == null ? null : ((DateTime)currentRecord.Begin_Date).ToString(FormatConstants.DateTimeFormatStringByDate);
                    worksheet.Cells[index + 2, 7].Value = currentRecord.End_Date == null ? null : ((DateTime)currentRecord.End_Date).ToString(FormatConstants.DateTimeFormatStringByDate);
                    worksheet.Cells[index + 2, 8].Value = currentRecord.Modified_UserName;
                    worksheet.Cells[index + 2, 9].Value = currentRecord.Modified_Date.ToString(FormatConstants.DateTimeFormatString);
                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }

            return new FileContentResult(stream.ToArray(), "application/octet-stream")
            { FileDownloadName = Server.UrlEncode(fileName) };
        }
        #endregion

        #region User Role Setting------------------Add By Allen 2015/11/16
        public ActionResult UserRoleSetting()
        {
            var apiUrl = "Common/GetAllRoles";
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            var rolesModel = JsonConvert.DeserializeObject<IEnumerable<SystemRoleDTO>>(result);
            return View(rolesModel);
        }

        /// <summary>
        /// get paged records of users by query conditions
        /// </summary>
        /// <param name="search">query conditions</param>
        /// <param name="page">page info, auto fill by front-end</param>
        /// <returns>json of paged records</returns>
        public ActionResult QueryUserRoles(UserRoleSearchModel search, Page page)
        {
            var apiUrl = string.Format("Settings/QueryUserRolesAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        /// <summary>
        /// get user by account id 
        /// </summary>
        /// <param name="uuid">account id</param>
        /// <returns>single user json</returns>
        public ActionResult QueryUserRole(int uruid)
        {
            var apiUrl = string.Format("Settings/QueryUserRoleAPI?uruid={0}", uruid);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        /// <summary>
        /// delete specific user
        /// </summary>
        /// <param name="dto">entity, or just fill the primary key account id</param>
        /// <returns></returns>
        public ActionResult DeleteUserRole(SystemUserRoleDTO dto)
        {
            var apiUrl = string.Format("Settings/DeleteUserRoleAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(dto, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        /// <summary>
        /// edit or add new user 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="isEdit">flag, true edit/false add</param>
        /// <returns></returns>
        public string EditUserRoleProfile(SystemUserRoleDTO dto)
        {
            //add new
            var apiUrl = string.Format("Settings/AddUserRoleAPI");
            dto.Modified_UID = this.CurrentUser.AccountUId;
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(dto, apiUrl);

            return responMessage.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// 檢查NTID
        /// </summary>
        /// <param name="User_NTID">User NTID</param>
        /// <returns>json, true/false</returns>
        public ActionResult CheckUserRoleExistByNtid(string ntid)
        {
            var apiUrl = string.Format("Settings/CheckUserRoleExistByNtidAPI/?ntid={0}", ntid);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            return Content(responMessage.Content.ReadAsStringAsync().Result, "application/json");
        }

        /// <summary>
        /// 取得RoleName
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public ActionResult GetRoleNameById(string roleid)
        {
            var apiUrl = string.Format("Settings/GetRoleNameByIdAPI?roleid={0}", roleid);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        /// <summary>
        /// Export data selected in grid   
        /// </summary>
        /// <param name="uuids">selected keys</param>
        /// <returns></returns>
        public ActionResult DoExportUserRole(string uruids)
        {
            //get Export datas
            var apiUrl = string.Format("Settings/DoExportUserRoleAPI?uruids={0}", uruids);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<List<UserRoleItem>>(result);

            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("SystemUserRole");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new string[] { "Seq", "User NTID", "User Name", "Role ID", "Role Name", "Modified User", "Modified Date" };

            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name    
                var worksheet = excelPackage.Workbook.Worksheets.Add("System UserRole");

                //set Title
                for (int colIndex = 0; colIndex < stringHeads.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = stringHeads[colIndex];
                }

                //set cell value
                for (int index = 0; index < list.Count; index++)
                {
                    var currentRecord = list[index];
                    //seq
                    worksheet.Cells[index + 2, 1].Value = index + 1;
                    worksheet.Cells[index + 2, 2].Value = currentRecord.User_NTID;
                    worksheet.Cells[index + 2, 3].Value = currentRecord.User_Name;
                    worksheet.Cells[index + 2, 4].Value = currentRecord.Role_ID;
                    worksheet.Cells[index + 2, 5].Value = currentRecord.Role_Name;
                    worksheet.Cells[index + 2, 6].Value = currentRecord.Modified_UserName;
                    worksheet.Cells[index + 2, 7].Value = currentRecord.Modified_Date.ToString(FormatConstants.DateTimeFormatString);
                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }

            return new FileContentResult(stream.ToArray(), "application/octet-stream")
            { FileDownloadName = Server.UrlEncode(fileName) };
        }

        #endregion //User Role Setting

        #region FuncPlant Maintenance----------------------------Add By Sidney 2016/02/23

        public ActionResult FuncPlantMatenance()
        {
            return View();
        }
        public ActionResult AddFuncPlant(string jsonAddPlant)
        {

            var apiUrl = "Settings/AddFuncPlantAPI";
            var entity = JsonConvert.DeserializeObject<FuncPlantMaintanance>(jsonAddPlant);
            entity.Modified_UID = this.CurrentUser.AccountUId;
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }
        public ActionResult QueryFuncPlants(FuncPlantSearchModel search, Page page)
        {
            var apiUrl = string.Format("Settings/QueryFuncPlantsAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }
        public ActionResult getPlant()
        {
            var apiUrl = "Settings/GetPlantSingleAPI";
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult QueryFuncPlant(int uuid)
        {
            var apiUrl = string.Format("Settings/QueryFuncPlantAPI?uuid={0}", uuid);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult DeleteFuncPlant(int System_FuncPlant_UID)
        {
            var apiUrl = string.Format("Settings/DeleteFuncPlantAPI?uuid={0}", System_FuncPlant_UID);
            //MVC调用WebAPI请使用SPP.Core.APIHelper中的方法。此方法内对授权和异常进行了处
            //理。如有特殊复杂需求，可自行封装。
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult EditFuncPlant(string jsonEditPlant)
        {
            var apiUrl = "Settings/ModifyFuncPlantAPI";
            var entity = JsonConvert.DeserializeObject<FuncPlantMaintanance>(jsonEditPlant);
            entity.Modified_UID = this.CurrentUser.AccountUId;

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult DoExportFuncPlant(string uuids)
        {
            //get Export datas
            var apiUrl = string.Format("Settings/DoExportFuncPlantAPI?uuids={0}", uuids);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<List<FuncPlantMaintanance>>(result);

            var stream = new MemoryStream();
            var fileName = PathHelper.SetGridExportExcelName("功能厂维护");
            var filePath = Path.Combine(PathHelper.GetGridExportExcelDirectory, fileName);
            var stringHeads = new string[] { "序号", "功能厂", "厂区", "OP类型", "功能厂管理者", "功能厂联系方式", "修改者", "修改时间" };
            using (var excelPackage = new ExcelPackage(stream))
            {
                //set sheet name
                var worksheet = excelPackage.Workbook.Worksheets.Add("功能厂维护");

                //set Title
                for (int colIndex = 0; colIndex < stringHeads.Length; colIndex++)
                {
                    worksheet.Cells[1, colIndex + 1].Value = stringHeads[colIndex];
                }

                //set cell value
                for (int index = 0; index < list.Count; index++)
                {
                    var currentRecord = list[index];
                    //seq
                    worksheet.Cells[index + 2, 1].Value = index + 1;
                    worksheet.Cells[index + 2, 2].Value = currentRecord.FunPlant;
                    worksheet.Cells[index + 2, 3].Value = currentRecord.Plant;
                    worksheet.Cells[index + 2, 4].Value = currentRecord.OPType;
                    worksheet.Cells[index + 2, 5].Value = currentRecord.Plant_Manager;
                    worksheet.Cells[index + 2, 6].Value = currentRecord.FuncPlant_Context;
                    worksheet.Cells[index + 2, 7].Value = currentRecord.Modified_UserName;
                    worksheet.Cells[index + 2, 8].Value = currentRecord.Modified_Date;
                }
                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();
            }

            return new FileContentResult(stream.ToArray(), "application/octet-stream")
            { FileDownloadName = Server.UrlEncode(fileName) };
        }

        #endregion
        #region User FuncPlant Setting----------------------------Add By Sidney 2016/02/23

        public ActionResult UserFuncPlantSetting()
        {
            return View();
        }



        #endregion

        #region key process Setting -----Add By Destiny

        public ActionResult MainProcessSetting()
        {
            return View();
        }


        public ActionResult QueryProjectTypes()
        {
            var apiUrl = string.Format("FlowChart/QueryProjectTypes");
            HttpResponseMessage responseMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responseMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult QueryProcessByFlowchartMasterUid(int flowchartMasterUId)
        {
            string apiUrl = string.Format("FlowChart/QueryProcessByFlowchartMasterUid?flowcharMasterUid={0}", flowchartMasterUId);
            HttpResponseMessage responseMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responseMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult DeleteKeyProcess(int Enum_UID)
        {
            var apiUrl = string.Format("Settings/DeleteKeyProcessAPI");
            EnumerationDTO dto = new EnumerationDTO();
            dto.Enum_UID = Enum_UID;
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(dto, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }


        public ActionResult AddKeyProcess(string partTypes, string process)
        {
            var apiUrl = string.Format("Settings/AddEnumeration");
            EnumerationDTO dto = new EnumerationDTO();
            dto.Decription = "Key Process";
            dto.Enum_Type = "Report_Key_Process";
            dto.Enum_Name = partTypes;
            dto.Enum_Value = process;
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(dto, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult QueryEnumkeyProcess(string partTypes, Page page)
        {
            FlowChartModelSearch search = new FlowChartModelSearch();
            search.Part_Types = partTypes;

            var apiUrl = string.Format("Settings/GetEnumValueForKeyProcess");
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        public ActionResult GetEnumNameForKeyProcess()
        {
            var apiUrl = string.Format("Settings/GetEnumNameForKeyProcess");
            HttpResponseMessage responseMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responseMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }



        #endregion
    }
}