using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Data;
using Permission.Model;
using Esquel.BaseManager;
using System.Web;
using System.Web.Security;

namespace Permission.BLL {
    #region  [放系统的共公变量，或系统的共公参数]
    /// <summary>
    /// 放系统的共公变量，或系统的共公参数
    /// </summary>
    public partial class Setting {

        #region [系统登录公共变量]
        /// <summary>
        /// 当时运行的设备状态
        /// </summary>
        public static List<int> lstRunMachine = new List<int>();

        /// <summary>
        /// PLC类型
        /// </summary>
        public static string plcType = string.Empty;
        /// <summary>
        /// PLC单个读指令
        /// </summary>
        public static string plcReaderCmd = string.Empty;
        /// <summary>
        /// PLC多个读指令
        /// </summary>
        public static string plcBatchReaderCmd = string.Empty;
        /// <summary>
        /// PLC单个写指令
        /// </summary>
        public static string plcWriterCmd = string.Empty;

        public static string DeptCode = string.Empty;
        public static string DeptName = string.Empty;

        public static string LastMenuCode = string.Empty;

        public static string RuningMachineNo = "01";

        public static bool IsPLCRun = false;
        /// <summary>
        /// 是否在采集PLC数据中--整线
        /// </summary>
        public static bool IsPLCRunAll = false;
        /// <summary>
        /// 冲片机
        /// </summary>
        public static bool IsPLCRunPunching = false;
        /// <summary>
        /// 叠片机
        /// </summary>
        public static bool IsPLCRunLamination = false;
        /// <summary>
        /// 焊接机
        /// </summary>
        public static bool IsPLCRunWeld = false;
        /// <summary>
        /// 包装机
        /// </summary>
        public static bool IsPLCRunPacking = false;
        /// <summary>
        /// 注液机
        /// </summary>
        public static bool IsPLCRunInjection = false;

        /// <summary>
        /// 二封Degas
        /// </summary>
        public static bool IsPLCRunDegas = false;
        /// <summary>
        /// 是否重注册
        /// </summary>
        public static bool IsRegLogin = false;

        public static string UploadPath = string.Empty;
        /// <summary>
        /// 是否为系统管理员
        /// </summary>
        public static bool IsAdmin = false;

        public static bool IsWriterSql = false;
        /// <summary>
        /// 登录ID全球唯一标识
        /// </summary>
        public static string TicketId = string.Empty;
        /// <summary>
        /// 登录ID全球唯一标识
        /// </summary>
        public static string Id = string.Empty;
        /// <summary>
        /// 登录用户ID
        /// </summary>
        public static string LoginID = string.Empty;
        /// <summary>
        /// 登录用户名
        /// </summary>
        public static string LoginName = string.Empty;

        /// <summary>
        /// 工厂名称
        /// </summary>
        public static string Factory = string.Empty;
        public static string FactoryName = string.Empty;
        /// <summary>
        /// 操作日期
        /// </summary>
        public static string LoginDate = string.Empty;
        /// <summary>
        /// 本地机器名
        /// </summary>
        public static string MachineName = Environment.MachineName;
        /// <summary>
        /// 是否登录
        /// </summary>
        public static bool IsLogin = false;
        /// <summary>
        /// 系统登录日期
        /// </summary>
        public static DateTime LoginTime = DateTime.MinValue;
        /// <summary>
        /// 设置菜单节点的属性名称
        /// </summary>
        public static string PropertyMenuName = "MenuCode";
        /// <summary>
        /// 语言
        /// </summary>
        public static Enums.LanguageType LanguageType = Enums.LanguageType.ZH_CN;

        /// <summary>
        /// 员工ID
        /// </summary>
        public static string UserID { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public static DateTime UserDate { get; set; }
        /// <summary>
        /// 换行符号
        /// </summary>
        public const string NewlineSymbol = "\r\n";

        #endregion

        #region [正则表达式]
        /// <summary>
        /// 电子邮箱正则
        /// </summary>
        public const string RegexEmail = @"^([a-z0-9A-Z]+[-|\.]?)+[a-z0-9A-Z]@([a-z0-9A-Z]+(-[a-z0-9A-Z]+)?\.)+[a-zA-Z]{2,}$";
        /// <summary>
        /// 固定电话正则
        /// </summary>
        public const string RegexPhone = @"^(0[0-9]{2,3}\-)?\d{6,8}$";
        /// <summary>
        /// 手机号码正则
        /// </summary>
        public const string RegexCellphone = @"^(13|15|18)\d{9}$";
        /// <summary>
        /// 身份证正则
        /// </summary>
        public const string RegexIdNumber = @"^(\d{15}|^\d{18}|^\d{17}(\d|X|x))$";
        /// <summary>
        /// 字母正则
        /// </summary>
        public const string RegexLetter = @"^[A-Za-z]$";
        /// <summary>
        /// 字母数字正则
        /// </summary>
        public const string RegexLetterNumber = @"^[A-Za-z0-9]$";
        #endregion

    }


