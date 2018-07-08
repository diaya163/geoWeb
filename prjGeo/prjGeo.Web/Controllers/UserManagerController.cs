using Permission.BLL;
using Permission.Model;
using prjGeo.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjGeo.Web.Controllers
{
    public class UserManagerController : BaseController
    {
        private string errMsg = string.Empty;
        //
        // GET: /UserManager/



        public ActionResult Index()
        {
            ViewBag.Title = "用户管理与维护";
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

        [HttpPost]
        public JsonResult GetList(GridPager pager)
        {
            string filters = string.Empty;
            int rCount = 0;
            var list = PermissionBLL.GetSys_UserList(filters, pager.page - 1, pager.rows, ref rCount, ref errMsg);
            var json = new
            {
                total = rCount,
                rows = list.ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetUserData(string loginId)
        {
            string filter = string.Format("Id='{0}'", loginId);
            var list = PermissionBLL.GetSys_UserList(filter, ref errMsg);
            return Json(list[0], JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteUser(string[] loginId)
        {

            PermissionBLL.DeleteSys_UserByfilters(loginId.ToList(), ref errMsg);

            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult SaveUserData(string action, Sys_User model, List<Sys_Role> listRoleModel)
        {
            List<Sys_RoleUser> lstRoleUser = new List<Sys_RoleUser>();
            model.Id = action == "new" ? Guid.NewGuid() : model.Id;
            model.Factory = ELoginInfo.Factory;
            if (listRoleModel != null)
            {
                foreach (Sys_Role role in listRoleModel)
                {
                    Sys_RoleUser m = new Sys_RoleUser();
                    m.RoleID = role.Id;
                    //  m.UserID = model.;
                    lstRoleUser.Add(m);
                }
            }
            if (action == "new")
            {
                PermissionBLL.BatchInsertUserAndRoleList(model, lstRoleUser, ref errMsg);
            }
            else
            {
                PermissionBLL.BatchUpdateUserAndRoleList(model, lstRoleUser, ref errMsg);
            }
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUserGroup(string groupId, string keyword, string selUser)
        {
            string filter = string.Format("IsUseStop = 0 and Factory='{0}'", ELoginInfo.Factory);

            if (!string.IsNullOrEmpty(keyword))
            {
                filter += string.Format(" and (LoginID like '%{0}%' or LoginName like '%{0}%' or Email like '%{0}%' or Cellphone like '%{0}%' Or DeptName like '%{0}%' or DeptCode like '%{0}%')", keyword);
            }

            if (!string.IsNullOrEmpty(selUser))
                filter += string.Format(" OR LoginID in ('{0}') ", selUser.Replace(",", "','"));

            var lstUser = PermissionBLL.GetSys_UserList(filter, ref errMsg);

            if (groupId != null)
            {
                var lstRole = PermissionBLL.GetSysRoleList(string.Format("RoleId='{0}'", groupId), ref errMsg);

                foreach (Sys_User user in lstUser)
                {
                    user.UserChecked = lstRole.Exists(o => o.UserID.Equals(user.Id));
                }
            }

            lstUser = lstUser.OrderByDescending(o => o.UserChecked).ToList();

            Dictionary<string, object> jsonObj = new Dictionary<string, object>(2);
            jsonObj.Add("total", 0);
            jsonObj.Add("data", lstUser);
            return Json(jsonObj, JsonRequestBehavior.AllowGet);

        }
    }
}
