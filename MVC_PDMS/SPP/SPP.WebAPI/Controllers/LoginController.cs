using SPP.Core;
using SPP.Core.Authentication;
using SPP.Model;
using SPP.Service;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace SPP.WebAPI.Controllers
{
    public class LoginController : ApiControllerBase
    {

        ICommonService commonService;
        ISystemService systemService;
        ISettingsService settingService;

        public LoginController(ICommonService commonService,ISystemService systemService,ISettingsService settingService)
        {
            this.commonService = commonService;
            this.systemService = systemService;
            this.settingService = settingService;
        }
        
        [AllowAnonymous]
        public HttpResponseMessage LoginIn(LoginUserMoel loginUser)
        {
            var systemUser = commonService.GetSystemUserByNTId(loginUser.UserName);

            if (systemUser == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "ACCOUNT NOT EXIST");
            }
            if (systemUser.Enable_Flag==false)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "ACCOUNT NOT ENABLED");
            }

            var LDAPswitch = ConfigurationManager.AppSettings["LDAPAuthentication"].ToString();

            if (!string.IsNullOrWhiteSpace(LDAPswitch) && LDAPswitch.Equals("ON", StringComparison.CurrentCultureIgnoreCase))
            {
                //LDAP Authentication
                if (string.IsNullOrEmpty(loginUser.Password) || !ValidateUser.LDAPValidate(loginUser.UserName, loginUser.Password))
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "WRONG PASSWORD");
                }
            }
            else
            {
                loginUser.Password = string.Empty;
            }

            //从db获取token数据并解密
            var userlogintoken = string.Empty;
            bool refresh = systemUser.LoginToken == null;
            FormsAuthenticationTicket ticket = null;

            if (!refresh)
            {
                userlogintoken = systemUser.LoginToken;

                try
                {
                    ticket = FormsAuthentication.Decrypt(userlogintoken);
                }
                catch
                {
                    refresh = true;
                }
            }

            if (refresh || loginUser.Password != ticket.UserData || loginUser.UserName != ticket.Name)
            {
                userlogintoken = ReFreshToken(systemUser.Account_UID, loginUser.Password);
            }

            return Request.CreateResponse(new AuthorizedLoginUser {
                Account_UID = systemUser.Account_UID,
                User_Name = systemUser.User_Name,
                Token = userlogintoken
            });
        }


        private string ReFreshToken(int accountID,string password)
        {
            //refresh token
            FormsAuthenticationTicket ticket =
                    new FormsAuthenticationTicket(1, accountID.ToString(), DateTime.Now, DateTime.Now.AddMonths(3), true, password);

            var userlogintoken = FormsAuthentication.Encrypt(ticket);

            //update token in db
            var updateUser = systemService.GetSystemUserByUId(accountID);
            updateUser.LoginToken = userlogintoken;
            settingService.ModifyUser(updateUser);

            return userlogintoken;
        }

    }

}
