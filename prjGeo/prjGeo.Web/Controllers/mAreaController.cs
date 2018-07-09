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
    public class mAreaController : BaseController
    {
        private string errMsg = string.Empty;

        private mAreaBLL objBLL = new mAreaBLL();

        public ActionResult Index()
        {
            ViewBag.Title = "中国分部维护";
            //List<mUsersModel> list =objUser.GetAllList(),
            var model = new
            {
                form = new
                {

                },
                GridInfo = new
                {
                    idField = "id",
                    ColInfo = new TableInfo().GetGridColInfo(112, 0),
                    sortName = "id"
                },
                GridColInfo = new
                {
                    columns = new TableInfo().GetInitGridCols(112),
                    rows = new TableInfo().GetInitGridRows(112)
                }
            };
            return View(model);
        }

        private string QryCondi(mArea objModel)
        {
            string filters = string.Empty;
            if (objModel != null)
            {
                if (objModel.Ccode != null || (objModel.Ccode + "").Trim() != "")
                {
                    filters =string.Format("   Ccode like '%{0}%'",objModel.Ccode.Trim());
                }
                if (objModel.Memo != null || (objModel.Memo + "").Trim() != "")
                {
                    if (filters.Length > 0)
                    {
                        filters =filters + string.Format("   or  Memo like '%{0}%'", objModel.Memo.Trim());
                    }
                    else
                    {
                        filters = string.Format("     Memo like '%{0}%'", objModel.Memo.Trim());
                    }
                }

            }

            return filters;
        }

        [HttpPost]
        public JsonResult GetList(GridPager pager, mArea objModel)
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

        public ActionResult SaveData(string action, mArea model)
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
        public ActionResult Delete(mArea model)
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
