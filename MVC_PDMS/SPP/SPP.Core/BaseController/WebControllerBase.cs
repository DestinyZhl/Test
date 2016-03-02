using Newtonsoft.Json;
using SPP.Common.Constants;
using SPP.Core.Authentication;
using SPP.Core.Filters;
using SPP.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Routing;
using SPP.Common.Helpers;

namespace SPP.Core.BaseController
{
    [SPPWebAuthorize]
    [UnauthorizedError]
    public abstract class WebControllerBase : Controller
    {
        public CurrentUser CurrentUser { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (CurrentUser == null)
            {
                CurrentUser = new CurrentUser();
            }
            base.Initialize(requestContext);
        }
    }

    public class CurrentUser
    {
        public int AccountUId
        {
            get
            {
                if (HttpContext.Current.Session[SessionConstants.CurrentAccountUID] == null)
                {
                    var cookie = HttpContext.Current.Request.Cookies[SessionConstants.CurrentAccountUID];

                    if (cookie!=null)
                    {
                        HttpContext.Current.Session[SessionConstants.CurrentAccountUID] = cookie.Value;
                    }
                    else
                    {
                        var apiUrl = string.Format("Common/GetSystemUserByNTId/?ntid={0}", HttpContext.Current.User.Identity.Name);
                        var responMessage = APIHelper.APIGetAsync(apiUrl);
                        var result = JsonConvert.DeserializeObject<SystemUserDTO>(responMessage.Content.ReadAsStringAsync().Result);

                        HttpContext.Current.Session[SessionConstants.CurrentAccountUID] = result.Account_UID;
                    }
                }
                return Convert.ToInt32(HttpContext.Current.Session[SessionConstants.CurrentAccountUID]);
            }
        }
        public static string UserName
        {
            get
            {
                if (HttpContext.Current.Session[SessionConstants.CurrentUserName] == null)
                {
                    var cookie = HttpContext.Current.Request.Cookies[SessionConstants.CurrentUserName];

                    if (cookie != null)
                    {
                        HttpContext.Current.Session[SessionConstants.CurrentUserName] = cookie.Value;
                    }
                    else
                    {
                        var apiUrl = string.Format("Common/GetSystemUserByNTId/?ntid={0}", HttpContext.Current.User.Identity.Name);
                        var responMessage = APIHelper.APIGetAsync(apiUrl);
                        var result = JsonConvert.DeserializeObject<SystemUserDTO>(responMessage.Content.ReadAsStringAsync().Result);

                        HttpContext.Current.Session[SessionConstants.CurrentUserName] = result.User_Name;
                    }
                }
                return HttpContext.Current.Session[SessionConstants.CurrentUserName].ToString();
            }
        }

        public IEnumerable<SystemPlantDTO> ValidPlants
        {
            get
            {
                if (HttpContext.Current.Session[SessionConstants.CurrentUserValidPlants] == null)
                {
                    var apiUrl = string.Format("Common/GetValidPlantsByUserUId/{0}", this.AccountUId);
                    var responMessage = APIHelper.APIGetAsync(apiUrl);
                    var result = JsonConvert.DeserializeObject<IEnumerable<SystemPlantDTO>>(responMessage.Content.ReadAsStringAsync().Result);

                    HttpContext.Current.Session[SessionConstants.CurrentUserValidPlants] = result;
                }
                return HttpContext.Current.Session[SessionConstants.CurrentUserValidPlants] as IEnumerable<SystemPlantDTO>;
            }
        }

        public IEnumerable<SystemBUMDTO> ValidBUMs
        {
            get
            {
                if (HttpContext.Current.Session[SessionConstants.CurrentUserValidBUMs] == null)
                {
                    var apiUrl = string.Format("Common/GetValidBUMsByUserUId/{0}", this.AccountUId);
                    var responMessage = APIHelper.APIGetAsync(apiUrl);
                    var result = JsonConvert.DeserializeObject<IEnumerable<SystemBUMDTO>>(responMessage.Content.ReadAsStringAsync().Result);

                    HttpContext.Current.Session[SessionConstants.CurrentUserValidBUMs] = result;
                }
                return HttpContext.Current.Session[SessionConstants.CurrentUserValidBUMs] as IEnumerable<SystemBUMDTO>;
            }
        }

        public IEnumerable<SystemBUDDTO> ValidBUDs
        {
            get
            {
                if (HttpContext.Current.Session[SessionConstants.CurrentUserValidBUDs] == null)
                {
                    var apiUrl = string.Format("Common/GetValidBUDsByUserUId/{0}", this.AccountUId);
                    var responMessage = APIHelper.APIGetAsync(apiUrl);
                    var result = JsonConvert.DeserializeObject<IEnumerable<SystemBUDDTO>>(responMessage.Content.ReadAsStringAsync().Result);

                    HttpContext.Current.Session[SessionConstants.CurrentUserValidBUDs] = result;
                }
                return HttpContext.Current.Session[SessionConstants.CurrentUserValidBUDs] as IEnumerable<SystemBUDDTO>;
            }
        }

        public IEnumerable<SystemOrgDTO> ValidOrgs
        {
            get
            {
                if (HttpContext.Current.Session[SessionConstants.CurrentUserValidOrgs] == null)
                {
                    var apiUrl = string.Format("Common/GetValidOrgsByUserUId/{0}", this.AccountUId);
                    var responMessage = APIHelper.APIGetAsync(apiUrl);
                    var result = JsonConvert.DeserializeObject<IEnumerable<SystemOrgDTO>>(responMessage.Content.ReadAsStringAsync().Result);

                    HttpContext.Current.Session[SessionConstants.CurrentUserValidOrgs] = result;
                }
                return HttpContext.Current.Session[SessionConstants.CurrentUserValidOrgs] as IEnumerable<SystemOrgDTO>;
            }
        }

        public IEnumerable<PageUnauthorizedElement> PageUnauthorizedElements
        {
            get
            {
                if (HttpContext.Current.Session[SessionConstants.PageUnauthorizedElements] == null)
                {
                    var apiUrl = string.Format("System/UnauthorizedElements/?ntid={0}", HttpContext.Current.User.Identity.Name);
                    var responMessage = APIHelper.APIGetAsync(apiUrl);
                    var result = JsonConvert.DeserializeObject<IEnumerable<PageUnauthorizedElement>>(responMessage.Content.ReadAsStringAsync().Result);

                    HttpContext.Current.Session[SessionConstants.PageUnauthorizedElements] = result;
                }
                return HttpContext.Current.Session[SessionConstants.PageUnauthorizedElements] as IEnumerable<PageUnauthorizedElement>;
            }
        }

    }
}
