using System;
using Esquel.Framework;
namespace Permission.Model {
    /// <summary>
    /// 实体类Sys_Role 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public partial class Sys_Role : EsqDbBusinessEntity {
        public Sys_Role() {
            selectTable = "Sys_Role";
            saveTable = "Sys_Role";
            backupTable = "Sys_Role";
        }
        #region Model
        private Guid _id;
        private string _rolecode;
        private string _rolename;
        public bool IsUse { get; set; }
        private string _remark;
        /// <summary>
        /// 
        /// </summary>
        public Guid Id {
            set { _id = value; }
            get { return _id; }
        }
        public bool RoleChecked { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RoleCode {
            set { _rolecode = value; }
            get { return _rolecode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RoleName {
            set { _rolename = value; }
            get { return _rolename; }
        }     
        public string RoleName_En { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Remark {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model


        public string Factory { get; set; }
    }
}

