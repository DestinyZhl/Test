using Newtonsoft.Json;
using SPP.Core;
using SPP.Core.Authentication;
using SPP.Model;
using SPP.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Results;

namespace SPP.WebAPI.Controllers
{
    public class EventReportManagerController : ApiControllerBase
    {
        ISettingsService settingsService;
        IEventReportManagerService EventReportManagerService;
        public EventReportManagerController(IEventReportManagerService EventReportManagerService, ISettingsService settingsService)
        {
            this.settingsService = settingsService;
            this.EventReportManagerService = EventReportManagerService;
        }
        #region WarningListDisplay ----------------------------------Destiny 2015/12/09

        [AcceptVerbs("Get")]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetWarningListAPI(int User_account_uid)
        {
            var EnumEntity = EventReportManagerService.GetWarningLists(User_account_uid);
            return Ok(EnumEntity);
        }

        [AcceptVerbs("GET")]
        public IHttpActionResult GetWarningDataByWarningUidAPI(int Warning_UID)
        {
            var result = EventReportManagerService.GetWarningDataByWarningUid(Warning_UID);
            return Ok(result);
        }

        #endregion
        #region ProductReportDisplay----------------------------------Sidney 2015/12/4
        /// <summary>
        /// 查询Product_Input的的数据API
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [AcceptVerbs("Post")]
        [IgnoreDBAuthorize]
        public IHttpActionResult QueryPPCheckDatasAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<PPCheckDataSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);
            var checkData = EventReportManagerService.QueryPPCheckDatas(searchModel, page, "QueryPPCheckDatas");
            return Ok(checkData);
        }
        /// <summary>
        /// 查询Product_Input的的数据API
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [AcceptVerbs("Post")]
        [IgnoreDBAuthorize]
        public IHttpActionResult QueryReportDatasAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<ReportDataSearch>(jsonData);
            var page = JsonConvert.DeserializeObject<Page>(jsonData);
            var checkData = EventReportManagerService.QueryReportDatas(searchModel, page, "QueryReportDatas");
            return Ok(checkData);
        }

        [AcceptVerbs("Post")]
        [IgnoreDBAuthorize]
        public IHttpActionResult CheckFunPlantDataIsFullAPI(dynamic data)
        {
            var jsonData = data.ToString();
            var searchModel = JsonConvert.DeserializeObject<PPCheckDataSearch>(jsonData);
            var checkData = EventReportManagerService.CheckFunPlantDataIsFull(searchModel);
            return Ok(checkData);
        }
        /// <summary>
        /// 修改WIP的API
        /// </summary>
        /// <param name="WIP"></param>
        /// <returns></returns>
        [IgnoreDBAuthorize]
        [AcceptVerbs("Post")]
        public IHttpActionResult EditWIPAPI(PPEditWIP WIP)
        {
            var EnumEntity = EventReportManagerService.EditWIP(WIP,WIP.Modified_UID);
            return Ok(EnumEntity);
        }
        [IgnoreDBAuthorize]
        [AcceptVerbs("Get")]
        public IHttpActionResult EditWIPViewAPI(int product_uid,int wip_qty, int wip_old, int wip_add, string comment,int modifiedUser)
        {
            var EnumEntity = EventReportManagerService.EditWIPView(product_uid,wip_qty,wip_old,wip_add,comment,modifiedUser);
            return Ok(EnumEntity);
        }
        /// <summary>
        /// 查询时段的API
        /// </summary>
        /// <param name="PageName"></param>
        /// <returns></returns>
        [IgnoreDBAuthorize]
        [AcceptVerbs("Get")]
        public IHttpActionResult GetIntervalTimeAPI(string PageName)
        {
            var EnumEntity = EventReportManagerService.GetIntervalTime(PageName);
            var enumVM = AutoMapper.Mapper.Map<List<EnumVM>>(EnumEntity);
            return Ok(enumVM);
        }
        /// <summary>
        /// 获取客户
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("Get")]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetCustomerSourceAPI()
        {
            var EnumEntity = EventReportManagerService.GetAllCustomer();
            return Ok(EnumEntity);
        }
        /// <summary>
        /// 获取专案
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [AcceptVerbs("Get")]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetProjectSourceAPI(string customer)
        {
            var EnumEntity = EventReportManagerService.GetAllProject(customer);
            return Ok(EnumEntity);
        }
        /// <summary>
        /// 获取生产阶段
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        [AcceptVerbs("Get")]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetProductPhaseSourceAPI(string customer, string project)
        {
            var EnumEntity = EventReportManagerService.GetAllProductPhase(customer, project);
            return Ok(EnumEntity);
        }
        /// <summary>
        /// 获取部件
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="project"></param>
        /// <param name="productphase"></param>
        /// <returns></returns>
        [AcceptVerbs("Get")]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetPartTypesSourceAPI(string customer, string project, string productphase)
        {
            var EnumEntity = EventReportManagerService.GetAllPartTypes(customer, project, productphase);
            return Ok(EnumEntity);
        }
        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="project"></param>
        /// <param name="productphase"></param>
        /// <param name="parttypes"></param>
        /// <returns></returns>
        [AcceptVerbs("Get")]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetColorSourceAPI(string customer, string project, string productphase, string parttypes)
        {
            var EnumEntity = EventReportManagerService.GetAllColor(customer, project, productphase, parttypes);
            return Ok(EnumEntity);

        }
        /// <summary>
        /// 获取时段及相关信息
        /// </summary>
        /// <param name="opType"></param>
        /// <returns></returns>
        [AcceptVerbs("Get")]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetIntervalInfoAPI(string opType)
        {
            var entity = EventReportManagerService.GetIntervalInfo(opType);
            return Ok(entity);
        }
        #endregion
        #region Day Week Month Report Function------------------------Sidney 2016/01/28
        [AcceptVerbs("Get")]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetVersionSourceAPI(string customer, string project, string productphase, string parttypes,DateTime beginTime
            ,DateTime endTime)
        {
            var EnumEntity = EventReportManagerService.GetAlVersion(customer, project, productphase, parttypes,beginTime,endTime);
            return Ok(EnumEntity);

        }

        public IHttpActionResult GetVersionBeginEndDateAPI(string customer, string project, string productphase, string parttypes, int version, DateTime startDay, DateTime endDay)
        {
            var EnumEntity = EventReportManagerService.GetVersionBeginEndDate(customer, project, productphase, parttypes, version);

            if (EnumEntity != null)
            {

                if (DateTime.Compare(startDay.Date, EnumEntity.VersionBeginDate.Date) > 0)
                {
                    EnumEntity.VersionBeginDate = startDay;
                }
                if (DateTime.Compare(endDay.Date, EnumEntity.VersionEndDate.Date) < 0 || EnumEntity.VersionEndDate.Date.ToShortDateString() == "1/1/0001")
                {
                    EnumEntity.VersionEndDate = endDay;
                }
                if (DateTime.Compare(DateTime.Now.Date, EnumEntity.VersionEndDate.Date) == 0)
                {
                    EnumEntity.VersionEndDate = DateTime.Now.AddDays(-1).Date;
                }
                EnumEntity.Interval = "从  " + EnumEntity.VersionBeginDate.ToShortDateString() + " 到 " + EnumEntity.VersionEndDate.ToShortDateString();
                return Ok(EnumEntity);
            }
            else
            {
                return Ok(EnumEntity);
            }
        }
        #endregion
    }
}