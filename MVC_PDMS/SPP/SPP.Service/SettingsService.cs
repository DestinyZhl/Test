using SPP.Data;
using SPP.Data.Infrastructure;
using SPP.Data.Repository;
using SPP.Model;
using SPP.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

using SPP.Common;

using SPP.Model.ViewModels.Settings;
using SPP.Common.Constants;
using SPP.Common.Helpers;
using MoreLinq;

namespace SPP.Service
{
    public interface ISettingsService
    {
        #region define System_User interface ---------------------Add by Tonny 2015/11/03

        PagedListModel<SystemUserDTO> QueryUsers(UserModelSearch search, Page page);
        IEnumerable<SystemUserDTO> DoExportUser(string uuids);
        System_Users QueryUsersSingle(int uuid);
        void AddUser(System_Users ent);
        void ModifyUser(System_Users ent);
        bool DeleteUser(System_Users ent);

        #endregion //define System_Users interface ---------------------Add by Tonny 2015/11/09

        #region All of Rock's interface -----------Start-----------Add by Rock 2015/11/18

        #region define System_BU_M interface ---------------------Add by Rock 2015/11/09
        PagedListModel<BUModelGet> QueryBus(BUModelSearch search, Page page);
        bool CheckBuExistById(string BU_ID, string BU_Name);
        void AddBU(System_BU_M bu);
        System_BU_M QueryBUSingle(int BU_M_UID);
        void ModifyBU(System_BU_M item);
        bool DeleteBU(System_BU_M item);

        IEnumerable<BUModelGet> DoExportBU(string uuids);
        #endregion //define System_BU_M interface ---------------------Add by Rock 2015/11/09

        #region define System_BU_D interface ---------------------Add by Rock 2015/11/12
        PagedListModel<BUDetailModelGet> QueryBUDs(BUDetailModelSearch search, Page page);
        bool CheckBuExistById_Two(string buid);
        SystemBUMDTO GetBUIDAndBUNameByBUID(string BU_ID);
        //bool CheckExistBU_D_ID(string BU_D_ID, int BU_D_UID, bool isEdit);
        void AddOrEditBUDetailInfo(SystemBUDDTO dto, string BU_ID, string BU_Name, bool isEdit);
        BUDetailModelGet QueryBUDSingle(int BU_D_UID);
        System_BU_D QueryBUDSingleByModule(int BU_D_UID);
        bool DeleteBUDSingle(System_BU_D budItem);
        IEnumerable<BUDetailModelGet> DoExportBUDetail(string BU_D_UIDS);
        string CheckBeginDateAndEndDate(string BU_ID, string BU_Name, string Begin_Date, string End_Date);
        SystemBUDDTO QueryBUDInfoByBU_D_ID(string BU_D_ID);
        #endregion //define System_BU_D interface ---------------------Add by Rock 2015/11/12

        #region define System_User_Business_Group ---------Start------------Add by Rock 2015/11/18
        PagedListModel<UserBUSettingGet> QueryUserBUs(UserBUSettingSearch search, Page page);
        List<BUAndBUDByUserNTID> QueryUserBU(int System_User_BU_UID);
        CustomBUItemAndBUD QueryBUAndBUDSByBUIDS(string BU_ID);
        string AddUserBU(UserBUAddOrSave addOrSave);
        string EditUserBU(UserBUAddOrSave addOrSave);
        IEnumerable<UserBUSettingGet> DoExportUserBU(string KeyIDS);
        #endregion define System_User_Business_Group ---------End------------Add by Rock 2015/11/18

        #endregion All of Rock's interface -----------End-----------Add by Rock 2015/11/18

        #region define System_Role interface ---------------------Add by Allen 2015/11/12
        PagedListModel<SystemRoleDTO> QueryRoles(RoleModelSearch search, Page page);
        IEnumerable<SystemRoleDTO> DoExportRole(string ruids);
        System_Role QueryRolesSingle(int uuid);
        void AddRole(System_Role ent);
        void ModifyRole(System_Role ent);
        string DeleteRole(System_Role ent);
        bool CheckRoleExistById(string rid);

        #endregion //define System_Role interface ---------------------Add by Allen 2015/11/12

        #region define Plant interface ---------------------Add by Sidney 2015/11/13
        PagedListModel<SystemPlantDTO> QueryPlants(PlantModelSearch search, Page page);
        IEnumerable<SystemPlantDTO> DoExportPlant(string uuids);
        System_Plant QueryPlantSingle(int uuid);
        string AddPlant(System_Plant ent);
        string ModifyPlant(System_Plant ent);
        string DeletePlant(System_Plant ent);
        bool CheckPlantExistByUId(int uuid);
        bool CheckPlantExistByPlant(string plant);
        DateTime? GetMaxEnddate4Plant(int uid);
        #endregion //define Plant interface ---------------------Add by Sidney 2015/11/13

        #region define System Function Maintenance interface ------------------ Add by Tonny 2015/11/11
        PagedListModel<FunctionItem> QueryFunctions(FunctionModelSearch searchModel, Page page);
        string DeleteFunction(int uid);
        string DeleteSubFunction(int subfun_uid);
        string AddFunctionWithSubs(FunctionWithSubs vm);
        System_Function QueryFunctionWithSubs(int uid);
        System_Function GetFunctionByID(string functionId);
        System_Function GetFunction(int uid);
        string ModifyFunctionWithSubs(FunctionWithSubs vm);
        IEnumerable<FunctionItem> DoExportFunction(string uids);
        #endregion //end define System Function Maintenance interface

        #region define User Plant Setting interface ------------------ Add by Sidney 2015/11/18
        PagedListModel<UserPlantItem> QueryUserPlants(UserPlantModelSearch searchModel, Page page);
        System_Plant GetPlantInfo(string Plant);
        string AddUserPlantWithSubs(UserPlantEditModel vm);
        IQueryable<UserPlantWithPlant> QueryUserPlantByAccountUID(int uid);
        string ModifyUserPlantWithSubs(UserPlantEditModel vm);
        IEnumerable<UserPlantItem> DoExportUserPlant(string uids);
        #endregion //end defineUser Plant Setting interface

        #region define Role Function Setting interface -------------- Add by Tonny 2015/11/18
        PagedListModel<RoleFunctionItem> QueryRoleFunctions(RoleFunctionSearchModel searchModel, Page page);
        RoleFunctionsWithSub QueryRoleFunction(int uid);
        IEnumerable<RoleFunctionItem> DoExportRoleFunctions(string uids);
        IEnumerable<SubFunction> QueryRoleSubFunctions(int roleUId, int functionUId);
        IEnumerable<SubFunction> QuerySubFunctionsByFunctionUID(int functionUId);
        void DeleteRoleFunction(int uid);
        string MaintainRoleFunctionWithSubs(RoleFunctionsWithSub vm);
        #endregion

        #region define System_User_Role interface ---------------------Add by Allen 2015/11/16
        PagedListModel<UserRoleItem> QueryUserRoles(UserRoleSearchModel search, Page page);
        IEnumerable<UserRoleItem> DoExportUserRole(string uruids);
        System_User_Role QueryUserRolesSingle(int uruid);
        void AddUserRole(System_User_Role ent);
        System_Role GetRoleNameById(string roleid);
        bool DeleteUserRole(System_User_Role ent);
        System_User_Role GetSystemUserRole(int accountUId, int roleUId);
        #endregion //define System_Role interface

        #region org  by justin
        PagedListModel<SystemOrgDTO> QueryOrgs(OrgModelSearch search, Page page);
        IEnumerable<SystemOrgDTO> DoExportOrg(string uids);
        System_Organization QueryOrgSingle(int uuid);
        void AddOrg(System_Organization ent);
        void ModifyOrg(System_Organization ent);
        bool DeleteOrg(int uid);
        bool CheckOrgExistByIdAndName(string id, string name);
        bool CheckOrgExistByIdAndNameWithUId(int uid, string id, string name);
        DateTime? GetMaxEnddate4Org(int orgUId);
        #endregion

        #region OrgBom  by justin 
        PagedListModel<SystemOrgAndBomDTO> QueryOrgBom(OrgBomModelSearch search, Page page);
        IEnumerable<SystemOrgAndBomDTO> DoExportOrgBom(string uuids);
        SystemOrgAndBomDTO QueryOrgBom(int uid);
        string AddOrgBom(System_OrganizationBOM ent);
        string ModifyOrgBom(SystemOrgBomDTO ent);
        bool CheckOrgBomExist(int uid, int parentUId, int childUId, int index);
        #endregion

        #region define User Organization Setting interface ------------------ Add by Justin
        PagedListModel<UserOrgItem> QueryUserOrgs(UserOrgModelSearch searchModel, Page page);
        IEnumerable<System_Organization> GetOrgInfo(string OrgID);
        string AddUserOrgWithSubs(UserOrgEditModel vm);
        IQueryable<UserOrgWithOrg> QueryUserOrgsByAccountUID(int uuid);
        string ModifyUserOrgWithSubs(UserOrgEditModel vm);
        IEnumerable<UserOrgItem> DoExportUserOrg(string uids);
        #endregion //end defineUser Plant Setting interface

        #region FuncPlant Maintanance--------------------------------Add By Sidney
        string AddFuncPlant(System_Function_Plant ent);
        System_Plant GetPlantByPlant(string Plant);
        PagedListModel<FuncPlantMaintanance> QueryFuncPlants(FuncPlantSearchModel search, Page page);
        List<string> GetPlantSingle();
        FuncPlantMaintanance QueryFuncPlant(int uuid);
        string DeleteFuncPlant(int uuid);
        System_Function_Plant QueryFuncPlantSingle(int uuid);
        string ModifyFuncPlant(System_Function_Plant ent);
        IEnumerable<FuncPlantMaintanance> DoExportFuncPlant(string uuids);
        #endregion

        #region 
        string AddEnumeration(Enumeration ent);
        bool DeleteEnumeration(int enum_uid);
        List<string> GetEnumNameForKeyProcess();
        PagedListModel<EnumerationDTO> GetEnumValueForKeyProcess(FlowChartModelSearch search, Page page);
        #endregion


    }
    public class SettingsService : ISettingsService
    {
        #region Private interfaces properties
        private readonly IUnitOfWork unitOfWork;
        private readonly ISystemFunctionRepository systemFunctionRepository;
        private readonly ISystemUserRepository systemUserRepository;
        private readonly ISystemRoleRepository systemRoleRepository;
        private readonly ISystemUserRoleRepository systemUserRoleRepository;
        private readonly ISystemBUMRepository systemBURepository;
        private readonly ISystemBUDRepository systemBUDRepository;
        private readonly ISystemUserBusinessGroupRepository systemUserBusinessGroupRepository;
        private readonly ISystemRoleFunctionRepository systemRoleFunctionRepository;
        private readonly ISystemPlantRepository systemPlantRepository;
        private readonly ISystemFunctionSubRepository systemFunctionSubRepository;
        private readonly ISystemRoleFunctionSubRepository systemRoleFunctionSubRepository;

        private readonly ISystemUserPlantRepository systemUserPlantRepository;
        private readonly IEnumerationRepository enumerationRepository;

        //org
        private readonly ISystemOrgRepository systemOrgRepository;
        //orgBom
        private readonly ISystemOrgBomRepository systemOrgBomRepository;

        private readonly ISystemUserOrgRepository systemUserOrgRepository;
        private readonly ISystemFunctionPlantRepository systemFunctionPlantRepository;
        private readonly ISystemUserFunPlantRepository systemUserFunPlantRepository;

        #endregion //Private interfaces properties

