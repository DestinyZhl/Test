using SPP.Data.Infrastructure;
using SPP.Model;
using SPP.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SPP.Data.Repository
{
    public interface IFlowChartDetailRepository : IRepository<FlowChart_Detail>
    {
        IQueryable<string> QueryDistinctColor(string customer, string project, string productphase, string parttypes);

        IQueryable<string> QueryFunPlant(string customername, string projectname, string productphasename, string parttypesname, string color);

        IQueryable<string> QueryProcess(string customername, string projectname, string productphasename, string parttypesname, string color, string funPlant);

        IQueryable<int> QueryDistinctVersion(string customer, string project, string productphase,
            string parttypes, DateTime beginTime, DateTime endTime);

        VersionBeginEndDate GetVersionBeginEndDate(string customer, string project, string productphase,
            string parttypes, int version);

        List<string> QueryProcess(int flowchartmasterUid);
    }
    public class FlowChartDetailRepository : RepositoryBase<FlowChart_Detail>, IFlowChartDetailRepository
    {
        public FlowChartDetailRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
        /// <summary>
        /// 从FlowChartDetail获取资料公用方法
        /// </summary>
        /// <param name="customername"></param>
        /// <param name="projectname"></param>
        /// <param name="productphasename"></param>
        /// <param name="parttypesname"></param>
        /// <returns></returns>
        public IQueryable<FlowChart_Detail> DetailCommonSource(string customername, string projectname, string productphasename, string parttypesname)
        {
            var query = from bud in DataContext.System_BU_D
                        join project in DataContext.System_Project on bud.BU_D_UID equals project.BU_D_UID
                        where (bud.BU_D_Name == customername && project.Project_Name == projectname)
                        select (project.Project_UID);
            var project_uid = query.FirstOrDefault();
            var query_parttypes = from flowmaster in DataContext.FlowChart_Master
                                  join flowdetail in DataContext.FlowChart_Detail on flowmaster.FlowChart_Master_UID equals flowdetail.FlowChart_Master_UID
                                  where (flowmaster.Project_UID == project_uid && flowmaster.Part_Types == parttypesname
                                  && flowmaster.FlowChart_Version == flowdetail.FlowChart_Version && flowdetail.Color != null && flowdetail.Color != "")
                                  select (flowdetail);
            return query_parttypes;
        }
        /// <summary>
        /// 获取颜色------------------------------------Sidney
        /// </summary>
        /// <param name="customername"></param>
        /// <param name="projectname"></param>
        /// <param name="productphasename"></param>
        /// <param name="parttypesname"></param>
        /// <returns></returns>
        public IQueryable<string> QueryDistinctColor(string customername, string projectname, string productphasename, string parttypesname)
        {
            var query = DetailCommonSource(customername, projectname, productphasename, parttypesname);
            return query.Select(p=>p.Color).Distinct();
        }
        /// <summary>
        /// QueryFunPlant
        /// </summary>
        /// <param name="customername"></param>
        /// <param name="projectname"></param>
        /// <param name="productphasename"></param>
        /// <param name="parttypesname"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public IQueryable<string> QueryFunPlant(string customername, string projectname, string productphasename,
            string parttypesname, string color)
        {
            var query = DetailCommonSource(customername, projectname, productphasename, parttypesname);
            if (color != "ALL")
            {
                query = query.Where(p => p.Color == color);
                return query.Select(p => p.System_Function_Plant.FunPlant).Distinct();
            }            
            else
            {
                return query.Select(p => p.System_Function_Plant.FunPlant).Distinct();
            }
            
        }
        /// <summary>
        /// QueryProcess
        /// </summary>
        /// <param name="customername"></param>
        /// <param name="projectname"></param>
        /// <param name="productphasename"></param>
        /// <param name="parttypesname"></param>
        /// <param name="color"></param>
        /// <param name="funPlant"></param>
        /// <returns></returns>
        public IQueryable<string> QueryProcess(string customername, string projectname, string productphasename, string parttypesname, string color, string funPlant)
        {
            var query = DetailCommonSource(customername, projectname, productphasename, parttypesname);
            if (color != "ALL")
                query = query.Where(p => p.Color == color);
            if (funPlant != "ALL")
                query = query.Where(p => p.System_Function_Plant.FunPlant == funPlant);
            var queryParttypes = query.Select(p => p.Process);
            return queryParttypes.Distinct();
        }

        #region Day Week Month Report Function------------------------Sidney 2016/01/28

        public IQueryable<int> QueryDistinctVersion(string customer, string project, string productphase,
            string parttypes, DateTime beginTime, DateTime endTime)
        {
            var query = from bud in DataContext.System_BU_D
                        join pro in DataContext.System_Project on bud.BU_D_UID equals pro.BU_D_UID
                        where (bud.BU_D_Name == customer && pro.Project_Name == project)
                        select (pro.Project_UID);
            var projectUid = query.FirstOrDefault();
            int betweenDay_B = (DateTime.Now - beginTime).Days; ;
            int betweenDay_T= (DateTime.Now - endTime).Days;
            if (betweenDay_B >= 7 && betweenDay_T >= 7) //  Justin 2-2 补充 需要查询出日期 如 版本 3 时间2-3到2~9   且计算版本时候有误， 开始日期大于七天，若结束日期在七天内，版本丢失
            {
                var queryVision = from fm in DataContext.FlowChart_Master
                                  join fd in DataContext.FlowChart_Detail on fm.FlowChart_Master_UID equals fd.FlowChart_Master_UID
                                  join pd in DataContext.Product_Input_History on fd.FlowChart_Detail_UID equals pd.FlowChart_Detail_UID
                                  where (pd.Create_Date >= beginTime && pd.Create_Date <= endTime &&
                                         fm.Project_UID == projectUid && fm.Part_Types == parttypes && pd.Is_Comfirm == true)
                                  select fd.FlowChart_Version;
                return queryVision.Distinct();
            }
            else if (betweenDay_B < 7 && betweenDay_T < 7)
            {
                var queryVision = from fm in DataContext.FlowChart_Master
                                  join fd in DataContext.FlowChart_Detail on fm.FlowChart_Master_UID equals fd.FlowChart_Master_UID
                                  join pd in DataContext.Product_Input on fd.FlowChart_Detail_UID equals pd.FlowChart_Detail_UID
                                  where (pd.Create_Date >= beginTime && pd.Create_Date <= endTime &&
                                         fm.Project_UID == projectUid && fm.Part_Types == parttypes && pd.Is_Comfirm == true)
                                  select fd.FlowChart_Version;
                return queryVision.Distinct();
            }
            else
            {
                var demoDay = DateTime.Now.AddDays(-7);
                var queryVision1 = from fm in DataContext.FlowChart_Master
                                  join fd in DataContext.FlowChart_Detail on fm.FlowChart_Master_UID equals fd.FlowChart_Master_UID
                                  join pd in DataContext.Product_Input_History on fd.FlowChart_Detail_UID equals pd.FlowChart_Detail_UID
                                  where (pd.Create_Date >= beginTime && pd.Create_Date <= demoDay &&
                                         fm.Project_UID == projectUid && fm.Part_Types == parttypes && pd.Is_Comfirm == true)
                                  select fd.FlowChart_Version;
                var queryVision2 = from fm in DataContext.FlowChart_Master
                                   join fd in DataContext.FlowChart_Detail on fm.FlowChart_Master_UID equals fd.FlowChart_Master_UID
                                   join pd in DataContext.Product_Input on fd.FlowChart_Detail_UID equals pd.FlowChart_Detail_UID
                                   where (pd.Create_Date >= demoDay && pd.Create_Date <= endTime &&
                                          fm.Project_UID == projectUid && fm.Part_Types == parttypes && pd.Is_Comfirm == true)
                                   select fd.FlowChart_Version;
                var result = queryVision1.Union(queryVision2).Distinct();
                return result;
            }
        }

        public VersionBeginEndDate GetVersionBeginEndDate(string customer, string project, string productphase,
            string parttypes,int version)
        {
            string sql = string.Empty;
            sql = @"select MIN(pi.Create_Date)VersionBeginDate,MAX(pi.Create_Date)VersionEndDate from
                    (SELECT   [Is_Comfirm] ,[Product_Date] ,[Time_Interval] ,[Customer] ,[Project] ,[Part_Types] ,
                   [FunPlant] ,[FunPlant_Manager] ,[Product_Phase] ,[Process_Seq] ,[Place] ,[Process] ,[FlowChart_Master_UID] ,
                   [FlowChart_Version] ,[Color] ,[Prouct_Plan] ,[Product_Stage] ,[Target_Yield] ,[Good_QTY] ,[Good_MismatchFlag] ,
                   [Picking_QTY] ,[WH_Picking_QTY] ,[Picking_MismatchFlag] ,[NG_QTY] ,[WH_QTY] ,[WIP_QTY] ,[Adjust_QTY] ,
                   [Creator_UID] ,[Create_Date] ,[Material_No] ,[Modified_UID] ,[Modified_Date] ,DRI ,FlowChart_Detail_UID
                   FROM     dbo.Product_Input
                    where Is_Comfirm=1
                   UNION
                   SELECT   [Is_Comfirm] ,[Product_Date] ,[Time_Interval] ,[Customer] ,[Project] ,[Part_Types] ,
                   [FunPlant] ,[FunPlant_Manager] ,[Product_Phase] ,[Process_Seq] ,[Place] ,[Process] ,[FlowChart_Master_UID] ,
                   [FlowChart_Version] ,[Color] ,[Prouct_Plan] ,[Product_Stage] ,[Target_Yield] ,[Good_QTY] ,[Good_MismatchFlag] ,
                   [Picking_QTY] ,[WH_Picking_QTY] ,[Picking_MismatchFlag] ,[NG_QTY] ,[WH_QTY] ,[WIP_QTY] ,[Adjust_QTY] ,
                   [Creator_UID] ,[Create_Date] ,[Material_No] ,[Modified_UID] ,[Modified_Date] ,DRI ,FlowChart_Detail_UID
                   FROM     dbo.Product_Input_History
                    where Is_Comfirm=1) AS pi,
                    dbo.FlowChart_Detail AS fcd,dbo.FlowChart_Master AS fcm,
                    dbo.System_BU_D AS sbd,dbo.System_Project AS sp
                    WHERE 
                    pi.FlowChart_Detail_UID=fcd.FlowChart_Detail_UID
                    AND pi.FlowChart_Master_UID=fcd.FlowChart_Master_UID
                    AND fcm.FlowChart_Master_UID=fcd.FlowChart_Master_UID
                    AND sp.BU_D_UID=sbd.BU_D_UID
                    AND sp.Project_UID=fcm.Project_UID
                    AND sbd.BU_D_Name='{0}'
                    AND sp.Project_Name='{1}'
                    AND sp.Product_Phase='{2}'
                    AND fcm.Part_Types='{3}'
                    AND pi.FlowChart_Version={4}";
            sql = string.Format(sql,customer,project,productphase,parttypes,version);
            var dbList = DataContext.Database.SqlQuery<VersionBeginEndDate>(sql).ToList();
            return dbList[0];
        }
        #endregion


        public List<string> QueryProcess(int flowchartmasterUid)
        {
            string sql = string.Format(@"
                                        SELECT  DISTINCT process as Process
                                        FROM    dbo.FlowChart_Detail FD WITH ( NOLOCK )
                                        WHERE   Process NOT IN (
                                                SELECT DISTINCT
                                                        Process
                                                FROM    dbo.FlowChart_Detail FD WITH ( NOLOCK )
                                                        INNER JOIN dbo.FlowChart_Master FM WITH ( NOLOCK ) ON FM.FlowChart_Master_UID = FD.FlowChart_Master_UID
                                                        INNER JOIN dbo.Enumeration en WITH ( NOLOCK ) ON en.Enum_Name = FM.Part_Types
                                                                                                      AND en.Enum_Value = FD.Process
                                                WHERE   en.Enum_Type = 'Report_Key_Process'
                                                        AND FM.FlowChart_Master_UID = {0} )
                                                AND FD.FlowChart_Master_UID = {0}
                                        ", flowchartmasterUid);

            var dbList = DataContext.Database.SqlQuery<string>(sql).ToList();
            return dbList;
        }
    }
}
