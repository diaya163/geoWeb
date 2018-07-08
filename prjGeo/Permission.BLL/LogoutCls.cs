using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Data;
using Permission.Model;

namespace Permission.BLL {
    #region  [放系统的共公变量，或系统的共公参数]
    /// <summary>
    /// 放系统的共公变量，或系统的共公参数
    /// </summary>
    public partial class LogoutCls {

        /// <summary>
        /// 注销用户
        /// </summary>
        public static void Logout() {
            string errMsg = string.Empty;
            PermissionBLL.Logout(new Guid(Setting.TicketId), ref errMsg);
            // Lixtech.WebBlock.LixWebUiUtil.ClearRuntimeCash();
        }
        /// <summary>
        /// 检测用户是否在其他地方登录
        /// </summary>
        /// <returns></returns>
        public static bool IsUserLogin() {
            string filters = string.Format("Id='{0}'", Setting.TicketId);
            string errMsg = string.Empty;
            List<Sys_Ticket> tickets = PermissionBLL.GetTicketList(filters, ref errMsg);
            if (!string.IsNullOrEmpty(errMsg)) {
                System.Windows.Forms.MessageBox.Show("检测账号时发生错误：" + errMsg + "!");
                return false;
            }
            if (tickets == null || tickets.Count <= 0) {
              //  ELoginInfo.IsLogin = true;
                //  Lixtech.WebBlock.LixWebUiUtil.ClearRuntimeCash();
                System.Windows.Forms.MessageBox.Show("该账号已在其他地方登录!");
                return true;
            }
            return false;
            // Sys_Ticket ticket = tickets[0]; 
        }


    }
    #endregion
}
