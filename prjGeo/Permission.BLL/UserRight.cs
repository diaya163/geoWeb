using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Permission.Model;


namespace Permission.BLL {
    #region  [UserRight 获取各用户的模块权限及功能权限]
    /// <summary>
    /// 获取各用户的模块权限及功能权限
    /// </summary>
    public class UserRight {
        public UserRight() { }

        #region 得到指定用户"UserName"对模块"menuCode"的操作权限.
        /// <summary>
        /// 得到指定用户"UserName"对模块"menuCode"的操作权限.
        /// </summary>
        /// <param name="userName">用户ID</param>
        /// <param name="menuCode">模块ID</param>
        /// <returns>权限值</returns>
        public static string GetModuleRight(string factory, string userID, string menuCode) {
            // return UserRight.Refresh(userID, menuCode);
            return GetRightValue(factory, userID, menuCode);
        }
        public static void SetToolStripRight(ToolStrip tools, string userID, string menuCode) {
            SetToolStripRight(tools, string.Empty, userID, menuCode);
        }
        /// <summary>
        /// 设置菜单功能按钮权限(调用此方法前，一定要指定TOOLSTRIP 控件按钮的TAG，刷新：0,新增:1,修改:2,删除:3,导入:4,导出:5,打印:6,审核:7,弃审:8,授权:9,转授:10,栏目:11,保存:12,取消:13)
        /// </summary>
        /// <param name="tools"></param>
        /// <param name="userID"></param>
        /// <param name="menuCode"></param>
        public static void SetToolStripRight(ToolStrip tools, string factory, string userID, string menuCode) {
            try {
                var rightVal = GetModuleRight(factory, userID, menuCode);
                var arr = rightVal.ToArray();
                Array.Reverse(arr);
                foreach (ToolStripItem item in tools.Items) {

                    item.Visible = false;
                    if (item.Tag != null && !string.IsNullOrEmpty(Convert.ToString(item.Tag))) {
                        if (Convert.ToInt32(item.Tag) < arr.Length) {
                            item.Visible = arr[Convert.ToInt32(item.Tag)] == '1' ? true : false;
                            //ToolStripItem _item = tools.GetNextItem(item, ArrowDirection.Right);
                            //if (_item.GetType().Name.ToLower().Equals("toolStripSeparator")) {
                            //    _item.Visible = item.Visible;
                            //}
                        }
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show("权限加载错误: " + ex.Message);
            }
        }

        /// <summary>
        /// 权限用户及模块ID获取权限值
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="_menuCode">模块ID</param>
        /// <returns>权限值</returns>
        private static string GetRightValue(string factory, string userid, string menuCode) {
            string exceptionInfo = string.Empty;
            string returnVal = string.Empty;
            if (Setting.IsAdmin || ELoginInfo.IsAdmin) {
                List<Sys_Menu> lstRightVal = PermissionBLL.GetSysMenuList(string.Format("MenuCode='{0}' and FunID IN (SELECT Id FROM dbo.Sys_FunModel WHERE Factory='{1}')", menuCode, factory), ref exceptionInfo);
                if (!string.IsNullOrEmpty(exceptionInfo)) {
                    //MessageBox.Show(Message_CN.GetRightError + exceptionInfo, "系统提示");
                    return string.Empty;
                }
                int maxRightVal = 0;
                foreach (Sys_Menu model in lstRightVal) {
                    maxRightVal += model.RightValue;
                }
                return Convert.ToString(maxRightVal, 2);

                //   return maxRightVal;
            } else {
                string relation = string.Format(" MenuID  in (select ID from Sys_Menu where MenuCode='{0}'  and  FunID IN (SELECT Id FROM dbo.Sys_FunModel WHERE Factory='{1}'))"
                                                + " and RoleID in( select RoleID from Sys_RoleUser"
                                                                    + " where UserID in(select ID from V_User"
                                                                                    + " where LoginID='{2}'))"
                                                , menuCode, factory, userid);
                List<int> lstRight = new List<int>();
                List<Sys_RoleRight> lstRoleRight = PermissionBLL.GetSysRoleRightList(relation, -1, -1, ref exceptionInfo);
                if (!string.IsNullOrEmpty(exceptionInfo)) {
                    // MessageBox.Show(Message_CN.GetRightError + exceptionInfo, "系统提示");
                    return string.Empty;
                }
                if (lstRoleRight == null || lstRoleRight.Count < 1) {
                    return string.Empty;
                } else if (lstRoleRight.Count == 1) {
                    returnVal = Convert.ToString(lstRoleRight[0].MenuVal, 2);

                } else if (lstRoleRight.Count > 1) {
                    string val = ""; int len = 0, sLen = 0;
                    List<string> lstVal = new List<string>();
                    foreach (Sys_RoleRight roleRight in lstRoleRight) {
                        var s = Convert.ToString(roleRight.MenuVal, 2);
                        len = s.Length;
                        if (!string.IsNullOrEmpty(val)) {
                            sLen = val.Length;
                            var strVal = Convert.ToInt64(val) + Convert.ToInt64(s);
                            val = strVal.ToString().Replace("2", "1");

                        } else {
                            val = s;
                        };
                    }
                    returnVal = val;
                }
                return returnVal;
            }
        }
        private static int num = 0;
        /// <summary>
        /// 获取权限按钮的总长度
        /// </summary>
        /// <returns></returns>
        private static int GetRightLen() {
            string errMsg = string.Empty;
            object obj = PermissionBLL.GetFiledsByFilters("COUNT(1) as [len]", "Sys_RightValue", string.Format("1=1"), ref errMsg);
            if (!string.IsNullOrEmpty(errMsg)) {
                MessageBox.Show(errMsg);
                return 0;
            }
            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 获取用户权限值
        /// </summary>
        /// <param name="rightVal"></param>
        /// <param name="lstRight"></param>
        private static void GetRight(int rightVal, ref List<int> lstRight) {
            string strRight = Convert.ToString(rightVal, 2);
            int rightLen = GetRightLen();
            for (int i = rightLen - 1; i > 0; i--) {
                strRight = format(rightLen - strRight.Length) + strRight;
                if (i > 0 && i <= rightLen) {
                    if (Convert.ToBoolean(Convert.ToInt32(strRight.Substring(i - 1, 1)))) {
                        num = 1;
                        num = Convert.ToInt32(Math.Pow(2, 21 - i));
                        if (!lstRight.Exists(ExistsRight)) {
                            lstRight.Add(num);
                        }
                    }
                }
            }
        }
        private static bool ExistsRight(int n) {
            if (n == num) {
                return true;
            }
            {
                return false;
            }
        }
        #endregion

        #region 根据按钮功能及权限值判断是否有权限
        /// <summary>
        /// 根据按钮功能及权限值判断是否有权限
        /// </summary>
        /// <param name="type">权限明细</param>
        /// <param name="strright">权限值</param>
        /// <returns>T:有权限;F：没权限</returns>
        public static bool IsRightByMenuCode(int type, string strright) {
            return isRightByType(type, strright);
        }

        private static bool isRightByType(int type, string strright) {
            int rightLen = GetRightLen();
            strright = format(rightLen - strright.Length) + strright;
            if (type > 0 && type <= rightLen) {
                return Convert.ToBoolean(Convert.ToInt32(strright.Substring(type - 1, 1)));
            }
            return false;
        }
        private static string format(int len) {
            string str = string.Empty;
            for (int i = 0; i < len; i++) {
                str += "0";
            }
            return str;
        }
        #endregion
        #region RightTypeEnums 权限结构体
        /// <summary>
        /// 权限明细
        /// </summary>
       #endregion
    }

    #endregion
}
