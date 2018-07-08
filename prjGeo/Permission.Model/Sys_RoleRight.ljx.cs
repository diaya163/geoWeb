using System;
namespace Permission.Model {
    /// <summary>
    /// 实体类Sys_RoleRight 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
   
    public partial class Sys_RoleRight {
        private bool _isEndGrade = false;
        public bool IsEndGrade {
            get { return _isEndGrade; }
            set { _isEndGrade = value; }
        }
    }
}

