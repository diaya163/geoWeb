using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjGeo.Models.Sys
{
    public class mMenuModel
    {
        public int id { get; set; }
        public string MenuID { get; set; }
        public string ParentID { get; set; }
        public string MenuName { get; set; }
        public string mURL { get; set; }
        public int IsLeef { get; set; }
        public string IconClass { get; set; }
        public string MenuDesc { get; set; }
        public int MenuSeqNo { get; set; }
        public int IsVisible { get; set; }
        public int mState { get; set; }
        public int mChecked { get; set; }
        public int RightFlag { get; set; }
    }
}
