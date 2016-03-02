using Newtonsoft.Json;
using SPP.Core;
using SPP.Data;
using SPP.Model;
using SPP.Model.ViewModels;
using SPP.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using SPP.Core.Authentication;
using SPP.Model.ViewModels.Settings;
using SPP.Core.Authentication;

namespace SPP.WebAPI.Controllers
{
    public class ProductInputController : ApiControllerBase
    {
        IProductDataService ProductDataService;

        public ProductInputController(IProductDataService ProductDataService)
        {
            this.ProductDataService = ProductDataService;
        }
        /// <summary>
        /// 获取制程信息，目前未使用
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [IgnoreDBAuthorize]
        public IHttpActionResult QueryProcessAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<ProcessDataSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);

            var process = ProductDataService.QueryProcessData(searchModel, page);
            return Ok(process);
        }

        /// <summary>
        /// 获取生产数据信息，该FlowChart的所有生产数据信息或者制程信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]

        public IHttpActionResult QueryProductsAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<ProcessDataSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);

            var datas = ProductDataService.QueryProductDatas(searchModel, page);
            int count = datas.TotalItemCount;
            //判断查询条件中有无符合条件的生产数据，若无数据，查询符合条件的制程信息。
            if (count == 0)
            {
                var process = ProductDataService.QueryProcessData(searchModel, page);
                return Ok(process);
            }
            else
                return Ok(datas);
        }
       
        /// <summary>
        /// 修改生产数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [IgnoreDBAuthorize]
        public string ModifyProductDataAPI(ProductDataDTO dto)
        {

            var ent = ProductDataService.QueryProductDataSingle(dto.Product_UID);
            ent.Picking_QTY = dto.Picking_QTY;
            ent.WH_Picking_QTY = dto.WH_Picking_QTY;
            ent.Good_QTY = dto.Good_QTY;
            ent.Adjust_QTY = dto.Adjust_QTY;
            ent.NG_QTY = dto.NG_QTY;
            ent.WH_QTY = dto.WH_QTY;
            ent.Modified_UID = dto.Modified_UID;
            ent.Modified_Date = DateTime.Now;

            var plantstring = ProductDataService.ModifyProduct(ent);
            if (plantstring != "SUCCESS")
                return plantstring;

            else
                return "SUCCESS";
        }

        /// <summary>
        /// 根据UID查询制定的生产数据
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [AcceptVerbs("Get")]
        [IgnoreDBAuthorize]
        public IHttpActionResult QueryProductDataAPI(int uuid)
        {
            var dto = new ProductDataDTO();
            dto = AutoMapper.Mapper.Map<ProductDataDTO>(ProductDataService.QueryProductDataSingle(uuid));
            return Ok(dto);
        }

        /// <summary>
        /// 根据用户uid 查询该用户所属的功能厂名字
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [IgnoreDBAuthorize]
        public string GetCurrentPlantNameAPI(int uid)
        {
            var datas = ProductDataService.GetCurrentPlantName(uid);
            return datas;
        }


        /// <summary>
        /// 批量存储数据，接收的是对量的数值对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SaveDatasAPI(dynamic data)
        {
            var entity = JsonConvert.DeserializeObject<ProductDataList>(data.ToString());
            return ProductDataService.AddProductDatas(entity);
        }
        [IgnoreDBAuthorize]

        public System_Function_Plant QueryFuncPlantInfoAPI(string funcPlant)
        {
            var datas = ProductDataService.QueryFuncPlantInfo(funcPlant);
            return datas;
        }

        /// <summary>
        /// 为未填写数据的FunPlant填充0值---------------------Sidney 2016-1-9
        /// </summary>
        /// <returns></returns>
        public string FillZeroProductDataAPI(ZeroProcessDataSearch funPlantInfo)
        {
            string result = null;
            //获取未填写数据的FunPlant的Data信息
            var searchModel =AutoMapper.Mapper.Map<ProcessDataSearch>(funPlantInfo);
            Page page = new Page();
            page.PageSize = 10;
            var process = ProductDataService.QueryProcessData(searchModel, page);

            //保存数据至Product_Input 
            ProductDataList ZeroData=new ProductDataList();
            List<ProductDataItem> ZeroDataList=new List<ProductDataItem>();
            List<ProductDataDTO> test = null;
            if (!process.Items.Any())
                return "制程或生产计划未导入！";
            else
            {
                test = AutoMapper.Mapper.Map<List<ProductDataDTO>>(process.Items);
            }
            foreach (var proList in test)
            {
                ProductDataItem ZeroDataItem = new ProductDataItem();
                ZeroDataItem.FlowChart_Detail_UID = proList.FlowChart_Detail_UID;
                ZeroDataItem.Adjust_QTY = proList.Adjust_QTY;
                ZeroDataItem.Color = proList.Color;
                ZeroDataItem.Create_Date = funPlantInfo.Create_Time;
                ZeroDataItem.Creator_UID = funPlantInfo.Create_User;
                ZeroDataItem.Customer = proList.Customer;
                ZeroDataItem.FlowChart_Master_UID = proList.FlowChart_Master_UID;
                ZeroDataItem.FlowChart_Version = proList.FlowChart_Version;
                ZeroDataItem.FunPlant = proList.FunPlant;
                ZeroDataItem.FunPlant_Manager = proList.FunPlant_Manager;
                ZeroDataItem.Good_MismatchFlag = proList.Good_MismatchFlag;
                ZeroDataItem.Good_QTY = proList.Good_QTY;
                ZeroDataItem.Is_Comfirm = false;
                ZeroDataItem.Material_No = proList.Material_No;
                ZeroDataItem.Modified_Date = funPlantInfo.Create_Time;
                ZeroDataItem.Modified_UID = funPlantInfo.Create_User;
                ZeroDataItem.NG_QTY = proList.NG_QTY;
                ZeroDataItem.Part_Types = proList.Part_Types;
                ZeroDataItem.Picking_MismatchFlag = proList.Picking_MismatchFlag;
                ZeroDataItem.Picking_QTY = proList.Picking_QTY;
                ZeroDataItem.Place = proList.Place;
                ZeroDataItem.Process = proList.Process;
                ZeroDataItem.Process_Seq = proList.Process_Seq;
                ZeroDataItem.Product_Date = proList.Product_Date;
                ZeroDataItem.Product_Phase = proList.Product_Phase;
                ZeroDataItem.Product_Stage = proList.Product_Stage;
                ZeroDataItem.Project = proList.Project;
                ZeroDataItem.Prouct_Plan = proList.Prouct_Plan;
                ZeroDataItem.Target_Yield = proList.Target_Yield;
                ZeroDataItem.Time_Interval = proList.Time_Interval;
                ZeroDataItem.WH_Picking_QTY = proList.WH_Picking_QTY;
                ZeroDataItem.WH_QTY = proList.WH_QTY;
                ZeroDataItem.WIP_QTY = proList.WIP_QTY;
                ZeroDataItem.DRI = proList.DRI;
                ZeroDataList.Add(ZeroDataItem);
            }
            ZeroData.ProductLists = ZeroDataList;
            if (ProductDataService.AddProductDatas(ZeroData) != "SUCCESS")
            {
                return funPlantInfo.Func_Plant;
            }
            else
            {
                return "SUCCESS";
            }
        }

        [IgnoreDBAuthorize]
        public IHttpActionResult QueryTimeSpanReport(ReportDataSearch searchModel)
        {
             var process = ProductDataService.QueryTimeSpanReport(searchModel);
            return Ok(process);
        }

        [IgnoreDBAuthorize]
        public IHttpActionResult QueryWeekReport(ReportDataSearch searchModel)
        {
            var process = ProductDataService.QueryWeekReport(searchModel);
            return Ok(process);
        }


    }
}