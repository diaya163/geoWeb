using System;
namespace Permission.Model {
    /// <summary>
    /// ʵ����Sys_RightValue ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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

