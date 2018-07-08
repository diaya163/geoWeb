using Esquel.Utility;
using Permission.DAL;
using Permission.Model;
using prjGeo.Models.Sys;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.BLL {
    public static partial class PermissionBLL {

        /// <summary>
        /// 换行符号
        /// </summary>
        private const string NEWLINE_SYMBOL = " \r\n";
        private const string ENDPOINTNAME = "PermissionService";

        private readonly static PersmissionDAL persmissionDAL = new PersmissionDAL("PermissionConnectionStringName");

        public static List<mUsersModel> GetUserListData(string filters, int pageSize, int pageIndex, ref int rCount) {

            string serverIP = ConfigurationManager.AppSettings["ServerIP"];
            var strCon = Esquel.BaseManager.DESEncrypt.Decrypt(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["PermissionConnectionStringName"]].ConnectionString);
            strCon = string.Format(strCon, string.IsNullOrEmpty(serverIP) ? "." : serverIP);
            if (string.IsNullOrEmpty(filters))
                filters = "1=1";
            SQLHelper helper = new SQLHelper(strCon);
            object objRecord = helper.ExecuteScalar(string.Format("select count(1) from mUser  where {0} ",  filters));
            rCount = Convert.ToInt32(objRecord);

            helper = new SQLHelper(strCon);
            StringBuilder selCmd = new StringBuilder();
            selCmd.Append(";with tmp_table as ( ");
            selCmd.Append("SELECT ");
            selCmd.Append("ROW_NUMBER() OVER(ORDER BY a.id DESC) AS tmp_Id,a.*,b.Id as RoleID from mUser a left join Sys_RoleUser b on a.UserCode=b.UserID");
            selCmd.AppendFormat(" where {0} ", filters);
            selCmd.AppendFormat(")select * from tmp_table where tmp_Id between {0} and {1}", pageIndex, pageSize * (pageIndex));
            var iList = helper.SelectReader<mUsersModel>(selCmd.ToString());

            return iList.ToList();

        }

        public static bool GetSysMenuCount(string filters, ref string errMsg) {
            return persmissionDAL.GetSysMenuCount(filters, ref errMsg);


        }
        public static List<Sys_Factory> GetFactory(string filter) {
            return persmissionDAL.GetFactory(filter);
        }
        public static Sys_Ticket Insert_SystemTicket(Sys_Ticket ticket, ref string errMsg) {
            return persmissionDAL.Insert_SystemTicket(ticket, ref errMsg);
        }

        public static bool UpdateSys_ExpiredTicket(Sys_Ticket ticket, ref string errMsg) {
            return persmissionDAL.UpdateSys_ExpiredTicket(ticket, ref errMsg);
        }

        /// <summary>
        /// 创建代理类.
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        //private static ChannelFactory<IPermission> CreateChannelFactory(ref string errMsg) {
        //    ChannelFactory<IPermission> channelFactory = null;
        //    try {
        //        channelFactory = new ChannelFactory<IPermission>(ENDPOINTNAME);
        //        //proxy = channelFactory.CreateChannel();

        //    } catch (Exception ex) {
        //        errMsg = ex.Message;
        //    }
        //    return channelFactory;
        //}
        #region 系统菜单大类   操作
        /// <summary>
        /// 修改系统菜单大类信息
        /// </summary>
        /// <param name="theObject">数据Model</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool ModifyFunModel(Sys_FunModel theObject, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.ModifyFunModel(theObject, ref errMsg);
                //    }
                //}
                return persmissionDAL.ModifyFunModel(theObject, ref errMsg);

            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        /// <summary>
        /// 删除系统菜单节点
        /// </summary>
        /// <param name="theObject">数据Model</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool DeleteFunModel(Sys_FunModel theObject, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.DeleteFunModel(theObject, ref errMsg);
                //    }
                //}
                return persmissionDAL.DeleteFunModel(theObject, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        /// <summary>
        /// 新增系统菜单大类信息
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool InsertFunModel(Sys_FunModel theObject, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.InsertFunModel(theObject, ref errMsg);
                //    }
                //}
                return persmissionDAL.InsertFunModel(theObject, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        /// <summary>
        /// 获取系统菜单大类
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static List<Sys_FunModel> GetSys_FunModelList(string filters, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return null;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.GetSys_FunModelList(filters, ref errMsg);
                //    }
                //}
                return persmissionDAL.GetSys_FunModelList(filters, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;
            }
        }


        #endregion

        #region 系统菜单   操作

        public static bool ModifySysMenuRightValue(Sys_Menu theObject, List<Sys_RightValue> listRight, ref string errMsg) {
            return persmissionDAL.ModifySysMenuRightValue(theObject, listRight, ref errMsg);
        }
        public static bool InsertSysMenuRightValue(Sys_Menu theObject, List<Sys_RightValue> listRight, ref string errMsg) {
            return persmissionDAL.InsertSysMenuRightValue(theObject, listRight, ref errMsg);
        }

        /// <summary>
        /// 删除菜单节点信息
        /// </summary>
        /// <param name="theObject">数据Model</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool DeleteSys_Menu(Sys_Menu theObject, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.DeleteSys_Menu(theObject, ref errMsg);
                //    }
                //}
                return persmissionDAL.DeleteSysMenu(theObject, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }

        /// <summary>
        /// 获取系统表菜单节点信息.返回 List
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static List<Sys_Menu> GetSysMenuList(string filters, int pageSize, int pageIndex, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return null;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.GetSysMenuList(filters, pageSize, pageIndex, ref errMsg);
                //    }
                //}
                return persmissionDAL.GetSysMenuList(filters, pageSize, pageIndex, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;
            }
        }
        /// <summary>
        /// 获取系统表菜单节点信息.返回 List
        /// </summary>
        /// <param name="filters">查询条件</param> 
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static List<Sys_Menu> GetSysMenuList(string filters, ref string errMsg) {
            return GetSysMenuList(filters, -1, -1, ref errMsg);
        }
        /// <summary>
        /// 获取系统菜单节点表信息 返回DataTable
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static System.Data.DataTable GetSysMenu(string filters, ref string errMsg) {
            try {

                return persmissionDAL.GetSysMenu(filters, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;
            }
        }
        #endregion

        #region 用户表 操作
        /// <summary>
        /// 创建批量新增用户与角色记录
        /// </summary>
        /// <param name="groupObject">角色组实体</param>
        /// <param name="roleObjects">角色实体集体</param>
        /// <returns>T:成功;F:失败</returns>
        public static bool BatchUpdateUserAndRoleList(Sys_User theObject, List<Sys_RoleUser> roleObjects, ref string errMsg) {

            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.BatchUpdateUserAndRoleList(theObject, roleObjects, ref errMsg);
                //    }
                //}
                return persmissionDAL.BatchUpdateUserAndRoleList(theObject, roleObjects, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }

        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="theObject">数据Model</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool InsertSys_User(Sys_User theObject, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.InsertSys_User(theObject, ref errMsg);
                //    }
                //}
                return persmissionDAL.InsertSys_User(theObject, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        /// <summary>
        ///  用户表数据更新
        /// </summary>
        /// <param name="theObject">数据实体</param>
        /// <returns>true:成功</returns>
        public static bool ModifySys_User(Sys_User theObject, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.ModifySys_User(theObject, ref errMsg);
                //    }
                //}
                return persmissionDAL.ModifySys_User(theObject, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        /// <summary>
        /// 用户表数据删除
        /// </summary>
        /// <param name="id">主键ID集</param>
        /// <returns>true:成功</returns>
        public static bool DeleteSys_UserByfilters(List<string> filters, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.DeleteSys_UserByfilters(filters, ref errMsg);
                //    }
                //}
                return persmissionDAL.DeleteSys_Userfilters(filters, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        /// <summary>
        /// 获取系统用户操作
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static List<Sys_User> GetSys_UserList(string filters, int pageIndex, int pageSize, ref int rCount, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return null;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.GetSys_UserList(filters, pageIndex, pageSize, ref errMsg);
                //    }
                //}
                return persmissionDAL.GetSys_UserList(filters, pageIndex, pageSize, ref rCount, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;
            }
        }
        /// <summary>
        /// 获取系统用户操作
        /// </summary>
        /// <param name="filters">查询条件</param> 
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static List<Sys_User> GetSys_UserList(string filters, ref string errMsg) {
            int rCount = 0;
            return GetSys_UserList(filters, -1, -1, ref rCount, ref errMsg);
        }
        /// <summary>
        /// 创建批量新增用户与角色记录
        /// </summary>
        /// <param name="groupObj">角色组实体</param>
        /// <param name="roleObjs">角色实体</param>
        /// <returns>T:成功;F:失败</returns>
        public static bool BatchInsertUserAndRoleList(Sys_User objUser, List<Sys_RoleUser> roleObjects, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.BatchInsertUserAndRoleList(objUser, roleObjects, ref errMsg);
                //    }
                //}
                return persmissionDAL.BatchInsertUserAndRoleList(objUser, roleObjects, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }

        #endregion

        #region  角色用户 操作
        /// <summary>
        /// LixSysRole 表数据查询
        /// </summary>
        /// <param name="filters">过滤字段</param>
        /// <param name="parameters">过滤参数</param>
        /// <param name="fields">查询字段</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex"></param>
        /// <param name="sortExpression">排序</param>
        /// <returns>List Sys_RoleUser数据实体 </returns>
        public static List<Sys_RoleUser> GetSysRoleList(string filters, int pageSize, int pageIndex, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return null;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.GetSysRoleList(filters, pageSize, pageIndex, ref errMsg);
                //    }
                //}
                return persmissionDAL.GetSysRoleList(filters, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;
            }

        }

        /// <summary>
        /// 获取系统用户权限操作
        /// </summary>
        /// <param name="filters">查询条件</param> 
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static List<Sys_RoleUser> GetSysRoleList(string filters, ref string errMsg) {
            return GetSysRoleList(filters, -1, -1, ref errMsg);
        }
        /// <summary>
        /// 权限转授
        /// </summary>
        /// <param name="UserId">被转授</param>
        /// <param name="listUser">转授接收用户集合</param>
        /// <returns>T:成功;F:失败</returns>
        public static bool CopyRight(string groupId, List<Sys_Role> lstGroup, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.CopyRight(groupId, lstGroup, ref errMsg);
                //    }
                //}
                return persmissionDAL.CopyRight(groupId, lstGroup, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        /// <summary>
        /// LixSysGroup 表数据查询
        /// </summary>
        /// <param name="filters">过滤字段</param> 
        /// <returns>List Sys_Role数据实体 </returns>
        public static List<Sys_Role> GetSysGroupList(string filters, int pageSize, int pageIndex, ref int rCount, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return null;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.GetSysGroupList(filters, pageSize, pageIndex, ref errMsg);
                //    }
                //}
                return persmissionDAL.GetSysGroupList(filters, pageSize, pageIndex, ref rCount, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;
            }
        }
        /// <summary>
        /// LixSysGroup 表数据查询
        /// </summary>
        /// <param name="filters">过滤字段</param>  
        /// <returns>List Sys_Role数据实体 </returns>
        public static List<Sys_Role> GetSysGroupList(string filters, ref string errMsg) {
            int rCount = 0;
            return GetSysGroupList(filters, -1, -1, ref rCount, ref errMsg);
        }

        public static bool InsertSysGroup(Sys_Role theObject, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.InsertSysGroup(theObject, ref errMsg);
                //    }
                //}
                return persmissionDAL.InsertSysGroup(theObject, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }

        /// <summary>
        /// 创建批量新增角色组与角色记录
        /// </summary>
        /// <param name="groupObject">角色组实体</param>
        /// <param name="roleObjects">角色实体集体</param>
        /// <returns>T:成功;F:失败</returns>
        public static bool BatchUpdateGroupAndRoleList(Sys_Role groupObject, List<Sys_RoleUser> roleObjects, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.BatchUpdateGroupAndRoleList(groupObject, roleObjects, ref errMsg);
                //    }
                //}
                return persmissionDAL.BatchUpdateGroupAndRoleList(groupObject, roleObjects, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        /// <summary>
        /// 创建批量新增角色组与角色记录
        /// </summary>
        /// <param name="groupObj">角色组实体</param>
        /// <param name="roleObjs">角色实体</param>
        /// <returns>T:成功;F:失败</returns>
        public static bool BatchInsertGroupAndRoleList(Sys_Role groupObject, List<Sys_RoleUser> roleObjects, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.BatchInsertGroupAndRoleList(groupObject, roleObjects, ref errMsg);
                //    }
                //}
                return persmissionDAL.BatchInsertGroupAndRoleList(groupObject, roleObjects, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        /// <summary>
        /// 删除用户角色组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool BatchDeleteSysGroup(List<string> groupId, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.BatchDeleteSysGroup(groupId, ref errMsg);
                //    }
                //}
                return persmissionDAL.BatchDeleteSysGroup(groupId, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }

        #endregion

        #region 权限值表 操作
        /// <summary>
        /// LixSysRightValue 表数据查询
        /// </summary>
        /// <param name="filters">过滤字段</param> 
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex"></param> 
        /// <returns>List Sys_RightValue数据实体 </returns>
        public static List<Sys_RightValue> GetSysRightValueList(string filters, int pageSize, int pageIndex, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return null;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.GetSysRightValueList(filters, pageSize, pageIndex, ref errMsg);
                //    }
                //}
                return persmissionDAL.GetSysRightValueList(filters, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;
            }
        }
        /// <summary>
        /// LixSysRightValue 表数据查询
        /// </summary>
        /// <param name="filters">过滤字段</param>  
        /// <returns>List Sys_RightValue数据实体 </returns>
        public static List<Sys_RightValue> GetSysRightValueList(string filters, ref string errMsg) {
            return GetSysRightValueList(filters, -1, -1, ref errMsg);
        }
        /// <summary>
        /// 获取权限值
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static List<Sys_RightValue> GetSysRightValueListByRightValueDesc(string filters, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return null;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.GetSysRightValueListByRightValueDesc(filters, ref errMsg);
                //    }
                //}
                return persmissionDAL.GetSysRightValueListByRightValueDesc(filters, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;
            }
        }
        /// <summary>
        /// 根据查询条件返回最大的权限值 (ljx)
        /// </summary>
        /// <param name="relation">条件</param>
        /// <returns>最大的权限值</returns>
        public static int GetRightValueByRelation(string relation, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return -1;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.GetRightValueByRelation(relation, ref errMsg);
                //    }
                //}
                return persmissionDAL.GetRightValueByRelation(relation, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return -1;
            }
        }


        #endregion

        #region 角色权限值表  操作
        /// <summary>
        /// LixSysRoleRight 表数据查询
        /// </summary>
        /// <param name="filters">过滤条件</param> 
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">页面</param>
        /// <param name="sortExpression">排序</param>
        /// <returns>List Sys_RoleRight数据实体 </returns>
        public static List<Sys_RoleRight> GetSysRoleRightList(string filters, int pageSize, int pageIndex, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return null;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.GetSysRoleRightList(filters, pageSize, pageIndex, ref errMsg);
                //    }
                //}
                return persmissionDAL.GetSysRoleRightList(filters, pageSize, pageIndex, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;
            }
        }
        /// <summary>
        /// LixSysRoleRight 表数据查询
        /// </summary>
        /// <param name="filters">过滤条件</param> 
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">页面</param>
        /// <param name="sortExpression">排序</param>
        /// <returns>List Sys_RoleRight数据实体 </returns>
        public static List<Sys_RoleRight> GetSysRoleRightList(string filters, ref string errMsg) {
            return GetSysRoleRightList(filters, -1, -1, ref errMsg);
        }
        /// <summary>
        /// LixSysRoleRight 数据修改
        /// </summary>
        /// <param name="theObj">数据实体</param>
        /// <param name="fields">修改的字段</param>
        /// <param name="filters">修改条件</param>
        /// <param name="parmeters">条件参数</param>
        /// <returns>T:成功;F失败</returns>
        public static bool ModifySysRoleRightByFilters(Sys_RoleRight theObj, string fields, string filter, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.ModifySysRoleRightByFilters(theObj, fields, filter, ref errMsg);
                //    }
                //}
                return persmissionDAL.ModifySysRoleRightByFilters(theObj, fields, filter, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }

        /// <summary>
        /// 角色 表数据新增
        /// </summary>
        /// <param name="theObject">数据实体</param>
        public static bool InsertSysRoleRight(Sys_RoleRight theObject, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.InsertSysRoleRight(theObject, ref errMsg);
                //    }
                //}
                return persmissionDAL.InsertSysRoleRight(theObject, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        /// <summary>
        /// 创建批量新增角色与菜单
        /// </summary>
        /// <param name="listRoleRight">权限角色</param>
        /// <returns>T:成功;F:失败</returns>
        public static bool BatchInsertSysRoleRight(List<Sys_RoleRight> listRoleRight, List<Sys_RoleRight> listUnRoleRight, string groudId, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.BatchInsertSysRoleRight(listRoleRight, groudId, ref errMsg);
                //    }
                //}
                return persmissionDAL.BatchInsertSysRoleRight(listRoleRight, listUnRoleRight, groudId, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        /// <summary>
        /// 根据查询条件返回角色权限实体
        /// </summary>
        /// <param name="relation">条件</param>
        /// <returns>LixSysRight  实体</returns>
        public static Sys_RoleRight GetSysRoleRightByRelation(string relation, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return null;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.GetSysRoleRightByRelation(relation, ref errMsg);
                //    }
                //}
                return persmissionDAL.GetSysRoleRightByRelation(relation, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;
            }
        }

        /// <summary>
        /// 修改角色权限值
        /// </summary>
        /// <param name="theObj"></param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool ModifySysRoleRight(Sys_RoleRight theObj, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.ModifySysRoleRight(theObj, ref errMsg);
                //    }
                //}
                return persmissionDAL.ModifySysRoleRight(theObj, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        /// <summary>
        /// 批量修改角色权限值
        /// </summary>
        /// <param name="objRole"></param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool BatchSysRoleInsertOrModify(List<Sys_RoleRight> objRole, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.BatchSysRoleInsertOrModify(objRole, ref errMsg);
                //    }
                //}
                return persmissionDAL.BatchSysRoleInsertOrModify(objRole, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }

        /// <summary>
        /// 删除角色权限信息.
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool DeleteLixSysRoleRightByFilters(string filters, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.DeleteLixSysRoleRightByFilters(filters, ref errMsg);
                //    }
                //}
                return persmissionDAL.DeleteLixSysRoleRightByFilters(filters, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        #endregion

        #region   登录门票表 操作
        /// <summary>
        /// 用户登录 并将登录信息写入到登录门票表
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="password"></param>
        /// <param name="accountIp"></param>
        /// <param name="expirationMinute"></param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static Sys_Ticket Login(string loginId, string accountIp, string factroy, int expirationMinute, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return null;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.Login(loginId, accountIp, expirationMinute, ref errMsg);
                //    }
                //}
                return persmissionDAL.Login(loginId, accountIp, factroy, expirationMinute, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;
            }
        }
        public static Sys_Ticket SuperComLogin(string loginId, string accountIp, string factroy, int expirationMinute, ref string errMsg) {
            try {
                return persmissionDAL.SuperComLogin(loginId, accountIp, factroy, expirationMinute, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;
            }
        }
        public static string GetSuperComLoginPwd(string loginId, string factory, ref string errMsg) {
            try {
                return persmissionDAL.GetLoginPwd(loginId, factory, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return string.Empty;
            }
        }
        public static string GetLoginPwd(string loginId, string factory, ref string errMsg) {
            try {
                return persmissionDAL.GetLoginPwd(loginId, factory, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取用户门票信息
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static List<Sys_Ticket> GetTicketList(string filters, ref string errMsg) {
            try {
                return persmissionDAL.GetTicketList(filters, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;

            }
        }
        /// <summary>
        /// 新增过期门票表
        /// </summary>
        /// <param name="theObject">数据Model</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool InsertSys_ExpiredTicket(Sys_Ticket theObject, ref string errMsg) {
            try {

                return persmissionDAL.InsertSys_ExpiredTicket(theObject, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        /// <summary>
        /// 获取用户门票信息
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool DeleteSys_Ticket(Sys_Ticket theObject, ref string errMsg) {
            try {
                return persmissionDAL.DeleteSys_Ticket(theObject, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        #endregion

        #region Sys_Button   操作
        /// <summary>
        /// 修改系统菜单大类信息
        /// </summary>
        /// <param name="theObject">数据Model</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool ModifySys_FunButton(Sys_FunButton theObject, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.ModifySys_Menu(theObject, ref errMsg);
                //    }
                //}
                return persmissionDAL.ModifySys_FunButton(theObject, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        /// <summary>
        /// 删除菜单节点信息
        /// </summary>
        /// <param name="theObject">数据Model</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool DeleteSys_FunButton(Sys_FunButton theObject, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.DeleteSys_Menu(theObject, ref errMsg);
                //    }
                //}
                return persmissionDAL.DeleteSys_FunButton(theObject, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        /// <summary>
        /// 新增系统菜单
        /// </summary>
        /// <param name="theObject">数据Model</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool InsertSys_FunButton(Sys_FunButton theObject, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.InsertSys_Menu(theObject, ref errMsg);
                //    }
                //}
                return persmissionDAL.InsertSys_FunButton(theObject, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }
        /// <summary>
        /// 获取系统表菜单节点信息.返回 List
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static List<Sys_FunButton> GetSys_FunButtonList(string filters, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return null;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.GetSysMenuList(filters, pageSize, pageIndex, ref errMsg);
                //    }
                //}
                return persmissionDAL.GetSys_FunButtonList(filters, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;
            }
        }

        /// <summary>
        /// 获取系统菜单节点表信息 返回DataTable
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static System.Data.DataTable GetSys_FunButton(string filters, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return null;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.GetSysMenu(filters, ref errMsg);
                //    }
                //}
                return persmissionDAL.GetSys_FunButton(filters, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;
            }
        }
        #endregion
        /// <summary>
        /// 用户注销、退出系统
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool Logout(Guid ticketId, ref string errMsg) {
            try {
                string filters = string.Format("Id='{0}'", ticketId);
                List<Sys_Ticket> tickets = GetTicketList(filters, ref errMsg);
                if (!string.IsNullOrEmpty(errMsg)) return false;
                if (tickets != null && tickets.Count > 0) {
                    Sys_Ticket ticket = tickets[0];
                    InsertSys_ExpiredTicket(ticket, ref errMsg);
                    if (!string.IsNullOrEmpty(errMsg)) return false;
                    DeleteSys_Ticket(ticket, ref errMsg);
                    if (!string.IsNullOrEmpty(errMsg)) return false;
                }
                return true;
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }


        /// <summary>
        /// 根据表自动修改某表的数据
        /// </summary>
        /// <param name="fileds">表字段</param>
        /// <param name="tableName">表名称</param>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>T:成功；F：错误</returns>
        public static bool ModifyFiledsByFilters(string fileds, string tableName, string filters, ref string errMsg) {
            try {
                //using (ChannelFactory<IPermission> channelFactory = CreateChannelFactory(ref errMsg)) {
                //    if (!string.IsNullOrEmpty(errMsg)) {
                //        return false;
                //    }
                //    IPermission ep = channelFactory.CreateChannel();
                //    using (ep as IDisposable) {
                //        return ep.ModifyFiledsByFilters(fileds, tableName, filters, ref errMsg);
                //    }
                //}
                return persmissionDAL.ModifyFiledsByFilters(fileds, tableName, filters, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return false;
            }
        }

        /// <summary>
        /// 根据XX字段及表查询相对应的数据
        /// </summary>
        /// <param name="fileds">表字段</param>
        /// <param name="tableName">表名称</param>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>字段数据值</returns>
        public static object GetFiledsByFilters(string fileds, string tableName, string filters, ref string errMsg) {
            try {

                return persmissionDAL.GetFiledsByFilters(fileds, tableName, filters, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;
            }

        }
        public static object GetAppSettingFiledsByFilters(string appSetting, string fileds, string tableName, string filters, ref string errMsg) {
            try {
                PersmissionDAL pDAL = new PersmissionDAL(appSetting);
                return pDAL.GetFiledsByFilters(fileds, tableName, filters, ref errMsg);
            } catch (Exception ex) {
                errMsg = ex.Message + NEWLINE_SYMBOL + errMsg;
                return null;
            }

        }

        public static void DeleteFactory(Sys_Factory model, ref string errMsg) {
            persmissionDAL.DeleteFactory(model, ref errMsg);

        }

        public static void AddFactory(Sys_Factory model, ref string errMsg) {
            persmissionDAL.AddFactory(model, ref errMsg);
        }

        public static void UpdateFactory(Sys_Factory model, ref string errMsg) {
            persmissionDAL.UpdateFactory(model, ref errMsg);
        }
        public static bool ExecuteSQL(string sql, ref string errMsg) {
            return persmissionDAL.ExecuteSQL(sql, ref errMsg);
        }

        public static bool ExecuteSQL(string conStr, string sql, ref string errMsg) {
            PersmissionDAL pdal = new PersmissionDAL(conStr);
            return pdal.ExecuteSQL(sql, ref errMsg);
        }
    }
}
