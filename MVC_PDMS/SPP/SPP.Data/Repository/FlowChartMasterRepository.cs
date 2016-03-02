
using SPP.Data.Infrastructure;
using SPP.Model;
using SPP.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SPP.Common.Constants;
using System.Data.Entity.SqlServer;
using System.Text;
using System.Data.Entity.Infrastructure;

namespace SPP.Data.Repository
{
    public interface IFlowChartMasterRepository : IRepository<FlowChart_Master>
    {
        IQueryable<string> QueryDistinctPartTypes(string customer, string project, string productphase);
        IQueryable<FlowChartMasterDTO> QueryProjectTypes();
        IQueryable<FlowChartModelGet> QueryFlowCharts(FlowChartModelSearch search, Page page, out int count);
        IQueryable<FlowChart_Detail> QueryFLDetailList(int id, int Version, out int count);

        IQueryable<FlowChart_Detail_Temp> QueryFLDetailTempList(int id, int Version, out int count);
        IQueryable<FlowChart_Master> QueryFLList(int id);

        List<FlowChartPlanManagerVM> QueryFlowMGData(int masterUID, DateTime date, out int count);
        IQueryable<PrjectListVM> QueryFlowChartMasterDatas(int user_account_uid, out int count);
        IQueryable<ProcessDataSearch> QueryFlowChartDataByMasterUid(int flowChartMaster_uid);
        void UpdateFolowCharts(FlowChartImport importItem);
        FlowChartPlanManagerVM QueryFlowMGDataSingle(int masterUID, DateTime date);
        IQueryable<FlowChart_MgData> UpdatePlan(int detailUID, DateTime date);
        void BatchImportPlan(List<FlowChartMgDataDTO> mgDataList, int FlowChart_Master_UID);


    }
    public class FlowChartMasterRepository : RepositoryBase<FlowChart_Master>, IFlowChartMasterRepository
    {
        public FlowChartMasterRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        public IQueryable<FlowChartMasterDTO> QueryProjectTypes()
        {
            var query_parttypes = from flowmaster in DataContext.FlowChart_Master
                                  where flowmaster.Is_Closed == false
                                  select new FlowChartMasterDTO
                                  {
                                      FlowChart_Master_UID=flowmaster.FlowChart_Master_UID,
                                      Part_Types=flowmaster.Part_Types
                                  };
            return query_parttypes.Distinct();
        }

        public IQueryable<string> QueryDistinctPartTypes(string customername, string projectname, string productphasename)
        {
            var query = from bud in DataContext.System_BU_D
                        join project in DataContext.System_Project on bud.BU_D_UID equals project.BU_D_UID
                        where (bud.BU_D_Name == customername && project.Project_Name == projectname)
                        select (project.Project_UID);
            var project_uid = query.FirstOrDefault();
            var query_parttypes = from flowmaster in DataContext.FlowChart_Master
                                  where (flowmaster.Project_UID == project_uid)
                                  select (flowmaster.Part_Types);
            return query_parttypes.Distinct();
        }