        #region Service constructor
        public SettingsService(
            ISystemFunctionRepository systemFunctionRepository,
            ISystemUserRepository systemUserRepository,
            ISystemUserRoleRepository systemUserRoleRepository,
            ISystemBUMRepository systemBURepository,
            ISystemBUDRepository systemBUDRepository,
            ISystemUserBusinessGroupRepository systemUserBusinessGroupRepository,
            ISystemRoleRepository systemRoleRepository,
            ISystemRoleFunctionRepository systemRoleFunctionRepository,
            ISystemPlantRepository systemPlantRepository,
            ISystemFunctionSubRepository systemFunctionSubRepository,
            ISystemRoleFunctionSubRepository systemRoleFunctionSubRepository,

            ISystemUserPlantRepository systemUserPlantRepository,

             ISystemOrgRepository systemOrgRepository,
            ISystemOrgBomRepository systemOrgBomRepository,

            ISystemUserOrgRepository systemUserOrgRepository,
            ISystemFunctionPlantRepository systemFunctionPlantRepository,
			ISystemUserFunPlantRepository systemUserFunPlantRepository,
            IEnumerationRepository enumerationRepository,
        IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.systemFunctionRepository = systemFunctionRepository;
            this.systemUserRepository = systemUserRepository;
            this.systemUserRoleRepository = systemUserRoleRepository;
            this.systemBURepository = systemBURepository;

            this.systemBUDRepository = systemBUDRepository;

            this.systemUserBusinessGroupRepository = systemUserBusinessGroupRepository;

            this.systemRoleRepository = systemRoleRepository;
            this.systemRoleFunctionRepository = systemRoleFunctionRepository;
            this.systemPlantRepository = systemPlantRepository;
            this.systemFunctionSubRepository = systemFunctionSubRepository;
            this.systemRoleFunctionSubRepository = systemRoleFunctionSubRepository;

            this.systemUserPlantRepository = systemUserPlantRepository;

            this.systemOrgRepository = systemOrgRepository;
            this.systemOrgBomRepository = systemOrgBomRepository;

            this.systemUserOrgRepository = systemUserOrgRepository;
            this.systemFunctionPlantRepository = systemFunctionPlantRepository;
            this.enumerationRepository = enumerationRepository;
            this.systemUserFunPlantRepository = systemUserFunPlantRepository;
        }
        #endregion //Service constructor

        #region System_Users implement interface---------------------------Add by Tonny 2015/11/14 

        public PagedListModel<SystemUserDTO> QueryUsers(UserModelSearch searchModel, Page page)
        {
            var totalCount = 0;
            var users = systemUserRepository.QueryUsers(searchModel, page, out totalCount);

            return new PagedListModel<SystemUserDTO>(totalCount, users);
        }

        public IEnumerable<SystemUserDTO> DoExportUser(string uuids)
        {
            var totalCount = 0;
            return systemUserRepository
                        .QueryUsers(new UserModelSearch { ExportUIds = uuids }, null, out totalCount)
                        .AsEnumerable();
        }

        public System_Users QueryUsersSingle(int uuid)
        {
            return systemUserRepository.GetById(uuid);
        }

        public void AddUser(System_Users ent)
        {
            systemUserRepository.Add(ent);
            unitOfWork.Commit();
        }

        public void ModifyUser(System_Users ent)
        {
            if (ent.Enable_Flag == false)
            {
                systemUserRoleRepository.Delete(q => q.Account_UID == ent.Account_UID);
            }
            systemUserRepository.Update(ent);
            unitOfWork.Commit();
        }

        public bool DeleteUser(System_Users ent)
        {
            if (systemUserRoleRepository.GetMany(r => r.Account_UID == ent.Account_UID).Count() == 0)
            {
                systemUserRepository.Delete(ent);
                unitOfWork.Commit();
                return true;
            }
            return false;
        }

        #endregion //System_Users implement interface---------------------------Add by Tonny 2015/11/14 

        #region  System_Role implement interface-------------------Add by Allen 2015/11/06

        public PagedListModel<SystemRoleDTO> QueryRoles(RoleModelSearch searchModel, Page page)
        {
            var totalCount = 0;
            var roles = systemRoleRepository.QueryRoles(searchModel, page, out totalCount);

            IList<SystemRoleDTO> rolesDTO = new List<SystemRoleDTO>();

            foreach (var role in roles)
            {
                var dto = AutoMapper.Mapper.Map<SystemRoleDTO>(role);
                dto.ModifiedUser = AutoMapper.Mapper.Map<SystemUserDTO>(role.System_Users);
                rolesDTO.Add(dto);
            }
            return new PagedListModel<SystemRoleDTO>(totalCount, rolesDTO);
        }

        //1.查詢 2.將EF Modal轉換成DTO 3.返回
        public IEnumerable<SystemRoleDTO> DoExportRole(string ruids)
        {
            var totalCount = 0;
            //查詢QueryRoles，已ruids參數去查詢

            var roles = systemRoleRepository
                        .QueryRoles(new RoleModelSearch { ExportUIds = ruids }, null, out totalCount).ToList();
            List<SystemRoleDTO> rolesDTO = new List<SystemRoleDTO>();
            //利用Mapper把EF一個一個轉換成DTO
            foreach (var role in roles)
            {
                var dto = AutoMapper.Mapper.Map<SystemRoleDTO>(role);
                dto.ModifiedUser = AutoMapper.Mapper.Map<SystemUserDTO>(role.System_Users);
                rolesDTO.Add(dto);
            }
            //返回DTO的值
            return rolesDTO;
        }

        public System_Role QueryRolesSingle(int uuid)
        {
            return systemRoleRepository.GetFirstOrDefault(r => r.Role_UID == uuid);
        }

        public void AddRole(System_Role ent)
        {
            systemRoleRepository.Add(ent);
            unitOfWork.Commit();
        }

        public void ModifyRole(System_Role ent)
        {
            systemRoleRepository.Update(ent);
            unitOfWork.Commit();
        }

        public string DeleteRole(System_Role ent)
        {
            if (systemRoleFunctionRepository.GetMany(r => r.Role_UID == ent.Role_UID).Count() != 0)
            {
                string error = "1";
                return error;
            }

            if (systemUserRoleRepository.GetMany(u => u.Role_UID == ent.Role_UID).Count() != 0)
            {

                string error = "2";
                return error;
            }
            else
            {
                systemRoleRepository.Delete(ent);
                unitOfWork.Commit();
                return "success";
            }
        }

        public bool CheckRoleExistById(string rid)
        {
            return systemRoleRepository.GetMany(p => p.Role_ID == rid).Count() == 0;
        }

        #endregion //System_Role implement interface-------------------Add by Allen 2015/11/06

        #region All of Rock's implement interface-------------------Start----------------Add by Rock 2015/11/18

        #region System_BU_M Module-------------------Add by Rock 2015/11/09
        /// <summary>
        /// 页面初始化加载
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public PagedListModel<BUModelGet> QueryBus(BUModelSearch searchModel, Page page)
        {
            var totalCount = 0;
            var bus = systemBURepository.QueryBUs(searchModel, page, out totalCount);
            IList<BUModelGet> busDTO = new List<BUModelGet>();

            foreach (var bu in bus)
            {
                busDTO.Add(new BUModelGet
                {
                    SystemBUMDTO = AutoMapper.Mapper.Map<SystemBUMDTO>(bu),
                    SystemUserDTO = AutoMapper.Mapper.Map<SystemUserDTO>(bu.System_Users)
                });
            }

            return new PagedListModel<BUModelGet>(totalCount, busDTO);
        }

