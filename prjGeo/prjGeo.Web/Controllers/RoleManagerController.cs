using Permission.BLL;
using Permission.Model;
using prjGeo.BLL;
using prjGeo.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjGeo.Web.Controllers
{

    public class RoleManagerController : BaseController
    {
        private string errMsg = string.Empty;

        public ActionResult Index()
        {
            ViewBag.Title = "角色维护";
            //List<mUsersModel> list =objUser.GetAllList(),
            var model = new
            {
                form = new
                {

                },
                GridInfo = new
                {
                    idField = "Id",
                    ColInfo = new TableInfo().GetGridColInfo(117, 0),
                    sortName = "Id"
                },
                GridColInfo = new
                {
                    columns = new TableInfo().GetInitGridCols(117),
                    rows = new TableInfo().GetInitGridRows(117)
                }


            };
            return View(model);
        }

        [HttpPost]
        public JsonResult GetList(GridPager pager)
        {
            string filter = ""; //string.Format("Id='{0}'", id);
            int rCount = 0;
            var list = PermissionBLL.GetSysGroupList(filter, pager.rows, pager.page, ref rCount, ref errMsg);
            //var list = objBLL.GetIndexList(filters, ref errMsg, ref pager);

            var json = new
            {
                total = rCount,
                rows = list.ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveData(string action, Sys_Role model)
        {
            List<Sys_RoleUser> lstRoleUser = new List<Sys_RoleUser>();
            if (action.Equals("new"))
            {
                model.Id = Guid.NewGuid();
                PermissionBLL.BatchInsertGroupAndRoleList(model, lstRoleUser, ref errMsg);
            }
            else if (action.Equals("modify"))
            {
                PermissionBLL.BatchUpdateGroupAndRoleList(model, lstRoleUser, ref errMsg);
            }
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveRoleUse(List<Sys_RoleUser> lstRoleUser, string roleId)
        {

            lstRoleUser.ForEach(o => o.RoleID = new Guid(roleId));
            lstRoleUser.ForEach(o => o.UserID = o.UserCode);
            lstRoleUser.ForEach(o => o.Id = Guid.NewGuid());

            PermissionBLL.BatchInsertGroupAndRoleList(null, lstRoleUser, ref errMsg);

            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Delete(Sys_Role model)
        {
            List<string> lstGroup = new List<string>();
            lstGroup.Add(Convert.ToString(model.Id));
            PermissionBLL.BatchDeleteSysGroup(lstGroup, ref errMsg);
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUserList(GridPager pager, string Id, string keyword)
        {
            int rCount = 0;
            string fitlers = "";
            if (!string.IsNullOrEmpty(Id))
            {
                fitlers = string.Format(" UserCode not in (SELECT UserID FROM dbo.Sys_RoleUser WHERE RoleID='{0}')", Id);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                if (!string.IsNullOrEmpty(fitlers)) fitlers += " and ";
                fitlers += string.Format(" (UserCode like '%{0}%' or UserName like '%{0}%' or DeptName like '%{0}%')", keyword);
            }
            var list = PermissionBLL.GetUserListData(fitlers, pager.rows, pager.page, ref rCount);
            Dictionary<string, object> jsonObj = new Dictionary<string, object>(2);
            jsonObj.Add("total", rCount);
            jsonObj.Add("rows", list);
            return Json(jsonObj, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetUserRoleList(GridPager pager, string Id, string keyword)
        {
            int rCount = 0;
            string fitlers = "";
            if (!string.IsNullOrEmpty(Id))
            {
                fitlers = string.Format(" UserCode in (SELECT UserID FROM dbo.Sys_RoleUser WHERE RoleID='{0}')", Id);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                if (!string.IsNullOrEmpty(fitlers)) fitlers += " and ";
                fitlers += string.Format(" (UserCode like '%{0}%' or UserName like '%{0}%' or DeptName like '%{0}%')", keyword);
            }

            var list = PermissionBLL.GetUserListData(fitlers, pager.rows, pager.page, ref rCount);
            Dictionary<string, object> jsonObj = new Dictionary<string, object>(2);
            jsonObj.Add("total", rCount);
            jsonObj.Add("rows", list);
            return Json(jsonObj, JsonRequestBehavior.AllowGet);

        }
        public ActionResult DeleteUserRoleList(List<Sys_RoleUser> lstRoleUser)
        {

            PermissionBLL.BatchUpdateGroupAndRoleList(null, lstRoleUser, ref errMsg);

            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);


        }

    }

}
