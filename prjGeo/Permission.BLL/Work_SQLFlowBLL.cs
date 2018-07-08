using Esquel.BaseManager;
using Esquel.Utility;
using Permission.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Permission.BLL {
    public class Work_SQLFlowBLL {
        private static string conStr = DESEncrypt.Decrypt(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["RemoteMESConnectionStringName"]].ConnectionString);
        public static IList<Work_SQLFlow> GetListRelation() {
            string selCmd = string.Format(@"SELECT b.ID, b.fID, b.lID, REPLACE(b.SqlRelation,'@SqlQuery',SqlQuery) as SqlRelation, b.SqlLevelRelation,b.SqlQuery,c.SQLName,a.tableName
                            FROM dbo.Work_TableFlow a
                            Inner JOIN dbo.Work_SQLFlow b ON a.ID = b.fID 
                            LEFT JOIN dbo.Work_LevelFlow c ON c.ID = b.lID
                            Order by fID,lID");

            SQLHelper helper = new SQLHelper(conStr);
            var list = helper.SelectReader<Work_SQLFlow>(typeof(Work_SQLFlow), selCmd.ToString());
            return list;
        }

        public static List<FlowQuery> GetListRelationToUser(ref string errMsg) {
            errMsg = string.Empty;
            List<FlowQuery> list = new List<FlowQuery>();
            var relationTable = GetListRelation();

            //DapperHelper helper = new DapperHelper();
            //var db = helper.SQLConnection(conStr);
            //db.Open();
            foreach (Work_SQLFlow m in relationTable) {

                var sqlQuery = m.SqlQuery.Replace("@UserID", Setting.LoginID);
                string sqlCmd = string.Empty;
                SQLHelper helper = new SQLHelper(conStr);

                sqlCmd = string.Format(@" SELECT case when tableName= 'SignCard' then '补签卡单' 
                                                      when tableName= 'LeaveApplication' then '请假单' 
                                                      when tableName= 'HolidayApplication' then '调休单'
                                                      when tableName= 'CashPay' then '报销单'
                                                      when tableName= 'Borrowing' then '借款单'
                                                 end as tableName,
                                                 case when tableName= 'SignCard' then '签卡管理,%Debug%SuperCom.MES.dll|SuperCom.MES.frmSignCard|0,031604' 
                                                      when tableName= 'LeaveApplication' then '请假管理,%Debug%SuperCom.MES.dll|SuperCom.MES.frmLeave|0,031602' 
                                                      when tableName= 'HolidayApplication' then '调休管理,%Debug%SuperCom.MES.dll|SuperCom.MES.frmHoliday|0,031605'
                                                      when tableName= 'CashPay' then '报销管理,%Debug%SuperCom.MES.dll|SuperCom.MES.frmCashPay|0,031603'
                                                      when tableName= 'Borrowing' then '借款管理,%Debug%SuperCom.MES.dll|SuperCom.MES.frmBorrowing|0,031606'
                                                 end as menu,
                                                 tableNo,a.ID
                                     FROM {0} a  Inner JOIN Work_Flow_Info b ON a.ID=b.tableID and tableName = '{0}' and b.levelNum={1} AND b.IsAudit IN (1,4) 
                                     WHERE {2} IN ({3})"
                                    , m.tableName, m.lID, m.tableName.Equals("SignCard") ? "sUser" : m.tableName.Equals("LeaveApplication") ? "lUser" : m.tableName.Equals("HolidayApplication") ? "hUser" : m.tableName.Equals("CashPay") ? "cUser" : m.tableName.Equals("Borrowing") ? "bUser" : "", sqlQuery);
                var _list = helper.SelectReader<FlowQuery>(sqlCmd);
                // = db.Query<FlowQuery>(sqlCmd);
                list.AddRange(_list);
            }
            //  db.Close();
            return list;
        }


        public static List<FlowQuery> GetAuditMsg(ref string errMsg, List<FlowQuery> lstEx = null) {
            List<FlowQuery> lstMsg = new List<FlowQuery>();
            var list = GetListRelationToUser(ref errMsg);
            foreach (FlowQuery m in list) {
                if (lstEx != null && lstEx.Exists(o => o.tableNo.Equals(m.tableNo))) continue;

                var msg = string.Format("你有单据号【{0}】需要审批,请及时进行审批", m.tableNo, m.tableName);// m.tableName
                m.msg = msg;
                lstMsg.Add(m);
            }
            return lstMsg;
        }

    }
}