        /// <summary>
        /// 根据buid检查是否存在
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        public bool CheckBuExistById(string buid = null, string buname = null)
        {
            var bus = systemBURepository.GetInfoByBU_ID(buid, buname);
            if (bus != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 新增BU信息
        /// </summary>
        /// <param name="bu"></param>
        public void AddBU(System_BU_M bu)
        {
            systemBURepository.Add(bu);
            unitOfWork.Commit();
        }

        /// <summary>
        /// 获取单笔信息
        /// </summary>
        /// <param name="BU_M_UID"></param>
        /// <returns></returns>
        public System_BU_M QueryBUSingle(int BU_M_UID)
        {
            return systemBURepository.GetById(BU_M_UID);
        }

        public void ModifyBU(System_BU_M item)
        {
            systemBURepository.Update(item);
            unitOfWork.Commit();
        }

        public bool DeleteBU(System_BU_M item)
        {
            var count1 = systemBUDRepository.GetMany(m => m.BU_M_UID == item.BU_M_UID).Count();
            var count2 = systemUserBusinessGroupRepository.GetMany(m => m.BU_M_UID == item.BU_M_UID).Count();
            if (count1 > 0 || count2 > 0)
            {
                return false;
            }
            else
            {
                systemBURepository.Delete(item);
                unitOfWork.Commit();
                return true;
            }
        }

        public IEnumerable<BUModelGet> DoExportBU(string uuids)
        {
            var totalCount = 0;
            var bus = systemBURepository.QueryBUs(new BUModelSearch { ExportUIds = uuids }, null, out totalCount);
            IList<BUModelGet> busDTO = new List<BUModelGet>();

            foreach (var bu in bus)
            {
                busDTO.Add(new BUModelGet
                {
                    SystemBUMDTO = AutoMapper.Mapper.Map<SystemBUMDTO>(bu),
                    SystemUserDTO = AutoMapper.Mapper.Map<SystemUserDTO>(bu.System_Users)
                });
            }

            return busDTO;
        }
        #endregion //System_BU_M implement interface-------------------Add by Rock 2015/11/09

        #region System_BU_D implement interface-------------------Add by Rock 2015/11/12

        /// <summary>
        /// 页面初始化加载
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public PagedListModel<BUDetailModelGet> QueryBUDs(BUDetailModelSearch searchModel, Page page)
        {
            var totalCount = 0;
            var bus = systemBUDRepository.QueryBUDetails(searchModel, page, out totalCount);
            IList<BUDetailModelGet> busDTO = new List<BUDetailModelGet>();

            foreach (var bu in bus)
            {
                busDTO.Add(SetAutoMapBUDetail(bu));
            }
            return new PagedListModel<BUDetailModelGet>(totalCount, busDTO);
        }

        private BUDetailModelGet SetAutoMapBUDetail(System_BU_D item)
        {
            BUDetailModelGet model = new BUDetailModelGet();
            model.SystemBUMDTO = AutoMapper.Mapper.Map<SystemBUMDTO>(item.System_BU_M);
            model.SystemBUDDTO = AutoMapper.Mapper.Map<SystemBUDDTO>(item);
            model.SystemUserDTO = AutoMapper.Mapper.Map<SystemUserDTO>(item.System_Users);
            return model;
        }

        /// <summary>
        /// 根据BU_D_ID或BUD信息
        /// </summary>
        /// <param name="BU_D_ID"></param>
        /// <returns></returns>
        public SystemBUDDTO QueryBUDInfoByBU_D_ID(string BU_D_ID)
        {
            //返回到前台的方法，所以如果为空必须手动new
            SystemBUDDTO dtoItem = new SystemBUDDTO();
            var item = systemBUDRepository.GetFirstOrDefault(q => q.BU_D_ID == BU_D_ID);
            if (item != null)
            {
                dtoItem = AutoMapper.Mapper.Map<SystemBUDDTO>(item);
            }
            return dtoItem;
        }

        /// <summary>
        /// 检查buid是否存在，如果存在则说明正确，不存在则不正确
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        public bool CheckBuExistById_Two(string buid)
        {
            var bus = systemBURepository.GetInfoByBU_ID(buid, null);
            if (bus != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据BU_ID获取BU_Name，将数据加载到画面上
        /// </summary>
        /// <param name="BU_ID"></param>
        /// <returns></returns>
        public SystemBUMDTO GetBUIDAndBUNameByBUID(string BU_ID)
        {
            var bum = systemBURepository.GetInfoByBU_ID(BU_ID, null);
            var bumDTO = AutoMapper.Mapper.Map<SystemBUMDTO>(bum);
            return bumDTO;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="BU_ID"></param>
        /// <param name="BU_Name"></param>
        /// <param name="isEdit"></param>
        public void AddOrEditBUDetailInfo(SystemBUDDTO dto, string BU_ID, string BU_Name, bool isEdit)
        {
            var item = new System_BU_D();
            dto.Modified_Date = DateTime.Now;
            //检查输入的主档是否存在
            var bum = systemBURepository.GetInfoByBU_ID(BU_ID, null);
            if (bum != null)
            {
                if (isEdit)
                {
                    item = systemBUDRepository.GetById(dto.BU_D_UID);
                    item.BU_D_ID = dto.BU_D_ID;
                    item.BU_D_Name = dto.BU_D_Name;
                    item.Begin_Date = dto.Begin_Date;
                    item.End_Date = dto.End_Date;
                    item.Modified_UID = dto.Modified_UID;
                    item.Modified_Date = dto.Modified_Date;
                    systemBUDRepository.Update(item);
                }
                else
                {
                    item = AutoMapper.Mapper.Map<System_BU_D>(dto);
                    item.BU_M_UID = bum.BU_M_UID;
                    systemBUDRepository.Add(item);
                }
                unitOfWork.Commit();
            }
        }

        public BUDetailModelGet QueryBUDSingle(int BU_D_UID)
        {
            BUDetailModelGet item = new BUDetailModelGet();
            var bud = systemBUDRepository.GetById(BU_D_UID);
            item.SystemBUDDTO = AutoMapper.Mapper.Map<SystemBUDDTO>(bud);
            item.SystemBUMDTO = AutoMapper.Mapper.Map<SystemBUMDTO>(bud.System_BU_M);
            return item;
        }

        public System_BU_D QueryBUDSingleByModule(int BU_D_UID)
        {
            var bud = systemBUDRepository.GetById(BU_D_UID);
            return bud;
        }

        public bool DeleteBUDSingle(System_BU_D budItem)
        {
            var count = systemUserBusinessGroupRepository.GetMany(m => m.BU_D_UID == budItem.BU_D_UID).Count();
            if (count == 0)
            {
                systemBUDRepository.Delete(budItem);
                unitOfWork.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<BUDetailModelGet> DoExportBUDetail(string BU_D_UIDS)
        {
            var totalCount = 0;
            var bus = systemBUDRepository.QueryBUDetails(new BUDetailModelSearch { ExportUIds = BU_D_UIDS }, null, out totalCount);
            IList<BUDetailModelGet> busDTO = new List<BUDetailModelGet>();

            foreach (var bu in bus)
            {
                busDTO.Add(SetAutoMapBUDetail(bu));
            }
            return busDTO;
        }

        public string CheckBeginDateAndEndDate(string BU_ID, string BU_Name, string Begin_Date, string End_Date)
        {
            var result = string.Empty;
            var buItem = systemBURepository.GetInfoByBU_ID(BU_ID, BU_Name);
            if (buItem != null)
            {
                if (buItem.Begin_Date > Convert.ToDateTime(Begin_Date))
                {
                    result = "BU Master Begin Date cann't be greater than BU Customer Begin Date";
                    return result;
                }
                if (buItem.End_Date != null)
                {
                    if (string.IsNullOrEmpty(End_Date))
                    {
                        result = "BU Customer End Date is not empty because BU Master End Date is not empty";
                        return result;
                    }
                    if (Convert.ToDateTime(End_Date) > buItem.End_Date)
                    {
                        result = "BU Customer End Date cann't be greater than BU Master End Date";
                        return result;
                    }
                }
            }
            return result;
        }

        #endregion //System_BU_D implement interface-------------------Add by Rock 2015/11/12

        #region System_User_Business_Group implement interface--------------------Start-----------Add by Rock 2015/11/18
        public PagedListModel<UserBUSettingGet> QueryUserBUs(UserBUSettingSearch search, Page page)
        {
            var totalCount = 0;
            var list = systemUserBusinessGroupRepository.QueryUserBUSettings(search, page, out totalCount);
            IList<UserBUSettingGet> listDTO = new List<UserBUSettingGet>();

            foreach (var item in list)
            {
                listDTO.Add(SetAutoMapUserBUSetting(item));
            }
            return new PagedListModel<UserBUSettingGet>(totalCount, listDTO);
        }

        /// <summary>
        /// 对象转换
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private UserBUSettingGet SetAutoMapUserBUSetting(System_User_Business_Group item)
        {
            UserBUSettingGet itemDTO = new UserBUSettingGet();
            itemDTO.SystemUserBusinessGroupDTO = AutoMapper.Mapper.Map<SystemUserBusinessGroupDTO>(item);
            itemDTO.SystemBUMDTO = AutoMapper.Mapper.Map<SystemBUMDTO>(item.System_BU_M);
            itemDTO.SystemBUDDTO = AutoMapper.Mapper.Map<CustomBUDDTO>(item.System_BU_D);
            itemDTO.SystemUserDTO = AutoMapper.Mapper.Map<SystemUserDTO>(item.System_Users);
            itemDTO.SystemUserDTO1 = AutoMapper.Mapper.Map<SystemUserDTO>(item.System_Users1);
            return itemDTO;
        }

        public IEnumerable<UserBUSettingGet> DoExportUserBU(string KeyIDS)
        {
            var totalCount = 0;
            var list = systemUserBusinessGroupRepository.QueryUserBUSettings(new UserBUSettingSearch { ExportUIds = KeyIDS }, null, out totalCount);
            IList<UserBUSettingGet> listDTO = new List<UserBUSettingGet>();

            foreach (var item in list)
            {
                listDTO.Add(SetAutoMapUserBUSetting(item));
            }
            return listDTO;
        }

        public List<BUAndBUDByUserNTID> QueryUserBU(int System_User_BU_UID)
        {
            var list = systemUserBusinessGroupRepository.QueryBUAndBUDByUserNTIDS(System_User_BU_UID);
            return list;
        }

        public CustomBUItemAndBUD QueryBUAndBUDSByBUIDS(string BU_ID)
        {
            var bubudItem = systemUserBusinessGroupRepository.QueryBUAndBUDSByBUIDS(BU_ID);
            CustomBUItemAndBUD item = new CustomBUItemAndBUD();
            if (bubudItem != null)
            {
                item.BU_M_UID = bubudItem.BU_M_UID;
                item.BU_ID = bubudItem.BU_ID;
                item.BU_Name = bubudItem.BU_Name;

                List<SystemBUDDTO> dtoList = new List<SystemBUDDTO>();
                foreach (var budItem in bubudItem.System_BU_D)
                {
                    dtoList.Add(AutoMapper.Mapper.Map<SystemBUDDTO>(budItem));
                }
                if (dtoList.Count() > 0)
                {
                    item.SystemBUDDTOList = dtoList;
                }
            }
            return item;
        }

        public string AddUserBU(UserBUAddOrSave addOrSave)
        {
            string errorInfo = string.Empty;
            UserBUAddOrSave newItem = SetUserBUItemValue(addOrSave);
            errorInfo = ValidUserBUDate(newItem);

            if (!string.IsNullOrEmpty(errorInfo))
            {
                return errorInfo;
            }

            //-------------第一步,根据用户名获取原先表里面对应的所有数据
            var userItem = systemUserRepository.GetFirstOrDefault(q => q.User_NTID == addOrSave.User_NTID);
            var userBUGroupList = systemUserBusinessGroupRepository.GetMany(q => q.Account_UID == userItem.Account_UID).AsEnumerable();

            List<System_User_Business_Group> UserBUList = new List<System_User_Business_Group>();
            foreach (var item in newItem.FunctionSubs)
            {
                System_User_Business_Group groupItem = new System_User_Business_Group();
                groupItem.Account_UID = userItem.Account_UID;
                groupItem.BU_M_UID = item.BU_M_UID;
                groupItem.BU_D_UID = item.BU_D_UID;
                groupItem.Begin_Date = item.Begin_Date;
                groupItem.End_Date = item.End_Date;
                groupItem.Modified_UID = newItem.Modified_UID;
                groupItem.Modified_Date = newItem.Modified_Date;
                UserBUList.Add(groupItem);
            }

            //第三步，对UserBUList这些数据进行涮选，去掉重复的
            var compare1 = ComparisonHelper<System_User_Business_Group>.CreateComparer(m => Tuple.Create(m.BU_M_UID, m.BU_D_UID));
            UserBUList = UserBUList.Distinct(compare1).ToList();


            //第四步，需要将原先DB表中的数据先排除掉
            foreach (var userBUGroupItem in userBUGroupList)
            {
                var hasItem = UserBUList.Where(m => m.BU_M_UID == userBUGroupItem.BU_M_UID && m.BU_D_UID == userBUGroupItem.BU_D_UID).FirstOrDefault();
                if (hasItem != null)
                {
                    UserBUList.Remove(hasItem);
                }
            }

            if (UserBUList.Count() == 0)
            {
                errorInfo = "This data has exist can't add this data";
                return errorInfo;
            }

            //第五步，插入数据
            foreach (var item in UserBUList)
            {
                systemUserBusinessGroupRepository.Add(item);
            }
            unitOfWork.Commit();
            return "Success";
        }

        public string EditUserBU(UserBUAddOrSave addOrSave)
        {
            string errorInfo = string.Empty;
            UserBUAddOrSave newItem = SetUserBUItemValue(addOrSave);
            errorInfo = ValidUserBUDate(newItem);

            if (!string.IsNullOrEmpty(errorInfo))
            {
                return errorInfo;
            }

            List<int> keyIdList = new List<int>();
            foreach (var item in newItem.FunctionSubs)
            {
                var userBUItem = systemUserBusinessGroupRepository.GetById(item.System_User_BU_UID);
                userBUItem.End_Date = item.End_Date;
                userBUItem.Modified_Date = DateTime.Now;
                userBUItem.Modified_UID = addOrSave.Modified_UID;

                systemUserBusinessGroupRepository.Update(userBUItem);
            }
            unitOfWork.Commit();
            return "Success";
        }

        private UserBUAddOrSave SetUserBUItemValue(UserBUAddOrSave addOrSave)
        {
            UserBUAddOrSave newItem = new UserBUAddOrSave();

            newItem.User_NTID = addOrSave.User_NTID;
            newItem.User_Name = addOrSave.User_Name;
            newItem.Modified_UID = addOrSave.Modified_UID;
            newItem.Modified_Date = DateTime.Now;
            //将画面上的数据插入到Model中
            var buidList = addOrSave.FunctionSubs.Select(m => m.BU_ID).Distinct().ToList();
            //从DB表中获取画面上所有BUID的信息
            var buInfoList = systemBURepository.GetMany(m => buidList.Contains(m.BU_ID)).ToList();

            //从DB表中获取画面上所有BUDID的信息
            var targetBUMUIds = buInfoList.Select(q => q.BU_M_UID);
            var budInfoList = systemBUDRepository.GetMany(m => targetBUMUIds.Contains(m.BU_M_UID)).ToList();
            List<CustomBUAndBUD> subList = new List<CustomBUAndBUD>();
            int i = 1;
            foreach (var item in addOrSave.FunctionSubs)
            {
                CustomBUAndBUD subItem = new CustomBUAndBUD();
                subItem.System_User_BU_UID = item.System_User_BU_UID;
                subItem.Begin_Date = item.Begin_Date;
                subItem.End_Date = item.End_Date;

                var buItem = buInfoList.Where(m => m.BU_ID == item.BU_ID).First();
                subItem.BU_M_UID = buItem.BU_M_UID;
                subItem.BU_BeginDate = buItem.Begin_Date;
                subItem.BU_EndDate = buItem.End_Date;
                subItem.RowNum = i++;
                if (item.BU_D_ID == ConstConstants.SelectAll)
                {
                    //获取所有匹配的BUD列表
                    var budList = budInfoList.Where(m => m.BU_M_UID == buItem.BU_M_UID).ToList();

                    foreach (var budItem in budList)
                    {
                        subItem.BU_D_UID = budItem.BU_D_UID;
                        subItem.BUD_BeginDate = budItem.Begin_Date;
                        subItem.BUD_EndDate = budItem.End_Date;
                        subList.Add(subItem);
                    }
                }
                else
                {
                    //如果BUD里面没有对应项目说明还没有建立BUD明细项
                    var budItem = budInfoList.Where(m => m.BU_D_ID == item.BU_D_ID).FirstOrDefault();
                    if (budItem != null)
                    {
                        subItem.BU_D_UID = budItem.BU_D_UID;
                    }
                    subList.Add(subItem);
                }
            }
            newItem.FunctionSubs = subList;
            return newItem;
        }

        private string ValidUserBUDate(UserBUAddOrSave addOrSave)
        {
            string errorInfo = string.Empty;
            foreach (var item in addOrSave.FunctionSubs)
            {
                if (item.BUD_BeginDate != null)
                {
                    if (item.Begin_Date < item.BUD_BeginDate)
                    {
                        errorInfo = string.Format("Incomplete context at row #{0}, Begin Date Can't less than BU Detail Begin Date.", item.RowNum);
                        break;
                    }
                }
                else
                {
                    if (item.Begin_Date < item.BU_BeginDate)
                    {
                        errorInfo = string.Format("Incomplete context at row #{0}, Begin Date Can't less than BU Master Begin Date.", item.RowNum);
                        break;

                    }
                }

                if (item.BUD_EndDate != null)
                {
                    if (item.End_Date == null)
                    {
                        errorInfo = string.Format("Incomplete context at row #{0}, End Date is not empty.", item.RowNum);
                        break;
                    }
                    if (item.End_Date > item.BUD_EndDate)
                    {
                        errorInfo = string.Format("Incomplete context at row #{0}, End Date Can't more than BU Detail End Date.", item.RowNum);
                        break;
                    }
                }
                else
                {
                    if (item.BU_EndDate != null)
                    {
                        if (item.End_Date == null)
                        {
                            errorInfo = string.Format("Incomplete context at row #{0}, End Date is not empty.", item.RowNum);
                            break;
                        }
                        if (item.End_Date > item.BU_EndDate)
                        {
                            errorInfo = string.Format("Incomplete context at row #{0}, End Date Can't less than BU Master End Date.", item.RowNum);
                            break;
                        }
                    }
                }
            }
            return errorInfo;
        }

        #endregion System_User_Business_Group implement interface--------------------End-----------Add by Rock 2015/11/18

        #endregion All of Rock's implement interface-------------------End----------------Add by Rock 2015/11/18

        #region System_Plant implement interface-------------------Add by Sidney 2015/11/12

        public PagedListModel<SystemPlantDTO> QueryPlants(PlantModelSearch searchModel, Page page)
        {
            var totalCount = 0;
            var plants = systemPlantRepository.QueryPlants(searchModel, page, out totalCount);

            IList<SystemPlantDTO> plantsDTO = new List<SystemPlantDTO>();

            foreach (var plant in plants)
            {
                var dto = AutoMapper.Mapper.Map<SystemPlantDTO>(plant);
                dto.ModifiedUser = AutoMapper.Mapper.Map<SystemUserDTO>(plant.System_Users);
                plantsDTO.Add(dto);
            }

            return new PagedListModel<SystemPlantDTO>(totalCount, plantsDTO);
        }
        public System_Plant QueryPlantSingle(int uuid)
        {
            return systemPlantRepository.GetById(uuid);
        }
        public string ModifyPlant(System_Plant ent)
        {
            systemPlantRepository.Update(ent);
            unitOfWork.Commit();
            return "SUCCESS";
        }

        public string AddPlant(System_Plant ent)
        {
            systemPlantRepository.Add(ent);
            unitOfWork.Commit();
            return "SUCCESS";
        }

        public string DeletePlant(System_Plant ent)
        {
            if (systemUserPlantRepository.GetMany(p => p.System_Plant_UID == ent.System_Plant_UID).Count() != 0)
            { return "FAIL"; }
            else
            {
                systemPlantRepository.Delete(ent);
                unitOfWork.Commit();
                return "SUCCESS";
            }
        }

        public IEnumerable<SystemPlantDTO> DoExportPlant(string uuids)
        {
            var totalCount = 0;
            var plants = systemPlantRepository.QueryPlants(new PlantModelSearch { ExportUIds = uuids }, null, out totalCount);

            IList<SystemPlantDTO> plantsDTO = new List<SystemPlantDTO>();

            foreach (var plant in plants)
            {
                var dto = AutoMapper.Mapper.Map<SystemPlantDTO>(plant);
                dto.ModifiedUser = AutoMapper.Mapper.Map<SystemUserDTO>(plant.System_Users);
                plantsDTO.Add(dto);
            }
            return plantsDTO;
        }
        public bool CheckPlantExistByUId(int uuid)
        {
            return systemPlantRepository.GetById(uuid) == null;
        }
        public bool CheckPlantExistByPlant(string plant)
        {
            return systemPlantRepository.GetMany(p => p.Plant == plant).Count() == 0;
        }

        public DateTime? GetMaxEnddate4Plant(int uid)
        {
            return systemUserPlantRepository.GetMaxEnddate4Plant(uid);
        }
        #endregion

        #region System Function Maintenance Module -------------- Add by Tonny 2015/11/12

        public PagedListModel<FunctionItem> QueryFunctions(FunctionModelSearch searchModel, Page page)
        {
            var totalCount = 0;
            var functions = systemFunctionRepository.QueryFunctions(searchModel, page, out totalCount).AsEnumerable();

            return new PagedListModel<FunctionItem>(totalCount, functions);
        }

        public string DeleteFunction(int uid)
        {
            if (systemFunctionRepository.GetById(uid) == null)
            {
                return "Record aleady deleted";
            }
            if (systemFunctionRepository.GetMany(r => r.Parent_Function_UID == uid).Count() > 0)
            {
                return "Can't delete since it is in used by other functions";
            }
            if (systemRoleFunctionRepository.GetMany(r => r.Function_UID == uid).Count() > 0)
            {
                return "Function aleady role assigned";
            }

            var subs = systemFunctionSubRepository.GetMany(f => f.Function_UID == uid);
            foreach (var item in subs)
            {
                var result = FunctionSubsCanDelete(item.System_FunctionSub_UID);
                if (result != "OK")
                {
                    return result;
                }
            }
            systemFunctionRepository.Delete(f => f.Function_UID == uid);
            unitOfWork.Commit();
            return "SUCCESS";
        }

        public System_Function QueryFunctionWithSubs(int uid)
        {
            return systemFunctionRepository.QueryFunctionWithSubs(uid);
        }

        public System_Function GetFunctionByID(string functionId)
        {
            var func = systemFunctionRepository.GetFirstOrDefault(p => p.Function_ID == functionId);
            return func;
        }

        public System_Function GetFunction(int uid)
        {
            var func = systemFunctionRepository.GetFirstOrDefault(p => p.Function_UID == uid);
            return func;
        }

        public string AddFunctionWithSubs(FunctionWithSubs vm)
        {
            var funcEntity = AutoMapper.Mapper.Map<System_Function>(vm);
            int? parentUId = null;

            //check parent_funtion_id and function_id if exist
            if (!string.IsNullOrEmpty(vm.Parent_Function_ID))
            {
                parentUId = systemFunctionRepository.GetFirstOrDefault(p => p.Function_ID == vm.Parent_Function_ID).Function_UID;
            }

            if (systemFunctionRepository.GetMany(p => p.Parent_Function_UID == parentUId && p.Function_ID == vm.Function_ID).Count() > 0)
            {
                return string.Format("item with same parent function Id [{0}] and function Id [{1}] is exist!", parentUId == null ? "NULL" : parentUId.ToString(), vm.Function_ID);
            }

            var now = DateTime.Now;
            funcEntity.Parent_Function_UID = parentUId;
            funcEntity.Modified_Date = now;
            funcEntity.URL = funcEntity.URL.ToUpper();

            foreach (var item in vm.FunctionSubs)
            {
                if (funcEntity.System_FunctionSub.Where(p => p.Sub_Fun == item.Sub_Fun).Count() > 0)
                {
                    return string.Format("Can't to add same Sub_Funs, The sub_fun is {0}", item.Sub_Fun);
                }

                var funcSub = AutoMapper.Mapper.Map<System_FunctionSub>(item);
                funcSub.Modified_UID = funcEntity.Modified_UID;
                funcSub.Modified_Date = now;
                funcSub.Sub_Fun_URL = funcSub.Sub_Fun_URL.ToUpper();
                funcEntity.System_FunctionSub.Add(funcSub);
            }

            systemFunctionRepository.Add(funcEntity);
            unitOfWork.Commit();

            return "SUCCESS";
        }

        public string ModifyFunctionWithSubs(FunctionWithSubs vm)
        {
            int? parentUId = null;
            //check parent_funtion_id and function_id if exist
            if (!string.IsNullOrEmpty(vm.Parent_Function_ID))
            {
                parentUId = systemFunctionRepository.GetFirstOrDefault(p => p.Function_ID == vm.Parent_Function_ID).Function_UID;
            }

            if (systemFunctionRepository.GetMany(p => p.Parent_Function_UID == parentUId && p.Function_ID == vm.Function_ID && p.Function_UID != vm.Function_UID).Count() > 0)
            {
                return string.Format("item with same parent function Id [{0}] and function Id [{1}] is exist!", parentUId == null ? "NULL" : parentUId.ToString(), vm.Function_ID);
            }

            var now = DateTime.Now;
            var funcEntity = systemFunctionRepository.GetFirstOrDefault(k => k.Function_UID == vm.Function_UID);
            if (funcEntity != null)
            {
                funcEntity.Function_Name = vm.Function_Name;
                funcEntity.Function_Desc = vm.Function_Desc;
                funcEntity.Function_ID = vm.Function_ID;
                funcEntity.Icon_ClassName = vm.Icon_ClassName;
                funcEntity.Is_Show = vm.Is_Show;
                funcEntity.Order_Index = vm.Order_Index;
                funcEntity.Parent_Function_UID = parentUId;
                funcEntity.URL = vm.URL.ToUpper();
                funcEntity.Mobile_URL = vm.Mobile_URL;
                funcEntity.Modified_UID = vm.Modified_UID;
                funcEntity.Modified_Date = now;
            }

            foreach (var item in vm.FunctionSubs)
            {
                var funcSub = AutoMapper.Mapper.Map<System_FunctionSub>(item);
                funcSub.Modified_UID = funcEntity.Modified_UID;
                funcSub.Modified_Date = now;

                var sub = funcEntity.System_FunctionSub.FirstOrDefault(p => p.Sub_Fun == item.Sub_Fun);

                if (sub == null)
                {
                    funcEntity.System_FunctionSub.Add(funcSub);
                }
                else
                {
                    sub.Function_UID = funcEntity.Function_UID;
                    sub.Sub_Fun_Name = item.Sub_Fun_Name;
                    sub.Sub_Fun_URL = item.Sub_Fun_URL.ToUpper();
                    sub.Modified_UID = funcEntity.Modified_UID;
                    sub.Modified_Date = now;
                }
            }

            systemFunctionRepository.Update(funcEntity);
            unitOfWork.Commit();

            return "SUCCESS";
        }

        private string FunctionSubsCanDelete(int function_sub_uid)
        {
            var entity = systemFunctionSubRepository.GetFirstOrDefault(p => p.System_FunctionSub_UID == function_sub_uid);
            if (entity == null)
            {
                return "Record aleady deleted";
            }
            if (systemRoleFunctionSubRepository.GetMany(p => p.System_FunctionSub_UID == function_sub_uid).Count() > 0)
            {
                return "Function aleady role assigned";
            }

            systemFunctionSubRepository.Delete(entity);
            return "OK";
        }

        public string DeleteSubFunction(int function_sub_uid)
        {
            var result = FunctionSubsCanDelete(function_sub_uid);
            if (result == "OK")
            {
                unitOfWork.Commit();
                return "SUCCESS";
            }
            else
            {
                return result;
            }

        }

        public IEnumerable<FunctionItem> DoExportFunction(string uids)
        {
            var totalCount = 0;
            return systemFunctionRepository
                    .QueryFunctions(new FunctionModelSearch { ExportUIds = uids }, null, out totalCount)
                    .AsEnumerable();
        }

        #endregion //End System Function Maintenance

        #region User Plant -------------- Add by Sidney 2015/11/17

        public System_Plant GetPlantInfo(string Plant)
        {
            var plantInfo = systemPlantRepository.GetFirstOrDefault(p => p.Plant == Plant);
            return plantInfo;
        }

        public PagedListModel<UserPlantItem> QueryUserPlants(UserPlantModelSearch searchModel, Page page)
        {
            var totalCount = 0;
            var functions = systemUserPlantRepository.QueryUserPlants(searchModel, page, out totalCount).AsEnumerable();

            return new PagedListModel<UserPlantItem>(totalCount, functions);
        }

        public IQueryable<UserPlantWithPlant> QueryUserPlantByAccountUID(int uuid)
        {
            return systemUserPlantRepository.QueryUserPlantByAccountUID(uuid);
        }

        public string AddUserPlantWithSubs(UserPlantEditModel vm)
        {
            var userplants = vm.UserPlantWithPlants;

            var distinctKeys = userplants.DistinctBy(k => k.System_Plant_UID);

            foreach (var userorg in distinctKeys)
            {
                var compareUserPlants = userplants.Where(k => k.System_Plant_UID == userorg.System_Plant_UID);
                var targetPlant = systemPlantRepository.GetById(userorg.System_Plant_UID);

                //取出新增数据的最小时间
                var minBeginDate = compareUserPlants.Min(q => q.User_Plant_Begin_Date);

                var notSetEndDateCount = compareUserPlants.Count(q => q.User_Plant_End_Date == null);
                if (notSetEndDateCount > 1)
                {
                    return string.Format("some plant which id is {0} with more than one EndDate is not set!", userorg.Plant);
                }
                foreach (var newUserPlant in compareUserPlants)
                {
                    var compareResult = DateCompareHelper.CompareInterval(
                            head: new DateCompareModel { Name = "Organization", BeginDate = targetPlant.Begin_Date, EndDate = targetPlant.End_Date }
                            , sub: new DateCompareModel { Name = "User Organization", BeginDate = newUserPlant.User_Plant_Begin_Date, EndDate = newUserPlant.User_Plant_End_Date });

                    if (compareResult != "PASS")
                    {
                        return compareResult;
                    }

                    var self = new UserPlantWithPlant[] { newUserPlant };

                    var invalidInputRecords =
                        compareUserPlants.Except(self).Where(q =>
                            (q.User_Plant_Begin_Date >= newUserPlant.User_Plant_Begin_Date && q.User_Plant_End_Date <= newUserPlant.User_Plant_End_Date)
                            || (q.User_Plant_Begin_Date <= newUserPlant.User_Plant_Begin_Date && q.User_Plant_End_Date >= newUserPlant.User_Plant_Begin_Date)
                            || (q.User_Plant_Begin_Date <= newUserPlant.User_Plant_End_Date && q.User_Plant_End_Date >= newUserPlant.User_Plant_End_Date)
                            || (q.User_Plant_End_Date == null && newUserPlant.User_Plant_End_Date > q.User_Plant_Begin_Date));

                    if (invalidInputRecords.Count() > 0)
                    {
                        return string.Format("Valid date of the pending insert has coincide zone, plant is {0}, data zone are [{1} ~ {2}] and [{3} ~ {4}]",
                            newUserPlant.Plant,
                            newUserPlant.User_Plant_Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate),
                            newUserPlant.User_Plant_End_Date == null ? "Endless" : ((DateTime)newUserPlant.User_Plant_End_Date).ToString(FormatConstants.DateTimeFormatStringByDate),
                            invalidInputRecords.First().User_Plant_Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate),
                            invalidInputRecords.First().User_Plant_End_Date == null ? "Endless" : ((DateTime)invalidInputRecords.First().User_Plant_End_Date).ToString(FormatConstants.DateTimeFormatStringByDate));
                    }
                }

                //根据最小时间查处可能有交集的所有时间
                var dbUserPlants =
                systemUserPlantRepository.GetMany(q => q.System_Plant_UID == userorg.System_Plant_UID
                                                && q.Account_UID == vm.Account_UID
                                                && (q.End_Date >= minBeginDate || q.End_Date == null))
                                        .Select(r => new { BeginDate = r.Begin_Date, EndDate = r.End_Date });

                //如果已有设置的记录
                if (dbUserPlants.Count() > 0)
                {
                    //如果设置记录未设置Enddate，则此记录无效
                    if (dbUserPlants.Where(q => q.EndDate == null).Count() > 0 && notSetEndDateCount > 0)
                    {
                        return "some org with more than one EndDate is not set!";
                    }
                    else
                    {
                        foreach (var newUserOrg in compareUserPlants)
                        {
                            //compare date with db
                            var invalidRecords =
                                dbUserPlants.Where(q =>
                                    (q.BeginDate >= newUserOrg.User_Plant_Begin_Date && q.EndDate <= newUserOrg.User_Plant_End_Date)
                                    || (q.BeginDate <= newUserOrg.User_Plant_Begin_Date && q.EndDate >= newUserOrg.User_Plant_Begin_Date)
                                    || (q.BeginDate <= newUserOrg.User_Plant_End_Date && q.EndDate >= newUserOrg.User_Plant_End_Date)
                                    || (q.EndDate == null && newUserOrg.User_Plant_End_Date > q.BeginDate));

                            if (invalidRecords.Count() > 0)
                            {
                                return string.Format("Valid date of the pending insert has coincide zone with exist data, plant is {0}, data zone are [{1} ~ {2}] and [{3} ~ {4}]",
                                    newUserOrg.Plant,
                                    newUserOrg.User_Plant_Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate),
                                    newUserOrg.User_Plant_End_Date == null ? "Endless" : ((DateTime)newUserOrg.User_Plant_End_Date).ToString(FormatConstants.DateTimeFormatStringByDate),
                                    invalidRecords.First().BeginDate.ToString(FormatConstants.DateTimeFormatStringByDate),
                                    invalidRecords.First().EndDate == null ? "Endless" : ((DateTime)invalidRecords.First().EndDate).ToString(FormatConstants.DateTimeFormatStringByDate));
                            }

                            //valid if exist
                            if (systemUserPlantRepository
                                    .GetMany(q => q.Account_UID == vm.Account_UID
                                            && q.System_Plant_UID == newUserOrg.System_Plant_UID
                                            && q.Begin_Date == newUserOrg.User_Plant_Begin_Date).Count() > 0)
                            {
                                return string.Format("Same data with org id [{0}], user [{1}] and Begin Date[{2}] already exist in system",
                                    newUserOrg.Plant, vm.User_Name, newUserOrg.User_Plant_Begin_Date);
                            }
                        }
                    }
                }
            }

            var now = DateTime.Now;
            foreach (var item in userplants)
            {
                var entity = new System_User_Plant();
                entity.Account_UID = vm.Account_UID;
                entity.Begin_Date = item.User_Plant_Begin_Date;
                entity.End_Date = item.User_Plant_End_Date;
                entity.System_Plant_UID = item.System_Plant_UID;
                entity.Modified_Date = now;
                entity.Modified_UID = vm.Modified_UID;
                systemUserPlantRepository.Add(entity);
            }

            unitOfWork.Commit();
            return "SUCCESS";
        }

        public string ModifyUserPlantWithSubs(UserPlantEditModel vm)
        {
            var userplants = vm.UserPlantWithPlants;

            var distinctKeys = userplants.DistinctBy(k => k.System_Plant_UID);

            foreach (var userorg in distinctKeys)
            {
                var compareUserPlants = userplants.Where(k => k.System_Plant_UID == userorg.System_Plant_UID);
                var modifyUserPlantsIds = userplants.Where(q => q.System_User_Plant_UID > 0).Select(r => r.System_User_Plant_UID);
                var targetPlant = systemPlantRepository.GetById(userorg.System_Plant_UID);

                //取出新增数据的最小时间
                var minBeginDate = compareUserPlants.Min(q => q.User_Plant_Begin_Date);

                var notSetEndDateCount = compareUserPlants.Count(q => q.User_Plant_End_Date == null);
                if (notSetEndDateCount > 1)
                {
                    return string.Format("some plant which id is {0} with more than one EndDate is not set!", userorg.Plant);
                }
                foreach (var newUserPlant in compareUserPlants)
                {
                    var compareResult = DateCompareHelper.CompareInterval(
                            head: new DateCompareModel { Name = "Organization", BeginDate = targetPlant.Begin_Date, EndDate = targetPlant.End_Date }
                            , sub: new DateCompareModel { Name = "User Organization", BeginDate = newUserPlant.User_Plant_Begin_Date, EndDate = newUserPlant.User_Plant_End_Date });

                    if (compareResult != "PASS")
                    {
                        return compareResult;
                    }

                    var self = new UserPlantWithPlant[] { newUserPlant };

                    var invalidInputRecords =
                        compareUserPlants.Except(self).Where(q =>
                            (q.User_Plant_Begin_Date >= newUserPlant.User_Plant_Begin_Date && q.User_Plant_End_Date <= newUserPlant.User_Plant_End_Date)
                            || (q.User_Plant_Begin_Date <= newUserPlant.User_Plant_Begin_Date && q.User_Plant_End_Date >= newUserPlant.User_Plant_Begin_Date)
                            || (q.User_Plant_Begin_Date <= newUserPlant.User_Plant_End_Date && q.User_Plant_End_Date >= newUserPlant.User_Plant_End_Date)
                            || (q.User_Plant_End_Date == null && newUserPlant.User_Plant_End_Date > q.User_Plant_Begin_Date));

                    if (invalidInputRecords.Count() > 0)
                    {
                        return string.Format("Valid date of the pending insert has coincide zone, plant is {0}, data zone are [{1} ~ {2}] and [{3} ~ {4}]",
                            newUserPlant.Plant,
                            newUserPlant.User_Plant_Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate),
                            newUserPlant.User_Plant_End_Date == null ? "Endless" : ((DateTime)newUserPlant.User_Plant_End_Date).ToString(FormatConstants.DateTimeFormatStringByDate),
                            invalidInputRecords.First().User_Plant_Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate),
                            invalidInputRecords.First().User_Plant_End_Date == null ? "Endless" : ((DateTime)invalidInputRecords.First().User_Plant_End_Date).ToString(FormatConstants.DateTimeFormatStringByDate));
                    }
                }

                //根据最小时间查处可能有交集的所有时间
                var dbUserPlants =
                systemUserPlantRepository.GetMany(q => q.System_Plant_UID == userorg.System_Plant_UID
                                                && q.Account_UID == vm.Account_UID
                                                && (q.End_Date >= minBeginDate || q.End_Date == null)).AsEnumerable();

                dbUserPlants = dbUserPlants.Where(q => !modifyUserPlantsIds.Contains(q.System_User_Plant_UID));

                //如果已有设置的记录
                if (dbUserPlants.Count() > 0)
                {
                    //如果设置记录未设置Enddate，则此记录无效
                    if (dbUserPlants.Where(q => q.End_Date == null).Count() > 0 && notSetEndDateCount > 0)
                    {
                        return "some org with more than one EndDate is not set!";
                    }
                    else
                    {
                        foreach (var newUserOrg in compareUserPlants)
                        {
                            //compare date with db
                            var invalidRecords =
                                dbUserPlants.Where(q =>
                                    (q.Begin_Date >= newUserOrg.User_Plant_Begin_Date && q.End_Date <= newUserOrg.User_Plant_End_Date)
                                    || (q.Begin_Date <= newUserOrg.User_Plant_Begin_Date && q.End_Date >= newUserOrg.User_Plant_Begin_Date)
                                    || (q.Begin_Date <= newUserOrg.User_Plant_End_Date && q.End_Date >= newUserOrg.User_Plant_End_Date)
                                    || (q.End_Date == null && newUserOrg.User_Plant_End_Date > q.Begin_Date));

                            if (invalidRecords.Count() > 0)
                            {
                                return string.Format("Valid date of the pending insert has coincide zone with exist data, plant is {0}, data zone are [{1} ~ {2}] and [{3} ~ {4}]",
                                    newUserOrg.Plant,
                                    newUserOrg.User_Plant_Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate),
                                    newUserOrg.User_Plant_End_Date == null ? "Endless" : ((DateTime)newUserOrg.User_Plant_End_Date).ToString(FormatConstants.DateTimeFormatStringByDate),
                                    invalidRecords.First().Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate),
                                    invalidRecords.First().End_Date == null ? "Endless" : ((DateTime)invalidRecords.First().End_Date).ToString(FormatConstants.DateTimeFormatStringByDate));
                            }

                            //valid if exist
                            if (systemUserPlantRepository
                                    .GetMany(q => q.Account_UID == vm.Account_UID
                                            && q.System_Plant_UID == newUserOrg.System_Plant_UID
                                            && q.Begin_Date == newUserOrg.User_Plant_Begin_Date).Count() > 0)
                            {
                                return string.Format("Same data with org id [{0}], user [{1}] and Begin Date[{2}] already exist in system",
                                    newUserOrg.Plant, vm.User_Name, newUserOrg.User_Plant_Begin_Date);
                            }
                        }
                    }
                }
            }

            var now = DateTime.Now;
            foreach (var item in userplants)
            {
                if (item.System_User_Plant_UID == 0)
                {
                    var entity = new System_User_Plant();
                    entity.Account_UID = vm.Account_UID;
                    entity.Begin_Date = item.User_Plant_Begin_Date;
                    entity.End_Date = item.User_Plant_End_Date;
                    entity.System_Plant_UID = item.System_Plant_UID;
                    entity.Modified_Date = now;
                    entity.Modified_UID = vm.Modified_UID;
                    systemUserPlantRepository.Add(entity);
                }
                else
                {
                    var entity = systemUserPlantRepository.GetById(item.System_User_Plant_UID);
                    entity.End_Date = item.User_Plant_End_Date;
                    entity.Modified_Date = now;
                    entity.Modified_UID = vm.Modified_UID;
                    systemUserPlantRepository.Update(entity);
                }
            }

            unitOfWork.Commit();
            return "SUCCESS";
        }

