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
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace prjGeo.Web.Controllers
{
    public class mMapController : BaseController
    {
        private string errMsg = string.Empty;

        private mKmlBLL objBLL = new mKmlBLL();
        public ActionResult Index()
        {
            ViewBag.Title = "分部维护";
            //List<mUsersModel> list =objUser.GetAllList(),
            var model = new
            {
                form = new
                {

                },
                GridInfo = new
                {
                    idField = "id",
                    ColInfo = new TableInfo().GetGridColInfo(120, 0),
                    sortName = "id"
                },
                GridColInfo = new
                {
                    columns = new TableInfo().GetInitGridCols(120),
                    rows = new TableInfo().GetInitGridRows(120)
                }


            };
            return View(model);
        }

        [HttpPost]
        public JsonResult GetListByFilter(string PrjName)
        {
            string filter = string.Empty;
            if (!string.IsNullOrEmpty(PrjName))
            {
                filter = "PrjName='" + PrjName + "'";
            }
            var list = objBLL.GetList(filter, ref errMsg);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetListData()
        {
            var list = objBLL.GetList(string.Empty, ref errMsg);
            return Json(list, JsonRequestBehavior.AllowGet);
        } 
    }
}
