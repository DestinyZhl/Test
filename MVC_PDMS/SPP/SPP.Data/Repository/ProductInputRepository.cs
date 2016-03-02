using SPP.Data.Infrastructure;
using SPP.Model;
using System.Linq;
using SPP.Common.Helpers;
using System;
using System.Data.Entity;
using SPP.Data;
using SPP.Model.ViewModels;

using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace SPP.Data.Repository
{
    public class ProductInputRepository : RepositoryBase<Product_Input>, IProductInputRepository
    {
        #region ProductInput----------------------------------Justin 2015/12/21
        public ProductInputRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
        /// <summary>
        /// 根据登陆UID找到对应功能厂名
        /// </summary>
        /// <returns></returns>
        public string GetCurrentPlantName(int uid)
        {
            var query = from Fun in DataContext.System_Function_Plant
                        join User in DataContext.System_User_FunPlant on Fun.System_FunPlant_UID equals User.System_FunPlant_UID
                        where (User.Account_UID == uid)
                        select Fun.FunPlant;
            var funcPlantName = query.FirstOrDefault();
            return funcPlantName;
        }

        /// <summary>
        /// 根据功能厂名，查询功能厂对象信息
        /// </summary>
        /// <param name="funcPlant"></param>
        /// <returns></returns>
        public System_Function_Plant QueryFuncPlantInfo(string funcPlant)
        {

            var query = from Fun in DataContext.System_Function_Plant

                        where Fun.FunPlant == funcPlant
                        select Fun;


            return query.FirstOrDefault();
        }
        /// <summary>
        /// 根据查询条件返回填写的生产数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IQueryable<Product_Input> QueryProductDatas(ProcessDataSearch search, Page page, out int count)
        {

            var query = from PData in DataContext.Product_Input

                        select PData;
            if (!string.IsNullOrWhiteSpace(search.Customer))
            {
                query = query.Where(p => p.Customer == search.Customer);

            }
            if (!string.IsNullOrWhiteSpace(search.Project))
            {
                query = query.Where(p => p.Project == search.Project);
            }
            if (!string.IsNullOrWhiteSpace(search.Product_Phase))
            {
                query = query.Where(p => p.Product_Phase == search.Product_Phase);

            }
            if (!string.IsNullOrWhiteSpace(search.Part_Types))
            {
                query = query.Where(p => p.Part_Types == search.Part_Types);
            }
            if (!string.IsNullOrWhiteSpace(search.Time))
            {
                query = query.Where(p => p.Time_Interval == search.Time);
            }
            if (search.Date.Year != 1)
            {
                query = query.Where(p => p.Product_Date == search.Date);
            }
            query = query.Where(p => p.FunPlant == search.Func_Plant);
            count = query.Count();
            return query.OrderBy(o => o.Process_Seq).ThenBy(o => o.Product_UID);
        }
        /// <summary>
        /// 查询制程数据
        /// </summary>
        /// <param name="search">查询条件</param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IQueryable<ProductDataDTO> QueryProcessData(ProcessDataSearch search, Page page, out int count)
        {
            var query = from bu in DataContext.System_BU_D
                        join p in DataContext.System_Project on bu.BU_D_UID equals p.BU_D_UID
                        join master in DataContext.FlowChart_Master on p.Project_UID equals master.Project_UID
                        join deteil in DataContext.FlowChart_Detail on new { master.FlowChart_Master_UID, master.FlowChart_Version } equals new { deteil.FlowChart_Master_UID, deteil.FlowChart_Version }
                        join fPlant in DataContext.System_Function_Plant on deteil.System_FunPlant_UID equals fPlant.System_FunPlant_UID
                        join mgdata in DataContext.FlowChart_MgData on deteil.FlowChart_Detail_UID equals mgdata.FlowChart_Detail_UID
                        where (bu.BU_D_Name == search.Customer && p.Project_Name == search.Project && p.Product_Phase == search.Product_Phase && master.Part_Types == search.Part_Types && fPlant.FunPlant == search.Func_Plant && mgdata.Product_Date == search.Date)
                        select new ProductDataDTO
                        {
                            Is_Comfirm = false,
                            Product_Date = search.Date,
                            Time_Interval = search.Time,
                            Customer = search.Customer,
                            Project = search.Project,
                            Part_Types = search.Part_Types,
                            FunPlant = search.Func_Plant,
                            FunPlant_Manager = fPlant.FunPlant_Manager,
                            Product_Phase = search.Product_Phase,
                            Process_Seq = deteil.Process_Seq,
                            Place = deteil.Place,
                            Process = deteil.Process,
                            FlowChart_Master_UID = master.FlowChart_Master_UID,
                            FlowChart_Version = master.FlowChart_Version,
                            Color = deteil.Color,
                            Prouct_Plan = mgdata.Product_Plan,
                            Product_Stage = deteil.Product_Stage,
                            Target_Yield = mgdata.Target_Yield,
                            Good_QTY = 0,
                            Picking_QTY = 0,
                            WH_Picking_QTY = 0,
                            NG_QTY = 0,
                            WH_QTY = 0,
                            WIP_QTY = 0,
                            Adjust_QTY = 0,
                            DRI = deteil.DRI,
                            Material_No = deteil.Material_No,
                            FlowChart_Detail_UID = deteil.FlowChart_Detail_UID

                        };

            count = query.Count();
            return query.OrderBy(o => o.Process_Seq);
        }

        public IQueryable<ProductDataDTO> QueryColorSparations(ProcessDataSearch search)
        {

            var query = from bu in DataContext.System_BU_D
                        join p in DataContext.System_Project on bu.BU_D_UID equals p.BU_D_UID
                        join master in DataContext.FlowChart_Master on p.Project_UID equals master.Project_UID
                        join deteil in DataContext.FlowChart_Detail on new { master.FlowChart_Master_UID, master.FlowChart_Version } equals new { deteil.FlowChart_Master_UID, deteil.FlowChart_Version }
                        join fPlant in DataContext.System_Function_Plant on deteil.System_FunPlant_UID equals fPlant.System_FunPlant_UID
                        join mgdata in DataContext.FlowChart_MgData on deteil.FlowChart_Detail_UID equals mgdata.FlowChart_Detail_UID
                        where (bu.BU_D_Name == search.Customer && p.Project_Name == search.Project && p.Product_Phase == search.Product_Phase && master.Part_Types == search.Part_Types && mgdata.Product_Date == search.Date)
                        select new ProductDataDTO
                        {
                            Is_Comfirm = false,
                            Product_Date = search.Date,
                            Time_Interval = search.Time,
                            Customer = search.Customer,
                            Project = search.Project,
                            Part_Types = search.Part_Types,
                            FunPlant = search.Func_Plant,
                            FunPlant_Manager = fPlant.FunPlant_Manager,
                            Product_Phase = search.Product_Phase,
                            Process_Seq = deteil.Process_Seq,
                            Place = deteil.Place,
                            Process = deteil.Process,
                            FlowChart_Master_UID = master.FlowChart_Master_UID,
                            FlowChart_Version = master.FlowChart_Version,
                            Color = deteil.Color,
                            Prouct_Plan = mgdata.Product_Plan,
                            Product_Stage = deteil.Product_Stage,
                            Target_Yield = mgdata.Target_Yield,
                            Good_QTY = 0,
                            Picking_QTY = 0,
                            WH_Picking_QTY = 0,
                            NG_QTY = 0,
                            WH_QTY = 0,
                            WIP_QTY = 0,
                            Adjust_QTY = 0,
                            Material_No = deteil.Material_No
                        };
            return query.OrderBy(o => o.Process_Seq);

        }
        /// <summary>
        /// 数据提交后执行制程数据判断，调用sp实现
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string ExecAlterSp(Product_Input search)
        {
            //根据search对象获取masterUID

            var FlowChart_Master_UID = new SqlParameter("FlowChart_Master_UID", search.FlowChart_Master_UID);
            var Time_interval = new SqlParameter("Time_interval", search.Time_Interval);
            var Product_date = new SqlParameter("Product_date", search.Product_Date);
            //var Product_UID = new SqlParameter("Product_UID", search.Product_UID);

            IEnumerable<SPReturnMessage> result = ((IObjectContextAdapter)this.DataContext).ObjectContext.ExecuteStoreQuery<SPReturnMessage>("usp_AlterMisMatchFlag @FlowChart_Master_UID , @Time_interval, @Product_date", FlowChart_Master_UID, Time_interval, Product_date).ToArray();

            return result.ToList()[0].Message;
        }

        /// <summary>
        /// 调用sp 计算当前制程的wip
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public string ExecWIPSp(Product_Input search)
        {
            var FlowChart_Master_UID = new SqlParameter("FlowChart_Master_UID", search.FlowChart_Master_UID);
            var Time_interval = new SqlParameter("Time_interval", search.Time_Interval);
            var Product_date = new SqlParameter("Product_date", search.Product_Date);
            var FunPlant = new SqlParameter("FunPlant", search.FunPlant);
            int id = 0;
            var Product_UID = new SqlParameter("Product_UID", id);
            IEnumerable<SPReturnMessage> result = ((IObjectContextAdapter)this.DataContext).ObjectContext.ExecuteStoreQuery<SPReturnMessage>("usp_CalculateWIP  @Time_Interval,@Product_Date,@FlowChart_Master_UID, @FunPlant,@Product_UID", Time_interval, Product_date, FlowChart_Master_UID, FunPlant, Product_UID).ToArray();
            return result.ToList()[0].Message;
        }

        /// <summary>
        /// 数据更新时候，调用sp计算当前wip
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public string ExecUpdateWIPSp(Product_Input search)
        {
            var FlowChart_Master_UID = new SqlParameter("FlowChart_Master_UID", search.FlowChart_Master_UID);
            var Time_interval = new SqlParameter("Time_interval", search.Time_Interval);
            var Product_date = new SqlParameter("Product_date", search.Product_Date);
            var FunPlant = new SqlParameter("FunPlant", search.FunPlant);
            var Product_UID = new SqlParameter("Product_UID", search.Product_UID);
            IEnumerable<SPReturnMessage> result = ((IObjectContextAdapter)this.DataContext).ObjectContext.ExecuteStoreQuery<SPReturnMessage>("usp_CalculateWIP  @Time_Interval,@Product_Date,@FlowChart_Master_UID, @FunPlant,@Product_UID", Time_interval, Product_date, FlowChart_Master_UID, FunPlant, Product_UID).ToArray();
            return result.ToList()[0].Message;
        }

        #endregion
        #region 查询Product_Input及Product_Input_Hitory------------------Sidney 2015/12/20
        /// <summary>
        /// SetProductDatas
        /// </summary>
        /// <param name="PDataList"></param>
        /// <returns></returns>
        public List<Product_Input> SetProductDatas(ProductDataList PDataList)
        {
            List<Product_Input> Lists = new List<Product_Input>();
            ProductDataItem search = PDataList.ProductLists[0];
            int count = PDataList.ProductLists.Count;

            var query = from bu in DataContext.System_BU_D
                        join p in DataContext.System_Project on bu.BU_D_UID equals p.BU_D_UID
                        join master in DataContext.FlowChart_Master on p.Project_UID equals master.Project_UID
                        join deteil in DataContext.FlowChart_Detail on master.FlowChart_Master_UID equals deteil.FlowChart_Master_UID
                        join fPlant in DataContext.System_Function_Plant on deteil.System_FunPlant_UID equals fPlant.System_FunPlant_UID
                        join mgdata in DataContext.FlowChart_MgData on deteil.FlowChart_Detail_UID equals mgdata.FlowChart_Detail_UID
                        where (bu.BU_D_Name == search.Customer && p.Project_Name == search.Project && p.Product_Phase == search.Product_Phase && master.Part_Types == search.Part_Types && fPlant.FunPlant == search.FunPlant)
                        select new Product_Input
                        {
                            Is_Comfirm = false,
                            Product_Date = search.Product_Date,
                            Time_Interval = search.Time_Interval,
                            Customer = bu.BU_D_Name,
                            Project = p.Project_Name,
                            Part_Types = master.Part_Types,
                            FunPlant = fPlant.FunPlant,
                            FunPlant_Manager = fPlant.FunPlant_Manager,
                            Product_Phase = p.Product_Phase,
                            Process_Seq = search.Process_Seq,
                            Place = deteil.Place,
                            Process = search.Process,
                            FlowChart_Master_UID = master.FlowChart_Master_UID,
                            FlowChart_Version = master.FlowChart_Version,
                            Color = search.Color,
                            Prouct_Plan = mgdata.Product_Plan,
                            Product_Stage = deteil.Product_Stage,
                            Target_Yield = mgdata.Target_Yield,
                            Good_QTY = search.Good_QTY,
                            Picking_QTY = search.Picking_QTY,
                            WH_Picking_QTY = search.WH_Picking_QTY,
                            NG_QTY = search.NG_QTY,
                            WH_QTY = search.WH_QTY,
                            WIP_QTY = 0,
                            Adjust_QTY = search.Adjust_QTY,
                            Creator_UID = search.Creator_UID,
                            Create_Date = search.Create_Date,
                            Material_No = deteil.Material_No,
                            Modified_UID = search.Modified_UID,
                            Modified_Date = search.Modified_Date
                        };
            Lists = query.ToList();
            return Lists;
        }
        /// <summary>
        /// 检查是否所有功能厂都有数据
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public List<string> CheckFunPlantDataIsFull(PPCheckDataSearch searchModel)
        {
            //获取所有的功能厂
            var query = from bud in DataContext.System_BU_D
                        join project in DataContext.System_Project on bud.BU_D_UID equals project.BU_D_UID
                        where (bud.BU_D_Name == searchModel.Customer && project.Project_Name == searchModel.Project)
                        select (project.Project_UID);
            var projectUid = query.FirstOrDefault();
            var queryFunPlantQ = from flowmaster in DataContext.FlowChart_Master
                                 join flowdetail in DataContext.FlowChart_Detail on flowmaster.FlowChart_Master_UID equals flowdetail.FlowChart_Master_UID
                                 where (flowmaster.Project_UID == projectUid && flowmaster.Part_Types == searchModel.Part_Types && flowmaster.Is_Closed == false
                                 && flowmaster.FlowChart_Version == flowdetail.FlowChart_Version)
                                 select (flowdetail.System_Function_Plant.FunPlant);
            var queryFunPlant = queryFunPlantQ.Distinct().ToList();
            //获取当前已有数据的功能厂
            var queryNowFunPlantQ = from inputItem in DataContext.Product_Input
                                    where (inputItem.Customer == searchModel.Customer && inputItem.Project == searchModel.Project
                                           && inputItem.Product_Phase == searchModel.Product_Phase &&
                                           inputItem.Part_Types == searchModel.Part_Types
                                           && DbFunctions.TruncateTime(inputItem.Product_Date) == searchModel.Reference_Date.Date
                                           && inputItem.Time_Interval == searchModel.Tab_Select_Text
                                        )
                                    select (inputItem.FunPlant);
            var queryNowFunPlant = queryNowFunPlantQ.Distinct().ToList();
            List<string> result = queryFunPlant.Except(queryNowFunPlant).ToList();
            return result;

        }

        public IQueryable<PPCheckDataItem> QueryPpCheckDatas(PPCheckDataSearch searchModel, Page page, out int count, string QueryType)
        {
            var query1 = QueryIntervalDatas(searchModel, "Product_Input", QueryType);

            if (searchModel.Tab_Select_Text != "ALL")
            {
                var test = QueryNowIntervalDatas(query1, searchModel);
                count = test.Count();
                return test;
            }
            else
            {
                var test = QueryAllDatas(query1, searchModel, "Product_Input");
                count = test.Count();
                return test;
            }

        }
        /// <summary>
        /// 查询Product_Input_History
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IQueryable<PPCheckDataItem> QueryHistoryDatas(PPCheckDataSearch searchModel, Page page, out int count, string QueryType)
        {
            var query1 = QueryIntervalDatas(searchModel, "Product_Input_History", QueryType);

            if (searchModel.Tab_Select_Text != "ALL")
            {
                count = QueryNowIntervalDatas(query1, searchModel).Count();
                return QueryNowIntervalDatas(query1, searchModel);
            }
            else
            {
                count = QueryAllDatas(query1, searchModel, "Product_Input_History").Count();
                return QueryAllDatas(query1, searchModel, "Product_Input_History");
            }
        }
        /// <summary>
        /// 查询原始制程数据，后续以此为依据
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private IQueryable<PPCheckDataItemOrigOriginal> QueryIntervalDatas(PPCheckDataSearch searchModel, string tableName, string QueryType)
        {
            //获取当前时段序号
            var nowIntervalNo = DateCompareHelper.GetNowTimeIntervalNo();
            #region 从Product_Input表查询数据
            if (tableName == "Product_Input")
            {
                var query = from inputItem in DataContext.Product_Input
                            join user in DataContext.System_Users on inputItem.Creator_UID equals user.Account_UID
                            join detail in DataContext.FlowChart_Detail on inputItem.FlowChart_Detail_UID equals detail.FlowChart_Detail_UID
                            where (inputItem.Customer == searchModel.Customer && inputItem.Project == searchModel.Project
                            && inputItem.Product_Phase == searchModel.Product_Phase && inputItem.Part_Types == searchModel.Part_Types
                            && DbFunctions.TruncateTime(inputItem.Product_Date) == searchModel.Reference_Date.Date
                            )
                            select new PPCheckDataItemOrigOriginal
                            {
                                Product_Stage = detail.Product_Stage,
                                Is_Comfirm = inputItem.Is_Comfirm == false ? 0 : 1,
                                Color = detail.Color ?? "",
                                Time_Interval = inputItem.Time_Interval,
                                Product_Input_UID = inputItem.Product_UID,
                                Process_Seq = detail.Process_Seq,
                                Place = detail.Place,
                                FunPlant = inputItem.FunPlant,
                                Process = detail.Process,
                                FunPlant_Manager = inputItem.DRI,
                                Target_Yield = Math.Round(inputItem.Target_Yield * 100.00, 2),
                                Product_Plan = inputItem.Prouct_Plan,
                                Product_Plan_Sum = inputItem.Prouct_Plan / 12,
                                Picking_QTY = inputItem.Picking_QTY,
                                Picking_MismatchFlag = inputItem.Picking_MismatchFlag,
                                WH_Picking_QTY = inputItem.WH_Picking_QTY,
                                Good_QTY = inputItem.Good_QTY,
                                Good_MismatchFlag = inputItem.Good_MismatchFlag,
                                Adjust_QTY = inputItem.Adjust_QTY,
                                WH_QTY = inputItem.WH_QTY,
                                NG_QTY = inputItem.NG_QTY,
                                Rolling_Yield_Rate = inputItem.Prouct_Plan == 0 ? 0 : (Math.Round((inputItem.Good_QTY) / (inputItem.Prouct_Plan / 12.0) * 100.00, 2)),
                                Finally_Field = (inputItem.Good_QTY + inputItem.WH_QTY + inputItem.NG_QTY) == 0 ? 0 : (Math.Round((inputItem.Good_QTY * 1.0F + inputItem.WH_QTY * 1.0F) / (inputItem.Good_QTY * 1.0F + inputItem.WH_QTY + inputItem.NG_QTY * 1.0F) * 100, 2)),
                                WIP_QTY = inputItem.WIP_QTY
                            };
                if (searchModel.Color != "ALL")
                {
                    query = query.Where(p => p.Color == searchModel.Color || p.Color == null || p.Color == "");
                }
                if (QueryType == "QueryReportDatas")
                {
                    query = query.Where(p => p.Is_Comfirm == 1);
                }
                return query.OrderBy(o => o.Process_Seq);
            }
            #endregion
            #region 从Product_input_History表查询数据
            else
            {
                var query = from inputItem in DataContext.Product_Input_History
                            join user in DataContext.System_Users on inputItem.Creator_UID equals user.Account_UID
                            where (inputItem.Customer == searchModel.Customer && inputItem.Project == searchModel.Project
                            && inputItem.Product_Phase == searchModel.Product_Phase && inputItem.Part_Types == searchModel.Part_Types
                            && DbFunctions.TruncateTime(inputItem.Product_Date) == searchModel.Reference_Date.Date
                            )
                            select new PPCheckDataItemOrigOriginal
                            {
                                Product_Stage = inputItem.Product_Stage,
                                Is_Comfirm = inputItem.Is_Comfirm == false ? 0 : 1,
                                Color = inputItem.Color ?? "",
                                Time_Interval = inputItem.Time_Interval,
                                Product_Input_UID = inputItem.Product_UID,
                                Process_Seq = inputItem.Process_Seq,
                                Place = inputItem.Place,
                                FunPlant = inputItem.FunPlant,
                                Process = inputItem.Process,
                                FunPlant_Manager = inputItem.DRI,
                                Target_Yield = Math.Round(inputItem.Target_Yield * 100.00, 2),
                                Product_Plan = inputItem.Prouct_Plan,
                                Product_Plan_Sum = (inputItem.Prouct_Plan) / 12,
                                Picking_QTY = inputItem.Picking_QTY,
                                Picking_MismatchFlag = inputItem.Picking_MismatchFlag,
                                WH_Picking_QTY = inputItem.WH_Picking_QTY,
                                Good_QTY = inputItem.Good_QTY,
                                Good_MismatchFlag = inputItem.Good_MismatchFlag,
                                Adjust_QTY = inputItem.Adjust_QTY,
                                WH_QTY = inputItem.WH_QTY,
                                NG_QTY = inputItem.NG_QTY,
                                Rolling_Yield_Rate = inputItem.Prouct_Plan == 0 ? 0 : (Math.Round((inputItem.Good_QTY) / (inputItem.Prouct_Plan / 12.0) * 100.00, 2)),
                                Finally_Field = (inputItem.Good_QTY + inputItem.WH_QTY + inputItem.NG_QTY) == 0 ? 0 : (Math.Round((inputItem.Good_QTY * 1.0F + inputItem.WH_QTY * 1.0F) / (inputItem.Good_QTY * 1.0F + inputItem.WH_QTY + inputItem.NG_QTY * 1.0F) * 100, 2)),
                                WIP_QTY = inputItem.WIP_QTY
                            };
                if (searchModel.Color != "ALL")
                {
                    query = query.Where(p => p.Color == searchModel.Color || p.Color == null || p.Color == "");
                }
                if (QueryType == "QueryReportDatas")
                {
                    query = query.Where(p => p.Is_Comfirm == 1);
                }
                return query.OrderBy(o => o.Process_Seq);
            }
            #endregion
        }
        /// <summary>
        /// 查询当前时段的数据
        /// </summary>
        /// <param name="query1"></param>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private IQueryable<PPCheckDataItem> QueryNowIntervalDatas(IQueryable<PPCheckDataItemOrigOriginal> query1, PPCheckDataSearch searchModel)
        {

            var query = query1.Where(p => p.Time_Interval == searchModel.Tab_Select_Text).Select(g => new PPCheckDataItem
            {
                Color = g.Color,
                Product_Stage = g.Product_Stage,
                Is_Comfirm = g.Is_Comfirm,
                Product_Input_UID = g.Product_Input_UID,
                Process_Seq = g.Process_Seq,
                Place = g.Place,
                FunPlant = g.FunPlant,
                Process = g.Process,
                FunPlant_Manager = g.FunPlant_Manager,
                Target_Yield = g.Target_Yield,
                Product_Plan = g.Product_Plan,
                Product_Plan_Sum = g.Product_Plan_Sum,
                Picking_QTY = g.Picking_QTY,
                Picking_MismatchFlag = g.Picking_MismatchFlag,
                WH_Picking_QTY = g.WH_Picking_QTY,
                Good_QTY = g.Good_QTY,
                Good_MismatchFlag = g.Good_MismatchFlag,
                Adjust_QTY = g.Adjust_QTY,
                WH_QTY = g.WH_QTY,
                NG_QTY = g.NG_QTY,
                Rolling_Yield_Rate = g.Rolling_Yield_Rate,
                Finally_Field = g.Finally_Field,
                WIP_QTY = g.WIP_QTY
            });
            return query.OrderBy(o => o.Process_Seq);
        }
        /// <summary>
        /// 查询当前累计数据
        /// </summary>
        /// <param name="query1"></param>
        /// <param name="searchModel"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private IQueryable<PPCheckDataItem> QueryAllDatas(IQueryable<PPCheckDataItemOrigOriginal> query1, PPCheckDataSearch searchModel, string tableName)
        {
            //获取当前时段序号
            var nowIntervalNo = DateCompareHelper.GetNowTimeIntervalNo();
            if (searchModel.Reference_Date.Date < System.DateTime.Now.Date)
            {
                nowIntervalNo = 12;
            }
            var query = query1.GroupBy(x => new
            {
                Color = x.Color.Trim(),
                x.Process_Seq,
                Place = x.Place.Trim(),
                FunPlant = x.FunPlant.Trim(),
                FunPlant_Manager = x.FunPlant_Manager.Trim(),
                Process = x.Process.Trim(),
                x.Target_Yield,
                x.Product_Plan,
                x.Product_Plan_Sum
            }).Select(g => new PPCheckDataItem
            {
                Color = g.Key.Color,
                Product_Stage = g.Max(p => p.Product_Stage),
                Is_Comfirm = 1,
                Product_Input_UID = g.Max(p => p.Product_Input_UID),
                Process_Seq = g.Key.Process_Seq,
                Place = g.Key.Place,
                FunPlant = g.Key.FunPlant,
                Process = g.Key.Process,
                FunPlant_Manager = g.Key.FunPlant_Manager,
                Target_Yield = g.Key.Target_Yield,
                Product_Plan = g.Key.Product_Plan,
                Product_Plan_Sum = (g.Key.Product_Plan_Sum) * (nowIntervalNo),
                Picking_QTY = g.Sum(p => p.Picking_QTY),
                Picking_MismatchFlag = "NULL",
                WH_Picking_QTY = g.Sum(p => p.WH_Picking_QTY),
                Good_QTY = g.Sum(p => p.Good_QTY),
                Good_MismatchFlag = "NULL",
                Adjust_QTY = g.Sum(p => p.Adjust_QTY),
                WH_QTY = g.Sum(p => p.WH_QTY),
                NG_QTY = g.Sum(p => p.NG_QTY),
                Rolling_Yield_Rate = (g.Key.Product_Plan_Sum) * (nowIntervalNo) == 0 ? 0 : Math.Round(g.Sum(p => p.Good_QTY) / ((g.Key.Product_Plan_Sum) * (nowIntervalNo) * 1.0F) * 100, 2),
                Finally_Field = (g.Sum(p => p.Good_QTY) + g.Sum(p => p.WH_QTY) + g.Sum(p => p.NG_QTY)) == 0 ? 0 : Math.Round((g.Sum(p => p.Good_QTY) * 1.0F + g.Sum(p => p.WH_QTY) * 1.0F) / (g.Sum(p => p.Good_QTY) * 1.0F + g.Sum(p => p.WH_QTY) * 1.0F + g.Sum(p => p.NG_QTY) * 1.0F) * 100, 2),
                WIP_QTY = g.Max(p => p.WIP_QTY)
            });
            var queryEntity = query.GroupBy(x => new
            {
                x.Color,
                x.Process_Seq,
                x.Place,
                x.FunPlant,
                x.Process,
                x.Target_Yield,
                x.Product_Plan,
                x.Product_Plan_Sum
            }).Select(g => new PPCheckDataItem
            {
                Color = g.Key.Color,
                Product_Stage = g.Max(p => p.Product_Stage),
                Is_Comfirm = 0,
                Product_Input_UID = g.Max(p => p.Product_Input_UID),
                Process_Seq = g.Key.Process_Seq,
                Place = g.Key.Place,
                FunPlant = g.Key.FunPlant,
                Process = g.Key.Process,
                FunPlant_Manager = g.Max(p => p.FunPlant_Manager),
                Target_Yield = g.Key.Target_Yield,
                Product_Plan = g.Key.Product_Plan,
                Product_Plan_Sum = g.Key.Product_Plan_Sum,
                Picking_QTY = g.Sum(p => p.Picking_QTY),
                Picking_MismatchFlag = "NULL",
                WH_Picking_QTY = g.Sum(p => p.WH_Picking_QTY),
                Good_QTY = g.Sum(p => p.Good_QTY),
                Good_MismatchFlag = "NULL",
                Adjust_QTY = g.Sum(p => p.Adjust_QTY),
                WH_QTY = g.Sum(p => p.WH_QTY),
                NG_QTY = g.Sum(p => p.NG_QTY),
                Rolling_Yield_Rate = g.Key.Product_Plan_Sum == 0 ? 0 : Math.Round((g.Sum(p => p.Good_QTY) * 1.00 / (g.Key.Product_Plan_Sum)) * 100, 2),
                Finally_Field = (g.Sum(p => p.Good_QTY) + g.Sum(p => p.WH_QTY) + g.Sum(p => p.NG_QTY)) == 0 ? 0 : Math.Round((g.Sum(p => p.Good_QTY) * 1.0F + g.Sum(p => p.WH_QTY) * 1.0F) / (g.Sum(p => p.Good_QTY) * 1.0F + g.Sum(p => p.WH_QTY) + g.Sum(p => p.NG_QTY) * 1.0F) * 100, 2),
                WIP_QTY = g.Sum(p => p.WIP_QTY)
            });
            return QueryAllIntervalDatas(tableName, queryEntity);
        }
        /// <summary>
        /// 查询当前累计数据的SubFunction
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        private IQueryable<PPCheckDataItem> QueryAllIntervalDatas(string tableName, IQueryable<PPCheckDataItem> queryEntity)
        {
            if (tableName == "Product_Input")
            {
                var queryAll = from q in queryEntity
                               join s in DataContext.Product_Input on q.Product_Input_UID equals s.Product_UID
                               select new PPCheckDataItem
                               {
                                   Color = q.Color,
                                   Product_Stage = s.Product_Stage,
                                   Is_Comfirm = s.Is_Comfirm == false ? 0 : 1,
                                   Product_Input_UID = q.Product_Input_UID,
                                   Process_Seq = q.Process_Seq,
                                   Place = q.Place,
                                   FunPlant = q.FunPlant,
                                   Process = q.Process,
                                   FunPlant_Manager = s.DRI,
                                   Target_Yield = q.Target_Yield,
                                   Product_Plan = q.Product_Plan,
                                   Product_Plan_Sum = q.Product_Plan_Sum,
                                   Picking_QTY = q.Picking_QTY,
                                   Picking_MismatchFlag = s.Picking_MismatchFlag,
                                   WH_Picking_QTY = q.WH_Picking_QTY,
                                   Good_QTY = q.Good_QTY,
                                   Good_MismatchFlag = s.Good_MismatchFlag,
                                   Adjust_QTY = q.Adjust_QTY,
                                   WH_QTY = q.WH_QTY,
                                   NG_QTY = q.NG_QTY,
                                   Rolling_Yield_Rate =
                                       q.Product_Plan_Sum == 0 ? 0 : (Math.Round((q.Good_QTY * 100.00 / q.Product_Plan_Sum), 2)),
                                   Finally_Field = q.Finally_Field,
                                   WIP_QTY = s.WIP_QTY
                               };
                return queryAll.OrderBy(o => o.Process_Seq);
            }
            else
            {
                var queryAll = from q in queryEntity
                               join s in DataContext.Product_Input_History on q.Product_Input_UID equals s.Product_UID
                               select new PPCheckDataItem
                               {
                                   Color = q.Color,
                                   Product_Stage = s.Product_Stage,
                                   Is_Comfirm = s.Is_Comfirm == false ? 0 : 1,
                                   Product_Input_UID = q.Product_Input_UID,
                                   Process_Seq = q.Process_Seq,
                                   Place = q.Place,
                                   FunPlant = q.FunPlant,
                                   Process = q.Process,
                                   FunPlant_Manager = s.FunPlant_Manager,
                                   Target_Yield = q.Target_Yield,
                                   Product_Plan = q.Product_Plan,
                                   Product_Plan_Sum = q.Product_Plan_Sum,
                                   Picking_QTY = q.Picking_QTY,
                                   Picking_MismatchFlag = s.Picking_MismatchFlag,
                                   WH_Picking_QTY = q.WH_Picking_QTY,
                                   Good_QTY = q.Good_QTY,
                                   Good_MismatchFlag = s.Good_MismatchFlag,
                                   Adjust_QTY = q.Adjust_QTY,
                                   WH_QTY = q.WH_QTY,
                                   NG_QTY = q.NG_QTY,
                                   Rolling_Yield_Rate =
                            q.Product_Plan_Sum == 0 ? 0 : (Math.Round((q.Good_QTY * 100.00 / q.Product_Plan_Sum), 2)),
                                   Finally_Field = q.Finally_Field,
                                   WIP_QTY = s.WIP_QTY
                               };
                return queryAll.OrderBy(o => o.Process_Seq);
            }
        }
        public List<Daily_ProductReportItem> QueryAll_ReportData(ReportDataSearch search, string nowInterval, string nowDate, out int count)
        {
            var Time_Interval = new SqlParameter("Time_Interval", search.Tab_Select_Text);
            var Product_Date = new SqlParameter("Product_Date", search.Reference_Date);
            var Customer = new SqlParameter("Customer", search.Customer);
            var Project = new SqlParameter("Project", search.Project);
            var Product_Phase = new SqlParameter("Product_Phase", search.Product_Phase);
            var Part_Types = new SqlParameter("Part_Types", search.Part_Types);
            var Color = new SqlParameter("Color", search.Color);
            var Now_Interval = new SqlParameter("Now_Interval", nowInterval);
            var Now_Date=new SqlParameter("Now_Date", System.Convert.ToDateTime(nowDate) );

            IEnumerable<Daily_ProductReportItem> result = ((IObjectContextAdapter)this.DataContext).ObjectContext.ExecuteStoreQuery<Daily_ProductReportItem>("usp_AllProductData @Time_Interval,@Product_Date , @Customer, @Project, @Product_Phase, @Part_Types, @Color,@Now_Interval,@Now_Date",
                Time_Interval, Product_Date, Customer, Project, Product_Phase, Part_Types, Color, Now_Interval, Now_Date).ToArray();
            count = result.Count();
            if (search.Tab_Select_Text == "Night_Sum" || search.Tab_Select_Text == "Daily_Sum"|| search.Tab_Select_Text =="ALL")
            {
                var temp = from item in result
                           select new Daily_ProductReportItem()
                           {
                               Product_Stage = item.Product_Stage,
                               Process_Seq = item.Process_Seq,
                               Place = item.Place,
                               FunPlant = item.FunPlant,
                               Process = item.Process,
                               Color = item.Color,
                               DRI = item.DRI,
                               Target_Yield = item.Target_Yield,
                               All_Product_Plan = item.All_Product_Plan,
                               All_Product_Plan_Sum = item.All_Product_Plan_Sum,
                               All_Picking_QTY = item.All_Picking_QTY,
                               All_Picking_MismatchFlag = item.All_Picking_MismatchFlag,
                               All_WH_Picking_QTY = item.All_WH_Picking_QTY,
                               All_Good_QTY = item.All_Good_QTY,
                               All_Good_MismatchFlag = item.All_Good_MismatchFlag,
                               All_Adjust_QTY = item.All_Adjust_QTY,
                               All_WH_QTY = item.All_WH_QTY,
                               All_NG_QTY = item.All_NG_QTY,
                               All_Rolling_Yield_Rate = item.All_Rolling_Yield_Rate,
                               All_Finally_Field = item.All_Finally_Field,
                               Product_Plan = 0,
                               Picking_QTY = 0,
                               Picking_MismatchFlag = string.Empty,
                               WH_Picking_QTY = 0,
                               Good_QTY = 0,
                               Good_MismatchFlag = string.Empty,
                               Adjust_QTY = 0,
                               WH_QTY = 0,
                               NG_QTY = 0,
                               Rolling_Yield_Rate = (decimal)0.00,
                               Finally_Field = (decimal)0.00,
                               WIP_QTY = item.WIP_QTY
                           };

                result = temp.Distinct(new ReportComparer());
            }
            return result.ToList();
        }

        #endregion
        //新建Compare类
        public class ReportComparer : IEqualityComparer<Daily_ProductReportItem>
        {
            #region IEqualityComparer<User> 成员  
            public bool Equals(Daily_ProductReportItem x, Daily_ProductReportItem y)
            {
                if (x.Product_Plan == y.Product_Plan && x.Picking_QTY == y.Picking_QTY && x.Product_Stage == y.Product_Stage &&
                    x.Process_Seq == y.Process_Seq && x.Place == y.Place && x.FunPlant == y.FunPlant && x.Process == y.Process && x.Color == y.Color &&
                    x.DRI == y.DRI && x.Target_Yield == y.Target_Yield && x.All_Product_Plan == y.All_Product_Plan
                           && x.All_Product_Plan_Sum == y.All_Product_Plan_Sum
                           && x.All_Picking_QTY == y.All_Picking_QTY
                           && x.All_Picking_MismatchFlag == y.All_Picking_MismatchFlag
                           && x.All_WH_Picking_QTY == y.All_WH_Picking_QTY
                           && x.All_Good_QTY == y.All_Good_QTY
                           && x.All_Good_MismatchFlag == y.All_Good_MismatchFlag
                           && x.All_Adjust_QTY == y.All_Adjust_QTY
                           && x.All_WH_QTY == y.All_WH_QTY
                           && x.All_NG_QTY == y.All_NG_QTY
                           && x.All_Rolling_Yield_Rate == y.All_Rolling_Yield_Rate
                           && x.All_Finally_Field == y.All_Finally_Field)
                    return true;
                else
                    return false;
            }

            public int GetHashCode(Daily_ProductReportItem obj)
            {
                return 0;
            }
            #endregion
        }

        #region 周，时间段 查询 生产表报 -----------------------Destiny 2016/01/31
        public List<TimeSpanReport> QueryTimeSpanReport(ReportDataSearch searchModel)
        {
            var color = new SqlParameter("Color", searchModel.Color == "ALL" ? "" : searchModel.Color);
            var startDate = new SqlParameter("StartDate", searchModel.Interval_Date_Start);
            var endDate = new SqlParameter("EndDate", searchModel.Interval_Date_End);
            var funPlant = new SqlParameter("FunPlant", string.IsNullOrEmpty(searchModel.FunPlant) ? "" : searchModel.FunPlant);
            var flowChart_Version = new SqlParameter("FlowChart_Version", searchModel.Verion_Interval);
            var flowChart_Master_UID = new SqlParameter("Flowchart_Master_UID", GetFlowchartMasterUID(searchModel));
            IEnumerable<TimeSpanReport> result = ((IObjectContextAdapter)this.DataContext).ObjectContext.ExecuteStoreQuery<TimeSpanReport>("usp_GetTimeSpanReport  @Color,@StartDate,@EndDate, @FunPlant,@FlowChart_Version,@Flowchart_Master_UID",
                color, startDate, endDate, funPlant, flowChart_Version, flowChart_Master_UID).ToArray();
            return result.ToList();
        }

        public List<WeekReport> QueryWeekReport(ReportDataSearch searchModel)
        {
            var color = new SqlParameter("Color", searchModel.Color == "ALL" ? "" : searchModel.Color);
            var SearchDate = new SqlParameter("SearchDate", searchModel.Week_Date_Start);
            var funPlant = new SqlParameter("FunPlant", string.IsNullOrEmpty(searchModel.FunPlant) ? "" : searchModel.FunPlant);
            var flowChart_Version = new SqlParameter("FlowChart_Version", searchModel.Week_Version);
            var flowChart_Master_UID = new SqlParameter("Flowchart_Master_UID", GetFlowchartMasterUID(searchModel));

            IEnumerable<WeekReport> result = ((IObjectContextAdapter)this.DataContext).ObjectContext.ExecuteStoreQuery<WeekReport>("usp_GetWeekReport  @Color,@SearchDate,@FlowChart_Version,@Flowchart_Master_UID, @FunPlant",
                color, SearchDate, flowChart_Version, flowChart_Master_UID, funPlant).ToArray();
            return result.ToList();
        }


        public int GetFlowchartMasterUID(ReportDataSearch searcharModel)
        {

            string color = (searcharModel.Color == "ALL" ? "" : searcharModel.Color);

            var query = (from inputData in DataContext.Product_Input
                         where inputData.Color == color
                         && inputData.Customer == searcharModel.Customer
                         && inputData.Project == searcharModel.Project
                         && inputData.Product_Phase == searcharModel.Product_Phase
                         select inputData.FlowChart_Master_UID).Union(
                        from inputHistoryData in DataContext.Product_Input_History
                        where inputHistoryData.Color == color
                        && inputHistoryData.Customer == searcharModel.Customer
                        && inputHistoryData.Project == searcharModel.Project
                        && inputHistoryData.Product_Phase == searcharModel.Product_Phase
                        select inputHistoryData.FlowChart_Master_UID
                        );

            return query.FirstOrDefault();
        }

        #endregion
    }
    public interface IProductInputRepository : IRepository<Product_Input>
    {
        #region ProductInput----------------------------------Justin 2015/12/21
        System_Function_Plant QueryFuncPlantInfo(string funcPlant);
        IQueryable<Product_Input> QueryProductDatas(ProcessDataSearch search, Page page, out int count);
        string GetCurrentPlantName(int uid);
        IQueryable<ProductDataDTO> QueryProcessData(ProcessDataSearch search, Page page, out int count);
        IQueryable<ProductDataDTO> QueryColorSparations(ProcessDataSearch search);
        List<Product_Input> SetProductDatas(ProductDataList PDataList);
        string ExecAlterSp(Product_Input search);
        string ExecWIPSp(Product_Input search);
        string ExecUpdateWIPSp(Product_Input search);
        #endregion
        #region 查询Product_Input及Product_Input_Hitory-----------Sidney 2015/12/20
        IQueryable<PPCheckDataItem> QueryPpCheckDatas(PPCheckDataSearch search, Page page, out int count, string QueryType);
        IQueryable<PPCheckDataItem> QueryHistoryDatas(PPCheckDataSearch search, Page page, out int count, string QueryType);
        List<string> CheckFunPlantDataIsFull(PPCheckDataSearch searchModel);
        List<Daily_ProductReportItem> QueryAll_ReportData(ReportDataSearch search,string nowInterval,string nowDate, out int count);
        #endregion


        List<TimeSpanReport> QueryTimeSpanReport(ReportDataSearch searchModel);
        List<WeekReport> QueryWeekReport(ReportDataSearch searchModel);
    }
}