        public IEnumerable<UserPlantItem> DoExportUserPlant(string uids)
        {
            var totalCount = 0;
            return systemUserPlantRepository.QueryUserPlants(new UserPlantModelSearch { ExportUIds = uids }, null, out totalCount)
                    .AsEnumerable();
        }
        #endregion

        #region Role Function Setting Module -------------- Add by Tonny 2015/11/18

        public PagedListModel<RoleFunctionItem> QueryRoleFunctions(RoleFunctionSearchModel searchModel, Page page)
        {
            var totalCount = 0;
            var roleFunctions = systemRoleFunctionRepository.QueryRoleFunctions(searchModel, page, out totalCount).AsEnumerable();

            return new PagedListModel<RoleFunctionItem>(totalCount, roleFunctions);
        }

        public RoleFunctionsWithSub QueryRoleFunction(int uid)
        {
            var result = new RoleFunctionsWithSub();
            var role = systemRoleRepository.QueryRoleFunctionsByRoleUID(uid);

            result.Role_UID = role.Role_UID;
            result.Role_ID = role.Role_ID;
            result.Role_Name = role.Role_Name;

            foreach (var item in role.System_Role_Function)
            {
                result.HeadFunctions.Add(new HeadFunction
                {
                    System_Role_Function_UID = item.System_Role_Function_UID,
                    Function_UID = item.System_Function.Function_UID,
                    Function_ID = item.System_Function.Function_ID,
                    Function_Name = item.System_Function.Function_Name,
                    Is_Show = item.System_Function.Is_Show,
                    Order_Index = item.System_Function.Order_Index,
                    URL = item.System_Function.URL,
                    Is_Show_Role = item.Is_Show
                });
            }

            //fill subFun for first head function
            if (result.HeadFunctions.Count() > 0)
            {
                var firstFunction = result.HeadFunctions.First();
                var firstSub = systemRoleFunctionSubRepository.QueryRoleSubFunctionsByRoleUIDAndFunctionUID(uid, firstFunction.Function_UID);
                firstFunction.SubFun = firstSub.ToList();
            }
            return result;
        }

