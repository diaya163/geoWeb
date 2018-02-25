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

namespace prjGeo.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        mMenuBLL objMenu = new mMenuBLL();
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
                List<SysModuleNavModel> list = objMenu.GetMenuByPersonId("", id);
                
                var json = (from r in list
                           select new SysModuleNavModel()
                           {
                               id = r.id.ToString().Trim(),
                               text = r.text,     //text
                               attributes = r.attributes,
                               iconCls = r.iconCls,
                               state = (objMenu.GetList("",r.id).Count > 0) ? "closed" : "open" , 
                               isLeft=r.isLeft
                           }).ToArray();
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json("0");
            }
        }

    }
}
