using System;
using Esquel.Framework;
namespace Permission.Model {
    /// <summary>
    /// ʵ����Sys_FunModel ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    public partial class Sys_FunModel : EsqDbBusinessEntity {
        public Sys_FunModel() {
            selectTable = "Sys_FunModel";
            saveTable = "Sys_FunModel";
            backupTable = "Sys_FunModel";
        }
        #region Model
        private Guid _id;
        private string _funcode;
        private string _funname;
        public string FunName_En { get; set; }
        private byte[] _funimage;
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
        public string FunCode {
            set { _funcode = value; }
            get { return _funcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FunName {
            set { _funname = value; }
            get { return _funname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] FunImage {
            set { _funimage = value; }
            get { return _funimage; }
        }
        #endregion Model


        public bool IsUse { get; set; }

        public string Factory { get; set; }
    }
}

