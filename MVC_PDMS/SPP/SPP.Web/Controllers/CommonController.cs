using SPP.Core;
using SPP.Core.BaseController;
using System.Net.Http;
using System.Web.Mvc;
using System.Net;

namespace SPP.Web.Controllers
{
    public class CommonController : WebControllerBase
    {
        #region System User
        /// <summary>
        /// 根据Account UID获取用户
        /// </summary>
        /// <param name="Account_UID">Account UID</param>
        /// <returns>null or entity json</returns>
        public ActionResult GetSystemUserByUId(int Account_UID)
        {
            var apiUrl = string.Format("Common/GetSystemUserByUId/{0}", Account_UID);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.StatusCode == HttpStatusCode.NotFound ? "null"
                            : responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        /// <summary>
        /// 根据NTID获取用户
        /// </summary>
        /// <param name="User_NTID">NTID</param>
        /// <returns>null or entity json</returns>
        public ActionResult GetSystemUserByNTId(string User_NTID)
        {
            var apiUrl = string.Format("Common/GetSystemUserByNTId/?ntid={0}", User_NTID);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.StatusCode == HttpStatusCode.NotFound ? "null"
                            : responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }
        #endregion //System User

        [HttpGet]
        public ActionResult GetValidPlantsByUserUId(int Account_UID)
        {
            var apiUrl = string.Format("Common/GetValidPlantsByUserUId/{0}",Account_UID);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }
        [HttpGet]
        public ActionResult GetValidBUMsByUserUId(int Account_UID)
        {
            var apiUrl = string.Format("Common/GetValidBUMsByUserUId/{0}", Account_UID);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }
        [HttpGet]
        public ActionResult GetValidBUDsByUserUId(int Account_UID)
        {
            var apiUrl = string.Format("Common/GetValidBUDsByUserUId/{0}", Account_UID);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }
        [HttpGet]
        public ActionResult GetValidOrgsByUserUId(int Account_UID)
        {
            var apiUrl = string.Format("Common/GetValidOrgsByUserUId/{0}", Account_UID);
            var responMessage = APIHelper.APIGetAsync(apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

    }
}