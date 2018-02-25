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
    public class UserController : BaseController
    {
        //
        // GET: /User/
        mUsersBLL objUser = new mUsersBLL();
        
        public ActionResult Index()
        {            
            ViewBag.Title = "用户管理";
            //List<mUsersModel> list =objUser.GetAllList(),
            var model = new
            {
                form = new
                {

                },
                GridInfo = new
                {
                    idField="id",
                    ColInfo = new TableInfo().GetGridColInfo(100, 0),                     
                    sortName="seqno"
                },
                GridColInfo=new 
                {
                    columns = new TableInfo().GetInitGridCols(100),
                    rows = new TableInfo().GetInitGridRows(100)
                }


            };
            return View(model);
        }

        [HttpPost]
        public JsonResult GetList(GridPager pager)
        {
            List<mUsersModel> list = objUser.GetIndexList(ref pager);
            var json = new
            {
                total = pager.totalRows,
                rows=list.ToArray()
            };

            return Json(json,JsonRequestBehavior.AllowGet);
        }

 



    }
}