        public IQueryable<FlowChartModelGet> QueryFlowCharts(FlowChartModelSearch search, Page page, out int count)
        {
            //var query = from flchart in DataContext.FlowChart_Master.Include("System_Project").Include("System_Users")
            //            select flchart;

            var query = (from M in DataContext.FlowChart_Master
                         join P in DataContext.System_Project
                         on M.Project_UID equals P.Project_UID
                         join BUD in DataContext.System_BU_D
                         on P.BU_D_UID equals BUD.BU_D_UID
                         join U in DataContext.System_Users
                         on M.Modified_UID equals U.Account_UID
                         select new FlowChartModelGet
                         {
                             FlowChart_Master_UID = M.FlowChart_Master_UID,
                             BU_D_Name = BUD.BU_D_Name,
                             Project_Name = P.Project_Name,
                             Part_Types = M.Part_Types,
                             Product_Phase = P.Product_Phase,
                             Is_Closed = M.Is_Closed,
                             Is_Latest = M.Is_Latest,
                             FlowChart_Version = M.FlowChart_Version,
                             FlowChart_Version_Comment = M.FlowChart_Version_Comment,
                             User_Name = U.User_Name,
                             Modified_Date = M.Modified_Date,
                             User_NTID = U.User_NTID,
                             IsTemp = false
                         }).Union(
                        from M in DataContext.FlowChart_Master
                        join DT in DataContext.FlowChart_Detail_Temp
                        on M.FlowChart_Master_UID equals DT.FlowChart_Master_UID
                        join P in DataContext.System_Project
                        on M.Project_UID equals P.Project_UID
                        join BUD in DataContext.System_BU_D
                        on P.BU_D_UID equals BUD.BU_D_UID
                        join U in DataContext.System_Users
                        on DT.Modified_UID equals U.Account_UID
                        select new FlowChartModelGet
                        {
                            FlowChart_Master_UID = M.FlowChart_Master_UID,
                            BU_D_Name = BUD.BU_D_Name,
                            Project_Name = P.Project_Name,
                            Part_Types = M.Part_Types,
                            Product_Phase = P.Product_Phase,
                            Is_Closed = M.Is_Closed,
                            Is_Latest = M.Is_Latest,
                            FlowChart_Version = DT.FlowChart_Version,
                            FlowChart_Version_Comment = DT.FlowChart_Version_Comment,
                            User_Name = U.User_Name,
                            Modified_Date = DT.Modified_Date,
                            User_NTID = U.User_NTID,
                            IsTemp = true
                        }
                ).Distinct();

            if (!string.IsNullOrWhiteSpace(search.BU_D_Name))
            {
                query = query.Where(m => m.BU_D_Name.Contains(search.BU_D_Name));
            }
            if (!string.IsNullOrWhiteSpace(search.Project_Name))
            {
                query = query.Where(m => m.Project_Name.Contains(search.Project_Name));
            }
            if (!string.IsNullOrWhiteSpace(search.Part_Types))
            {
                query = query.Where(m => m.Part_Types.Contains(search.Part_Types));
            }
            if (!string.IsNullOrWhiteSpace(search.Product_Phase))
            {
                query = query.Where(m => m.Product_Phase.Contains(search.Product_Phase));
            }
            switch (search.Is_Closed)
            {
                case StructConstants.IsClosedStatus.ClosedKey:
                    query = query.Where(m => m.Is_Closed == true && m.IsTemp == false);
                    break;
                case StructConstants.IsClosedStatus.ProcessKey:
                    query = query.Where(m => m.Is_Closed == false && m.IsTemp == false);
                    break;
                case StructConstants.IsClosedStatus.ApproveKey:
                    query = query.Where(m => m.IsTemp == true);
                    break;
            }
            if (search.Is_Latest == StructConstants.IsLastestStatus.LastestKey)
            {
                query = query.Where(m => m.Is_Latest == true);
            }
            if (search.Modified_Date_From != null)
            {
                query = query.Where(m => SqlFunctions.DateDiff("dd", m.Modified_Date, search.Modified_Date_From) <= 0);
            }
            if (search.Modified_Date_End != null)
            {
                query = query.Where(m => SqlFunctions.DateDiff("dd", m.Modified_Date, search.Modified_Date_End) >= 0);
            }
            if (search.Modified_By != null)
            {
                query = query.Where(m => m.User_NTID == search.Modified_By);
            }

            count = query.Count();
            query = query.OrderByDescending(m => m.Modified_Date).ThenBy(m => m.Project_Name).GetPage(page);
            return query;
        }

        public IQueryable<FlowChart_Detail> QueryFLDetailList(int id, int Version, out int count)
        {
            var flMasterItem = DataContext.FlowChart_Master.Find(id);
            var flDetailQuery = from p in DataContext.FlowChart_Detail.Include("System_Function_Plant").Include("System_Users")
                                where p.FlowChart_Master_UID == id && p.FlowChart_Version == Version
                                select p;
            count = flDetailQuery.Count();
            return flDetailQuery.OrderBy(m => m.Process_Seq);
        }

        public IQueryable<FlowChart_Detail_Temp> QueryFLDetailTempList(int id, int Version, out int count)
        {
            var flMasterItem = DataContext.FlowChart_Master.Find(id);
            var flDetailQuery = from p in DataContext.FlowChart_Detail_Temp
                                where p.FlowChart_Master_UID == id && p.FlowChart_Version == Version
                                select p;
            count = flDetailQuery.Count();
            return flDetailQuery.OrderBy(m => m.Process_Seq);
        }

