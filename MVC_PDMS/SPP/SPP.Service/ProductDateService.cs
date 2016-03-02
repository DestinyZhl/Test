using SPP.Data;
using SPP.Data.Infrastructure;
using SPP.Data.Repository;
using SPP.Model.ViewModels;
using SPP.Model;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SPP.Service
{
    public interface IProductDataService
    {
        #region define ProductDataService ---------------------
        PagedListModel<ProductDataVM> QueryProductDatas(ProcessDataSearch search, Page page);
        PagedListModel<ProductDataDTO> QueryProcessData(ProcessDataSearch search, Page page);
        Product_Input QueryProductDataSingle(int uuid);
        string ModifyProduct(Product_Input ent);
        string GetCurrentPlantName(int uid);
        System_Function_Plant QueryFuncPlantInfo(string funcPlant);
        string AddProductDatas(ProductDataList productDataList);
        PagedListModel<TimeSpanReportVM> QueryTimeSpanReport(ReportDataSearch searchModel);
        PagedListModel<WeekReportVM> QueryWeekReport(ReportDataSearch searchModel);

        #endregion //define System interface
    }

    public class ProductDataService : IProductDataService
    {
        #region Private interfaces properties
        private readonly IUnitOfWork unitOfWork;
        private readonly IProductInputRepository productInputRepository;


        #endregion //Private interfaces properties

        #region Service constructor
        public ProductDataService(
            IProductInputRepository productInputRepository,
            IUnitOfWork unitOfWork)
        {
            this.productInputRepository = productInputRepository;
            this.unitOfWork = unitOfWork;

        }
        #endregion //Service constructor
        /// <summary>
        /// 通过查询条件，查询生产数据，并返回数据对象数据集
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public PagedListModel<ProductDataVM> QueryProductDatas(ProcessDataSearch searchModel, Page page)
        {
            var totalCount = 0;
            var ProductDatas = productInputRepository.QueryProductDatas(searchModel, page, out totalCount);

            IList<ProductDataVM> ProductDatasDTO = new List<ProductDataVM>();

            foreach (var item in ProductDatas)
            {
                var pd = new ProductDataVM();
                pd.Is_Comfirm = item.Is_Comfirm;
                pd.Product_Date = item.Product_Date;
                pd.Time_Interval = item.Time_Interval;
                pd.Customer = item.Customer;
                pd.Project = item.Project;
                pd.Part_Types = item.Part_Types;
                pd.FunPlant = item.FunPlant;
                pd.FunPlant_Manager = item.FunPlant_Manager;
                pd.Product_Phase = item.Product_Phase;
                pd.Process_Seq = item.Process_Seq;
                pd.Place = item.Place;
                pd.Process = item.Process;
                pd.FlowChart_Master_UID = item.FlowChart_Master_UID;
                pd.FlowChart_Version = item.FlowChart_Version;
                pd.Color = item.Color;
                pd.Prouct_Plan = item.Prouct_Plan;
                pd.Product_Stage = item.Product_Stage;
                pd.Target_Yield = item.Target_Yield;
                pd.Good_QTY = item.Good_QTY;
                pd.Picking_QTY = item.Picking_QTY;
                pd.WH_Picking_QTY = item.WH_Picking_QTY;
                pd.NG_QTY = item.NG_QTY;
                pd.WH_QTY = item.WH_QTY;
                pd.WIP_QTY = item.WIP_QTY;
                pd.Adjust_QTY = item.Adjust_QTY;
                pd.Creator_UID = item.Creator_UID;
                pd.Create_Date = item.Create_Date;
                pd.Material_No = item.Material_No;
                pd.Modified_UID = item.Modified_UID;
                pd.Modified_Date = item.Modified_Date;
                pd.Product_UID = item.Product_UID;
                pd.Good_MismatchFlag = item.Good_MismatchFlag;
                if (!string.IsNullOrWhiteSpace(item.Good_MismatchFlag))
                {
                    pd.Good_Contact = productInputRepository.QueryFuncPlantInfo(item.Good_MismatchFlag).FunPlant_Contact;
                }

                pd.Picking_MismatchFlag = item.Picking_MismatchFlag;
                if (!string.IsNullOrWhiteSpace(item.Picking_MismatchFlag))
                {
                    pd.Picking_Contact = productInputRepository.QueryFuncPlantInfo(item.Picking_MismatchFlag).FunPlant_Contact;
                }

                ProductDatasDTO.Add(pd);
            }

            return new PagedListModel<ProductDataVM>(totalCount, ProductDatasDTO);
        }

        /// <summary>
        /// 批量新增生产数据
        /// </summary>
        /// <param name="productDataList"></param>
        /// <returns></returns>
        public string AddProductDatas(ProductDataList productDataList)
        {
            List<ProductDataItem> pDataList = productDataList.ProductLists;
            Product_Input search = new Product_Input();
            int i = 0;
            foreach (ProductDataItem pData in pDataList)
            {
                Product_Input item = new Product_Input();
                item.Adjust_QTY = pData.Adjust_QTY;
                item.Color = pData.Color;
                item.Create_Date = pData.Create_Date;
                item.Creator_UID = pData.Creator_UID;
                item.Customer = pData.Customer;
                item.FlowChart_Master_UID = pData.FlowChart_Master_UID;
                item.FlowChart_Version = pData.FlowChart_Version;
                item.FunPlant = pData.FunPlant;
                item.FunPlant_Manager = pData.FunPlant_Manager;
                item.Good_QTY = pData.Good_QTY;
                item.Is_Comfirm = pData.Is_Comfirm == true ? true : false;
                item.Modified_Date = DateTime.Now;
                item.Modified_UID = pData.Creator_UID;
                item.NG_QTY = pData.NG_QTY;
                item.Part_Types = pData.Part_Types;
                item.Picking_QTY = pData.Picking_QTY;
                item.Place = pData.Place;
                item.Process = pData.Process;
                item.Process_Seq = pData.Process_Seq;
                item.Product_Date = pData.Product_Date;
                item.Product_Phase = pData.Product_Phase;
                item.Product_Stage = pData.Product_Stage;
                item.Project = pData.Project;
                item.Prouct_Plan = pData.Prouct_Plan;
                item.Target_Yield = pData.Target_Yield;
                item.Time_Interval = pData.Time_Interval;
                item.WH_Picking_QTY = pData.WH_Picking_QTY;
                item.WH_QTY = pData.WH_QTY;
                item.WIP_QTY = pData.WIP_QTY;
                item.DRI = pData.DRI;
                item.FlowChart_Detail_UID = pData.FlowChart_Detail_UID;
                item.IsLast = true;
                productInputRepository.Add(item);
                i++;
                if (i == 1)  // 取第一条数据作为查询条件即可
                {
                    search = item;
                }
            }
            try
            {
                unitOfWork.Commit();
                string message = string.Empty;
                message = productInputRepository.ExecWIPSp(search);
                if (message == "SUCCESS")
                {
                    message = productInputRepository.ExecAlterSp(search);
                }
                else
                    message += productInputRepository.ExecAlterSp(search);
                return message;
            }
            catch(Exception ex) 
            {
                return "保存出错！当数据已经保存时，再次保存会出现该错误。请刷新页面，查看数据是否正确，若数据不正确，请联系管理员。";
            }
            

        }

        /// <summary>
        /// 通过功能厂名获取功能厂对象
        /// </summary>
        /// <param name="funcPlant"></param>
        /// <returns></returns>
        public System_Function_Plant QueryFuncPlantInfo(string funcPlant)
        {
            return productInputRepository.QueryFuncPlantInfo(funcPlant);
        }

        /// <summary>
        /// 根据UID查询单条生产数据信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Product_Input QueryProductDataSingle(int uid)
        {

            var ProductDatas = productInputRepository.GetById(uid);

            return ProductDatas;
        }

        /// <summary>
        /// 根据条件 查询身材制程信息
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public PagedListModel<ProductDataDTO> QueryProcessData(ProcessDataSearch searchModel, Page page)
        {
            var totalCount = 0;
            var ProductDatas = productInputRepository.QueryProcessData(searchModel, page, out totalCount);
            //var colorSpar = productInputRepository.QueryColorSparations(searchModel);
            //IList<ProductDataDTO> colorSparDTO = new List<ProductDataDTO>();
            //var lastColorSeq = 0;
            //var ColorSparSeq = 0;
            //foreach (var cspar in colorSpar)
            //{
            //    if (cspar.Process_Seq == lastColorSeq)
            //    {
            //        ColorSparSeq = cspar.Process_Seq;
            //        break;
            //    }
            //    else
            //    {
            //        lastColorSeq = cspar.Process_Seq;
            //    }
            //}

            IList<ProductDataDTO> ProductDatasDTO = new List<ProductDataDTO>();
            bool isFork = false;
            foreach (var pd in ProductDatas)
            {
                var dto = AutoMapper.Mapper.Map<ProductDataDTO>(pd);
                ProductDatasDTO.Add(dto);
                
                //    if (pd.Process_Seq == ColorSparSeq&&isFork==false)
                //    {
                //        isFork = true;
                //    }
                //    else
                //    {
                //        var dto = AutoMapper.Mapper.Map<ProductDataDTO>(pd);
                //        if (isFork == true && pd.Process_Seq == ColorSparSeq)
                //        {
                //            dto.Color = "";
                //        }
                //        ProductDatasDTO.Add(dto);
                //    }  
            }
            return new PagedListModel<ProductDataDTO>(totalCount, ProductDatasDTO);
            //return new PagedListModel<ProductDataDTO>(totalCount-1, ProductDatasDTO);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public string ModifyProduct(Product_Input ent)
        {
            if (ent.Is_Comfirm)   //限定数据只能在，PP确认报表前才可以修改
            {
                return "PP已经确认了报表，不能修改数据。";
            }
            productInputRepository.Update(ent);
            unitOfWork.Commit();
            string message = string.Empty;
            //调用sp计算wip 和进行上下制程对比
            message = productInputRepository.ExecUpdateWIPSp(ent);
            if (message == "SUCCESS")
            {
                message = productInputRepository.ExecAlterSp(ent);
            }
            else
                message += productInputRepository.ExecAlterSp(ent);
            return message;
        }

        /// <summary>
        /// 根据uid获取功能厂名字
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string GetCurrentPlantName(int uid)
        {
            return productInputRepository.GetCurrentPlantName(uid);
        }

        public PagedListModel<TimeSpanReportVM> QueryTimeSpanReport(ReportDataSearch searchModel)
        {
            var pplist=productInputRepository.QueryTimeSpanReport(searchModel);
            List<TimeSpanReportVM> result = new List<TimeSpanReportVM>();
            foreach (TimeSpanReport data in pplist)
            {
                result.Add(new TimeSpanReportVM
                {
                    Process = data.Process,
                    SumGoodQty = data.SumGoodQty,
                    SumPlan = data.SumPlan,
                    SumYieldRate = data.SumYieldRate.ToString("P")
                });
            }
            return new PagedListModel<TimeSpanReportVM>(0, result);
        }

        public PagedListModel<WeekReportVM> QueryWeekReport(ReportDataSearch searchModel)
        {
            var totalCount = 0;
            var PPlist = productInputRepository.QueryWeekReport(searchModel);
            List<WeekReportVM> result = new List<WeekReportVM>();
            foreach (WeekReport data in PPlist)
            {
                result.Add(new WeekReportVM
                {
                    Process = data.Process,
                    SumPlan = data.SumPlan,
                    SumGoodQty = data.SumGoodQty,
                    SumYieldRate = data.SumYieldRate.ToString("P"),

                    MondayPlan = data.MondayPlan,
                    MondayGoodQty = data.MondayGoodQty,
                    MondayYieldRate = data.MondayYieldRate.ToString("P"),

                    TuesdayPlan = data.TuesdayPlan,
                    TuesdayGoodQty = data.TuesdayGoodQty,
                    TuesdayYieldRate = data.TuesdayYieldRate.ToString("P"),

                    WednesdayPlan = data.WednesdayPlan,
                    WednesdayGoodQty = data.WednesdayGoodQty,
                    WednesdayYieldRate = data.WednesdayYieldRate.ToString("P"),

                    ThursdayPlan = data.ThursdayPlan,
                    ThursdayGoodQty = data.ThursdayGoodQty,
                    ThursdayYieldRate = data.ThursdayYieldRate.ToString("P"),

                    FridayPlan = data.FridayPlan,
                    FridayGoodQty = data.FridayGoodQty,
                    FridayYieldRate = data.FridayYieldRate.ToString("P"),

                    SaterdayPlan = data.SaterdayPlan,
                    SaterdayGoodQty = data.SaterdayGoodQty,
                    SaterdayYieldRate = data.SaterdayYieldRate.ToString("P"),

                    SundayPlan = data.SundayPlan,
                    SundayGoodQty = data.SundayGoodQty,
                    SundayYieldRate = data.SundayYieldRate.ToString("P")

                });
            }
            return new PagedListModel<WeekReportVM>(0, result);
        }

    }
}

