using System;
using Esquel.Framework;
namespace Permission.Model {
    /// <summary>
    /// ʵ����Sys_RoleUser ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    public partial class Sys_RoleUser : EsqDbBusinessEntity {
        public Sys_RoleUser() {
            selectTable = "Sys_RoleUser";
            saveTable = "Sys_RoleUser";
            backupTable = "Sys_RoleUser";
        }
        #region Model
        private Guid _id;
        private Guid _roleid;
        private string _userid;

        public string UserCode { get; set; }

        public bool IsUse { get; set; }
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
        public Guid RoleID {
            set { _roleid = value; }
            get { return _roleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserID {
            set { _userid = value; }
            get { return _userid; }
        }
        public string RoleName {
            get;
            set;
        }
        public string RoleCode {
            get;
            set;
        }
        #endregion Model

    }
}

