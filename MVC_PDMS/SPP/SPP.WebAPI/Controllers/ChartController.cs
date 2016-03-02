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
    public class ChartController : ApiControllerBase
    {
        IChartService ChartService;

        public ChartController(IChartService ChartService)
        {
            this.ChartService = ChartService;
        }

        [AcceptVerbs("Get")]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetFunPlantAPI(string CustomerName, string ProjectName, string ProductPhaseName,
            string PartTypesName, string Color)
        {
            var entity = ChartService.GetFunPlant(CustomerName,ProjectName,ProductPhaseName,
            PartTypesName,Color);
            return Ok(entity);
        }

        [AcceptVerbs("Get")]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetProcessAPI(string CustomerName, string ProjectName, string ProductPhaseName,
            string PartTypesName, string Color, string FunPlant)
        {
            var entity = ChartService.GetProcess(CustomerName,ProjectName,ProductPhaseName,
            PartTypesName,Color,FunPlant);
            return Ok(entity);
        }
    }
}
