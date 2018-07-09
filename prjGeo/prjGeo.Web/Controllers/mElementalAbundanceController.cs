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
    public class mElementalAbundanceController : BaseController
    {
        private string errMsg = string.Empty;

        private mElementalAbundanceBLL objBLL = new mElementalAbundanceBLL();

        public ActionResult Index()
        {
            ViewBag.Title = "元素丰度管理";
            //List<mUsersModel> list =objUser.GetAllList(),
            var model = new
            {
                form = new
                {

                },
                GridInfo = new
                {
                    idField = "id",
                    ColInfo = new TableInfo().GetGridColInfo(113, 0),
                    sortName = "id"
                },
                GridColInfo = new
                {
                    columns = new TableInfo().GetInitGridCols(113),
                    rows = new TableInfo().GetInitGridRows(113)
                }


            };
            return View(model);
        }

        private string QryCondi(mElementalAbundance objModel)
        {
            string filters = string.Empty;
            if (objModel != null)
            {
                if (objModel.Element != null || (objModel.Element + "").Trim() != "")
                {
                    filters = string.Format("   Element like '%{0}%'", objModel.Element.Trim());
                }
            }
            return filters;
        }

        [HttpPost]
        public JsonResult GetList(GridPager pager, mElementalAbundance objModel)
        {
            string filters = string.Empty;
            filters = QryCondi(objModel);
            var list = objBLL.GetIndexList(filters, ref errMsg, ref pager);
            var json = new
            {
                total = pager.totalRows,
                rows = list.ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveData(string action, mElementalAbundance model)
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
        public ActionResult Delete(mElementalAbundance model)
        {
            objBLL.Delete(model, ref errMsg);
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);
        }



    }
}
