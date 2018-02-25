using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using prjGeo.Commons;
using prjGeo.Models;
using prjGeo.BLL;
using prjGeo.Models.Sys;

namespace prjGeo.Web.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Login()
        {
            ViewBag.CnName = "地质信息管理平台";
            ViewBag.EnName = "Geology Information Management Platform ";
            ViewBag.EnNameStyle = "left:298px;";
            return View();
        }

        public JsonResult DoAction(JObject request)
        {
            var message = new mUsersBLL().Login(request);
            return Json(message, JsonRequestBehavior.DenyGet);
        }

        public ActionResult Logout()
        {
            FormsAuth.SingOut();
            return Redirect("~/Login");
        }

    }
}
