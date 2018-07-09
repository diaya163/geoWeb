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
    public class mGeologyUnitController : BaseController
    {
        private string errMsg = string.Empty;

        private mGeologyUnitBLL objBLL = new mGeologyUnitBLL();
        private readonly int intFormId = 110; 
        public ActionResult Index()
        {
            ViewBag.Title = "地质单元维护";
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
                },
                DropInfo=new
                {
                    mGeoType = new ComboxInfo().GetDropList(intFormId, "geotype", "请选择地质类型", ref errMsg),  
                }

            };
            return View(model);
        }
        private string QryCondi(mGeologyUnit objModel)
        {
            string filters = "";

            if (!string.IsNullOrEmpty(objModel.GeoNameCHN))
            {
                filters = "GeoNameCHN like '%" + objModel.GeoNameCHN.Trim() + "%' ";
            }
            if (!string.IsNullOrEmpty(objModel.GeoType))
            {
                if (!string.IsNullOrEmpty(filters))
                {
                    filters += " and GeoType like '%" + objModel.GeoType.Trim() + "%' ";
                }
                else
                {
                    filters = " GeoType like '%" + objModel.GeoType.Trim() + "%' ";
                }

            }

            return filters;
        }
        [HttpPost]
        public JsonResult GetList(GridPager pager, mGeologyUnit objModel)
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
        public ActionResult SaveData(string action, mGeologyUnit model)
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
        public ActionResult Delete(mGeologyUnit model)
        {
            objBLL.Delete(model, ref errMsg);
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);
        }


    }
}
