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
    public class MenuManagerController : BaseController
    {
        //
        // GET: /MenuManager/
        private string errMsg = string.Empty;
        public ActionResult Index()
        {
            ViewBag.Title = "菜单维护";
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
            var list = PermissionBLL.GetSys_FunModelList(filter, ref errMsg);
            //var list = objBLL.GetIndexList(filters, ref errMsg, ref pager);

            var json = new
            {
                total = rCount,
                rows = list.ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveData(string action, Sys_FunModel model)
        {
            model.Factory = ELoginInfo.Factory;
            switch (action)
            {
                case "new":
                    model.Id = Guid.NewGuid();
                    PermissionBLL.InsertFunModel(model, ref errMsg);
                    break;
                case "edit":
                    PermissionBLL.ModifyFunModel(model, ref errMsg);
                    break;
            }
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMenuCode(string menuCode, bool isRoot)
        {
            object obj = PermissionBLL.GetFiledsByFilters("ISNULL(MAX(MenuCode),'01')MenuCode", "Sys_Menu"
                , string.Format("MenuParentCode='{0}'", menuCode), ref errMsg);
            menuCode = Convert.ToString(Convert.ToInt32(obj.ToString()) + 1);
            if (menuCode.Length < Convert.ToString(obj).Length)
            {
                menuCode = "0" + menuCode;
            }
            return Json(menuCode, JsonRequestBehavior.AllowGet);

        }
        public ActionResult SaveMenuData(string action, Sys_Menu model, string menuCode, List<Sys_RightValue> lstRight)
        {
            model.MenuParentCode = menuCode;
            if (lstRight == null)
            {
                lstRight = new List<Sys_RightValue>();
            }
            model.RightValue = lstRight.Sum(o => o.RightValue);
            lstRight.ForEach(o => o.Factory = ELoginInfo.Factory);
            lstRight.ForEach(o => o.IsUse = true);
            lstRight.ForEach(o => o.Id = o.Id == Guid.Empty ? Guid.NewGuid() : o.Id);
            switch (action)
            {
                case "add":
                    model.Id = Guid.NewGuid();
                    lstRight.ForEach(o => o.MenuID = model.Id);

                    PermissionBLL.InsertSysMenuRightValue(model, lstRight, ref errMsg);
                    break;
                case "edit":
                    lstRight.ForEach(o => o.MenuID = o.MenuID == Guid.Empty ? model.Id : o.MenuID);
                    PermissionBLL.ModifySysMenuRightValue(model, lstRight, ref errMsg);
                    break;
            }
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DeleteData(string id)
        {
            Sys_FunModel m = new Sys_FunModel();
            m.Id = new Guid(id);
            PermissionBLL.DeleteFunModel(m, ref errMsg);
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetFunModelData(string id)
        {
            var dic = PermissionBLL.GetSys_FunModelList(string.Format("Id='{0}'", id), ref errMsg);
            return Json(dic[0], JsonRequestBehavior.AllowGet);
        }
        private List<Sys_Menu> listMenu { get; set; }
        public JsonResult GetMenu()
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

                    tree.id = item.Id.ToString() + "|" + item.MenuCode + "|" + item.MenuName + "|" + item.AssemblyUrl + "|" + item.IsUse;

                    tree.text = item.MenuName;

                    CategoryTreeJson(tree, item.MenuCode);
                    categoryList.Add(tree);
                }
            }
        }

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
                    tree.id = item.Id.ToString() + "|" + item.MenuCode + "|" + item.MenuName + "|" + item.AssemblyUrl + "|" + item.IsUse;
                    tree.text = item.MenuName;

                    CategoryTreeJson(tree, item.MenuCode);
                    sbCategory.children.Add(tree);
                }
            }
        }

        public ActionResult GetMenuData(string menuCode)
        {

            var data = PermissionBLL.GetSysMenuList(string.Format(" FunID Is not Null and MenuCode='{0}' and FunID in (select Id from Sys_FunModel where Factory='{1}')", menuCode, ELoginInfo.Factory), ref errMsg);
            return Json(data[0], JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetRight(string menuId)
        {
            var arr = menuId.Split('|');
            if (arr.Length > 0)
            {
                menuId = arr[0];
            }
            var list = PermissionBLL.GetSysRightValueList(string.Format(" IsUse = 1 and MenuID='{0}'", menuId), -1, -1, ref errMsg);


            Dictionary<string, object> jsonObj = new Dictionary<string, object>(2);
            jsonObj.Add("total", 0);
            jsonObj.Add("rows", list);
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteMenuData(string id)
        {
            Sys_Menu model = new Sys_Menu();
            model.Id = new Guid(id);
            PermissionBLL.DeleteSys_Menu(model, ref errMsg);
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);

        }




    }
}
