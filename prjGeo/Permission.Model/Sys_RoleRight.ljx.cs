using System;
namespace Permission.Model {
    /// <summary>
    /// ʵ����Sys_RoleRight ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
   
    public partial class Sys_RoleRight {
        private bool _isEndGrade = false;
        public bool IsEndGrade {
            get { return _isEndGrade; }
            set { _isEndGrade = value; }
        }
    }
}

