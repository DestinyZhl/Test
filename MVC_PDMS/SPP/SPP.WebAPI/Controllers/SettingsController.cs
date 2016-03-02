using Newtonsoft.Json;
using SPP.Core;
using SPP.Data;
using SPP.Model;
using SPP.Model.ViewModels;
using SPP.Service;
using System;
using System.Collections.Generic;
using System.Web.Http;
using SPP.Model.ViewModels.Settings;
using SPP.Common.Constants;
using System.Linq;
using SPP.Core.Authentication;

namespace SPP.WebAPI.Controllers
{
    public class SettingsController : ApiControllerBase
    {
        ISettingsService settingsService;
        ICommonService commonService;

        public SettingsController(ISettingsService settingsService, ICommonService commonService)
        {
            this.settingsService = settingsService;
            this.commonService = commonService;
        }

        #region User Maintenance API-------------------Add By Tonny 2015/11/02
        /// <summary>
        /// get paged records of users by query conditions
        /// </summary>
        /// <param name="data">post search modal and page modal</param>
        /// <returns>json of paged records</returns>
        [HttpPost]
        public IHttpActionResult QueryUsersAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<UserModelSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);

            var users = settingsService.QueryUsers(searchModel, page);
            return Ok(users);
        }

        [HttpGet]
        public IHttpActionResult QueryUserAPI(int uuid)
        {
            var dto = new SystemUserDTO();
            dto = AutoMapper.Mapper.Map<SystemUserDTO>(settingsService.QueryUsersSingle(uuid));
            return Ok(dto);
        }

        public string ModifyUserAPI(SystemUserDTO dto)
        {
            var ent = settingsService.QueryUsersSingle(dto.Account_UID);
            ent.User_NTID = dto.User_NTID;
            ent.User_Name = dto.User_Name;
            ent.Tel = dto.Tel;
            ent.Email = dto.Email;
            ent.Enable_Flag = dto.Enable_Flag;
            ent.Modified_UID = dto.Modified_UID;
            ent.Modified_Date = DateTime.Now;
            settingsService.ModifyUser(ent);
            return "SUCCESS";
        }

        public void AddUserAPI(SystemUserDTO dto)
        {
            var ent = AutoMapper.Mapper.Map<System_Users>(dto);
            ent.Modified_Date = DateTime.Now;
            settingsService.AddUser(ent);
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>SUCCSSS/FAIL</returns>
        [AcceptVerbs("Post")]
        public string DeleteUserAPI(SystemUserDTO dto)
        {
            var ent = settingsService.QueryUsersSingle(dto.Account_UID);
            var result = settingsService.DeleteUser(ent) ? "SUCCESS" : "FAIL";
            return result;
        }

        [HttpGet]
        public IHttpActionResult DoExportUserAPI(string uuids)
        {
            var users = settingsService.DoExportUser(uuids);
            return Ok(users);
        }
        #endregion //#region User Maintenance API

        #region PlantAPI-------------------Add By Sidney 2015/11/12
        /// <summary>
        /// get paged records of users by query conditions
        /// </summary>
        /// <param name="data">post search modal and page modal</param>
        /// <returns>json of paged records</returns>
        [HttpPost]
        public IHttpActionResult QueryPlantsAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<PlantModelSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);
            var Plants = settingsService.QueryPlants(searchModel, page);
            return Ok(Plants);
        }
        [HttpGet]
        public IHttpActionResult QueryPlantAPI(int uuid)
        {
            var dto = new SystemPlantDTO();
            dto = AutoMapper.Mapper.Map<SystemPlantDTO>(settingsService.QueryPlantSingle(uuid));
            return Ok(dto);
        }

        public string ModifyPlantAPI(SystemPlantDTO dto)
        {
            var ent = settingsService.QueryPlantSingle(dto.System_Plant_UID);
            ent.Address_EN = dto.Address_EN;
            ent.Address_ZH = dto.Address_ZH;
            ent.CCODE = dto.CCODE;
            ent.Legal_Entity_EN = dto.Legal_Entity_EN;
            ent.Legal_Entity_ZH = dto.Legal_Entity_ZH;
            ent.Location = dto.Location;
            ent.Name_0 = dto.Name_0;
            ent.Name_1 = dto.Name_1;
            ent.Name_2 = dto.Name_2;
            ent.Plant = dto.Plant;
            ent.Type = dto.Type;
            ent.PlantManager_Name = dto.PlantManager_Name;
            ent.PlantManager_Tel = dto.PlantManager_Tel;
            ent.PlantManager_Email = dto.PlantManager_Email;
            if (ent.End_Date == null && dto.End_Date != null)
            {
                ent.End_Date = dto.End_Date;
            }

            ent.Coordinate = dto.Coordinate;
            ent.Modified_UID = dto.Modified_UID;
            ent.Modified_Date = DateTime.Now;

            var plantstring = settingsService.ModifyPlant(ent);
            return plantstring;
        }

        public string AddPlantAPI(SystemPlantDTO dto)
        {
            var ent = AutoMapper.Mapper.Map<System_Plant>(dto);
            ent.Modified_Date = DateTime.Now;
            var plantstring = settingsService.AddPlant(ent);
            if (plantstring != "SUCCESS")
                return plantstring;
            else
                return "SUCCESS";
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>SUCCSSS/FAIL</returns>
        [AcceptVerbs("Post")]
        public string DeletePlantAPI(SystemPlantDTO dto)
        {
            var ent = settingsService.QueryPlantSingle(dto.System_Plant_UID);
            var result = settingsService.DeletePlant(ent);
            return result;
        }

        [HttpGet]
        public IHttpActionResult DoExportPlantAPI(string uuids)
        {
            var listEnt = settingsService.DoExportPlant(uuids);
            return Ok(listEnt);
        }

        [HttpGet]
        [IgnoreDBAuthorize]
        public bool CheckPlantExistByUId(int uuid)
        {
            return settingsService.CheckPlantExistByUId(uuid);
        }
        [HttpGet]
        [IgnoreDBAuthorize]
        public bool CheckPlantExistByPlant(string Plant)
        {
            return settingsService.CheckPlantExistByPlant(Plant);
        }

        [HttpGet]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetMaxEnddate4Plant(int uid)
        {
            var enddate = settingsService.GetMaxEnddate4Plant(uid);
            if (enddate != null)
            {
                return Ok(new { Enddate = ((DateTime)enddate).ToString(FormatConstants.DateTimeFormatStringByDate) });
            }
            else
            {
                return Ok(new { Enddate = enddate });
            }

        }
        #endregion

        #region All of Rock's API ------------------Start--------Add by Rock 2015/11/18


        #region BU Master Maintenance API -------------------------Add by Rock 2015/11/09
        /// <summary>
        /// get paged records of users by query conditions
        /// </summary>
        /// <param name="data">post search modal and page modal</param>
        /// <returns>json of paged records</returns>
        [HttpPost]
        public IHttpActionResult QueryBUsAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<BUModelSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);

            var bus = settingsService.QueryBus(searchModel, page);
            return Ok(bus);
        }

        [HttpGet]
        [IgnoreDBAuthorize]
        public bool CheckBuExistById(string buid)
        {
            return settingsService.CheckBuExistById(buid, null);
        }

        //[HttpGet]
        //public bool CheckBuExistByName(string buname)
        //{
        //    return settingsService.CheckBuExistById(null, buname);
        //}

        [HttpGet]
        public IHttpActionResult QueryBUAPI(int BU_M_UID)
        {
            var dto = new SystemBUMDTO();
            dto = AutoMapper.Mapper.Map<SystemBUMDTO>(settingsService.QueryBUSingle(BU_M_UID));
            return Ok(dto);
        }

        public void AddBUAPI(SystemBUMDTO dto)
        {
            var ent = AutoMapper.Mapper.Map<System_BU_M>(dto);
            ent.Modified_Date = DateTime.Now;
            settingsService.AddBU(ent);
        }

        public string ModifyBUAPI(SystemBUMDTO dto)
        {
            var ent = settingsService.QueryBUSingle(dto.BU_M_UID);
            ent.BU_Name = dto.BU_Name;
            ent.Begin_Date = dto.Begin_Date;
            ent.End_Date = dto.End_Date;
            ent.BUManager_Name = dto.BUManager_Name;
            ent.BUManager_Tel = dto.BUManager_Tel;
            ent.BUManager_Email = dto.BUManager_Email;
            ent.Modified_UID = dto.Modified_UID;
            ent.Modified_Date = DateTime.Now;
            settingsService.ModifyBU(ent);
            return "SUCCESS";
        }

        [AcceptVerbs("Post")]
        public string DeleteBUAPI(SystemBUMDTO dto)
        {
            var ent = settingsService.QueryBUSingle(dto.BU_M_UID);
            var result = settingsService.DeleteBU(ent) ? "SUCCESS" : "FAIL";
            return result;
        }


        [HttpGet]
        public IHttpActionResult DoExportBUAPI(string uuids)
        {
            var listDto = settingsService.DoExportBU(uuids);
            return Ok(listDto);
        }





        #endregion

        #region BU Detail Maintenance --------------------------- Add by Rock 2015/11/13
        [HttpPost]
        public IHttpActionResult QueryBUDetailsAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<BUDetailModelSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);

            var bus = settingsService.QueryBUDs(searchModel, page);
            return Ok(bus);
        }

        [HttpGet]
        [IgnoreDBAuthorize]
        public bool CheckBuExistById_TwoAPI(string buid)
        {
            return settingsService.CheckBuExistById_Two(buid);
        }

        [HttpGet]
        [IgnoreDBAuthorize]
        public string CheckBeginDateAndEndDateAPI(string BU_ID, string BU_Name, string Begin_Date, string End_Date)
        {
            return settingsService.CheckBeginDateAndEndDate(BU_ID, BU_Name, Begin_Date, End_Date);
        }



        [HttpGet]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetBUIDAndBUNameByBUIDAPI(string BU_ID)
        {
            var dto = settingsService.GetBUIDAndBUNameByBUID(BU_ID);
            return Ok(dto);
        }

        [HttpGet]
        public IHttpActionResult QueryBUDInfoByBU_D_IDAPI(string BU_D_ID)
        {
            var dto = settingsService.QueryBUDInfoByBU_D_ID(BU_D_ID);
            return Ok(dto);
        }

        public void EditBUDetailInfoAPI(SystemBUDDTO dto, string BU_ID, string BU_Name, bool isEdit)
        {
            settingsService.AddOrEditBUDetailInfo(dto, BU_ID, BU_Name, isEdit);
        }

        public void AddBUDetailInfoAPI(SystemBUDDTO dto, string BU_ID, string BU_Name, bool isEdit)
        {
            settingsService.AddOrEditBUDetailInfo(dto, BU_ID, BU_Name, isEdit);
        }

        [HttpGet]
        public IHttpActionResult QueryBUDetailAPI(int BU_D_UID)
        {
            var dto = settingsService.QueryBUDSingle(BU_D_UID);
            return Ok(dto);
        }

        [HttpGet]
        public string DeleteBUDDetailAPI(int BU_D_UID)
        {
            var ent = settingsService.QueryBUDSingleByModule(BU_D_UID);
            var result = settingsService.DeleteBUDSingle(ent) ? "SUCCESS" : "FAIL";
            return result;
        }

        [HttpGet]
        public IHttpActionResult DoExportBUDetailAPI(string BU_D_UIDS)
        {
            var list = settingsService.DoExportBUDetail(BU_D_UIDS);
            return Ok(list);
        }

        #endregion

        #region User BU Setting API-----------------Start-----------Add by Rock 2015/11/13
        [HttpPost]
        public IHttpActionResult QueryUserBUsAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<UserBUSettingSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);

            var bus = settingsService.QueryUserBUs(searchModel, page);
            return Ok(bus);
        }

        [HttpGet]
        public IHttpActionResult QueryUserBUAPI(int System_User_BU_UID)
        {
            var item = settingsService.QueryUserBU(System_User_BU_UID);
            return Ok(item);
        }

        [HttpGet]
        public IHttpActionResult QueryBUAndBUDSByBUIDAPI(string BU_ID)
        {
            var list = settingsService.QueryBUAndBUDSByBUIDS(BU_ID);
            return Ok(list);
        }

        public string AddUserBUAPI(dynamic data)
        {
            var entity = JsonConvert.DeserializeObject<UserBUAddOrSave>(data.ToString());
            return settingsService.AddUserBU(entity);
        }

        public string EditUserBUAPI(dynamic data)
        {
            var entity = JsonConvert.DeserializeObject<UserBUAddOrSave>(data.ToString());
            return settingsService.EditUserBU(entity);
        }

        [HttpGet]
        public IHttpActionResult DoExportUserBUAPI(string KeyIDS)
        {
            var list = settingsService.DoExportUserBU(KeyIDS);
            return Ok(list);

        }
        #endregion User BU Setting API-----------------End-----------Add by Rock 2015/11/13


        #endregion All of Rock's API ------------------End--------Add by Rock 2015/11/18

        //------------------------------------------------------------------

        #region Role Maintenance API------------------Add By Allen 2015/11/9

        /// <summary>
        /// get paged records of users by query conditions
        /// </summary>
        /// <param name="data">post search modal and page modal</param>
        /// <returns>json of paged records</returns>
        [HttpPost]
        public IHttpActionResult QueryRolesAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<RoleModelSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);

            var role = settingsService.QueryRoles(searchModel, page);
            return Ok(role);

        }
        [HttpGet]
        public IHttpActionResult QueryRoleAPI(int uuid)
        {
            var dto = new SystemRoleDTO();
            dto = AutoMapper.Mapper.Map<SystemRoleDTO>(settingsService.QueryRolesSingle(uuid));
            return Ok(dto);
        }

        public void AddRoleAPI(SystemRoleDTO dto)
        {
            var ent = AutoMapper.Mapper.Map<System_Role>(dto);
            ent.Modified_Date = DateTime.Now;
            settingsService.AddRole(ent);
        }
        public void ModifyRoleAPI(SystemRoleDTO dto)
        {
            var ent = AutoMapper.Mapper.Map<System_Role>(dto);
            ent.Modified_Date = DateTime.Now;
            settingsService.ModifyRole(ent);
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>SUCCSSS/FAIL</returns>
        [AcceptVerbs("Post")]
        public string DeleteRoleAPI(SystemRoleDTO dto)
        {
            var ent = settingsService.QueryRolesSingle(dto.Role_UID);
            var result = settingsService.DeleteRole(ent);
            return result;
        }

        [HttpGet]
        public IHttpActionResult DoExportRoleAPI(string ruids)
        {
            return Ok(settingsService.DoExportRole(ruids));
        }

        [HttpGet]
        [IgnoreDBAuthorize]
        public bool CheckRoleExistById(string rid)
        {
            return settingsService.CheckRoleExistById(rid);
        }
        #endregion //Role Maintenance API

        #region System Function Maintenance module Add by Tonny 2015/11/12

        [AcceptVerbs("Post")]
        public IHttpActionResult QueryFunctionsAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<FunctionModelSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);

            var functions = settingsService.QueryFunctions(searchModel, page);
            return Ok(functions);
        }

        /// <summary>
        /// Get specific function and its subfunctions by key
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult QueryFunctionWithSubsAPI(int uid)
        {
            var entity = settingsService.QueryFunctionWithSubs(uid);
            System_Function parentFunction = null;

            if (entity.Parent_Function_UID != null)
            {
                parentFunction = settingsService.GetFunction((int)entity.Parent_Function_UID);
            }

            var result = new FunctionWithSubs
            {
                Function_UID = entity.Function_UID,
                Parent_Function_ID = parentFunction == null ? string.Empty : parentFunction.Function_ID,
                Parent_Function_Name = parentFunction == null ? string.Empty : parentFunction.Function_Name,
                Function_Name = entity.Function_Name,
                Function_ID = entity.Function_ID,
                Function_Desc = entity.Function_Desc,
                Is_Show = entity.Is_Show,
                Order_Index = entity.Order_Index,
                URL = entity.URL,
                Icon_ClassName = entity.Icon_ClassName,
                Mobile_URL = entity.Mobile_URL,
                FunctionSubs = AutoMapper.Mapper.Map<List<SystemFunctionSubDTO>>(entity.System_FunctionSub)
            };
            return Ok(result);
        }

        /// <summary>
        /// Add function and its sub funtions
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string AddFunctionWithSubsAPI(dynamic data)
        {
            var entity = JsonConvert.DeserializeObject<FunctionWithSubs>(data.ToString());
            return settingsService.AddFunctionWithSubs(entity);
        }

        /// <summary>
        /// Modify function and its sub functions
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string ModifyFunctionWithSubsAPI(dynamic data)
        {
            var entity = JsonConvert.DeserializeObject<FunctionWithSubs>(data.ToString());
            return settingsService.ModifyFunctionWithSubs(entity);
        }

        /// <summary>
        /// Get specific function by function_id
        /// </summary>
        /// <param name="functionId"></param>
        /// <returns></returns>
        [HttpGet]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetFunctionByIDAPI(string functionId)
        {
            var entity = settingsService.GetFunctionByID(functionId);
            var dto = entity == null ? new SystemFunctionDTO() : AutoMapper.Mapper.Map<SystemFunctionDTO>(entity);
            return Ok(dto);
        }

        /// <summary>
        /// Delete function
        /// </summary>
        /// <param name="uid">key</param>
        /// <returns>operate result</returns>
        [HttpGet]
        public string DeleteFunctionAPI(int uid)
        {
            return settingsService.DeleteFunction(uid);
        }

        [HttpGet]
        public string DeleteSubFunctionAPI(int subfunction_UId)
        {
            return settingsService.DeleteSubFunction(subfunction_UId);
        }

        [HttpGet]
        public IHttpActionResult DoExportFunctionAPI(string uids)
        {
            return Ok(settingsService.DoExportFunction(uids));
        }

        #endregion //end System Function Maintenance module

        #region User Plant Setting Add by Sidney 2015/11/17
        [HttpGet]
        [IgnoreDBAuthorize]
        public SystemPlantDTO GetPlantInfoAPI(string Plant)
        {
            var result = settingsService.GetPlantInfo(Plant);
            var result1 = AutoMapper.Mapper.Map<SystemPlantDTO>(result);
            return result1;
        }

        [AcceptVerbs("Post")]
        public IHttpActionResult QueryUserPlantsAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<UserPlantModelSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);
            var userPlants = settingsService.QueryUserPlants(searchModel, page);
            return Ok(userPlants);
        }
        [HttpGet]
        public IHttpActionResult QueryUserPlantByAccountUID(int uuid)
        {
            var Users = commonService.GetSystemUserByUId(uuid);
            var userPlantList = settingsService.QueryUserPlantByAccountUID(uuid);

            var result = new UserPlantEditModel
            {
                Account_UID = Users.Account_UID,
                User_NTID = Users.User_NTID,
                User_Name = Users.User_Name,
                UserPlantWithPlants = userPlantList,
            };

            return Ok(result);
        }

        public string AddUserPlantAPI(dynamic data)
        {
            var entity = JsonConvert.DeserializeObject<UserPlantEditModel>(data.ToString());
            return settingsService.AddUserPlantWithSubs(entity);
        }

        public string ModifyUserPlantAPI(dynamic data)
        {
            var entity = JsonConvert.DeserializeObject<UserPlantEditModel>(data.ToString());
            return settingsService.ModifyUserPlantWithSubs(entity);
        }


        [HttpGet]
        public IHttpActionResult DoExportUserPlantAPI(string uids)
        {
            return Ok(settingsService.DoExportUserPlant(uids));
        }
        #endregion

        #region Role Function Setting Module -------------- Add by Tonny 2015/11/18

        public IHttpActionResult QueryRoleFunctionsAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<RoleFunctionSearchModel>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);

            var roleFunctions = settingsService.QueryRoleFunctions(searchModel, page);

            return Ok(roleFunctions);
        }

        [HttpGet]
        public IHttpActionResult QueryRoleFunctionAPI(int uid)
        {
            var roleFunction = settingsService.QueryRoleFunction(uid);
            return Ok(roleFunction);
        }

        [HttpGet]
        [IgnoreDBAuthorize]
        public IHttpActionResult QueryRoleSubFunctionsAPI([FromUri] RoleSubFunctionSearchModel searchModel)
        {
            var roleSubFunctions = settingsService.QueryRoleSubFunctions(searchModel.Role_UID, searchModel.Function_UID);
            return Ok(roleSubFunctions);
        }

        [HttpGet]
        [IgnoreDBAuthorize]
        public IHttpActionResult QuerySubFunctionsByFunctionUIDAPI(int uid)
        {
            var result = settingsService.QuerySubFunctionsByFunctionUID(uid);
            return Ok(result);
        }

        public string ModifyRoleFunctionWithSubsAPI(dynamic data)
        {
            var entity = JsonConvert.DeserializeObject<RoleFunctionsWithSub>(data.ToString());
            return settingsService.MaintainRoleFunctionWithSubs(entity);
        }

        public string AddRoleFunctionWithSubsAPI(dynamic data)
        {
            var entity = JsonConvert.DeserializeObject<RoleFunctionsWithSub>(data.ToString());
            return settingsService.MaintainRoleFunctionWithSubs(entity);
        }

        [HttpGet]
        public IHttpActionResult DeleteRoleFunctionAPI(int uid)
        {
            settingsService.DeleteRoleFunction(uid);
            return Ok("SUCCESS");
        }

        [HttpGet]
        public IHttpActionResult DoExportRoleFunctionsAPI(string uids)
        {
            return Ok(settingsService.DoExportRoleFunctions(uids));
        }
        #endregion

        #region User Role Setting API------------------Add By Allen 2015/11/16
        /// <summary>
        /// get paged records of users by query conditions
        /// </summary>
        /// <param name="data">post search modal and page modal</param>
        /// <returns>json of paged records</returns>
        [HttpPost]
        public IHttpActionResult QueryUserRolesAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<UserRoleSearchModel>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);

            var userroles = settingsService.QueryUserRoles(searchModel, page);
            return Ok(userroles);

        }
        [HttpGet]
        public IHttpActionResult QueryUserRoleAPI(int uruid)
        {
            var dto = new UserRoleItem();
            dto = AutoMapper.Mapper.Map<UserRoleItem>(settingsService.QueryUserRolesSingle(uruid));
            return Ok(dto);
        }

        public string AddUserRoleAPI(SystemUserRoleDTO dto)
        {
            //检查是否已有相同的数据存在
            var ent = settingsService.GetSystemUserRole(dto.Account_UID, dto.Role_UID);
            if (ent == null)
            {
                var newEnt = AutoMapper.Mapper.Map<System_User_Role>(dto);
                newEnt.Modified_Date = DateTime.Now;
                settingsService.AddUserRole(newEnt);

                return "SUCCESS";
            }

            return "DATAEXIST";
        }

        /// <summary>
        /// Delete User Role
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>SUCCSSS/FAIL</returns>
        [AcceptVerbs("Post")]
        public string DeleteUserRoleAPI(SystemUserRoleDTO dto)
        {
            var ent = settingsService.QueryUserRolesSingle(dto.System_User_Role_UID);
            var result = settingsService.DeleteUserRole(ent) ? "SUCCESS" : "FAIL";
            return result;
        }

        [HttpGet]
        public IHttpActionResult DoExportUserRoleAPI(string uruids)
        {
            return Ok(settingsService.DoExportUserRole(uruids));
        }

        [IgnoreDBAuthorize]
        public SystemRoleDTO GetRoleNameByIdAPI(string roleid)
        {
            return AutoMapper.Mapper.Map<SystemRoleDTO>(settingsService.GetRoleNameById(roleid));
        }

        #endregion //Role Maintenance API

        #region OrgAPI   by  justin

        [HttpPost]
        public IHttpActionResult QueryOrgsAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<OrgModelSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);
            var Orgs = settingsService.QueryOrgs(searchModel, page);
            return Ok(Orgs);
        }
        [HttpGet]
        public IHttpActionResult QueryOrgAPI(int uuid)
        {
            var dto = new SystemOrgDTO();
            dto = AutoMapper.Mapper.Map<SystemOrgDTO>(settingsService.QueryOrgSingle(uuid));
            return Ok(dto);
        }

        public string ModifyOrgAPI(SystemOrgDTO dto)
        {
            var ent = settingsService.QueryOrgSingle(dto.Organization_UID);
            ent.Organization_ID = dto.Organization_ID;
            ent.Organization_Name = dto.Organization_Name;
            ent.Organization_Desc = dto.Organization_Desc;
            ent.Cost_Center = dto.Cost_Center;
            ent.OrgManager_Name = dto.OrgManager_Name;
            ent.OrgManager_Tel = dto.OrgManager_Tel;
            ent.OrgManager_Email = dto.OrgManager_Email;
            ent.Modified_UID = dto.Modified_UID;
            ent.Modified_Date = DateTime.Now;

            if (ent.End_Date == null)
            {
                ent.End_Date = dto.End_Date;
            }

            settingsService.ModifyOrg(ent);
            return "SUCCESS";
        }

        public string AddOrgAPI(SystemOrgDTO dto)
        {
            var ent = AutoMapper.Mapper.Map<System_Organization>(dto);
            ent.Modified_Date = DateTime.Now;
            settingsService.AddOrg(ent);
            return "SUCCESS";
        }

        [AcceptVerbs("Post")]
        public string DeleteOrgAPI(int uid)
        {
            var result = settingsService.DeleteOrg(uid) ? "SUCCESS" : "FAIL";
            return result;
        }

        [HttpGet]
        public IHttpActionResult DoExportOrgAPI(string uids)
        {
            var listDto = settingsService.DoExportOrg(uids);
            return Ok(listDto);
        }

        [HttpGet]
        [IgnoreDBAuthorize]
        public bool CheckOrgExistByIdAndName(string id, string name)
        {
            return settingsService.CheckOrgExistByIdAndName(id, name);
        }
        [HttpGet]
        [IgnoreDBAuthorize]
        public bool CheckOrgExistByIdAndNameWithUId(int uid, string id, string name)
        {
            return settingsService.CheckOrgExistByIdAndNameWithUId(uid, id, name);
        }

        [HttpGet]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetMaxEnddate4Org(int uid)
        {
            var enddate = settingsService.GetMaxEnddate4Org(uid);
            if (enddate != null)
            {
                return Ok(new { Enddate = ((DateTime)enddate).ToString(FormatConstants.DateTimeFormatStringByDate) });
            }
            else
            {
                return Ok(new { Enddate = enddate });
            }

        }
        #endregion

        #region OrgBomAPI   by  justin

        [HttpPost]
        public IHttpActionResult QueryOrgBomsAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<OrgBomModelSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);
            var OrgBom = settingsService.QueryOrgBom(searchModel, page);
            return Ok(OrgBom);
        }

        [HttpGet]
        public IHttpActionResult QueryOrgBomAPI(int uid)
        {
            var dto = settingsService.QueryOrgBom(uid);
            return Ok(dto);
        }

        public string ModifyOrgBomAPI(SystemOrgBomDTO dto)
        {
            settingsService.ModifyOrgBom(dto);
            return "SUCCESS";
        }

        public string AddOrgBomAPI(SystemOrgBomDTO dto)
        {
            var ent = AutoMapper.Mapper.Map<System_OrganizationBOM>(dto);
            ent.Modified_Date = DateTime.Now;
            var plantstring = settingsService.AddOrgBom(ent);
            if (plantstring != "SUCCESS")
                return plantstring;
            else
                return "SUCCESS";
        }

        [HttpGet]
        public IHttpActionResult DoExportOrgBomAPI(string uids)
        {
            var listDto = settingsService.DoExportOrgBom(uids);
            return Ok(listDto);
        }

        [HttpGet]
        [IgnoreDBAuthorize]
        public bool CheckOrgBomExistAPI(int uid, int parentUId, int childUId, int index)
        {
            return settingsService.CheckOrgBomExist(uid, parentUId, childUId, index);
        }
        #endregion

        #region User Organization by Justin

        [HttpGet]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetOrgInfoAPI(string OrgID)
        {
            var orglist = settingsService.GetOrgInfo(OrgID);

            var result = AutoMapper.Mapper.Map<IEnumerable<SystemOrgDTO>>(orglist);
            return Ok(result);
        }

        [AcceptVerbs("Post")]
        public IHttpActionResult QueryUserOrgsAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<UserOrgModelSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);
            var userOrgs = settingsService.QueryUserOrgs(searchModel, page);
            return Ok(userOrgs);
        }

        [HttpGet]
        public IHttpActionResult QueryUserOrgsByAccountUIDAPI(int uuid)
        {
            var orgList = settingsService.QueryUserOrgsByAccountUID(uuid);
            var Users = commonService.GetSystemUserByUId(uuid);

            var result = new UserOrgEditModel
            {
                Account_UID = Users.Account_UID,
                User_NTID = Users.User_NTID,
                User_Name = Users.User_Name,
                UserOrgWithOrgs = orgList.AsEnumerable()
            };
            return Ok(result);
        }

        public string AddUserOrgAPI(dynamic data)
        {
            var entity = JsonConvert.DeserializeObject<UserOrgEditModel>(data.ToString());
            return settingsService.AddUserOrgWithSubs(entity);
        }

        public string ModifyUserOrgAPI(dynamic data)
        {
            var entity = JsonConvert.DeserializeObject<UserOrgEditModel>(data.ToString());
            return settingsService.ModifyUserOrgWithSubs(entity);
        }

        [HttpGet]
        public IHttpActionResult DoExportUserOrgAPI(string uids)
        {
            return Ok(settingsService.DoExportUserOrg(uids));
        }
        #endregion

        #region FuncPlant Maintenance---------------Add By Sidney 2016/02/23
        //[HttpPost]、[HttpGet]只是使用规范的问题，其在WEB层调用时需要与此匹配。同时IHttpActionResult表示返回HTTP的结果
        public string AddFuncPlantAPI(FuncPlantMaintanance fpm)
        {
            //获取Plant对应的Plant_UID
            var Plant_UID=settingsService.GetPlantByPlant(fpm.Plant).System_Plant_UID;
            SystemFunctionPlantDTO dto =new SystemFunctionPlantDTO();
            dto.System_FunPlant_UID = 0;
            dto.System_Plant_UID = Plant_UID;
            dto.OP_Types = fpm.OPType;
            dto.FunPlant = fpm.FunPlant;
            dto.FunPlant_Manager = fpm.Plant_Manager;
            dto.FunPlant_Contact = fpm.FuncPlant_Context;
            dto.Modified_UID = fpm.Modified_UID;
            dto.Modified_Date= DateTime.Now;
            var ent = AutoMapper.Mapper.Map<System_Function_Plant>(dto);
            var plantstring = settingsService.AddFuncPlant(ent);
            if (plantstring != "SUCCESS")
                return plantstring;
            else
                return "SUCCESS";
        }

        public System_Plant GetPlantByPlant(string Plant)
        {
            return settingsService.GetPlantByPlant(Plant);
        }
        [IgnoreDBAuthorize]
        [HttpPost]
        public IHttpActionResult QueryFuncPlantsAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<FuncPlantSearchModel>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);
            var FuncPlants = settingsService.QueryFuncPlants(searchModel, page);
            return Ok(FuncPlants);
        }
        [AcceptVerbs("Get")]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetPlantSingleAPI()
        {
            var EnumEntity = settingsService.GetPlantSingle();
            return Ok(EnumEntity);
        }
        [HttpGet]
        public IHttpActionResult QueryFuncPlantAPI(int uuid)
        {
            var dto = new FuncPlantMaintanance();
            dto = settingsService.QueryFuncPlant(uuid);
            return Ok(dto);
        }

        [HttpGet]
        public IHttpActionResult DeleteFuncPlantAPI(int uuid)
        {
            var dto = settingsService.DeleteFuncPlant(uuid);
            return Ok(dto);
        }
        public string ModifyFuncPlantAPI(FuncPlantMaintanance dto)
        {
            var ent = settingsService.QueryFuncPlantSingle(dto.System_FuncPlant_UID);
            ent.System_FunPlant_UID = dto.System_FuncPlant_UID;
            ent.OP_Types = dto.OPType;
            ent.FunPlant = dto.FunPlant;
            ent.FunPlant_Manager = dto.Plant_Manager;
            ent.FunPlant_Contact = dto.FuncPlant_Context;
            ent.Modified_UID = dto.Modified_UID;
            ent.Modified_Date = DateTime.Now;

            var plantstring = settingsService.ModifyFuncPlant(ent);
            return plantstring;
        }

        [HttpGet]
        public IHttpActionResult DoExportFuncPlantAPI(string uuids)
        {
            var listEnt = settingsService.DoExportFuncPlant(uuids);
            return Ok(listEnt);
        }

        
        #endregion

        #region 
        public string AddEnumeration(dynamic data)
        {
            var entity = JsonConvert.DeserializeObject<Enumeration>(data.ToString());
            return settingsService.AddEnumeration(entity);
        
        }

        public IHttpActionResult GetEnumNameForKeyProcess()
        {
            return Ok(settingsService.GetEnumNameForKeyProcess());
        }

        [AcceptVerbs("Post")]
        public IHttpActionResult GetEnumValueForKeyProcess(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<FlowChartModelSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);
            var enumerations = settingsService.GetEnumValueForKeyProcess(searchModel, page);
            return Ok(enumerations);
        }

        [AcceptVerbs("Post")]
        public IHttpActionResult DeleteKeyProcessAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var entity = JsonConvert.DeserializeObject<Enumeration>(jsonData);
            var enumerations = settingsService.DeleteEnumeration(entity.Enum_UID);
            return Ok(enumerations);
        }

        #endregion



    }
}