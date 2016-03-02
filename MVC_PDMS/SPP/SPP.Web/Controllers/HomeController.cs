using Newtonsoft.Json;
using SPP.Core;
using SPP.Core.BaseController;
using SPP.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

namespace SPP.Web.Controllers
{
    public class HomeController : WebControllerBase
    {
        public ActionResult Index()
        {
            ViewBag.PageTitle = null;
            return View();
        }

        /// <summary>
        /// Get authorized menus
        /// API Path - SYSTEMSERVICE/GETFUNCTIONSBYUSERUID
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuListPartial()
        {
            List<SystemFunctionDTO> menuModel = null;
            var apiUrl = string.Format("System/GetFunctionsByUserUId/{0}", this.CurrentUser.AccountUId);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            var item = responMessage.Content.ReadAsStringAsync().Result;

            menuModel = JsonConvert.DeserializeObject<List<SystemFunctionDTO>>(item);

            return PartialView(menuModel);
        }

        /// <summary>
        /// call by system, get unauthorized elements by page id. fronte-end hide those elements.
        /// </summary>
        /// <param name="pageUrl">page id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PageUnauthorizedElements(string pageUrl)
        {
            var pageUnauthorizedElements = this.CurrentUser.PageUnauthorizedElements.FirstOrDefault(p => p.PageURL == pageUrl);
            if (pageUnauthorizedElements == null)
            {
                return HttpNotFound();
            }
            return Json(pageUnauthorizedElements, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult ComingSoon()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult UnauthorizedRequest()
        {
            return View();
        }

        public ActionResult MenuHome()
        {
            return RedirectToAction("Index");
        }
    }
}