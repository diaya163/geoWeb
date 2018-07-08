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
    public class mProjectController : BaseController
    {

        private string errMsg = string.Empty;
        private mProjectBLL objBLL = new mProjectBLL();

        public ActionResult Index()
        {
            ViewBag.Title = "项目维护";
            //List<mUsersModel> list =objUser.GetAllList(),
            var model = new
            {
                form = new
                {

                },
                GridInfo = new
                {
                    idField = "id",
                    ColInfo = new TableInfo().GetGridColInfo(114, 0),
                    sortName = "id"
                },
                GridColInfo = new
                {
                    columns = new TableInfo().GetInitGridCols(114),
                    rows = new TableInfo().GetInitGridRows(114)
                }


            };
            return View(model);
        }

        [HttpPost]
        public JsonResult GetList(GridPager pager)
        {
            string filters = string.Empty;
            var list = objBLL.GetIndexList(filters, ref errMsg, ref pager);
            var json = new
            {
                total = pager.totalRows,
                rows = list.ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListByFilter(string PrjName, string PrjId, GridPager pager)
        {
            string filters = "";
            if (!string.IsNullOrEmpty(PrjName))
            {
                filters = "a.CName like '%" + PrjName + "%' ";
            }
            if (!string.IsNullOrEmpty(PrjId))
            {
                if (!string.IsNullOrEmpty(filters))
                {
                    filters += " and a.Ccode like '%" + PrjId + "%' ";
                }
                else
                {
                    filters = " a.Ccode like '%" + PrjId + "%' ";
                }

            }
            var list = objBLL.GetIndexList(filters, ref errMsg, ref pager);
            var json = new
            {
                total = pager.totalRows,
                rows = list.ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveData(string action, mProject model)
        {
            if (action.Equals("new"))
            {
                objBLL.Add(model, ref errMsg);
            }
            else if (action.Equals("modify"))
            {
                objBLL.Update(model, ref errMsg);
            }
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(mProject model)
        {
            objBLL.Delete(model, ref errMsg);
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListData()
        {
            var list = objBLL.GetList(string.Empty, ref errMsg);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
