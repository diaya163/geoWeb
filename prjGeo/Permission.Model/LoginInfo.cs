using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Model {
    [Serializable]
    public partial class LoginInfo {
    
        /// <summary>
        /// 是否为系统管理员
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 登录ID全球唯一标识
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// 登录ID全球唯一标识
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 登录用户ID
        /// </summary>
        public string LoginID { get; set; }
        /// <summary>
        /// 登录用户名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 工厂名称
        /// </summary>
        public string Factory { get; set; }
        public string FactoryName { get; set; }
        /// <summary>
        /// 操作日期
        /// </summary>
        public string LoginDate { get; set; }
        /// <summary>
        /// 本地机器名
        /// </summary>
        public string MachineName { get; set; }

        public string IpAddress { get; set; }

        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogin { get; set; }
        /// <summary>
        /// 系统登录日期
        /// </summary>
        public DateTime LoginTime { get; set; }

        public string DeptCode { get; set; }

    }
  
}
