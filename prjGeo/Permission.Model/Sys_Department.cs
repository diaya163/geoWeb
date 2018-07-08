using System;
using Esquel.Framework;
namespace Permission.Model {
    /// <summary>
    /// Sys_Department
    /// </summary>
    public partial class Sys_Department : EsqDbBusinessEntity {
        public Sys_Department() {
            selectTable = "Sys_Department";
            saveTable = "Sys_Department";
            backupTable = "bak_Sys_Department";
        }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public Guid ID {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string DeptCode {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string DeptName {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string DeptCodeParent {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsUse {
            set;
            get;
        }
        #endregion Model


        public string Factory { get; set; }
    }
}


