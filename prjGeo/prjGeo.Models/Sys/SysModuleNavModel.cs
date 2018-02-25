using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjGeo.Models.Sys
{
    [Serializable]
    public class SysModuleNavModel
    {
        public string attributes { get; set; }
        public List<SysModuleNavModel> children { get; set; }
        public string iconCls { get; set; }
        public string id { get; set; }
        public string state { get; set; }
        public string text { get; set; }
        public bool isLeft { get; set; }
    }
}
