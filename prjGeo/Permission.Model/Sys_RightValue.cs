using System;
using Esquel.Framework;
namespace Permission.Model {
    /// <summary>
    /// 实体类Sys_RightValue 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public partial class Sys_RightValue : EsqDbBusinessEntity {
        public Sys_RightValue() {
            selectTable = "Sys_RightValue";
            saveTable = "Sys_RightValue";
            backupTable = "Sys_RightValue";
        }
        #region Model
        private Guid _id;
        private string _rightname;
        private int _rightvalue;
        public string RigthName_En { get; set; }
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
        public string RightName {
            set { _rightname = value; }
            get { return _rightname; }
        }
        public string Factory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RightValue {
            set { _rightvalue = value; }
            get { return _rightvalue; }
        }
        public bool IsUse { get; set; }
        #endregion Model


        public string RightName_En { get; set; }

        public Guid MenuID { get; set; }
    }
}

