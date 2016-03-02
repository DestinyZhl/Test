using SPP.Data;
using SPP.Data.Infrastructure;
using SPP.Data.Repository;
using SPP.Model;
using SPP.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using SPP.Common.Helpers;

namespace SPP.Service
{
    /// <summary>
    /// Interface
    /// </summary>
    public interface IEventReportManagerService
    {
        List<Enumeration> GetIntervalTime(string PageName);
        List<string> GetAllCustomer();
        List<string> GetAllProject(string customer);
        List<string> GetAllProductPhase(string customer, string project);
        List<string> GetAllPartTypes(string customer, string project, string productphase);
        List<string> GetAllColor(string customer, string project, string productphase, string parttypes);
        PagedListModel<PPCheckDataItem> QueryPPCheckDatas(PPCheckDataSearch searchModel, Page page, string QueryType);
        PagedListModel<Daily_ProductReportItem> QueryReportDatas(ReportDataSearch searchModel, Page page, string QueryType);
        string EditWIP(PPEditWIP WIP,int modified_UID);
        string EditWIPView(int product_uid, int wip_qty, int wip_old, int wip_add, string comment, int modifiedUser);
        IntervalEnum GetIntervalInfo(string opType);
        PagedListModel<WarningListVM> GetWarningLists(int user_account_uid);
        ProcessDataSearch GetWarningDataByWarningUid(int WarningUid);
        List<string> CheckFunPlantDataIsFull(PPCheckDataSearch searchModel);
        List<string> GetAlVersion(string customer, string project, string productphase, string parttypes,
            DateTime beginTime, DateTime endTime);
        VersionBeginEndDate GetVersionBeginEndDate(string customer, string project, string productphase,
            string parttypes, int version);
    }
    /// <summary>
    /// EventReportManagerService: IEventReportManagerService
    /// </summary>
    public class EventReportManagerService : IEventReportManagerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEnumerationRepository EnumerationRepository;
        private readonly ISystemBUDRepository SystemBUDRepository;
        private readonly ISystemProjectRepository SystemProjectRepository;
        private readonly IFlowChartMasterRepository FlowChartMasterRepository;
        private readonly IFlowChartDetailRepository FlowChartDetailRepository;
        private readonly ISystemUserRepository systemUserRepository;
        private readonly IProductInputRepository ProductInputRepository;
        private readonly IProductInputHistoryRepository ProductInputHistoryRepository;
        private readonly IWarningListRepository warningListRepository;
        private readonly IWIPChangeHistoryRepository WIPChangeHistoryRepository;
        public EventReportManagerService(
            IUnitOfWork unitOfWork,
            IEnumerationRepository EnumerationRepository,
            ISystemBUDRepository SystemBUDRepository,
            ISystemProjectRepository SystemProjectRepository,
            IFlowChartMasterRepository FlowChartMasterRepository,
            IFlowChartDetailRepository FlowChartDetailRepository,
            ISystemUserRepository systemUserRepository,
            IProductInputRepository ProductInputRepository,
            IProductInputHistoryRepository ProductInputHistoryRepository,
            IWarningListRepository warningListRepository,
            IWIPChangeHistoryRepository WIPChangeHistoryRepository
            )
        {
            this.unitOfWork = unitOfWork;
            this.EnumerationRepository = EnumerationRepository;
            this.SystemBUDRepository = SystemBUDRepository;
            this.SystemProjectRepository = SystemProjectRepository;
            this.FlowChartMasterRepository = FlowChartMasterRepository;
            this.FlowChartDetailRepository = FlowChartDetailRepository;
            this.systemUserRepository = systemUserRepository;
            this.ProductInputRepository = ProductInputRepository;
            this.ProductInputHistoryRepository = ProductInputHistoryRepository;
            this.warningListRepository = warningListRepository;
            this.WIPChangeHistoryRepository = WIPChangeHistoryRepository;
        }

        #region  WarningList-----add by Destiny Zhang 2015/12/21

        public PagedListModel<WarningListVM> GetWarningLists(int user_account_uid)
        {
            int totalCount = 0;
            var warningListData = warningListRepository.GetWarninglistDatas(user_account_uid, out totalCount).ToList();

            List<WarningListVM> tempList = new List<WarningListVM>();
            foreach (var VARIABLE in warningListData)
            {
                tempList.Add(new WarningListVM
                {
                    Warning_UID = VARIABLE.Warning_UID,
                    Part_Types = VARIABLE.Part_Types,
                    Product_Date = VARIABLE.Product_Date.ToString("yyyy-M-d"),
                    Product_Phase = VARIABLE.Product_Phase,
                    Project = VARIABLE.Project,
                    Customer = VARIABLE.Customer,
                    FncPlant_Effect = VARIABLE.FncPlant_Effect,
                    Time_Interval = VARIABLE.Time_Interval
                });
            }

            return new PagedListModel<WarningListVM>(totalCount, tempList);
        }

