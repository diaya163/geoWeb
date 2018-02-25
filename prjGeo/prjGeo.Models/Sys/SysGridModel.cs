using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjGeo.Models.Sys
{
    public class SysGridModel
    {
        int? FormID { get; set; }
        int? seqno { get; set; }
        string ColName { get; set; }
        int? ColWidth { get; set; }
        string ColAlign { get; set; }
        string ColTitle { get; set; }
        int? ColRowspan { get; set; }
        bool Colhidden { get; set; }
        string ColField { get; set; }
        string ColEditor { get; set; }
        bool isSum { get; set; }
        bool isAvg { get; set; }
        bool isMax { get; set; }
        bool isMin { get; set; }
        bool isSort { get; set; }
        bool isUsed { get; set; }
    }
    
}
