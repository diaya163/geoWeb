using System;
using Esquel.Framework;

namespace Permission.Model {

    public partial class Sys_Menu : EsqDbBusinessEntity {
        public Sys_Menu() {
            selectTable = "Sys_Menu";
            saveTable = "Sys_Menu";
            backupTable = "Sys_Menu";
        }
        #region Model
        public bool IsAutoRun { get; set; }
        public byte[] ImageType { get; set; }
        private Guid _id;
        private string _menucode;
        private string _menuname;
        private string _assemblypath;
        private string _classname;
        private int _rightvalue;
        private string _menuparentcode;
        private Guid _funid;
        private int _assemblytype;
        public int? Seq { get; set; }
        private string _assemblyUrl = string.Empty;
        public string AssemblyUrl {
            get { return _assemblyUrl; }
            set { _assemblyUrl = value; }
        
        }
        public string BitRightVal {
            get;
            set;
        }
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
        public string MenuCode {
            set { _menucode = value; }
            get { return _menucode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MenuName {
            set { _menuname = value; }
            get { return _menuname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AssemblyPath {
            set { _assemblypath = value; }
            get { return _assemblypath; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ClassName {
            set { _classname = value; }
            get { return _classname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int RightValue {
            set { _rightvalue = value; }
            get { return _rightvalue; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MenuParentCode {
            set { _menuparentcode = value; }
            get { return _menuparentcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid FunID {
            set { _funid = value; }
            get { return _funid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AssemblyType {
            set { _assemblytype = value; }
            get { return _assemblytype; }
        }
        #endregion Model

        public string MenuName_En { get; set; }

        public bool IsUse { get; set; }
    }
}