        public IQueryable<FlowChart_Master> QueryFLList(int id)
        {
            var flMasterItem = DataContext.FlowChart_Master.Find(id);
            var query = from P in DataContext.FlowChart_Master
                        where P.Project_UID == flMasterItem.Project_UID && P.Part_Types == flMasterItem.Part_Types
                        select P;
            return query;
        }

        public IQueryable<PrjectListVM> QueryFlowChartMasterDatas(int user_account_uid, out int count)
        {
            var query = (from flowchartMaster in DataContext.FlowChart_Master
                         join project in DataContext.System_Project on flowchartMaster.Project_UID equals project.Project_UID
                         join BUD in DataContext.System_BU_D on project.BU_D_UID equals BUD.BU_D_UID
                         join flowchartdetail in DataContext.FlowChart_Detail on flowchartMaster.FlowChart_Master_UID equals flowchartdetail.FlowChart_Master_UID
                         join userFunPlant in DataContext.System_User_FunPlant on flowchartdetail.System_FunPlant_UID equals userFunPlant.System_FunPlant_UID
                         where userFunPlant.Account_UID == user_account_uid && flowchartMaster.Is_Closed != true
                         select new PrjectListVM
                         {
                             FlowChartMaster_Uid = flowchartMaster.FlowChart_Master_UID,
                             Part_Types = flowchartMaster.Part_Types,
                             Product_Phase = project.Product_Phase,
                             Project = project.Project_Name,
                             Customer = BUD.BU_D_Name
                         }).Distinct();

            count = query.Count();
            return query.OrderBy(o => o.Project).OrderBy(o => o.Product_Phase);
        }

        public IQueryable<ProcessDataSearch> QueryFlowChartDataByMasterUid(int flowChartMaster_uid)
        {
            var query = from flowchartMaster in DataContext.FlowChart_Master
                        join project in DataContext.System_Project on flowchartMaster.Project_UID equals project.Project_UID
                        join BUD in DataContext.System_BU_D on project.BU_D_UID equals BUD.BU_D_UID

                        where flowchartMaster.FlowChart_Master_UID == flowChartMaster_uid && flowchartMaster.Is_Closed != true
                        select new ProcessDataSearch
                        {
                            Part_Types = flowchartMaster.Part_Types,
                            Product_Phase = project.Product_Phase,
                            Project = project.Project_Name,
                            Customer = BUD.BU_D_Name,

                        };

            return query;
        }

        public void UpdateFolowCharts(FlowChartImport importItem)
        {
            //进行更新操作，全删全插
            var oldMasterFLItem = DataContext.FlowChart_Master.Find(importItem.FlowChartMasterDTO.FlowChart_Master_UID);

            var tempDetailList = DataContext.FlowChart_Detail_Temp.Where(m => m.FlowChart_Master_UID == oldMasterFLItem.FlowChart_Master_UID).ToList();
            var tempIdList = tempDetailList.Select(m => m.FlowChart_DT_UID).ToList();


            using (var trans = DataContext.Database.BeginTransaction())
            {
                //全删除操作
                foreach (var id in tempIdList)
                {
                    var deleteSubSql = string.Format("delete from FlowChart_MgData_Temp where FlowChart_DT_UID={0}", id);
                    DataContext.Database.ExecuteSqlCommand(deleteSubSql);
                }
                foreach (var id in tempIdList)
                {
                    var deleteSql = string.Format("delete from FlowChart_Detail_Temp where FlowChart_DT_UID={0}", id);
                    DataContext.Database.ExecuteSqlCommand(deleteSql);
                }

                //全插入操作
                foreach (var detailDTOItem in importItem.FlowChartImportDetailDTOList)
                {
                    var sql = string.Format("insert into FlowChart_Detail_Temp values ({0},{1},{2},N'{3}',N'{4}',N'{5}',{6},N'{7}',N'{8}',N'{9}',{10},N'{11}',{12},{13},'{14}')",
                                oldMasterFLItem.FlowChart_Master_UID,
                                detailDTOItem.FlowChartDetailDTO.System_FunPlant_UID,
                                detailDTOItem.FlowChartDetailDTO.Process_Seq,
                                detailDTOItem.FlowChartDetailDTO.DRI,
                                detailDTOItem.FlowChartDetailDTO.Place,
                                detailDTOItem.FlowChartDetailDTO.Process,
                                detailDTOItem.FlowChartDetailDTO.Product_Stage,
                                detailDTOItem.FlowChartDetailDTO.Color,
                                detailDTOItem.FlowChartDetailDTO.Process_Desc,
                                detailDTOItem.FlowChartDetailDTO.Material_No,
                                detailDTOItem.FlowChartDetailDTO.FlowChart_Version,
                                detailDTOItem.FlowChartDetailDTO.FlowChart_Version_Comment,
                                1,
                                detailDTOItem.FlowChartDetailDTO.Modified_UID,
                                detailDTOItem.FlowChartDetailDTO.Modified_Date);

                    //var subSql = string.Format("insert into FlowChart_MgData_Temp values (@@IDENTITY,{0},{1},{2},'{3}')",
                    //    detailDTOItem.FlowChartMgDataDTO.Product_Plan,
                    //    detailDTOItem.FlowChartMgDataDTO.Target_Yield,
                    //    detailDTOItem.FlowChartMgDataDTO.Modified_UID,
                    //    detailDTOItem.FlowChartMgDataDTO.Modified_Date);
                    DataContext.Database.ExecuteSqlCommand(sql);
                    //DataContext.Database.ExecuteSqlCommand(subSql);
                }

                trans.Commit();
            }

        }