    public class ELoginInfo {



        /// <summary>
        /// 是否为系统管理员
        /// </summary>
        public static bool IsAdmin {
            get {
                try {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    var login = FilesManager.DeSerialize(ticket.UserData) as LoginInfo;
                    return login.IsAdmin;

                } catch {

                }
                return false;
            }

        }
        /// <summary>
        /// 登录ID全球唯一标识
        /// </summary>
        public static string TicketId {
            get {
                #region
                try {

                    HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                    //   string ticketString = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies["loginInfo"].Value);
                    var login = FilesManager.DeSerialize(ticket.UserData) as LoginInfo;

                    return login.TicketId;

                } catch {

                }
                return string.Empty;

                #endregion
            }
        }
        /// <summary>
        /// 登录ID全球唯一标识
        /// </summary>
        public static string Id {
            get {
                #region
                try {
                    //string ticketString = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies["loginInfo"].Value);
                    //var login = FilesManager.DeSerialize(ticketString) as LoginInfo;
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    var login = FilesManager.DeSerialize(ticket.UserData) as LoginInfo;
                    return login.Id;

                } catch {

                }
                return string.Empty;

                #endregion
            }
        }
        /// <summary>
        /// 登录用户ID
        /// </summary>
        public static string LoginID {
            get {
                #region
                try {
                    //string ticketString = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies["loginInfo"].Value);
                    //var login = FilesManager.DeSerialize(ticketString) as LoginInfo;
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    var login = FilesManager.DeSerialize(ticket.UserData) as LoginInfo;
                    return login.LoginID;

                } catch {

                }
                return string.Empty;

                #endregion
            }
        }
        /// <summary>
        /// 登录用户名
        /// </summary>
        public static string LoginName {
            get {
                #region
                try {
                    //string ticketString = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies["loginInfo"].Value);
                    //var login = FilesManager.DeSerialize(ticketString) as LoginInfo;
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    var login = FilesManager.DeSerialize(ticket.UserData) as LoginInfo;
                    return login.LoginName;

                } catch {

                }
                return string.Empty;

                #endregion
            }
        }

        /// <summary>
        /// 工厂名称
        /// </summary>
        public static string Factory {
            get {
                #region
                try {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    var login = FilesManager.DeSerialize(ticket.UserData) as LoginInfo;

                    return login.Factory;

                } catch {

                }
                return string.Empty;

                #endregion
            }
        }
        public static string FactoryName {
            get {
                #region
                try {
                    //     string ticketString = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies["loginInfo"].Value);
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    var login = FilesManager.DeSerialize(ticket.UserData) as LoginInfo;
                    return login.FactoryName;

                } catch {

                }
                return string.Empty;

                #endregion
            }
        }

        /// <summary>
        /// 本地机器名
        /// </summary>
        public static string MachineName {
            get {
                #region
                try {
                    //string ticketString = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies["loginInfo"].Value);
                    //var login = FilesManager.DeSerialize(ticketString) as LoginInfo;
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    var login = FilesManager.DeSerialize(ticket.UserData) as LoginInfo;
                    return login.MachineName;

                } catch {

                }
                return string.Empty;

                #endregion
            }
        }
        public static string IpAddress {
            get {
                #region
                try {
                    //string ticketString = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies["loginInfo"].Value);
                    //var login = FilesManager.DeSerialize(ticketString) as LoginInfo;
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    var login = FilesManager.DeSerialize(ticket.UserData) as LoginInfo;
                    return login.IpAddress;

                } catch {

                }
                return string.Empty;

                #endregion
            }
        }
        /// <summary>
        /// 系统登录日期
        /// </summary>
        public static DateTime LoginTime {
            get {
                #region
                try {
                    //string ticketString = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies["loginInfo"].Value);
                    //var login = FilesManager.DeSerialize(ticketString) as LoginInfo;
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    var login = FilesManager.DeSerialize(ticket.UserData) as LoginInfo;

                    return login.LoginTime;

                } catch {

                }
                return DateTime.MinValue;

                #endregion
            }
        }

        public static string DeptCode {
            get {
                try {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    var login = FilesManager.DeSerialize(ticket.UserData) as LoginInfo;

                    return login.DeptCode;

                } catch {

                }
                return string.Empty;

            }

        }

    }

    #endregion
}
