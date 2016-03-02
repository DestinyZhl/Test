using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using SPP.Common.Enums;
using System;
using SPP.Model;
using Newtonsoft.Json;
using SPP.Common.Constants;
using System.Collections.Generic;

namespace SPP.Core.Authentication
{
    public class SPPWebAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var identity = filterContext.HttpContext.User.Identity;
            if (identity.IsAuthenticated)
            {
                if (!filterContext.IsChildAction && !filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var actionName = filterContext.ActionDescriptor.ActionName.ToUpper();
                    var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToUpper();
                    var url = string.Format("{0}/{1}", controllerName, actionName);
                    var anonymous = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false);

                    if (!anonymous)
                    {
                        if (string.IsNullOrEmpty(APIHelper.GetUserToken()))
                        {
                            //redirect to login page if cookie and session are invalid
                            filterContext.HttpContext.Response.Redirect("~/Login", true);
                        }
                        else
                        {
                            //check page is authenticated
                            var pageUrl = string.Format("System/HasPageQulification/?controller={0}&action={1}", controllerName, actionName);
                            HttpResponseMessage responMessage = APIHelper.APIGetAsync(pageUrl);
                            var message = JsonConvert.DeserializeObject<Message>(responMessage.Content.ReadAsStringAsync().Result);
                            var result = (EnumAuthorize)Enum.Parse(typeof(EnumAuthorize), message.Content);

                            switch (result)
                            {
                                case EnumAuthorize.PageNotAuthorized:
                                    filterContext.HttpContext.Response.Redirect("~/Home/UnauthorizedRequest", true);
                                    break;
                                case EnumAuthorize.PageAuthorized:
                                    {
                                        filterContext.Controller.ViewBag.PageID = url;

                                        #region Get pagetitle from session/db
                                        IEnumerable<SystemFunctionDTO> functions;

                                        if (filterContext.RequestContext.HttpContext.Session[SessionConstants.Functions] == null)
                                        {
                                            functions
                                                = JsonConvert.DeserializeObject<IEnumerable<SystemFunctionDTO>>(
                                                    APIHelper.APIGetAsync("System/GetSystemValidFunctions").Content.ReadAsStringAsync().Result);
                                            filterContext.RequestContext.HttpContext.Session[SessionConstants.Functions] = functions;
                                        }
                                        else
                                        {
                                            functions = filterContext.RequestContext.HttpContext.Session[SessionConstants.Functions] as IEnumerable<SystemFunctionDTO>;
                                        }

                                        var target = functions.FirstOrDefault(q => q.URL == url);
                                        filterContext.Controller.ViewBag.PageTitle = target == null ? string.Empty : target.Function_Name;
                                        #endregion
                                    }
                                    break;
                                case EnumAuthorize.NotPageRequest:
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                //Anonymous user, judge if controller allows anonymous access.
                var attr = filterContext.ActionDescriptor.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>();
                bool isAnonymous = attr.Any(a => a is AllowAnonymousAttribute);

                if (!isAnonymous)
                {
                    filterContext.HttpContext.Response.Redirect("~/Login", true);
                }
            }
        }
    }
}