        public IEnumerable<SubFunction> QueryRoleSubFunctions(int roleUId, int functionUId)
        {
            var query = systemRoleFunctionSubRepository.QueryRoleSubFunctionsByRoleUIDAndFunctionUID(roleUId, functionUId);
            return query;
        }

        public IEnumerable<SubFunction> QuerySubFunctionsByFunctionUID(int functionUId)
        {
            return AutoMapper.Mapper.Map<IEnumerable<SubFunction>>(systemFunctionSubRepository.GetMany(q => q.Function_UID == functionUId));
        }

        public void DeleteRoleFunction(int uid)
        {
            systemRoleFunctionSubRepository.Delete(p => p.System_Role_Function_UID == uid);
            systemRoleFunctionRepository.Delete(p => p.System_Role_Function_UID == uid);
            unitOfWork.Commit();
        }

        public string MaintainRoleFunctionWithSubs(RoleFunctionsWithSub vm)
        {
            var roleUId = vm.Role_UID;
            var now = DateTime.Now;

            foreach (var func in vm.HeadFunctions)
            {
                var queryFunc = systemRoleFunctionRepository.GetFirstOrDefault(p => p.Function_UID == func.Function_UID && p.Role_UID == roleUId);
                if (queryFunc != null)
                {
                    #region 更新已存在项
                    queryFunc.Is_Show = func.Is_Show_Role;
                    queryFunc.Modified_UID = vm.Modified_UID;
                    queryFunc.Modified_Date = now;

                    systemRoleFunctionRepository.Update(queryFunc);

                    foreach (var sub in func.SubFun)
                    {
                        if (sub.System_Role_FunctionSub_UID > 0)
                        {
                            var entity = systemRoleFunctionSubRepository.GetById(sub.System_Role_FunctionSub_UID);
                            entity.Sub_Flag = sub.Grant;
                            entity.Modified_UID = vm.Modified_UID;
                            entity.Modified_Date = now;

                            systemRoleFunctionSubRepository.Update(entity);
                        }
                        else
                        {
                            systemRoleFunctionSubRepository.Add(new System_Role_FunctionSub
                            {
                                System_Role_Function_UID = queryFunc.System_Role_Function_UID,
                                Sub_Flag = sub.Grant,
                                System_FunctionSub_UID = sub.System_FunctionSub_UID,
                                Modified_UID = vm.Modified_UID,
                                Modified_Date = now
                            });
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 新增主项
                    var newFunc = new System_Role_Function
                    {
                        Role_UID = roleUId,
                        Function_UID = func.Function_UID,
                        Is_Show = func.Is_Show_Role,
                        Modified_UID = vm.Modified_UID,
                        Modified_Date = now
                    };


                    foreach (var sub in func.SubFun)
                    {
                        newFunc.System_Role_FunctionSub.Add(new System_Role_FunctionSub
                        {
                            Sub_Flag = sub.Grant,
                            System_FunctionSub_UID = sub.System_FunctionSub_UID,
                            Modified_UID = vm.Modified_UID,
                            Modified_Date = now
                        });
                    }

                    systemRoleFunctionRepository.Add(newFunc);
                    #endregion
                }

            }

            unitOfWork.Commit();

            return "SUCCESS";
        }

        public IEnumerable<RoleFunctionItem> DoExportRoleFunctions(string uids)
        {
            var totalCount = 0;
            return systemRoleFunctionRepository
                    .QueryRoleFunctions(new RoleFunctionSearchModel { ExportUIds = uids }, null, out totalCount)
                    .AsEnumerable();
        }

        #endregion

        #region System_User_Role Setting -------------------Add by Allen 2015/11/16
        public PagedListModel<UserRoleItem> QueryUserRoles(UserRoleSearchModel searchModel, Page page)
        {
            var totalCount = 0;
            var userroles = systemUserRoleRepository.QueryUserRoles(searchModel, page, out totalCount);
            return new PagedListModel<UserRoleItem>(totalCount, userroles);
        }

        public IEnumerable<UserRoleItem> DoExportUserRole(string uruids)
        {
            var totalCount = 0;
            return systemUserRoleRepository
                    .QueryUserRoles(new UserRoleSearchModel { ExportUIds = uruids }, null, out totalCount)
                    .AsEnumerable();
        }

        public System_User_Role QueryUserRolesSingle(int uruid)
        {
            return systemUserRoleRepository.GetFirstOrDefault(r => r.System_User_Role_UID == uruid);
        }

        public void AddUserRole(System_User_Role ent)
        {
            systemUserRoleRepository.Add(ent);
            unitOfWork.Commit();
        }
        /// <summary>
        /// ?
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public bool DeleteUserRole(System_User_Role ent)
        {
            if (systemUserRoleRepository.GetMany(r => r.System_User_Role_UID == ent.System_User_Role_UID).Count() != 0)
            {
                systemUserRoleRepository.Delete(ent);
                unitOfWork.Commit();
                return true;
            }
            return false;
        }

        public System_User_Role GetSystemUserRole(int accountUId, int roleUId)
        {
            return systemUserRoleRepository.GetFirstOrDefault(q => q.Account_UID == accountUId && q.Role_UID == roleUId);
        }

        /// <summary>
        ///檢查User NTID
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public System_Role GetRoleNameById(string roleid)
        {
            var userroleid = systemRoleRepository.GetFirstOrDefault(p => p.Role_ID == roleid);
            return userroleid;
        }
        #endregion //System_User_Role Module

        #region OrgBom by justin
        public PagedListModel<SystemOrgAndBomDTO> QueryOrgBom(OrgBomModelSearch searchModel, Page page)
        {
            var totalCount = 0;
            var OrgBoms = systemOrgBomRepository.QueryOrgBoms(searchModel, page, out totalCount);

            return new PagedListModel<SystemOrgAndBomDTO>(totalCount, OrgBoms);
        }

        public IEnumerable<SystemOrgAndBomDTO> DoExportOrgBom(string uuids)
        {
            var totalCount = 0;
            var OrgBoms = systemOrgBomRepository.QueryOrgBoms(new OrgBomModelSearch { ExportUIds = uuids }, null, out totalCount);

            return OrgBoms;
        }

        public SystemOrgAndBomDTO QueryOrgBom(int uid)
        {
            return systemOrgBomRepository.QueryOrgBom(uid);
        }

        public string AddOrgBom(System_OrganizationBOM ent)
        {
            ///保存时候验证bom关系的有效时间在child ORG的时间中， child org id 输入时候，需要先现在子ORG有效时间是否在父ORG时间中
            var childOrg = systemOrgRepository.GetById(ent.ChildOrg_UID);

            var compareResult = DateCompareHelper.CompareInterval(
               head: new DateCompareModel { Name = "Child Organization", BeginDate = childOrg.Begin_Date, EndDate = childOrg.End_Date }
               , sub: new DateCompareModel { Name = "Organization BOM", BeginDate = ent.Begin_Date, EndDate = ent.End_Date });

            if (compareResult != "PASS")
            {
                return compareResult;
            }

            if (ent.ParentOrg_UID != null)
            {
                var parentOrg = systemOrgRepository.GetById((int)ent.ParentOrg_UID);

                compareResult = DateCompareHelper.CompareInterval(
                    head: new DateCompareModel { Name = "Parent Organization", BeginDate = parentOrg.Begin_Date, EndDate = parentOrg.End_Date }
                    , sub: new DateCompareModel { Name = "Organization BOM", BeginDate = ent.Begin_Date, EndDate = ent.End_Date });

                if (compareResult != "PASS")
                {
                    return compareResult;
                }
            }

            systemOrgBomRepository.Add(ent);

            unitOfWork.Commit();
            return "SUCCESS";
        }

        public string ModifyOrgBom(SystemOrgBomDTO dto)
        {
            var ent = systemOrgBomRepository.GetById(dto.OrganizationBOM_UID);
            ent.ParentOrg_UID = dto.ParentOrg_UID;
            ent.ChildOrg_UID = dto.ChildOrg_UID;
            if (ent.End_Date == null && dto.End_Date != null) { ent.End_Date = dto.End_Date; };

            var childOrg = systemOrgRepository.GetById(ent.ChildOrg_UID);

            var compareResult = DateCompareHelper.CompareInterval(
               head: new DateCompareModel { Name = "Child Organization", BeginDate = childOrg.Begin_Date, EndDate = childOrg.End_Date }
               , sub: new DateCompareModel { Name = "Organization BOM", BeginDate = ent.Begin_Date, EndDate = ent.End_Date });

            if (compareResult != "PASS")
            {
                return compareResult;
            }

            if (ent.ParentOrg_UID != null)
            {
                var parentOrg = systemOrgRepository.GetById((int)ent.ParentOrg_UID);

                compareResult = DateCompareHelper.CompareInterval(
                    head: new DateCompareModel { Name = "Parent Organization", BeginDate = parentOrg.Begin_Date, EndDate = parentOrg.End_Date }
                    , sub: new DateCompareModel { Name = "Organization BOM", BeginDate = ent.Begin_Date, EndDate = ent.End_Date });

                if (compareResult != "PASS")
                {
                    return compareResult;
                }
            }

            ent.Modified_Date = DateTime.Now;
            ent.Modified_UID = dto.Modified_UID;
            ent.Order_Index = dto.Order_Index;

            systemOrgBomRepository.Update(ent);
            unitOfWork.Commit();
            return "SUCCESS";
        }

        public bool CheckOrgBomExist(int uid, int parentUId, int childUId, int index)
        {
            int? pUid = null;
            if (parentUId > 0) { pUid = parentUId; }

            if (uid == 0)
            {
                return systemOrgBomRepository
                            .GetMany(
                                q => q.ChildOrg_UID == childUId
                                && q.Order_Index == index
                                && q.ParentOrg_UID == pUid)
                            .Count() > 0;
            }
            else
            {
                return systemOrgBomRepository
                            .GetMany(
                                q => q.OrganizationBOM_UID != uid
                                && q.ChildOrg_UID == childUId
                                && q.Order_Index == index
                                && q.ParentOrg_UID == pUid)
                            .Count() > 0;
            }
        }

        #endregion

        #region Org by Justin
        public PagedListModel<SystemOrgDTO> QueryOrgs(OrgModelSearch searchModel, Page page)
        {
            var totalCount = 0;
            var Orgs = systemOrgRepository.QueryOrgs(searchModel, page, out totalCount);

            return new PagedListModel<SystemOrgDTO>(totalCount, Orgs);
        }

        public IEnumerable<SystemOrgDTO> DoExportOrg(string uids)
        {
            var totalCount = 0;
            var Orgs = systemOrgRepository.QueryOrgs(new OrgModelSearch { ExportUIds = uids }, null, out totalCount);
            return Orgs;
        }

        public System_Organization QueryOrgSingle(int uuid)
        {
            return systemOrgRepository.GetById(uuid);
        }

        public void AddOrg(System_Organization ent)
        {
            systemOrgRepository.Add(ent);
            unitOfWork.Commit();
        }

        public void ModifyOrg(System_Organization ent)
        {
            systemOrgRepository.Update(ent);
            unitOfWork.Commit();
        }

        public bool DeleteOrg(int uid)
        {
            if (systemOrgBomRepository.GetMany(q => q.ChildOrg_UID == uid || q.ParentOrg_UID == uid).Count() > 0)
            {
                return false;
            }

            if (systemUserOrgRepository.GetMany(q => q.Organization_UID == uid).Count() > 0)
            {
                return false;
            }

            systemOrgRepository.Delete(k => k.Organization_UID == uid);
            unitOfWork.Commit();
            return true;
        }

        public bool CheckOrgExistByIdAndName(string id, string name)
        {
            return systemOrgRepository.GetMany(p => p.Organization_ID == id && p.Organization_Name == name).Count() > 0;
        }

        public bool CheckOrgExistByIdAndNameWithUId(int uid, string id, string name)
        {
            return systemOrgRepository.GetMany(p => p.Organization_ID == id && p.Organization_Name == name && p.Organization_UID != uid).Count() > 0;
        }

        public DateTime? GetMaxEnddate4Org(int orgUId)
        {
            return systemOrgBomRepository.GetMaxEnddate4Org(orgUId);
        }
        #endregion

        #region User Organization by Justin
        public IEnumerable<System_Organization> GetOrgInfo(string OrgID)
        {
            var orglist = systemOrgRepository.GetMany(p => p.Organization_ID == OrgID);
            return orglist;
        }

        public PagedListModel<UserOrgItem> QueryUserOrgs(UserOrgModelSearch searchModel, Page page)
        {
            var totalCount = 0;
            var functions = systemUserOrgRepository.QueryUserOrgs(searchModel, page, out totalCount).AsEnumerable();

            return new PagedListModel<UserOrgItem>(totalCount, functions);
        }

        public IQueryable<UserOrgWithOrg> QueryUserOrgsByAccountUID(int uuid)
        {
            return systemUserOrgRepository.QueryUserOrgsByAccountUID(uuid);
        }

        public string AddUserOrgWithSubs(UserOrgEditModel vm)
        {
            var userorgs = vm.UserOrgWithOrgs;

            var distinctKeys = userorgs.DistinctBy(k => k.Organization_UID);

            foreach (var userorg in distinctKeys)
            {
                var compareUserOrgs = userorgs.Where(k => k.Organization_UID == userorg.Organization_UID);
                var targetOrg = systemOrgRepository.GetById(userorg.Organization_UID);

                //取出新增数据的最小时间
                var minBeginDate = compareUserOrgs.Min(q => q.UserOrg_Begin_Date);

                var notSetEndDateCount = compareUserOrgs.Count(q => q.UserOrg_End_Date == null);
                if (notSetEndDateCount > 1)
                {
                    return string.Format("some orginaztion which id is {0} with more than one EndDate is not set!", userorg.Organization_ID);
                }
                foreach (var newUserOrg in compareUserOrgs)
                {
                    var compareResult = DateCompareHelper.CompareInterval(
                            head: new DateCompareModel { Name = "Organization", BeginDate = targetOrg.Begin_Date, EndDate = targetOrg.End_Date }
                            , sub: new DateCompareModel { Name = "User Organization", BeginDate = newUserOrg.UserOrg_Begin_Date, EndDate = newUserOrg.UserOrg_End_Date });

                    if (compareResult != "PASS")
                    {
                        return compareResult;
                    }

                    var self = new UserOrgWithOrg[] { newUserOrg };

                    var invalidInputRecords =
                        compareUserOrgs.Except(self).Where(q =>
                            (q.UserOrg_Begin_Date >= newUserOrg.UserOrg_Begin_Date && q.UserOrg_End_Date <= newUserOrg.UserOrg_End_Date)
                            || (q.UserOrg_Begin_Date <= newUserOrg.UserOrg_Begin_Date && q.UserOrg_End_Date >= newUserOrg.UserOrg_Begin_Date)
                            || (q.UserOrg_Begin_Date <= newUserOrg.UserOrg_End_Date && q.UserOrg_End_Date >= newUserOrg.UserOrg_End_Date)
                            || (q.UserOrg_End_Date == null && newUserOrg.UserOrg_End_Date > q.UserOrg_Begin_Date));

                    if (invalidInputRecords.Count() > 0)
                    {
                        return string.Format("Valid date of the pending insert has coincide zone, org id is {0}, data zone are [{1} ~ {2}] and [{3} ~ {4}]",
                            newUserOrg.Organization_ID,
                            newUserOrg.UserOrg_Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate),
                            newUserOrg.UserOrg_End_Date == null ? "Endless" : ((DateTime)newUserOrg.UserOrg_End_Date).ToString(FormatConstants.DateTimeFormatStringByDate),
                            invalidInputRecords.First().UserOrg_Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate),
                            invalidInputRecords.First().UserOrg_End_Date == null ? "Endless" : ((DateTime)invalidInputRecords.First().UserOrg_End_Date).ToString(FormatConstants.DateTimeFormatStringByDate));
                    }
                }

                //根据最小时间查处可能有交集的所有时间
                var dbUserOrgs =
                systemUserOrgRepository.GetMany(q => q.Organization_UID == userorg.Organization_UID
                                                && q.Account_UID == vm.Account_UID
                                                && (q.End_Date >= minBeginDate || q.End_Date == null))
                                        .Select(r => new { BeginDate = r.Begin_Date, EndDate = r.End_Date });

                //如果已有设置的记录
                if (dbUserOrgs.Count() > 0)
                {
                    //如果设置记录未设置Enddate，则此记录无效
                    if (dbUserOrgs.Where(q => q.EndDate == null).Count() > 0 && notSetEndDateCount > 0)
                    {
                        return "some org with more than one EndDate is not set!";
                    }
                    else
                    {
                        foreach (var newUserOrg in compareUserOrgs)
                        {
                            //compare date with db
                            var invalidRecords =
                                dbUserOrgs.Where(q =>
                                    (q.BeginDate >= newUserOrg.UserOrg_Begin_Date && q.EndDate <= newUserOrg.UserOrg_End_Date)
                                    || (q.BeginDate <= newUserOrg.UserOrg_Begin_Date && q.EndDate >= newUserOrg.UserOrg_Begin_Date)
                                    || (q.BeginDate <= newUserOrg.UserOrg_End_Date && q.EndDate >= newUserOrg.UserOrg_End_Date)
                                    || (q.EndDate == null && newUserOrg.UserOrg_End_Date > q.BeginDate));

                            if (invalidRecords.Count() > 0)
                            {
                                return string.Format("Valid date of the pending insert has coincide zone with exist data, org id is {0}, data zone are [{1} ~ {2}] and [{3} ~ {4}]",
                                    newUserOrg.Organization_ID,
                                    newUserOrg.UserOrg_Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate),
                                    newUserOrg.UserOrg_End_Date == null ? "Endless" : ((DateTime)newUserOrg.UserOrg_End_Date).ToString(FormatConstants.DateTimeFormatStringByDate),
                                    invalidRecords.First().BeginDate.ToString(FormatConstants.DateTimeFormatStringByDate),
                                    invalidRecords.First().EndDate == null ? "Endless" : ((DateTime)invalidRecords.First().EndDate).ToString(FormatConstants.DateTimeFormatStringByDate));
                            }

                            //valid if exist
                            if (systemUserOrgRepository
                                    .GetMany(q => q.Account_UID == vm.Account_UID
                                            && q.Organization_UID == newUserOrg.Organization_UID
                                            && q.Begin_Date == newUserOrg.UserOrg_Begin_Date).Count() > 0)
                            {
                                return string.Format("Same data with org id [{0}], user [{1}] and Begin Date[{2}] already exist in system",
                                    newUserOrg.Organization_ID, vm.User_Name, newUserOrg.UserOrg_Begin_Date);
                            }
                        }
                    }
                }
            }

