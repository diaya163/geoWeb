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
using prjGeo.Models.Buss;

namespace prjGeo.Web.Controllers
{
    public class HSAllSeqController : Controller
    {
        //
        // GET: /HSAllSeq/
        private string errMsg = string.Empty;

        private HSAllSeqBLL objBLL = new HSAllSeqBLL();
        private readonly int intFormId = 130;
        public ActionResult Index()
        {
            ViewBag.Title = "综合异常评序表";
     
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
                    mGecata = new ComboxInfo().GetDropList(intFormId, "gecata","请选择地质分类", ref errMsg),
                    mValueCata = new ComboxInfo().GetDropList(intFormId, "valuecata", "请选择价值分类", ref errMsg)
                }
            };
            return View(model);
        }
        [HttpPost]
        public JsonResult GetList(GridPager pager, HSAllSeq objModel)
        {
            //string filters = new FiterCond().GetFiterCond(1, objModel, ref errMsg);
            errMsg = string.Empty;
            var list = objBLL.GetIndexList(objModel, ref errMsg, ref pager);
            var json = new
            {
                total = pager.totalRows,
                rows = list.ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

 

    }
}
