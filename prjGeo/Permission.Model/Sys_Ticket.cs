using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Esquel.Framework;

namespace Permission.Model { 
    public partial class Sys_Ticket : EsqDbBusinessEntity {
        public Sys_Ticket() {
            selectTable = "Sys_Ticket";
            saveTable = "Sys_Ticket";
            backupTable = "bak_Sys_Ticket";
        }
        #region Model
        private Guid _id = Guid.Empty;
        private Guid _accountid = Guid.Empty;
        private string _loginid = string.Empty;
        private string _nickname = string.Empty;
        private bool _isadmin = false;
        private string _ip = string.Empty;
        private DateTime _expiredtime = DateTime.MinValue;
        private DateTime _logintime = DateTime.MinValue;
        public string DeptName { get; set; }
        public string DeptCode { get; set; }
        public string Factory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid Id {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid AccountId {
            set { _accountid = value; }
            get { return _accountid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LoginId {
            set { _loginid = value; }
            get { return _loginid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LoginName {
            set { _nickname = value; }
            get { return _nickname; }
        }
        public DateTime LoginTime {
            get { return _logintime; }
            set { _logintime = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsAdmin {
            set { _isadmin = value; }
            get { return _isadmin; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string IP {
            set { _ip = value; }
            get { return _ip; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ExpiredTime {
            set { _expiredtime = value; }
            get { return _expiredtime; }
        }
        #endregion Model

    }
}
