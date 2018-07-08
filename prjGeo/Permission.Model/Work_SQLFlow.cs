using System;
using Esquel.Framework;
namespace Permission.Model {
	/// <summary>
	/// Work_SQLFlow
	/// </summary>
	public partial class Work_SQLFlow : EsqDbBusinessEntity{
		public Work_SQLFlow(){
			selectTable = "Work_SQLFlow";
			saveTable = "Work_SQLFlow";
			backupTable = "bak_Work_SQLFlow";
		}
		#region Model
		/// <summary>
		/// 
		/// </summary>
		public int? ID
		{
			set;get;
		}
		/// <summary>
		/// 
		/// </summary>
		public int? fID
		{
			set;get;
		}
		/// <summary>
		/// 
		/// </summary>
		public int? lID
		{
			set;get;
		}
		/// <summary>
		/// 
		/// </summary>
		public string SqlRelation
		{
			set;get;
		}
        #endregion Model

    }
}


