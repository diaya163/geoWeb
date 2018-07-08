using System;
using Esquel.Framework;
namespace Permission.Model {
    /// <summary>
    /// ʵ����Sys_RoleRight ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    public partial class Sys_RoleRight : EsqDbBusinessEntity {
        public Sys_RoleRight() {
            selectTable = "Sys_RoleRight";
            saveTable = "Sys_RoleRight";
            backupTable = "Sys_RoleRight";
        }
        #region Model
        private Guid _id;
        private Guid _roleid;
        private Guid _menuid;
        private int _menuval;
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
        public Guid MenuID {
            set { _menuid = value; }
            get { return _menuid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MenuVal {
            set { _menuval = value; }
            get { return _menuval; }
        }
        #endregion Model

    }
}

