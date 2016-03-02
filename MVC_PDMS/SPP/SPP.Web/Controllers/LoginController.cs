using Newtonsoft.Json;
using SPP.Common.Constants;
using SPP.Model;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SPP.Common.Helpers;

namespace SPP.Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(LoginUserMoel loginUser, string returnUrl)
        {
            HttpResponseMessage responMessage;
            var apiPath = ConfigurationManager.AppSettings["WebApiPath"].ToString();
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(loginUser));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                responMessage = client.PostAsync(apiPath + "Login/LoginIn", content).Result;
            }

            if (responMessage.IsSuccessStatusCode)
            {
                var user = JsonConvert.DeserializeObject<AuthorizedLoginUser>(responMessage.Content.ReadAsStringAsync().Result);

                #region put token/user info into Cookie and Session and SetAuthCookie make Identity true
                if (Request.Cookies[SessionConstants.LoginTicket] != null)
                {
                    CookiesHelper.RemoveCookiesByCookieskey(Request, Response, SessionConstants.LoginTicket);
                    CookiesHelper.RemoveCookiesByCookieskey(Request, Response, SessionConstants.CurrentAccountUID);
                    CookiesHelper.RemoveCookiesByCookieskey(Request, Response, SessionConstants.CurrentUserName);
                }
                else
                {
                    CookiesHelper.AddCookies(Response, SessionConstants.LoginTicket, user.Token, 1);
                    CookiesHelper.AddCookies(Response, SessionConstants.CurrentAccountUID, user.Account_UID.ToString(), 1);
                    CookiesHelper.AddCookies(Response, SessionConstants.CurrentUserName, user.User_Name, 1);
                }

                if (Request.Cookies["APIPath"] ==null)
                {
                    CookiesHelper.AddCookies(Response, "APIPath", apiPath, 1);
                }

                FormsAuthentication.SetAuthCookie(loginUser.UserName, true);

                Session[SessionConstants.LoginTicket] = user.Token;
                Session[SessionConstants.CurrentAccountUID] = user.Account_UID.ToString();
                Session[SessionConstants.CurrentUserName] = user.User_Name; 
                #endregion

                //get ticket of login user
                var ticket = FormsAuthentication.Decrypt(user.Token);

                //set principal
                IIdentity identity = new FormsIdentity(ticket);
                IPrincipal principal = new GenericPrincipal(identity, null);
                HttpContext.User = principal;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.InvalidCode = string.Empty;

                switch (responMessage.StatusCode)
                {
                    case HttpStatusCode.Forbidden:
                        ViewBag.InvalidCode = "ACCOUNTNOTENABLED";
                        break;
                    case HttpStatusCode.NotFound:
                        ViewBag.InvalidCode = "ACCOUNTNOTEXIST";
                        break;
                    case HttpStatusCode.Unauthorized:
                        ViewBag.InvalidCode = "WRONGPASSWORD";
                        break;
                    case HttpStatusCode.InternalServerError:
                        throw new Exception("API Server Error");
                    default:
                        break;
                }
               
                return View("Index");
            }
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            #region remove all sessions and token cookie                   
            Session.RemoveAll();

            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                CookiesHelper.RemoveCookiesByCookieskey(Request, Response, FormsAuthentication.FormsCookieName);
            }
            #endregion

            return RedirectToAction("Index", "Login");
        }
    }
}