            var now = DateTime.Now;

            foreach (var item in userorgs)
            {
                var newUserOrg = new System_UserOrg();

                newUserOrg.Account_UID = vm.Account_UID;
                newUserOrg.Begin_Date = item.UserOrg_Begin_Date;
                newUserOrg.End_Date = item.UserOrg_End_Date;
                newUserOrg.Organization_UID = item.Organization_UID;
                newUserOrg.Modified_Date = now;
                newUserOrg.Modified_UID = vm.Modified_UID;
                systemUserOrgRepository.Add(newUserOrg);
            }

            unitOfWork.Commit();
            return "SUCCESS";
        }

        public string ModifyUserOrgWithSubs(UserOrgEditModel vm)
        {
            var userorgs = vm.UserOrgWithOrgs;

            var distinctKeys = userorgs.DistinctBy(k => k.Organization_UID);
            var modifyUserOrgsIds = userorgs.Where(q => q.System_UserOrgUID > 0).Select(r => r.System_UserOrgUID);

            foreach (var userorg in distinctKeys)
            {
                var compareUserOrgs = userorgs.Where(k => k.Organization_UID == userorg.Organization_UID);
                var targetOrg = systemOrgRepository.GetById(userorg.Organization_UID);

                //取出新增数据的最小时间
                var minBeginDate = compareUserOrgs.Min(q => q.UserOrg_Begin_Date);

                var notSetEndDateCount = compareUserOrgs.Count(q => q.UserOrg_End_Date == null);
                if (notSetEndDateCount > 1)
                {
                    return string.Format("some orginaztion witch id is {0} with more than one EndDate is not set!", userorg.Organization_ID);
                }
                foreach (var newUserOrg in compareUserOrgs)
                {
                    var compareResult = DateCompareHelper.CompareInterval(
                        head: new DateCompareModel { Name = "Organization", BeginDate = targetOrg.Begin_Date, EndDate = targetOrg.End_Date }
                        , sub: new DateCompareModel { Name = "User Organization", BeginDate = newUserOrg.UserOrg_Begin_Date, EndDate = newUserOrg.UserOrg_End_Date });

                    if (compareResult != "PASS")
                    {
                        return compareResult;
                    }

                    var self = new UserOrgWithOrg[] { newUserOrg };

                    var invalidInputRecords =
                        compareUserOrgs.Except(self).Where(q =>
                            (q.UserOrg_Begin_Date >= newUserOrg.UserOrg_Begin_Date && q.UserOrg_End_Date <= newUserOrg.UserOrg_End_Date)
                            || (q.UserOrg_Begin_Date <= newUserOrg.UserOrg_Begin_Date && q.UserOrg_End_Date >= newUserOrg.UserOrg_Begin_Date)
                            || (q.UserOrg_Begin_Date <= newUserOrg.UserOrg_End_Date && q.UserOrg_End_Date >= newUserOrg.UserOrg_End_Date)
                            || (q.UserOrg_End_Date == null && newUserOrg.UserOrg_End_Date > q.UserOrg_Begin_Date));

                    if (invalidInputRecords.Count() > 0)
                    {
                        return string.Format("Valid date of the pending insert has coincide zone, org id is {0}, data zone are [{1} ~ {2}] and [{3} ~ {4}]",
                            newUserOrg.Organization_ID,
                            newUserOrg.UserOrg_Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate),
                            newUserOrg.UserOrg_End_Date == null ? "Endless" : ((DateTime)newUserOrg.UserOrg_End_Date).ToString(FormatConstants.DateTimeFormatStringByDate),
                            invalidInputRecords.First().UserOrg_Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate),
                            invalidInputRecords.First().UserOrg_End_Date == null ? "Endless" : ((DateTime)invalidInputRecords.First().UserOrg_End_Date).ToString(FormatConstants.DateTimeFormatStringByDate));
                    }
                }

