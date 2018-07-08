using Permission.BLL;
using Permission.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjGeo.Web.Controllers
{
    public class RightManagerController : BaseController
    {
        //
        // GET: /RightManager/
        private string errMsg = string.Empty;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetGroup(int pageIndex = 0, int pageSize = 50)
        {
            int rCount = 0;

            var lstRole = PermissionBLL.GetSysGroupList(string.Format("IsUse = 1 and Factory='{0}'", ELoginInfo.Factory), pageSize, pageIndex, ref rCount, ref errMsg);

            Dictionary<string, object> jsonObj = new Dictionary<string, object>(2);
            jsonObj.Add("total", rCount);
            jsonObj.Add("data", lstRole);
            return Json(jsonObj, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetMenuByRole(string Id)
        {

            var listMenu = PermissionBLL.GetSysRoleRightList(string.Format(" RoleID='{0}'", Id), ref errMsg);

            return Json(listMenu, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetRightMenu()
        {

            listMenu = PermissionBLL.GetSysMenuList("", ref errMsg);

            var list = new List<Category>();

            CategoryJson(list, "0");
            //  CacheHelper<List<Category>>.SetCache(CategoryAllListCacheKEY, list);


            return Json(list, JsonRequestBehavior.AllowGet);

        }
        public class Category
        {
            public string id { get; set; }
            public string text { get; set; }
            public string state { get; set; }
            public List<Category> children { get; set; }
        }


        /// <summary>
        /// 取得兄弟节点
        /// </summary>
        /// <param name="categoryList"></param>
        /// <param name="parentCode"></param>
        public void CategoryJson(List<Category> categoryList, string parentCode)
        {
            var list = listMenu.FindAll(p => p.MenuParentCode == parentCode);
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    var tree = new Category();
                    tree.id = item.Id.ToString();
                    tree.text = item.MenuName;

                    CategoryTreeJson(tree, item.MenuCode);
                    categoryList.Add(tree);
                }
            }
        }
        private List<Sys_Menu> listMenu { get; set; }

        /// <summary>
        /// 递归出子对象
        /// </summary>
        /// <param name="sbCategory"></param>
        /// <param name="parentCode"></param>
        private void CategoryTreeJson(Category sbCategory, string parentCode)
        {
            var list = listMenu.FindAll(p => p.MenuParentCode == parentCode);
            if (list.Count > 0)
            {
                sbCategory.children = new List<Category>();
                foreach (var item in list)
                {
                    var tree = new Category();
                    tree.id = item.Id.ToString();
                    tree.text = item.MenuName;

                    CategoryTreeJson(tree, item.MenuCode);
                    sbCategory.children.Add(tree);
                }
            }
        }


        public ActionResult GetMenu(string id)
        {

            var listMenu = PermissionBLL.GetSysMenuList(string.Format(" IsUse = 1 and FunID in (select ID from Sys_FunModel where Factory='{0}')", ELoginInfo.Factory), ref errMsg);
            listMenu.ForEach(o => o.BitRightVal = Convert.ToString(o.RightValue, 2));
            var lstRole = PermissionBLL.GetSysRoleRightList(string.Format("[RoleID]='{0}'", id), ref errMsg);


            foreach (Sys_Menu menu in listMenu)
            {
                if (lstRole != null)
                {
                    var m = lstRole.FirstOrDefault(o => o.MenuID.Equals(menu.Id));
                    if (m != null)
                    {
                        menu.RightValue = m.MenuVal;
                        menu.BitRightVal = Convert.ToString(menu.RightValue, 2);
                    }
                }
            }

            return Json(listMenu, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SaveData(List<Sys_RoleRight> listRoleRight, string roleId)
        {
            //string roleId, string menuCode
            // List<Sys_RoleRight> listRoleRight = new List<Sys_RoleRight>();

            var listMenu = PermissionBLL.GetSysMenuList(string.Format(" IsUse = 1"), ref errMsg);

            List<Sys_RoleRight> lstUn = new List<Sys_RoleRight>();
            foreach (Sys_Menu menu in listMenu)
            {
                if (listRoleRight.Exists(o => o.MenuID.Equals(menu.Id))) continue;

                Sys_RoleRight model = new Sys_RoleRight();
                model.MenuID = menu.Id;
                model.Id = Guid.NewGuid();
                model.RoleID = new Guid(roleId);
                lstUn.Add(model);

            }
            PermissionBLL.BatchInsertSysRoleRight(listRoleRight, lstUn, roleId, ref errMsg);

            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);


        }


        public ActionResult GetRightValueByMenu(string roleId, string menuId)
        {

            var menuVal = PermissionBLL.GetFiledsByFilters("MenuVal", "Sys_RoleRight", string.Format("RoleID='{0}' and MenuID='{1}'", roleId, menuId), ref errMsg);

            var strVal = Convert.ToString(Convert.ToInt32(menuVal), 2);

            return Json(strVal, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SaveRightData(List<Sys_RightValue> listRoleRight, string roleId, string menuID)
        {


            Sys_RoleRight model = new Sys_RoleRight();
            model.Id = Guid.NewGuid();
            model.MenuID = new Guid(menuID);
            model.RoleID = new Guid(roleId);
            model.MenuVal = listRoleRight.Sum(o => o.RightValue);

            string relation = string.Format("[RoleID] = '{0}' and [MenuID] = '{1}'", model.RoleID, model.MenuID);
            Sys_RoleRight right = PermissionBLL.GetSysRoleRightByRelation(relation, ref errMsg);
            if (right == null)
            {
                PermissionBLL.InsertSysRoleRight(model, ref errMsg);
            }
            else
            {
                PermissionBLL.ModifySysRoleRightByFilters(model, "[MenuVal]", relation, ref errMsg);
            }
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);


        }
        public ActionResult GetRight(string menuId)
        {
            var list = PermissionBLL.GetSysRightValueList(string.Format(" IsUse = 1 and MenuID='{0}'", menuId), -1, -1, ref errMsg);
            Dictionary<string, object> jsonObj = new Dictionary<string, object>(2);
            jsonObj.Add("total", 0);
            jsonObj.Add("rows", list);
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ClearData(string roleId)
        {

            PermissionBLL.DeleteLixSysRoleRightByFilters(string.Format("RoleID='{0}'", roleId), ref errMsg);
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult SaveCopyRight(string id, List<string> ids)
        {
            List<Sys_Role> lstGroup = new List<Sys_Role>();
            foreach (string s in ids)
            {
                Sys_Role model = new Sys_Role();
                model.Id = new Guid(s);
                lstGroup.Add(model);

            }
            PermissionBLL.CopyRight(id, lstGroup, ref errMsg);

            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SaveRight(string menuId, string roleId, string irightvalue)
        {
            Sys_RoleRight model = new Sys_RoleRight();
            model.Id = Guid.NewGuid();
            model.MenuID = new Guid(menuId);
            model.RoleID = new Guid(roleId);
            model.MenuVal = Convert.ToInt32(irightvalue);

            string relation = string.Format("[RoleID] = '{0}' and [MenuID] = '{1}'", model.RoleID, model.MenuID);
            Sys_RoleRight right = PermissionBLL.GetSysRoleRightByRelation(relation, ref errMsg);
            if (right == null)
            {
                PermissionBLL.InsertSysRoleRight(model, ref errMsg);
            }
            else
            {
                PermissionBLL.ModifySysRoleRightByFilters(model, "[MenuVal]", relation, ref errMsg);
            }

            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);


        }
        public ActionResult GetRightValue(string menuId)
        {

            var list = PermissionBLL.GetSysRightValueList(string.Format("MenuID='{0}'", menuId), -1, -1, ref errMsg);


            //  var list = PermissionBLL.GetSysRightValueList("IsUse = 1", -1, -1, ref errMsg);
            // List<Sys_RightValue> lst = new List<Sys_RightValue>();
            //if (!string.IsNullOrEmpty(strBit)) {
            //    for (var i = 0; i < list.Count; i++) {
            //        if (strBit.Length > i) {
            //            if (strBit.Substring(i, 1).Equals("1")) {
            //                lst.Add(list[i]);
            //            }
            //        }
            //    }
            //} else {
            //    lst = list;
            //}
            Dictionary<string, object> jsonObj = new Dictionary<string, object>(2);
            jsonObj.Add("total", 0);
            jsonObj.Add("data", list);
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRole(string id)
        {

            //string relation = string.Format("[RoleID]='{0}'", id);
            //List<Sys_RoleRight> listRoleRight = PermissionBLL.GetSysRoleRightList(relation, ref errMsg);


            string relation = string.Format(@"Id IN (
                                            SELECT MenuID
                                            FROM dbo.Sys_RoleRight 
                                            where  [RoleID]='{0}')", id);

            var list = PermissionBLL.GetSysMenuList(relation, ref errMsg);

            var str = string.Join(",", list.Select(o => o.MenuCode));


            return Json(str, JsonRequestBehavior.AllowGet);

        }
    }
}