        public void BatchImportPlan(List<FlowChartMgDataDTO> mgDataList, int FlowChart_Master_UID)
        {
            if (mgDataList.Count() == 0)
            {
                return;
            }

            using (var trans = DataContext.Database.BeginTransaction())
            {
                var minDate = mgDataList.Select(m => m.Product_Date).Min().ToShortDateString();
                var maxDate = mgDataList.Select(m => m.Product_Date).Max().ToShortDateString();
                //全删操作
                var deleteSql = @"delete from FlowChart_MgData where Product_Date >= '{0}' and Product_Date <= '{1}' and FlowChart_Detail_UID in
                                    (select FlowChart_Detail_UID from FlowChart_Detail where FlowChart_Master_UID={2} and 
                                    FlowChart_Version=(select max(FlowChart_Version) from FlowChart_Detail where FlowChart_Master_UID={2}))";
                deleteSql = string.Format(deleteSql, minDate, maxDate, FlowChart_Master_UID);
                DataContext.Database.ExecuteSqlCommand(deleteSql);

                //全插操作
                foreach (var mgDataItem in mgDataList)
                {
                    var insertSql = string.Format("insert into FlowChart_MgData values ({0},'{1}',{2},{3},{4},'{5}')",
                        mgDataItem.FlowChart_Detail_UID,
                        mgDataItem.Product_Date.ToShortDateString(),
                        mgDataItem.Product_Plan,
                        mgDataItem.Target_Yield,
                        mgDataItem.Modified_UID,
                        mgDataItem.Modified_Date.ToString("yyyy-MM-dd hh:mm:ss"));
                    DataContext.Database.ExecuteSqlCommand(insertSql);
                }
                trans.Commit();
            }
        }

        public List<FlowChartPlanManagerVM> QueryFlowMGData(int masterUID, DateTime date, out int count)
        {
            int uid = 0;
            var Flowchart_master_uid = new SqlParameter("Flowchart_master_uid", masterUID);
            var Monday_Date = new SqlParameter("Monday_Date", date);
            var Flowchar_detail_uid = new SqlParameter("Flowchar_detail_uid", uid);
         
            var result = ((IObjectContextAdapter)this.DataContext).ObjectContext.ExecuteStoreQuery<FlowChartPlanManagerVM>("usp_SearchProductPlan @Flowchart_master_uid , @Monday_Date, @Flowchar_detail_uid", Flowchart_master_uid, Monday_Date, Flowchar_detail_uid).ToList();
            count = 0;
         
            return result;
            
        }


