using System;
using Esquel.Framework;
namespace Permission.Model {
	/// <summary>
	/// Work_SQLFlow
	/// </summary>
	public partial class Work_SQLFlow {
		 public string SQLName { get; set; }

        public string tableName { get; set; }
        public string SqlQuery { get; set; }
        public string SqlAuditQuery { get; set; }



    }
}


