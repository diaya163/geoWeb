using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjGeo.BLL.Core;
using prjGeo.Models;
using prjGeo.Models.Sys;
using prjGeo.DAL;

namespace prjGeo.BLL
{
    public class mMenuBLL : BaseBLL
    {
        menuDAL objMenuDAL = new menuDAL();
        public List<mMenuModel> GetMenu(string strUser, string id)
        {
            IEnumerable<mMenu> queryData = null;
            queryData = objMenuDAL.GetMenuByPersonId(strUser, id, db);

            List<mMenuModel> lstMenu = (from r in queryData
                                        select new mMenuModel()
                                        {
                                            id = r.id,
                                            MenuID = r.MenuID,
                                            ParentID = r.ParentID,
                                            MenuName = r.MenuName,
                                            mURL = r.mURL,
                                            IsLeef = r.IsLeef,
                                            IconClass = r.IconClass,
                                            MenuDesc = r.MenuDesc,
                                            MenuSeqNo = r.MenuSeqNo,
                                            IsVisible = r.IsVisible,
                                            mState = r.mState,
                                            mChecked = r.mChecked,
                                            RightFlag = r.RightFlag
                                        }).ToList();
            return lstMenu;
        }

        public List<SysModuleNavModel> GetMenuByPersonId(string strUser, string id)
        {
            IEnumerable<mMenu> queryData = null;
            queryData = objMenuDAL.GetMenuByPersonId(strUser, id, db);

            List<SysModuleNavModel> lstMenu = (from r in queryData
                                               select new SysModuleNavModel()
                                        {
                                            id = r.MenuID.ToString().Trim(),
                                            text = r.MenuDesc,
                                            attributes = r.mURL,
                                            iconCls = r.IconClass,
                                            state = int.Parse(r.mState.ToString().Trim()) == 1 ? "open" : "close",
                                            isLeft=r.IsLeef==1? true:false
                                        }).ToList();
            return lstMenu;
        }



        public int GetMenusCount(string strUser, string id)
        {
            int cnt = 0;
            cnt = objMenuDAL.GetMenuByPersonId(strUser, id, db).Count();
            return cnt;
        }

        public List<SysModuleNavModel> GetList(string strUser, string id)
        {
            IEnumerable<mMenu> queryData = null;
            queryData = objMenuDAL.GetList(strUser, id, db);
            List<SysModuleNavModel> lstMenu = (from r in queryData
                                               select new SysModuleNavModel() 
                                               {
                                                   id = r.MenuID.ToString().Trim(),
                                                   text = r.MenuDesc,
                                                   attributes = r.mURL,
                                                   iconCls = r.IconClass,
                                                   state = int.Parse(r.mState.ToString().Trim()) == 1 ? "open" : "close",
                                                   isLeft = r.IsLeef == 1 ? true : false                                               
                                               }).ToList();
 
            return lstMenu;
            
        }

    }
}
