
using System;
namespace Permission.Model {

    public partial class Sys_Menu {
        //Class begin
        private string pageType = string.Empty;
        public string PageType {
            get { return pageType; }
            set { pageType = value; }
        }
        public int MenuVal { get; set; }
        //Class end
    }
}