        //public List<FlowChartPlanManagerVM> QueryFlowMGData(int masterUID, DateTime date, out int count)
        //{
        //    //遍历所有制程，通过制程UID detalUID获取MGdata
        //    DateTime CurrentDate;
        //    var query1 = from flowchartdetail in DataContext.FlowChart_Detail
        //                 where flowchartdetail.FlowChart_Master_UID == masterUID
        //                 select flowchartdetail;
        //    List<FlowChartPlanManagerVM> result = new List<FlowChartPlanManagerVM>();
        //    foreach (var item in query1)
        //    {
        //        FlowChartPlanManagerVM returnItem = new FlowChartPlanManagerVM();
        //        returnItem.Detail_UID = item.FlowChart_Detail_UID;
        //        returnItem.Color = item.Color;
        //        returnItem.Process = item.Process;
        //        //  周一到周五
        //        var detailUID = item.FlowChart_Detail_UID;
        //        for (int i = 0; i < 7; i++)
        //        {
        //            CurrentDate = date.AddDays(i);
        //            var query = from MGData in DataContext.FlowChart_MgData
        //                        where MGData.Product_Date == CurrentDate && MGData.FlowChart_Detail_UID == detailUID
        //                        select MGData;
        //            var MgdataItem = query.FirstOrDefault();

        //            if (i == 0 && MgdataItem != null)
        //            {
        //                returnItem.MonDayProduct_Plan = MgdataItem.Product_Plan;
        //                returnItem.MonDayTarget_Yield = MgdataItem.Target_Yield;
        //            }
        //            if (i == 1 && MgdataItem != null)
        //            {
        //                returnItem.TuesDayProduct_Plan = MgdataItem.Product_Plan;
        //                returnItem.TuesDayTarget_Yield = MgdataItem.Target_Yield;
        //            }

        //            if (i == 2 && MgdataItem != null)
        //            {
        //                returnItem.WednesdayProduct_Plan = MgdataItem.Product_Plan;
        //                returnItem.WednesdayTarget_Yield = MgdataItem.Target_Yield;
        //            }

        //            if (i == 3 && MgdataItem != null)
        //            {
        //                returnItem.ThursdayProduct_Plan = MgdataItem.Product_Plan;
        //                returnItem.ThursdayTarget_Yield = MgdataItem.Target_Yield;
        //            }

        //            if (i == 4 && MgdataItem != null)
        //            {
        //                returnItem.FriDayProduct_Plan = MgdataItem.Product_Plan;
        //                returnItem.FridayTarget_Yield = MgdataItem.Target_Yield;
        //            }

        //            if (i == 5 && MgdataItem != null)
        //            {
        //                returnItem.SaterDayProduct_Plan = MgdataItem.Product_Plan;
        //                returnItem.SaterDayTarget_Yield = MgdataItem.Target_Yield;
        //            }
        //            if (i == 6 && MgdataItem != null)
        //            {
        //                returnItem.SunDayProduct_Plan = MgdataItem.Product_Plan;
        //                returnItem.SunDayTarget_Yield = MgdataItem.Target_Yield;
        //            }
        //        }
        //        result.Add(returnItem);
        //    }
        //    count = 0;
        //    return result;
        //}

        public FlowChartPlanManagerVM QueryFlowMGDataSingle(int detailUID, DateTime date)
        {
            int uid = 0;
            var Flowchart_master_uid = new SqlParameter("Flowchart_master_uid", uid);
            var Monday_Date = new SqlParameter("Monday_Date", date);
            var Flowchar_detail_uid = new SqlParameter("Flowchar_detail_uid", detailUID);

            var result = ((IObjectContextAdapter)this.DataContext).ObjectContext.ExecuteStoreQuery<FlowChartPlanManagerVM>("usp_SearchProductPlan @Flowchart_master_uid , @Monday_Date, @Flowchar_detail_uid", Flowchart_master_uid, Monday_Date, Flowchar_detail_uid).ToList();
            if (result.Count == 0) return null;
            else
            return result[0];
        }

        public IQueryable<FlowChart_MgData> UpdatePlan(int detailUID, DateTime date)
        {
            DateTime endDay = date.AddDays(6);
            var query = from detail in DataContext.FlowChart_Detail
                        join mgdata in DataContext.FlowChart_MgData on detail.FlowChart_Detail_UID equals mgdata.FlowChart_Detail_UID
                        where mgdata.Product_Date >= date && mgdata.Product_Date <=endDay
                        select mgdata;

            return query.OrderBy(o=>o.Product_Plan);
        }
    }
}

