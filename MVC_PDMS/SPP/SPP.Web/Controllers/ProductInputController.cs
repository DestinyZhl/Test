using Newtonsoft.Json;
using OfficeOpenXml;
using SPP.Common.Constants;
using SPP.Common.Helpers;
using SPP.Core;
using SPP.Core.BaseController;
using SPP.Model;
using SPP.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web.Mvc;
using SPP.Model.ViewModels.Settings;
using System.Web.Helpers;
using System.Net.Mail;

namespace SPP.Web.Controllers
{
    public class ProductInputController : WebControllerBase
    {
        // GET: ProductInput
        public ActionResult ProductData()
        {
            return View();
        }

        /// <summary>
        /// 根据查询条件获取制程信息
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryProcessData(ProcessDataSearch search, Page page)
        {
            var apiUrl = string.Format("ProductInput/QueryProcessAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }


        /// <summary>
        /// 根据查询条件，查出相应的生产数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult QueryProductDatas(ProcessDataSearch search, Page page)
        {

            search.Date = DateTime.Parse(search.Date.ToShortDateString());
            var apiUrl = string.Format("ProductInput/QueryProductsAPI");

            HttpResponseMessage responMessage = APIHelper.APIPostAsync(search, page, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        /// <summary>
        /// 用户warringList 和Project List那里调用，查询展现生产数据
        /// </summary>
        /// <returns></returns>
        public ActionResult QuestProductDatas()
        {
            Page page = new Page();
            page.PageSize = 10;
            if (TempData["ProcessDataSearch"] == null)
            {
                return View("ProductData");
            }
            //使用tempdata 获取传过来的查询条件
            ProcessDataSearch search = (ProcessDataSearch)TempData["ProcessDataSearch"];

            ViewBag.Func_Plant = search.Func_Plant;
            ViewBag.Customer = search.Customer;
            ViewBag.Project = search.Project;
            ViewBag.Part_Types = search.Part_Types;
            ViewBag.Product_Phase = search.Product_Phase;
            ViewBag.Date = search.Date;
            ViewBag.Time = search.Time;
            ViewBag.Flag = search.QuertFlag;
            return View("ProductData");
        }

        /// <summary>
        /// 单条数据修改product数据
        /// </summary>
        /// <param name="jsonWithProduct"></param>
        /// <returns></returns>
        public ActionResult ModifyProductData(string jsonWithProduct)
        {

            var apiUrl = "ProductInput/ModifyProductDataAPI";
            var entity = JsonConvert.DeserializeObject<ProductDataDTO>(jsonWithProduct);
            entity.Modified_UID = this.CurrentUser.AccountUId;
            entity.Modified_Date = DateTime.Now;
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        /// <summary>
        /// 批量提交保存用户输入的战情报表数据
        /// </summary>
        /// <param name="jsonWithProduct"></param>
        /// <returns></returns>
        public ActionResult SaveDatas(string jsonWithProduct)
        {

            var apiUrl = "ProductInput/SaveDatasAPI";
            var entity = JsonConvert.DeserializeObject<ProductDataList>(jsonWithProduct);

            foreach (ProductDataItem item in entity.ProductLists)
            {
                item.Creator_UID = this.CurrentUser.AccountUId;
                item.Create_Date = DateTime.Now;
                item.Modified_UID = this.CurrentUser.AccountUId;
                item.Modified_Date = DateTime.Now;
            }
            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        public ActionResult YieldChartDatas(string jsonWithProduct)
        {

            var apiUrl = "ProductInput/YieldChartDatasAPI";
            var entity = JsonConvert.DeserializeObject<YieldChartSearch>(jsonWithProduct);


            HttpResponseMessage responMessage = APIHelper.APIPostAsync(entity, apiUrl);
            var result = responMessage.Content.ReadAsStringAsync().Result;

            return Content(result, "application/json");
        }

        /// <summary>
        /// 根据UID查询特定的生产数据，用于修改数据时候
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public ActionResult QueryProductData(int uuid)
        {
            var apiUrl = string.Format("ProductInput/QueryProductDataAPI?uuid={0}", uuid);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        /// <summary>
        /// 根据当前登录人信息，获取所在功能厂名字，这里主要是在Project List中调用。
        /// </summary>
        /// <returns></returns>
        public ActionResult getCurrentPlantName()
        {
            int uid = this.CurrentUser.AccountUId;
            var apiUrl = string.Format("ProductInput/getCurrentPlantNameAPI?uid={0}", uid);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        /// <summary>
        /// 通过功能厂名获取功能厂信息  查询功能厂表，将功能厂对象返回
        /// </summary>
        /// <param name="funcPlant"></param>
        /// <returns></returns>
        public ActionResult QueryFuncPlantInfo(string funcPlant)
        {
            var apiUrl = string.Format("ProductInput/QueryFuncPlantInfoAPI?funcPlant={0}", funcPlant);
            HttpResponseMessage responMessage = APIHelper.APIGetAsync(apiUrl);

            var result = responMessage.Content.ReadAsStringAsync().Result;
            return Content(result, "application/json");
        }

        /// <summary>
        /// FillZeroProductData--------------------------Sidney 为未填写数据的FunPlant填充0值
        /// </summary>
        /// <returns></returns>
        public ActionResult FillZeroProductData(string jsonWithFunPlantInfo)
        {
            string result = "SUCCESS";
            var apiUrl = "ProductInput/FillZeroProductDataAPI";
            var Creator_UID = this.CurrentUser.AccountUId;
            var Create_Date = DateTime.Now;
            var entity = JsonConvert.DeserializeObject<ZeroFunPlantInfo>(jsonWithFunPlantInfo);
            foreach (var list in entity.ZeroList)
            {
                list.Create_Time = Create_Date;
                list.Create_User = Creator_UID;
                HttpResponseMessage responMessage = APIHelper.APIPostAsync(list, apiUrl);
                var result1 = responMessage.Content.ReadAsStringAsync().Result.ToString();
                if (result1 != "\"SUCCESS\"")
                    result = result1;
            }
            return Json(result);
        }
        public ActionResult TestAjax(string jsonWithFunPlantInfo)
        {
            return Content("TEST", "application/json");
        }

        public ActionResult SendEmail()
        {
            try
            {
                //发送电子邮件的SMTP的服务器名称

                WebMail.SmtpServer = "smtp.qq.com";
                //发送端口
                WebMail.SmtpPort = 25;
                //启用SSL（GMAIL需要），其他的都不需要
                WebMail.EnableSsl = true;
                //-----------配置 
                //账户名 
                WebMail.UserName = "1023929186";
                //邮箱名
                WebMail.From = "1023929186@qq.com";
                //密码
                WebMail.Password = "?yang1989";
                //设置默认配置
                WebMail.SmtpUseDefaultCredentials = true;

                WebMail.Send(
                to: "Sidney_Yang@jabil.com", //指定地址
                subject: "测试标题1", //标题
                body: "天天开心" //内容
                  );

                ////用 System.Web.Mail 的写法
                //MailAddress from = new MailAddress("1023929186@qq.com", "Sidney"); //填写电子邮件地址，和显示名称
                //MailAddress to = new MailAddress("Sidney_yang@jabil.com", "Sidney"); //填写邮件的收件人地址和名称
                ////设置好发送地址，和接收地址，接收地址可以是多个
                //MailMessage mail = new MailMessage();
                //mail.From = from;
                //mail.To.Add(to);
                //mail.Subject = "TEST_SendEmail";
                //mail.Body = "你好";
                //mail.IsBodyHtml = true; //设置显示htmls
                ////设置好发送邮件服务地址
                //SmtpClient client = new SmtpClient();
                //client.Host = "smtp.qq.com";
                ////填写服务器地址相关的用户名和密码信息
                //client.Credentials = new System.Net.NetworkCredential("", "123456");
                ////client.Credentials = new System.Net.NetworkCredential("1023929186@qq.com", "?yang1989");
                ////发送邮件
                //client.Send(mail);
                return Content("SUCCESS", "application/json");
            }
            catch (Exception e)
            {
                return Content("Fail", "application/json");
            }
        }

    }
}