using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Esquel.Framework;

namespace Permission.Model {
    public partial class Sys_FunButton :EsqDbBusinessEntity {
        public Sys_FunButton() {
            selectTable = "Sys_FunButton";
            saveTable = "Sys_FunButton";
            backupTable = "Sys_FunButton";
        }

        public Guid Id { get; set; }
        public Guid MenuID { get; set; }
        public string cButtonKey { get; set; }
        public string cButtonType { get; set; }
        public string cFormKey { get; set; }
        public string cVoucherKey { get; set; }
        public string cKeyBefore { get; set; }
        public string iOrder { get; set; }
        public string cClassName { get; set; }
        public Guid RightValueID { get; set; }
        public string cHotKey { get; set; }
        public bool bActive { get; set; }
        public string Ufts { get; set; }
        public string Caption { get; set; }
        public int iStatus { get; set; }
    }
}
