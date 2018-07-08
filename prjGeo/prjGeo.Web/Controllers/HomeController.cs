using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using prjGeo.Commons;
using prjGeo.BLL;
using prjGeo.Models;
using prjGeo.Models.Sys;
using Permission.BLL;

namespace prjGeo.Web.Controllers
{
    public class HomeController : Controller
    {
        private string errMsg = string.Empty;
        //
        // GET: /Home/
        //mMenuBLL objMenu = new mMenuBLL();
        public ActionResult Index()
        {
            var loginer = FormsAuth.GetUserData<LoginerBase>();
            ViewBag.UserName = loginer.UserName;
            ViewBag.UserCode = loginer.UserCode;
            ViewBag.Title = "地质信息管理平台";
            return View();
        }
        public JsonResult GetTreeByEasyui(string id)
        {
            //加入本地化
            //CultureInfo info = Thread.CurrentThread.CurrentCulture;
            //string infoName = info.Name;
            try
            {
                //  List<SysModuleNavModel> list = objMenu.GetMenuByPersonId("", id);
                var list = PermissionBLL.GetSysMenuList(string.Format("IsUse = 1 and MenuParentCode={0}", id), ref errMsg);
                if (!string.IsNullOrEmpty(errMsg))
                {
                    return Json("0");
                }

                var json = (from r in list
                            select new SysModuleNavModel()
                            {
                                id = r.MenuCode.Trim(),
                                text = r.MenuName,     //text
                                //     attributes = r.attributes,
                                iconCls = r.ClassName,
                                state = "closed",// (objMenu.GetList("", r.Id).Count > 0) ? "closed" : "open",
                                isLeft = true,//r.isLeft
                            }).ToArray();
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json("0");
            }
        }

        public JsonResult GetTreeMenuByEasyui(string id)
        {
            //加入本地化
            //CultureInfo info = Thread.CurrentThread.CurrentCulture;
            //string infoName = info.Name;
            try
            {

                var list = PermissionBLL.GetSysMenuList(string.Format("IsUse = 1 and MenuParentCode='{0}'", id), -1, -1, ref errMsg);

                var json = (from r in list
                            select new SysModuleNavModel()
                            {
                                id = r.MenuCode.ToString().Trim(),
                                text = r.MenuName,     //text
                                attributes = r.AssemblyUrl,
                                iconCls = r.MenuName_En,
                                state = (PermissionBLL.GetSysMenuCount(string.Format(" MenuCode<>'{0}' and MenuParentCode = '{0}' ", r.MenuCode), ref errMsg)) ? "closed" : "open",
                                isLeft = true,//r.isLeft
                            }).ToArray();
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json("0");
            }
        }


        public ActionResult GetRight(string menuCode)
        {

            var loginer = FormsAuth.GetUserData<LoginerBase>();
            var rightVal = UserRight.GetModuleRight("GeoGis", loginer.UserCode, menuCode);
            var arr = rightVal.ToArray();
            Array.Reverse(arr);

            return Json(arr, JsonRequestBehavior.AllowGet);


        }

    }
}