        public ProcessDataSearch GetWarningDataByWarningUid(int WarningUid)
        {
            var tempData = warningListRepository.GetWarningDataByWarningUid(WarningUid);
            if (tempData.ToList().Count != 0)
            {
                ProcessDataSearch reuslt = tempData.ToList()[0];
                return reuslt;
            }
            else
            {
                return new ProcessDataSearch();
            }

        }

        #endregion
        #region Product_Input And Product_Input_History Common Function-------Sidney 2015/12/20
        /// <summary>
        /// 查询Product_Input
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public PagedListModel<PPCheckDataItem> QueryPPCheckDatas(PPCheckDataSearch searchModel, Page page, string QueryType)
        {
            var nowTimeInfo = GetIntervalInfo("OP1");
            searchModel.Reference_Date = Convert.ToDateTime(nowTimeInfo.NowDate);
            var totalCount = 0;
            var betweenDay = DateTime.Now.Day - searchModel.Reference_Date.Day;
            if (betweenDay > 7)
            {
                var PPlist = ProductInputRepository.QueryHistoryDatas(searchModel, page, out totalCount, QueryType);
                return new PagedListModel<PPCheckDataItem>(totalCount, PPlist);
            }
            else
            {
                var PPlist = ProductInputRepository.QueryPpCheckDatas(searchModel, page, out totalCount, QueryType);
                return new PagedListModel<PPCheckDataItem>(0, PPlist);
            }
        }

        public PagedListModel<Daily_ProductReportItem> QueryReportDatas(ReportDataSearch searchModel, Page page, string QueryType)
        {
            var totalCount = 0;
            if (searchModel.Tab_Select_Text == "夜班小计")
                searchModel.Tab_Select_Text = "Night_Sum";
            else if (searchModel.Tab_Select_Text == "白班小计")
                searchModel.Tab_Select_Text = "Daily_Sum";
            else if (searchModel.Tab_Select_Text == "全天")
                searchModel.Tab_Select_Text = "ALL";
            //获取当前时段
            var temp = EnumerationRepository.GetIntervalInfo("OP1");
            var firstOrDefault = temp.FirstOrDefault();
            var PPlist = ProductInputRepository.QueryAll_ReportData(searchModel,firstOrDefault.Time_Interval,firstOrDefault.NowDate, out totalCount);
            return new PagedListModel<Daily_ProductReportItem>(0, PPlist);
        }
        /// <summary>
        /// 检查功能厂是否存在数据
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public List<string> CheckFunPlantDataIsFull(PPCheckDataSearch searchModel)
        {
            var nowTimeInfo = GetIntervalInfo("OP1");
            searchModel.Reference_Date = Convert.ToDateTime(nowTimeInfo.NowDate);
            return ProductInputRepository.CheckFunPlantDataIsFull(searchModel);
        }
        /// <summary>
        /// 获取时段
        /// </summary>
        /// <param name="PageName"></param>
        /// <returns></returns>
        public List<Enumeration> GetIntervalTime(string PageName)
        {
            if (PageName == "PPCheckData")
            {
                
                //var nowDate = DateTime.Now;
                //var nowInterval = from item in temp
                //                  where (item.BeginTime < nowDate && item.EndTime > nowDate)
                //                  select item;
                List<Enumeration> ppCheck = new List<Enumeration>();
                Enumeration pp1 = new Enumeration();
                Enumeration pp2 = new Enumeration();
                //获取当前时段
                //var firstOrDefault = nowInterval.FirstOrDefault();
                //if (firstOrDefault != null) pp1.Enum_Value = firstOrDefault.Time_Interval;
                //获取当前时段
                var temp = EnumerationRepository.GetIntervalInfo("OP1");
                var firstOrDefault = temp.FirstOrDefault();
                if (firstOrDefault != null) pp1.Enum_Value = firstOrDefault.Time_Interval;

                pp2.Enum_Value = "ALL";
                ppCheck.Add(pp2);
                ppCheck.Add(pp1);
                return ppCheck;
            }
            else
            {
                List<Enumeration> ppCheck = new List<Enumeration>();
                Enumeration en1 = new Enumeration();
                Enumeration en2 = new Enumeration();
                Enumeration en3 = new Enumeration();

                en1.Enum_Value = "全天";
                en2.Enum_Value = "白班小计";
                en3.Enum_Value = "夜班小计";
                ppCheck.Add(en1);
                ppCheck.Add(en2);
                ppCheck.Add(en3);
                var EnumEntity = EnumerationRepository.GetIntervalOrder();
                ppCheck.AddRange(EnumEntity);
                return ppCheck;
            }
        }
        /// <summary>
        /// 获取所有的客户
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllCustomer()
        {
            var EnumEntity = SystemBUDRepository.QueryDistinctCustomer();
            var customerList = AutoMapper.Mapper.Map<List<string>>(EnumEntity);
            return customerList;
        }
        /// <summary>
        /// 获取所有专案
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public List<string> GetAllProject(string customer)
        {
            var EnumEntity = SystemProjectRepository.QueryDistinctProject(customer);
            var customerList = AutoMapper.Mapper.Map<List<string>>(EnumEntity);
            return customerList;
        }
        /// <summary>
        /// 获取所有生产阶段
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public List<string> GetAllProductPhase(string customer, string project)
        {
            var EnumEntity = SystemProjectRepository.QueryDistinctProductPhase(customer, project);
            var customerList = AutoMapper.Mapper.Map<List<string>>(EnumEntity);
            return customerList;
        }
        /// <summary>
        /// 获取所有的部件
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="project"></param>
        /// <param name="productphase"></param>
        /// <returns></returns>
        public List<string> GetAllPartTypes(string customer, string project, string productphase)
        {
            var EnumEntity = FlowChartMasterRepository.QueryDistinctPartTypes(customer, project, productphase);
            var customerList = AutoMapper.Mapper.Map<List<string>>(EnumEntity);
            return customerList;
        }
        /// <summary>
        /// 获取所有颜色
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="project"></param>
        /// <param name="productphase"></param>
        /// <param name="parttypes"></param>
        /// <returns></returns>
        public List<string> GetAllColor(string customer, string project, string productphase, string parttypes)
        {
            List<string> result = new List<string>();
            result.Add("ALL");
            var EnumEntity = FlowChartDetailRepository.QueryDistinctColor(customer, project, productphase, parttypes);
            var customerList = AutoMapper.Mapper.Map<List<string>>(EnumEntity);
            result.AddRange(customerList);
            return result;
        }
        /// <summary>
        /// 获取当前时段及当前日期
        /// </summary>
        /// <param name="opType">OP类型</param>
        /// <returns>包含当前时段及日期的类</returns>
        public IntervalEnum GetIntervalInfo(string opType = "OP1")
        {
            var temp = EnumerationRepository.GetIntervalInfo(opType);
            return temp.FirstOrDefault();
        }
        /// <summary>
        /// 修改WIP
        /// </summary>
        /// <param name="WIP"></param>
        /// <returns></returns>
        public string EditWIP(PPEditWIP WIP,int modified_UID)
        {

            var WIPlist = WIP.PPEditValue;
            string EditFlag = null;
            foreach (var wipitem in WIPlist)
            {
                //var WipQty = Convert.ToInt32(wipitem.Wip_Qty.ToString());
                
                try
                {
                    string Product_UID = wipitem.Product_UID.ToString();
                    Product_UID = Product_UID.Remove(0, 3);
                    int Product_ID = Convert.ToInt32(Product_UID);
                    Product_Input proinput = ProductInputRepository.GetFirstOrDefault(c => c.Product_UID == Product_ID);
                    proinput.Modified_Date = DateTime.Now;
                    proinput.Is_Comfirm = true;
                    proinput.Modified_UID = modified_UID;
                    ProductInputRepository.Update(proinput);
                }
                catch (Exception e)
                { EditFlag += "FALSE"; }
            }
            if (EditFlag != null)
            {
                unitOfWork.Commit();
                return "WIP Edit False";
            }

            else
            {
                unitOfWork.Commit();
                return "SUCCESS";
            }
        }