                //根据最小时间查处可能有交集的所有时间
                var dbUserOrgs =
                systemUserOrgRepository.GetMany(q => q.Organization_UID == userorg.Organization_UID
                                                && q.Account_UID == vm.Account_UID
                                                && (q.End_Date >= minBeginDate || q.End_Date == null)).AsEnumerable();

                dbUserOrgs = dbUserOrgs.Where(q => !modifyUserOrgsIds.Contains(q.System_UserOrgUID));

                //如果已有设置的记录
                if (dbUserOrgs.Count() > 0)
                {
                    //如果设置记录未设置Enddate，则此记录无效
                    if (dbUserOrgs.Where(q => q.End_Date == null).Count() > 0 && notSetEndDateCount > 0)
                    {
                        return "some org with more than one EndDate is not set!";
                    }
                    else
                    {
                        foreach (var newUserOrg in compareUserOrgs)
                        {
                            //compare date with db
                            var invalidRecords =
                                dbUserOrgs.Where(q =>
                                    (q.Begin_Date >= newUserOrg.UserOrg_Begin_Date && q.End_Date <= newUserOrg.UserOrg_End_Date)
                                    || (q.Begin_Date <= newUserOrg.UserOrg_Begin_Date && q.End_Date >= newUserOrg.UserOrg_Begin_Date)
                                    || (q.Begin_Date <= newUserOrg.UserOrg_End_Date && q.End_Date >= newUserOrg.UserOrg_End_Date)
                                    || (q.End_Date == null && newUserOrg.UserOrg_End_Date > q.Begin_Date));

                            if (invalidRecords.Count() > 0)
                            {
                                return string.Format("Valid date of the pending insert has coincide zone with exist data, org id is {0}, data zone are [{1} ~ {2}] and [{3} ~ {4}]",
                                    newUserOrg.Organization_ID,
                                    newUserOrg.UserOrg_Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate),
                                    newUserOrg.UserOrg_End_Date == null ? "Endless" : ((DateTime)newUserOrg.UserOrg_End_Date).ToString(FormatConstants.DateTimeFormatStringByDate),
                                    invalidRecords.First().Begin_Date.ToString(FormatConstants.DateTimeFormatStringByDate),
                                    invalidRecords.First().End_Date == null ? "Endless" : ((DateTime)invalidRecords.First().End_Date).ToString(FormatConstants.DateTimeFormatStringByDate));
                            }

                            //valid if exist
                            if (newUserOrg.System_UserOrgUID == 0)
                            {
                                if (systemUserOrgRepository
                                    .GetMany(q => q.Account_UID == vm.Account_UID
                                            && q.Organization_UID == newUserOrg.Organization_UID
                                            && q.Begin_Date == newUserOrg.UserOrg_Begin_Date).Count() > 0)
                                {
                                    return string.Format("Same data with org id [{0}], user [{1}] and Begin Date[{2}] already exist in system",
                                        newUserOrg.Organization_ID, vm.User_Name, newUserOrg.UserOrg_Begin_Date);
                                }
                            }
                        }
                    }
                }
            }

            var now = DateTime.Now;

            foreach (var item in userorgs)
            {
                if (item.System_UserOrgUID > 0)
                {
                    var entity = systemUserOrgRepository.GetById(item.System_UserOrgUID);
                    entity.End_Date = item.UserOrg_End_Date;
                    entity.Modified_Date = now;
                    entity.Modified_UID = vm.Modified_UID;
                    systemUserOrgRepository.Update(entity);
                }
                else
                {
                    var newUserOrg = new System_UserOrg();

                    newUserOrg.Account_UID = vm.Account_UID;
                    newUserOrg.Begin_Date = item.UserOrg_Begin_Date;
                    newUserOrg.End_Date = item.UserOrg_End_Date;
                    newUserOrg.Organization_UID = item.Organization_UID;
                    newUserOrg.Modified_Date = now;
                    newUserOrg.Modified_UID = vm.Modified_UID;
                    systemUserOrgRepository.Add(newUserOrg);
                }
            }

            unitOfWork.Commit();
            return "SUCCESS";
        }

        public IEnumerable<UserOrgItem> DoExportUserOrg(string uids)
        {
            var totalCount = 0;
            return systemUserOrgRepository.QueryUserOrgs(new UserOrgModelSearch { ExportUIds = uids }, null, out totalCount)
                    .AsEnumerable();
        }
        #endregion

        #region FuncPlant Maintanance--------------------------------Add By Sidney
        public string AddFuncPlant(System_Function_Plant ent)
        {
            systemFunctionPlantRepository.Add(ent);
            unitOfWork.Commit();
            return "SUCCESS";
        }

        public System_Plant GetPlantByPlant(string Plant)
        {
            return systemPlantRepository.GetPlantByPlant(Plant);
        }

        public PagedListModel<FuncPlantMaintanance> QueryFuncPlants(FuncPlantSearchModel search, Page page)
        {
            var totalCount = 0;
            var plants = systemFunctionPlantRepository.QueryFuncPlants(search, page, out totalCount);

            IList<FuncPlantMaintanance> plantsDTO = new List<FuncPlantMaintanance>();

            foreach (var plant in plants)
            {
                var dto = AutoMapper.Mapper.Map<FuncPlantMaintanance>(plant);
                plantsDTO.Add(dto);
            }

            return new PagedListModel<FuncPlantMaintanance>(totalCount, plantsDTO);
        }

        public List<string> GetPlantSingle()
        {
            var allentity = systemPlantRepository.GetAll();
            var entity = allentity.Where(p=>p.Name_0== "Metal Chengdu").Select(p => p.Plant).Distinct().ToList();
            return entity;
        }

        public FuncPlantMaintanance QueryFuncPlant(int uuid)
        {
            var result= systemFunctionPlantRepository.QueryFuncPlant(uuid);
            return result;
        }
        public string DeleteFuncPlant(int uuid)
        {
            try
            {
                if (systemUserFunPlantRepository.checkFuncPlantIsExit(uuid) > 0)
                    return "FuncPlant_Is_Exit";
                else
                {
                    var result = systemFunctionPlantRepository.GetById(uuid);
                    systemFunctionPlantRepository.Delete(result);
                    unitOfWork.Commit();
                    return "SUCCESS";
                } 
            }
            catch (Exception)
            {

                return "FAIL";
            }
            
        }

        public System_Function_Plant QueryFuncPlantSingle(int uuid)
        {
            return systemFunctionPlantRepository.GetById(uuid);
        }

        public string ModifyFuncPlant(System_Function_Plant ent)
        {
            try
            {
                systemFunctionPlantRepository.Update(ent);
                unitOfWork.Commit();
                return "SUCCESS";
            }
            catch (Exception e)
            {
                return e.ToString();

            }
            
        }

        public IEnumerable<FuncPlantMaintanance> DoExportFuncPlant(string uuids)
        {
            var totalCount = 0;
            var plants = systemFunctionPlantRepository.QueryFuncPlants(new FuncPlantSearchModel(){ ExportUIds = uuids }, null, out totalCount);
            return plants;

        }

        #endregion

        #region key process by destiny

        public bool DeleteEnumeration(int enum_uid)
        {
            enumerationRepository.Delete(k => k.Enum_UID == enum_uid);
            unitOfWork.Commit();
            return true;
        }


        public string AddEnumeration(Enumeration ent)
        {
            enumerationRepository.Add(ent);
            unitOfWork.Commit();
            return "SUCCESS";
        }


        public List<string> GetEnumNameForKeyProcess()
        {
            return enumerationRepository.GetEnumNameForKeyProcess();
        }

        public PagedListModel<EnumerationDTO> GetEnumValueForKeyProcess(FlowChartModelSearch search, Page page)
        {
            var enumerations = enumerationRepository.GetEnumValueForKeyProcess(search.Part_Types);
            var totalCount = 0;
            return new PagedListModel<EnumerationDTO>(totalCount, enumerations);
        }

        #endregion

    }
}
