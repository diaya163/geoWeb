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
    public class mZoneController : BaseController
    {
        private string errMsg = string.Empty;

        private mZoneBLL objBLL = new mZoneBLL();
        private readonly int intFormId = 115;
        public ActionResult Index()
        {
            ViewBag.Title = "全区基础数据维护";
            //List<mUsersModel> list =objUser.GetAllList(),
            var model = new
            {
                form = new
                {

                },
                GridInfo = new
                {
                    idField = "id",
                    ColInfo = new TableInfo().GetGridColInfo(intFormId, 0),
                    sortName = "id"
                },
                GridColInfo = new
                {
                    columns = new TableInfo().GetInitGridCols(intFormId),
                    rows = new TableInfo().GetInitGridRows(intFormId)
                }
            };
            return View(model);
        }
        private string QryCondi(mZone objModel)
        {
            string filters = "";
            if (!string.IsNullOrEmpty(objModel.SampleNumber))
            {
                filters = "SampleNumber like '%" + objModel.SampleNumber.Trim() + "%' ";
            }
 

            return filters;
        }
        [HttpPost]
        public JsonResult GetList(GridPager pager, string filter, mZone objModel)
        {
            //Request
            string filters = string.Empty;
            filters=QryCondi(objModel);
            var list = objBLL.GetIndexList(filters, ref errMsg, ref pager);
            var json = new
            {
                total = pager.totalRows,
                rows = list.ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveData(string action, mZone model)
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
        public ActionResult Delete(mZone model)
        {
            objBLL.Delete(model, ref errMsg);
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);
        }

    }
}
