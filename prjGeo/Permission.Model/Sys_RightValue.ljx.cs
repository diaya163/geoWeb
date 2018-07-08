using System;
namespace Permission.Model {
    /// <summary>
    /// 实体类Sys_RightValue 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
   
    public partial class Sys_RightValue {
        private int _rightId = 0;
        public int RightID {
            get { return _rightId; }
            set { _rightId = value; }
        }
        public bool RoleChecked { get; set; }
    }
}

