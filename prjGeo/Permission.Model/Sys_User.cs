using System;
using Esquel.Framework;
namespace Permission.Model {
    /// <summary>
    /// 实体类Sys_User 。(属性说明自动提取数据库字段的描述信息)
    /// </summary> 
    public partial class Sys_User : EsqDbBusinessEntity {
        public Sys_User() {
            selectTable = "v_User";
            saveTable = "Sys_User";
            backupTable = "Sys_User";
        }
        #region Model
        private Guid _id;
        private string _loginid;
        private string _password;
        private string _address;
        private string _email;
        private string _securityemail;
        private string _phone;
        private string _cellphone;
        private string _idnumber;
        private bool _isadmin;
        private bool _isusestop;
        public string ConfirmPassword { get; set; }
        public bool UserChecked { get; set; }
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
        public string LoginID {
            set { _loginid = value; }
            get { return _loginid; }
        }
        private string _loginName = string.Empty;
        public string LoginName { get { return _loginName; } set { _loginName = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string Password {
            set { _password = value; }
            get { return _password; }
        }
        public string Factory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Email {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SecurityEmail {
            set { _securityemail = value; }
            get { return _securityemail; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Cellphone {
            set { _cellphone = value; }
            get { return _cellphone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Address {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IdNumber {
            set { _idnumber = value; }
            get { return _idnumber; }
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
        public bool IsUseStop {
            set { _isusestop = value; }
            get { return _isusestop; }
        }
        #endregion Model


        public string WorkID { get; set; }

        public string DeptCode { get; set; }

        public string FactoryName { get; set; }

        public string DeptName { get; set; }

        public string JobTitle { get; set; }
    }
}

