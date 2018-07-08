using Esquel.BaseManager;
using Permission.BLL;
using Permission.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace prjGeo.Web.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            Esquel.WebBlock.LixWebUiUtil.ClearRuntimeCash();
            //HttpCookie aCookie = Request.Cookies["srvUser"];
            //if (aCookie != null) {
            //    aCookie.Expires = DateTime.Now.AddDays(-1);
            //    Response.Cookies.Add(aCookie);
            //}
            return View();
        }
        public ActionResult GetFactory()
        {
            var data = PermissionBLL.GetFactory("");

            return Json(data, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Login(string userAccount, string password, bool remember, string factory, string returnUrl)
        {
            userAccount = userAccount.Trim();
            string msg = string.Empty;
            //if (string.IsNullOrEmpty(factory))
            factory = ConfigurationManager.AppSettings["Factory"];
            string IP = Esquel.BaseManager.OSClass.GetLocalIp();
            if (password != "")
            {
                AppSettingsReader asrDB = new AppSettingsReader();
                string encry = Convert.ToString(asrDB.GetValue("Encry", typeof(string)));//BLL.ModuleBLL.GetUpateURL("Factory"); 
                if (encry.Equals("1"))
                {
                    password = EnDeCrypt.EncryPass(password);
                }
                else if (encry.Equals("2"))
                {
                    password = DESEncrypt.Encrypt(password);
                }
                else if (encry.Equals("3"))
                {
                    password = DESEncrypt.EncryptPassWord(password);
                }
            }

            string Pwd = PermissionBLL.GetLoginPwd(userAccount, factory, ref msg);
            if (!string.IsNullOrEmpty(msg))
            {
                return Json(new { Message = "未找到用户信息", Url = returnUrl });

            }

            if (!password.Equals(Pwd))
            {
                return Json(new { Message = "用户名或密码不正确!", Url = returnUrl });
            }

            Sys_Ticket ticket = PermissionBLL.Login(userAccount, IP.ToString(), factory, 0, ref msg);
            if (!string.IsNullOrEmpty(msg))
            {
                return Json(new { Message = msg, Url = returnUrl });

            }
            if (ticket == null)
            {
                return Json(new { Message = "未找到用户信息", Url = returnUrl });
            }

            LoginInfo userModel = new LoginInfo();

            userModel.TicketId = ticket.Id.ToString();
            userModel.Id = ticket.AccountId.ToString();
            userModel.LoginID = ticket.LoginId;
            userModel.LoginName = ticket.LoginName;
            userModel.Factory = ticket.Factory;
            //userModel.DeptCode = ticket.DeptCode;
            userModel.IsAdmin = ticket.IsAdmin;
            userModel.LoginTime = ticket.LoginTime;
            userModel.FactoryName = factory;
            userModel.IpAddress = IP;
            //HttpCookie hc = new HttpCookie("loginInfo");
            //hc.Expires = DateTime.Now.AddDays(1);
            //hc.Value = FilesManager.BinarySerialize(userModel);
            //Response.Cookies.Add(hc);

            DateTime dt = DateTime.Now;
            FormsAuthenticationTicket formTicket = new FormsAuthenticationTicket(
               2, userAccount, dt, dt.Add(FormsAuthentication.Timeout), false, FilesManager.BinarySerialize(userModel));
            HttpCookie cookie = new HttpCookie(
                FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(formTicket));
            cookie.Domain = FormsAuthentication.CookieDomain;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            Response.Cookies.Add(cookie);
            Session.Timeout = (int)FormsAuthentication.Timeout.TotalMinutes;
            var cookieT = Response.Cookies["srvUser"];

            if (Convert.ToBoolean(remember))
            {
                var srvUser = FilesManager.BinarySerialize(userAccount + "|" + remember + "|" + password);
                //记住用户名
                if (cookieT != null)
                {
                    cookieT.Value = srvUser;
                }
                else
                {
                    cookieT = new HttpCookie("srvUser", srvUser);
                }
                cookieT.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Add(cookieT);
            }
            else
            {
                var srvUser = FilesManager.BinarySerialize(userAccount + "|" + remember + "|");
                //记住用户名
                if (cookieT != null)
                {
                    cookieT.Value = srvUser;
                }
                else
                {
                    cookieT = new HttpCookie("srvUser", srvUser);
                }
                cookieT.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Add(cookieT);
            }
            return Json(new { Message = msg, Url = "/" });
        }
        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCookie()
        {
            var cookie = Request.Cookies["srvUser"];
            string userName = string.Empty, userPwd = string.Empty;
            bool remember = false;
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                var srvUser = FilesManager.DeSerialize(cookie.Value);
                var arr = Convert.ToString(srvUser).Split('|');
                userName = arr.Length > 0 ? arr[0] : "";
                remember = arr.Length > 1 ? Convert.ToBoolean(arr[1]) : false;
                userPwd = arr.Length > 2 ? arr[2] : string.Empty;

                AppSettingsReader asrDB = new AppSettingsReader();
                string encry = Convert.ToString(asrDB.GetValue("Encry", typeof(string)));//BLL.ModuleBLL.GetUpateURL("Factory"); 
                if (encry.Equals("1"))
                {
                    userPwd = EnDeCrypt.DecryPass(userPwd);
                }
                else if (encry.Equals("2"))
                {
                    userPwd = DESEncrypt.Decrypt(userPwd);
                }

            }
            return Json(new { userName, remember, userPwd }, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 注销。
        /// </summary>
        /// <returns></returns>
        public ActionResult SignOut()
        {
            string errMsg = string.Empty;
            if (!string.IsNullOrEmpty(ELoginInfo.TicketId))
            {
                PermissionBLL.Logout(new Guid(ELoginInfo.TicketId), ref errMsg);
                Esquel.WebBlock.LixWebUiUtil.ClearRuntimeCash();
                FormsAuthentication.SignOut();
            }
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SignUnLogin()
        {
            string errMsg = string.Empty;
            if (!string.IsNullOrEmpty(ELoginInfo.TicketId))
            {
                PermissionBLL.Logout(new Guid(ELoginInfo.TicketId), ref errMsg);
                var cookieT = Request.Cookies["srvUser"];
                var srvUser = FilesManager.BinarySerialize(ELoginInfo.LoginID + "|" + false + "|");
                if (cookieT != null)
                {
                    cookieT.Value = srvUser;
                }
                else
                {
                    cookieT = new HttpCookie("srvUser", srvUser);
                }
                cookieT.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Add(cookieT);

                Esquel.WebBlock.LixWebUiUtil.ClearRuntimeCash();

                FormsAuthentication.SignOut();

            }
            return Json(new { errMsg, Url = "/Account" });

            //  return RedirectToAction("Index", "Account");

            // return Json(new { errMsg }, JsonRequestBehavior.AllowGet);

        }

    }
    public class EnDeCrypt
    {
        public static string EncryPass(string strPassWord)
        {
            string text = "";
            strPassWord = strPassWord.Trim();
            byte b = 0;
            while ((int)b < strPassWord.Length)
            {
                if (b % 2 != 1)
                {
                    string text2 = ((int)(strPassWord[(int)b] + '\v')).ToString();
                    while (text2.Length < 3)
                    {
                        text2 = '0' + text2;
                    }
                    text += text2;
                }
                else
                {
                    string text2 = ((int)(strPassWord[(int)b] + '\n')).ToString();
                    while (text2.Length < 3)
                    {
                        text2 = '0' + text2;
                    }
                    text += text2;
                }
                b += 1;
            }
            return text;
        }

        public static string DecryPass(string strPassWord)
        {
            string text = "";
            strPassWord = strPassWord.Trim();
            byte b = 1;
            byte b2 = 0;
            while ((int)b < strPassWord.Length)
            {
                b2 = Convert.ToByte((int)(b2 + 1));
                string value = strPassWord.Substring((int)(b - 1), 3);
                byte b3 = Convert.ToByte(value);
                if (b2 % 2 == 1)
                {
                    b3 = Convert.ToByte((int)(b3 - 11));
                }
                else
                {
                    b3 = Convert.ToByte((int)(b3 - 10));
                }
                text += (char)b3;
                b = Convert.ToByte((int)(b + 3));
            }
            return text;
        }
    }
}
