using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace prjGeo.Models.Sys
{
    public class mUsersModel
    {
        [Display(Name = "系统编号")]
        public int id { get; set; }

        public string UserCode { get; set; }
        public string UserSeq { get; set; }
        public string UserName { get; set; }
        public string DeptName { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public string OrganizeName { get; set; }
        public string ConfigJSON { get; set; }
        public string IsEnable { get; set; }
        public int? LoginCount { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public string CreatePerson { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