        public string EditWIPView(int product_uid, int wip_qty, int wip_old,int wip_add,string comment,int modifiedUser)
        {
            
            try
            {
                //修改Product_Input表的WIP
                Product_Input proinput = ProductInputRepository.GetFirstOrDefault(c => c.Product_UID == product_uid);
                proinput.WIP_QTY = wip_qty;
                ProductInputRepository.Update(proinput);

                //插入到WIP_Change_Hitory表中
                WIP_Change_History item = new WIP_Change_History();
                item.Change_Type = "AbNormal";
                item.Comment = comment;
                item.Modified_Date = DateTime.Now;
                item.Modified_UID = modifiedUser;
                item.WIP_Add = wip_add;
                item.WIP_Old = wip_old;
                item.Product_UID = product_uid;
                WIPChangeHistoryRepository.Add(item);
                unitOfWork.Commit();

                return "SUCCESS";
            }
            catch (Exception e)
            { return "FALSE"; }
        }

        #endregion
        #region Day Week Month Report Function------------------------Sidney 2016/01/28
        public List<string> GetAlVersion(string customer, string project, string productphase, string parttypes, DateTime beginTime, DateTime endTime)
        {
            List<string> result = new List<string>();
            var EnumEntity = FlowChartDetailRepository.QueryDistinctVersion(customer, project, productphase, parttypes, beginTime, endTime);
            var customerList = AutoMapper.Mapper.Map<List<string>>(EnumEntity);
            result.AddRange(customerList);
            return result;
        }

        public VersionBeginEndDate GetVersionBeginEndDate(string customer, string project, string productphase, string parttypes, int version)
        {
            var EnumEntity = FlowChartDetailRepository.GetVersionBeginEndDate(customer, project, productphase, parttypes, version);
            return EnumEntity;
        }
        #endregion
    }
}
