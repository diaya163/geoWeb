using System;
using Esquel.Framework;
namespace Permission.Model {
    /// <summary>
    /// DictionaryRefMaster
    /// </summary>
    public partial class DictionaryRefMaster : EsqDbBusinessEntity {
        public DictionaryRefMaster() {
            selectTable = "DictionaryRef";
            saveTable = "DictionaryRef";
            backupTable = "bak_DictionaryRef";
        }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int? ID {
            set; get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RefKey {
            set; get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RefCode {
            set; get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RefCode2 {
            set; get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RefValue {
            set; get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RefValue2 {
            set; get;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? RefIsUse {
            set; get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int? RefSeq {
            set; get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RefRemark {
            set; get;
        }
        public bool RefSystem {
            get; set;
        }
        #endregion Model

    }
}


