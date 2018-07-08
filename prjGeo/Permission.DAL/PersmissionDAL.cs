using Esquel.Utility;
using Permission.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.DAL {
    public partial class PersmissionDAL {

        public PersmissionDAL() {
            string serverIP = ConfigurationManager.AppSettings["ServerIP"];
            strCon = Esquel.BaseManager.DESEncrypt.Decrypt(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["PermissionConnectionStringName"]].ConnectionString);
            strCon = string.Format(strCon, string.IsNullOrEmpty(serverIP) ? "." : serverIP);
        }
        public PersmissionDAL(string conStr) {
            string serverIP = ConfigurationManager.AppSettings["ServerIP"];
            strCon = Esquel.BaseManager.DESEncrypt.Decrypt(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings[conStr]].ConnectionString);
            strCon = string.Format(strCon, string.IsNullOrEmpty(serverIP) ? "." : serverIP);
        }
        private string strCon = string.Empty;
        private const string logPath = @"C:\Log\";
        /// <summary>
        /// 计算过期时间
        /// </summary>
        /// <param name="expirationMinute"></param>
        /// <returns></returns>
        private DateTime GetExpiration(int expirationMinute) {
            if (expirationMinute < 0) {
                return DateTime.Now.AddMinutes(20);
            } else if (expirationMinute == 0) {
                return DateTime.Now.AddYears(10);
            } else {
                return DateTime.Now.AddMinutes(expirationMinute);
            }
        }
        public List<Sys_Factory> GetFactory(string filter) {
            SQLHelper helper = new SQLHelper(strCon);
            if (string.IsNullOrEmpty(filter))
                filter = " 1=1";
            string selCmd = string.Format("select * from Sys_Factory where {0}", filter);

            var ilist = helper.SelectReader<Sys_Factory>(typeof(Sys_Factory), selCmd.ToString());

            return ilist.ToList();
        }


        #region Sys_Ticket 用户登录门票表

        public Sys_Ticket Insert_SystemTicket(Sys_Ticket ticket, ref string errMsg) {

            //先查找该帐户正在使用门票，如果找到则删除
            List<Sys_Ticket> tickets = GetTicketList(string.Format("AccountId='{0}'", ticket.AccountId), ref errMsg);
            if (!string.IsNullOrEmpty(errMsg)) {
                return null;
            }
            Sys_Ticket oldTicket = null;
            if (tickets.Count > 0) {
                //找到门票，移动到过期门票表。
                oldTicket = tickets[0];
                oldTicket.ExpiredTime = ticket.LoginTime;
                InsertSys_ExpiredTicket(oldTicket, ref errMsg);
                DeleteSys_Ticket(oldTicket, ref errMsg);
            }

            ticket.Id = Guid.NewGuid();

            InsertSys_Ticket(ticket, ref errMsg);

            return ticket;
        }

        public bool UpdateSys_ExpiredTicket(Sys_Ticket theObject, ref string errMsg) {
            try {
                SQLHelper helper = new SQLHelper(strCon);

                string insCmd = string.Format("Update Sys_ExpiredTicket set ExpiredTime='{0}' where AccountId='{1}'", DateTime.MinValue, theObject.AccountId);

                helper.ExecuteNonQuery(insCmd);

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "DeleteSys_Ticket", errMsg);
                return false;
            }
            return true;
        }

        public string GetLoginPwd(string loginId, string factory, ref string errMsg) {
            SQLHelper helper = new SQLHelper(strCon);
            string Pwd = string.Empty;
            try {
                string selcmd = string.Format("select [password] from {0}  where LoginID='{1}' and Factory='{2}'"
                    , "v_User", loginId, factory);
                object obj = helper.ExecuteScalar(selcmd);
                if (!Convert.IsDBNull(obj)) {
                    Pwd = Convert.ToString(obj);
                }
            } catch (Exception ex) {
                errMsg = ex.Message;
                Pwd = string.Empty;
            }
            return Pwd;
        }

        /// <summary>
        /// 用户登录 并将登录信息写入到登录门票表
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="password"></param>
        /// <param name="accountIp"></param>
        /// <param name="expirationMinute"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public Sys_Ticket Login(string loginId, string accountIp, string factroy, int expirationMinute, ref string errMsg) {
            SQLHelper helper = new SQLHelper(strCon);
            try {
                int rCount = 0;
                List<Sys_User> list = GetSys_UserList(string.Format("LoginID='{0}' and Factory='{1}' and IsUseStop=0", loginId, factroy), -1, -1, ref rCount, ref errMsg);
                //   string selCmd = @"select Employee_ID,Name, from  hr_EmployeeBase (nolock) where LoginID='{0}'";

                if (!string.IsNullOrEmpty(errMsg) || list == null || list.Count <= 0) {
                    return null;
                }
                Sys_User obj = list[0];
                //创建空白无效门票
                Sys_Ticket ticket = new Sys_Ticket();
                ticket.Id = Guid.Empty;
                if (obj.Id == null) {
                    string logId = "4C25E000-0000-0000-0000-000000000000";
                    obj.Id = new Guid(obj.LoginID + logId.Substring(obj.LoginID.Length));
                }
                ticket.AccountId = obj.Id;
                ticket.LoginId = obj.LoginID;
                ticket.LoginName = obj.LoginName;
                ticket.IsAdmin = obj.IsAdmin;
                ticket.Factory = obj.Factory;
                ticket.LoginTime = DateTime.Now;
                ticket.IP = accountIp;
                ticket.ExpiredTime = DateTime.MinValue;
                //先查找该帐户正在使用门票，如果找到则删除
                List<Sys_Ticket> tickets = GetTicketList(string.Format("AccountId='{0}'", obj.Id), ref errMsg);
                if (!string.IsNullOrEmpty(errMsg) || list == null || list.Count <= 0) {
                    return null;
                }
                if (tickets.Count > 0) {
                    //找到门票，移动到过期门票表。
                    ticket = tickets[0];
                    InsertSys_ExpiredTicket(ticket, ref errMsg);
                    DeleteSys_Ticket(ticket, ref errMsg);
                }
                //添加新的门票并返回
                ticket = new Sys_Ticket();
                ticket.Id = Guid.NewGuid();
                ticket.AccountId = obj.Id;
                ticket.LoginId = obj.LoginID;
                ticket.LoginName = obj.LoginName;
                ticket.IsAdmin = obj.IsAdmin;
                ticket.IP = accountIp;
                ticket.LoginTime = DateTime.Now;
                ticket.Factory = obj.Factory;
                ticket.DeptCode = obj.DeptCode;
                ticket.DeptName = obj.DeptName;
                ticket.ExpiredTime = GetExpiration(expirationMinute);
                InsertSys_Ticket(ticket, ref errMsg);

                return ticket;

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "Login", errMsg);
                return null;
            }
        }
        /// <summary>
        /// 新增门票表
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool InsertSys_Ticket(Sys_Ticket theObject, ref string errMsg) {
            try {
                SQLHelper helper = new SQLHelper(strCon);
                StringBuilder insCmd = new StringBuilder();
                insCmd.AppendFormat("Insert into {0}(Id,AccountId,LoginId,LoginName,IsAdmin,IP,ExpiredTime,LoginTime,Factory)", theObject.SaveTable);
                insCmd.AppendFormat("values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')"
                                               , theObject.Id, theObject.AccountId, theObject.LoginId, theObject.LoginName
                                                , theObject.IsAdmin, theObject.IP, theObject.ExpiredTime, theObject.LoginTime, theObject.Factory);

                helper.ExecuteNonQuery(insCmd.ToString());


            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "InsertSys_Ticket", errMsg);
                return false;
            }
            return true;
        }
        public Sys_Ticket SuperComLogin(string loginId, string accountIp, string factroy, int expirationMinute, ref string errMsg) {
            SQLHelper helper = new SQLHelper(strCon);
            try {
                int rCount = 0;
                string selCmd = string.Format(@"select Employee_ID as LoginID,Name as LoginName  from nrCY.dbo.hr_EmployeeBase (nolock) where Employee_ID='{0}'", loginId);

                var list = helper.SelectReader<Sys_User>(typeof(Sys_User), selCmd);
                Sys_User obj = new Sys_User();
                Sys_Ticket ticket = new Sys_Ticket();

                if (list.Count <= 0) {
                    list = GetSys_UserList(string.Format("LoginID='{0}'", loginId), -1, -1, ref rCount, ref errMsg);
                    obj = list[0];
                    ticket.Id = Guid.Empty;
                    ticket.AccountId = obj.Id;
                    ticket.LoginId = obj.LoginID;
                    ticket.LoginName = obj.LoginName;
                    ticket.IsAdmin = obj.IsAdmin;
                    ticket.Factory = obj.Factory;
                    ticket.LoginTime = DateTime.Now;
                    ticket.IP = accountIp;
                    ticket.ExpiredTime = DateTime.MinValue;
                } else {
                    obj = list[0];
                    //创建空白无效门票
                    string logId = "4C25E200-0000-0000-0000-000000000000";
                    obj.Id = new Guid(obj.LoginID + logId.Substring(obj.LoginID.Length));
                    ticket.Id = Guid.Empty;
                    ticket.AccountId = obj.Id;
                    ticket.LoginId = obj.LoginID;
                    ticket.LoginName = obj.LoginName;
                    ticket.IsAdmin = false;
                    ticket.Factory = "Supercom";
                    ticket.LoginTime = DateTime.Now;
                    ticket.IP = accountIp;
                    ticket.ExpiredTime = DateTime.MinValue;
                }
                //先查找该帐户正在使用门票，如果找到则删除
                List<Sys_Ticket> tickets = GetTicketList(string.Format("AccountId='{0}'", obj.Id), ref errMsg);
                if (!string.IsNullOrEmpty(errMsg) || list == null || list.Count <= 0) {
                    return null;
                }
                if (tickets.Count > 0) {
                    //找到门票，移动到过期门票表。
                    ticket = tickets[0];
                    InsertSys_ExpiredTicket(ticket, ref errMsg);
                    DeleteSys_Ticket(ticket, ref errMsg);
                }
                //添加新的门票并返回
                ticket = new Sys_Ticket();
                ticket.Id = Guid.NewGuid();
                ticket.AccountId = obj.Id;
                ticket.LoginId = obj.LoginID;
                ticket.LoginName = obj.LoginName;
                ticket.IsAdmin = obj.IsAdmin;
                ticket.IP = accountIp;
                ticket.LoginTime = DateTime.Now;
                ticket.Factory = obj.Factory;
                ticket.ExpiredTime = GetExpiration(expirationMinute);
                InsertSys_Ticket(ticket, ref errMsg);

                return ticket;

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "Login", errMsg);
                return null;
            }
        }

        /// <summary>
        /// 新增过期门票表
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool InsertSys_ExpiredTicket(Sys_Ticket theObject, ref string errMsg) {
            try {
                SQLHelper helper = new SQLHelper(strCon);
                StringBuilder insCmd = new StringBuilder();
                insCmd.AppendFormat("Insert into {0}(Id,AccountId,LoginId,LoginName,IsAdmin,IP,ExpiredTime,LoginTime,Factory)", "Sys_ExpiredTicket");
                insCmd.AppendFormat("values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')"
                                                , theObject.Id, theObject.AccountId, theObject.LoginId, theObject.LoginName
                                                , theObject.IsAdmin, theObject.IP, theObject.ExpiredTime, theObject.LoginTime, theObject.Factory);
                helper.ExecuteNonQuery(insCmd.ToString());

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "InsertSys_ExpiredTicket", errMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 删除门票信息表
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool DeleteSys_Ticket(Sys_Ticket theObject, ref string errMsg) {
            try {
                SQLHelper helper = new SQLHelper(strCon);

                string insCmd = string.Format("Delete {0} where Id='{1}'", theObject.SaveTable, theObject.Id);

                helper.ExecuteNonQuery(insCmd);

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "DeleteSys_Ticket", errMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取用户门票信息
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public List<Sys_Ticket> GetTicketList(string filters, ref string errMsg) {
            SQLHelper helper = new SQLHelper(strCon);
            try {
                if (string.IsNullOrEmpty(filters)) filters = "1=1";
                Sys_Ticket ticket = new Sys_Ticket();

                List<Sys_Ticket> list = new List<Sys_Ticket>();
                string selcmd = string.Format("select * from {0} where {1}", ticket.SaveTable, filters);
                using (SqlDataReader resultDataReader = helper.ExecuteReader(selcmd)) {
                    while (resultDataReader.Read()) {
                        ticket = new Sys_Ticket();
                        ticket.Id = new Guid(Convert.ToString(resultDataReader["Id"]));
                        ticket.AccountId = new Guid(Convert.ToString(resultDataReader["AccountId"]));
                        ticket.LoginId = Convert.ToString(resultDataReader["LoginID"]);
                        ticket.LoginName = Convert.ToString(resultDataReader["LoginName"]);
                        ticket.IsAdmin = Convert.ToBoolean(resultDataReader["IsAdmin"]);
                        ticket.IP = Convert.ToString(resultDataReader["IP"]);
                        ticket.ExpiredTime = Convert.ToDateTime(resultDataReader["ExpiredTime"]);
                        ticket.LoginTime = Convert.ToDateTime(resultDataReader["LoginTime"]);
                        ticket.Factory = Convert.ToString(resultDataReader["Factory"]);
                        list.Add(ticket);
                    }
                }

                return list;
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //  Lixtech.Utility.Files.WriteOperateLog(logPath, "GetTicketList", errMsg);
                return null;
            }
        }
        #endregion

        #region 系统菜单大类表
        /// <summary>
        /// 获取系统菜单大类
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public List<Sys_FunModel> GetSys_FunModelList(string filters, ref string errMsg) {
            try {

                if (string.IsNullOrEmpty(filters)) filters = " 1=1";
                Sys_FunModel entity = new Sys_FunModel();

                List<Sys_FunModel> list = new List<Sys_FunModel>();
                SQLHelper helper = new SQLHelper(strCon);
                string selcmd = string.Format("select * from {0} where {1} Order By Seq"
                    // , System.Threading.Thread.CurrentThread.CurrentCulture.Name == "EN" ? "FunName_En" : "FunName"
                    , entity.SaveTable, filters);
                using (SqlDataReader resultDataReader = helper.ExecuteReader(selcmd)) {
                    while (resultDataReader.Read()) {
                        entity = new Sys_FunModel();
                        entity.Id = new Guid(resultDataReader["Id"].ToString());
                        entity.FunCode = resultDataReader["FunCode"].ToString();
                        entity.FunName = Convert.ToString(resultDataReader["FunName"]);
                        entity.FunName_En = Convert.ToString(resultDataReader["FunName_En"]);
                        if (!Convert.IsDBNull(resultDataReader["FunImage"]))
                            entity.FunImage = (byte[])resultDataReader["FunImage"];
                        entity.IsUse = Convert.ToBoolean(resultDataReader["IsUse"]);
                        entity.Factory = Convert.ToString(resultDataReader["Factory"]);
                        list.Add(entity);
                    }
                }

                return list;
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //  Lixtech.Utility.Files.WriteOperateLog(logPath, "GetSys_FunModelList", errmsg);
                return null;
            }
        }
        /// <summary>
        /// 新增系统菜单大类信息
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool InsertFunModel(Sys_FunModel theObject, ref string errMsg) {
            SQLHelper helper = new SQLHelper(strCon);
            try {

                //  StringBuilder insCmd = new StringBuilder();
                string insCmd = string.Format("Insert into {0}(Id,FunCode,FunName,FunName_En,FunImage,IsUse,Factory)values(@Id,@FunCode,@FunName,@FunName_En,@FunImage,@IsUse,@Factory)", theObject.SaveTable);

                helper.SQLTransBegin();

                helper.ExecuteNonQuery(insCmd.ToString(), true, theObject);
                // insCmd = new StringBuilder();
                Sys_Menu sysM = new Sys_Menu();
                insCmd = string.Format("Insert into {0}(Id,MenuCode,MenuName,MenuName_En,RightValue,MenuParentCode,FunID)", sysM.SaveTable);
                insCmd += string.Format("values(@Id,@MenuCode,@MenuName,@MenuName_En,@RightValue,@MenuParentCode,@FunID)");
                //SqlParameter[] paras = {		 
                //                            new SqlParameter("@Id", SqlDbType.UniqueIdentifier),
                //                            new SqlParameter("@MenuCode", SqlDbType.VarChar),
                //                            new SqlParameter("@MenuName", SqlDbType.NVarChar),
                //                            new SqlParameter("@RightValue", SqlDbType.Int),
                //                            new SqlParameter("@MenuParentCode", SqlDbType.VarChar),
                //                            new SqlParameter("@FunID", SqlDbType.UniqueIdentifier),
                //                            new SqlParameter("@MenuName_En",SqlDbType.VarChar),
                //                            };



                object obj = GetFiledsByFilters("ISNULL(MAX(MenuCode),'01')MenuCode", "Sys_Menu", "MenuParentCode=0", ref errMsg);
                if (!string.IsNullOrEmpty(errMsg)) return false;

                string MenuCode = Convert.ToString(Convert.ToInt32(obj.ToString()) + 1);
                if (MenuCode.Length <= 2) {
                    MenuCode = "0" + MenuCode;
                }

                sysM.Id = Guid.NewGuid();
                sysM.MenuCode = MenuCode;
                sysM.MenuName = theObject.FunName;
                sysM.RightValue = 0;
                sysM.MenuParentCode = "0";
                sysM.FunID = theObject.Id;
                sysM.MenuName_En = theObject.FunName_En;
                helper.ExecuteNonQuery(insCmd.ToString(), true, sysM);

                helper.SQLTransCommit();


            } catch (Exception ex) {
                helper.SQLRollback();
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //   Lixtech.Utility.Files.WriteOperateLog(logPath, "InsertFunModel", errMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 修改系统菜单大类信息
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool ModifyFunModel(Sys_FunModel theObject, ref string errMsg) {
            SQLHelper helper = new SQLHelper(strCon);
            try {

                StringBuilder modCmd = new StringBuilder();
                modCmd.AppendFormat("Update {0} set FunCode=@FunCode,FunName=@FunName,FunName_En=@FunName_En,FunImage=@FunImage,IsUse=@IsUse,Factory=@Factory where Id=@Id ", theObject.SaveTable);
                //SqlParameter[] parameters = {	
                //                              new SqlParameter("@FunCode", SqlDbType.VarChar),
                //                              new SqlParameter("@FunName", SqlDbType.VarChar),
                //                              new SqlParameter("@FunImage", SqlDbType.Binary),
                //                              new SqlParameter("@FunName_En",SqlDbType.VarChar),
                //                              new SqlParameter("@IsUse",SqlDbType.Bit),
                //                              new SqlParameter("@Id", SqlDbType.UniqueIdentifier),
                //                            };
                //parameters[0].Value = theObject.FunCode;
                //parameters[1].Value = theObject.FunName;
                //parameters[2].Value = theObject.FunImage == null ? DBNull.Value : theObject.FunImage as object;
                //parameters[3].Value = theObject.FunName_En == null ? DBNull.Value : theObject.FunName_En as object;
                //parameters[4].Value = theObject.IsUse;
                //parameters[5].Value = theObject.Id;

                helper.ExecuteNonQuery(modCmd.ToString(), true, theObject);
                modCmd = new StringBuilder();
                Sys_Menu sysM = new Sys_Menu();
                modCmd.AppendFormat("Update {0} set MenuName=@MenuName,MenuName_En=@MenuName_En  where FunID=@Id and MenuParentCode='0'", sysM.SaveTable);
                SqlParameter[] pars = {
                                              new SqlParameter("@MenuName", SqlDbType.NVarChar),
                                              new SqlParameter("@MenuName_En",SqlDbType.VarChar),
                                              new SqlParameter("@Id", SqlDbType.UniqueIdentifier),
                                            };
                sysM.MenuName = theObject.FunName;
                sysM.MenuName_En = theObject.FunName_En;
                sysM.Id = theObject.Id;

                helper.ExecuteNonQuery(modCmd.ToString(), true, pars);
                helper.SQLTransCommit();

            } catch (Exception ex) {
                helper.SQLRollback();
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                // Lixtech.Utility.Files.WriteOperateLog(logPath, "ModifyFunModel", errMsg);
                return false;
            }
            return true;
        }
        #endregion

        #region 系统菜单节点
        /// <summary>
        /// 删除系统菜单节点
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool DeleteFunModel(Sys_FunModel theObject, ref string errMsg) {
            SQLHelper dbHelper = new SQLHelper(strCon);
            try {
                dbHelper.SQLTransBegin();
                Sys_Menu theMenu = new Sys_Menu();
                Sys_RoleRight theRole = new Sys_RoleRight();
                string selCmd = string.Format("Delete {0} where MenuID in (select Id from {1} where FunID='{2}')"
                    , theRole.SelectTable, theMenu.SelectTable, theObject.Id);
                dbHelper.ExecuteNonQuery(selCmd, true, null);

                selCmd = string.Format("Delete Sys_RightValue where MenuID in (select Id from Sys_Menu where FunID='0}')", theObject.Id);
                dbHelper.ExecuteNonQuery(selCmd, true, null);


                selCmd = string.Format("Delete {0} where FunID='{1}'"
                                     , theMenu.SelectTable, theObject.Id);
                dbHelper.ExecuteNonQuery(selCmd, true, null);

                selCmd = string.Format("Delete {0} where Id='{1}'", theObject.SelectTable, theObject.Id);
                dbHelper.ExecuteNonQuery(selCmd, true, null);

                dbHelper.SQLTransCommit();

                return true;

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                dbHelper.SQLRollback();
                // Lixtech.Utility.Files.WriteOperateLog(logPath, "DeleteFunModel", errMsg);
                return false;
            }
        }
        public bool InsertSysMenuRightValue(Sys_Menu theObject, List<Sys_RightValue> listRight, ref string errMsg) {
            SQLHelper dbHelper = new SQLHelper(strCon);
            try {
                dbHelper.SQLTransBegin();
                InsertSysMenu(dbHelper, theObject, ref errMsg);
                if (!string.IsNullOrEmpty(errMsg)) {
                    dbHelper.SQLRollback();
                    return false;
                }
                InsertSys_RightValue(dbHelper, listRight, ref errMsg);
                if (!string.IsNullOrEmpty(errMsg)) {
                    dbHelper.SQLRollback();
                    return false;
                }

                dbHelper.SQLTransCommit();

                return true;

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                dbHelper.SQLRollback();
                return false;
            }
        }
        public bool ModifySysMenuRightValue(Sys_Menu theObject, List<Sys_RightValue> listRight, ref string errMsg) {
            SQLHelper dbHelper = new SQLHelper(strCon);
            errMsg = string.Empty;
            try {
                dbHelper.SQLTransBegin();
                ModifySysMenu(dbHelper, theObject, ref errMsg);
                if (!string.IsNullOrEmpty(errMsg)) {
                    dbHelper.SQLRollback();
                    return false;
                }
                InsertSys_RightValue(dbHelper, listRight, ref errMsg);
                if (!string.IsNullOrEmpty(errMsg)) {
                    dbHelper.SQLRollback();
                    return false;
                }

                dbHelper.SQLTransCommit();

                return true;

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                dbHelper.SQLRollback();
                return false;
            }
        }

        public bool InsertSys_RightValue(SQLHelper helper, List<Sys_RightValue> listRight, ref string errMsg) {
            bool isTrue = true;
            if (helper == null) {
                helper = new SQLHelper(strCon);
                isTrue = false;
            }
            try {
                if (listRight.Count > 0) {
                    string delCmd = "delete Sys_RightValue where MenuID=@MenuID";
                    helper.ExecuteNonQuery(delCmd, isTrue, listRight[0]);
                }
                foreach (Sys_RightValue m in listRight) {
                    string cmd = string.Format(@" Insert Sys_RightValue(Id,MenuID,Factory,RightName,RightValue,IsUse)Values(@Id,@MenuID,@Factory,@RightName,@RightValue,@IsUse) ");
                    helper.ExecuteNonQuery(cmd, isTrue, m);
                }

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "InsertSysMenu", errMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 新增系统菜单
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool InsertSysMenu(SQLHelper helper, Sys_Menu theObject, ref string errMsg) {
            bool isTrue = true;
            if (helper == null) {
                helper = new SQLHelper(strCon);
                isTrue = false;
            }
            try {
                StringBuilder insCmd = new StringBuilder();
                insCmd.AppendFormat("Insert into {0}", theObject.SaveTable);
                insCmd.AppendFormat("(Id,MenuCode,MenuName,AssemblyPath,RightValue,MenuParentCode,FunID,AssemblyType,ClassName,AssemblyUrl,MenuName_En,IsUse,IsAutoRun,Seq,ImageType)");
                insCmd.AppendFormat("values(@Id,@MenuCode,@MenuName,@AssemblyPath,@RightValue,@MenuParentCode,@FunID,@AssemblyType,@ClassName,@AssemblyUrl,@MenuName_En,@IsUse,@IsAutoRun,@Seq,@ImageType)");
                //insCmd.AppendFormat("values('{0}','{1}','{2}','{3}',{4},'{5}','{6}',{7},'{8}','{9}','{10}','{11}','{12}',{13},'{14}') "
                //                            , theObject.Id, theObject.MenuCode, theObject.MenuName, theObject.AssemblyPath
                //                            , theObject.RightValue, theObject.MenuParentCode, theObject.FunID
                //                            , theObject.AssemblyType, theObject.ClassName, theObject.AssemblyUrl
                //                            , theObject.MenuName_En, theObject.IsUse, theObject.IsAutoRun, theObject.Seq, theObject.ImageType);
                helper.ExecuteNonQuery(insCmd.ToString(), isTrue, theObject);

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "InsertSysMenu", errMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 删除菜单节点信息
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool DeleteSysMenu(Sys_Menu theObject, ref string errMsg) {
            SQLHelper helper = new SQLHelper(strCon);
            try {
                Sys_RoleRight tableName = new Sys_RoleRight();
                helper.SQLTransBegin();
                string selCmd = string.Format("Delete {0} where MenuID = '{1}' ", tableName.SaveTable, theObject.Id);
                helper.ExecuteNonQuery(selCmd, true, null);

                selCmd = string.Format("Delete Sys_RightValue where MenuID='{0}' ", theObject.Id);
                helper.ExecuteNonQuery(selCmd, true, null);

                selCmd = string.Format("Delete {0} where Id='{1}' ", theObject.SaveTable, theObject.Id);
                helper.ExecuteNonQuery(selCmd, true, null);



                helper.SQLTransCommit();

            } catch (Exception ex) {
                helper.SQLRollback();
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //  Lixtech.Utility.Files.WriteOperateLog(logPath, "DeleteSysMenu", errMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 修改系统菜单节点
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool ModifySysMenu(SQLHelper helper, Sys_Menu theObject, ref string errMsg) {
            try {
                bool isTrue = true;
                if (helper == null) {
                    helper = new SQLHelper(strCon);
                    isTrue = false;
                }
                //string selCmd = string.Format("Update {0} set MenuName='{1}',AssemblyPath='{2}',RightValue={3},AssemblyType={4},className='{5}',AssemblyUrl='{6}',MenuName_En='{7}',IsUse='{8}',IsAutoRun='{9}',Seq={10},ImageType=@ImageType where Id='{11}' ",
                //                            theObject.SaveTable, theObject.MenuName, theObject.AssemblyPath,
                //                            theObject.RightValue, theObject.AssemblyType, theObject.ClassName, theObject.AssemblyUrl, theObject.MenuName_En, theObject.IsUse, theObject.IsAutoRun, theObject.Seq, theObject.Id);

                string selCmd = string.Format("Update {0} set MenuName=@MenuName,AssemblyPath=@AssemblyPath,RightValue=@RightValue,AssemblyType=@AssemblyType,ClassName=@ClassName,AssemblyUrl=@AssemblyUrl,MenuName_En=@MenuName_En,IsUse=@IsUse,IsAutoRun=@IsAutoRun,Seq=@Seq,ImageType=@ImageType where Id=@Id ",
                                        theObject.SaveTable);

                helper.ExecuteNonQuery(selCmd, isTrue, theObject);

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "ModifySysMenu", errMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取系统菜单节点表信息 返回DataTable 
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public DataTable GetSysMenu(string filters, ref string errMsg) {
            if (string.IsNullOrEmpty(filters)) filters = " 1=1";
            string tableName = new Sys_Menu().SelectTable;

            DataTable list = null;

            try {

                list = new DataTable();
                SQLHelper helper = new SQLHelper(strCon);
                string selcmd = string.Format("select *,(select FunCode from Sys_FunModel where Id=Sys_Menu.FunID) as PageType from {0} where {1} order by MenuCode"
                 //   , System.Threading.Thread.CurrentThread.CurrentCulture.Name == "EN" ? "MenuName_En" : "MenuName"
                 , tableName, filters);
                list = helper.ExecuteDataSet(selcmd).Tables[0];

                return list;
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "GetSysMenu", errMsg);
                return null;
            }

        }
        /// <summary>
        /// 获取系统表菜单节点信息.返回 List
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public List<Sys_Menu> GetSysMenuList(string filters, int pageSize, int pageIndex, ref string errMsg) {
            if (string.IsNullOrEmpty(filters)) filters = " 1=1";
            string tableName = new Sys_Menu().SelectTable;

            List<Sys_Menu> list = null;
            try {

                list = new List<Sys_Menu>();
                SQLHelper helper = new SQLHelper(strCon);
                string selcmd = string.Format("select * ,(select FunCode from Sys_FunModel where Id=Sys_Menu.FunID) as PageType from {0} where {1} order by MenuParentCode,Seq"
                   // , System.Threading.Thread.CurrentThread.CurrentCulture.Name == "EN" ? "MenuName_En" : "MenuName"
                   , tableName, filters);
                using (SqlDataReader resultDataReader = helper.ExecuteReader(selcmd)) {
                    while (resultDataReader.Read()) {
                        Sys_Menu entity = new Sys_Menu();
                        entity.Id = new Guid(resultDataReader["Id"].ToString());
                        entity.FunID = new Guid(resultDataReader["FunID"].ToString());
                        entity.MenuCode = resultDataReader["MenuCode"].ToString();
                        entity.MenuName = resultDataReader["MenuName"].ToString();
                        entity.MenuName_En = Convert.ToString(resultDataReader["MenuName_En"]);
                        entity.MenuParentCode = resultDataReader["MenuParentCode"].ToString();
                        entity.AssemblyPath = resultDataReader["AssemblyPath"].ToString();
                        entity.ClassName = resultDataReader["ClassName"].ToString();
                        entity.AssemblyUrl = resultDataReader["AssemblyUrl"].ToString();
                        entity.PageType = resultDataReader["PageType"].ToString();
                        entity.RightValue = Convert.ToInt32(resultDataReader["RightValue"]);
                        entity.AssemblyType = Convert.ToInt32(resultDataReader["AssemblyType"]);
                        entity.IsUse = Convert.ToBoolean(resultDataReader["IsUse"]);
                        entity.IsAutoRun = Convert.ToBoolean(resultDataReader["IsAutoRun"]);
                        entity.BitRightVal = Convert.ToString(entity.RightValue, 2);//转换成2进制
                        if (!Convert.IsDBNull(resultDataReader["Seq"]))
                            entity.Seq = Convert.ToInt32(resultDataReader["Seq"]);
                        list.Add(entity);
                    }
                }

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "GetSysMenuList", errMsg);
                return null;
            }
            return list;
        }


        public bool GetSysMenuCount(string filters, ref string errMsg) {
            if (string.IsNullOrEmpty(filters)) filters = " 1=1";
            string tableName = new Sys_Menu().SelectTable;

            List<Sys_Menu> list = null;
            try {

                list = new List<Sys_Menu>();
                SQLHelper helper = new SQLHelper(strCon);
                string selcmd = string.Format("select count(1) from {0} where {1} "
                   , tableName, filters);
                var obj = helper.ExecuteScalar(selcmd);
                return Convert.ToInt32(obj) > 0 ? true : false;

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "GetSysMenuList", errMsg);
                return false;
            }
            return true;
        }

        #endregion

        #region 系统用户操作
        /// <summary>
        /// 获取系统用户操作
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>

        public List<Sys_User> GetSys_UserList(string filters, int pageIndex, int pageSize, ref int rCount, ref string errMsg) {
            try {

                Sys_User entity = new Sys_User();
                if (string.IsNullOrEmpty(filters)) filters = " 1=1";

                //string selcmd = string.Format("select * from {0} where {1} ", entity.SelectTable, filters);
                //if (pageIndex >= 0 && pageSize > 0) {

                //    selcmd = string.Format("select top {0} * from {1} where {2} and Id not in (selec top ({0}*{3}) Id from {1} where {2})"
                //        , pageSize, entity.SelectTable, filters, pageIndex);
                //}
                StringBuilder selCmd = new StringBuilder();
                SQLHelper helper;
                if (pageSize != -1) {
                    helper = new SQLHelper(strCon);
                    object objRecord = helper.ExecuteScalar(string.Format("select count(1) from {0}  where {1} ", entity.SelectTable, filters));
                    rCount = Convert.ToInt32(objRecord);
                }
                if (pageSize != -1) {
                    selCmd.Append(";with tmp_table as ( ");

                    selCmd.Append("SELECT ");
                    selCmd.Append("ROW_NUMBER() OVER(ORDER BY a.Id DESC) AS tmp_Id,");

                    selCmd.Append("a.Id,LoginID,a.WorkID,LoginName,a.DeptCode,Password,(select FName from Sys_Factory where fCode=A.Factory) as FName, DeptName,a.Factory,Address,Email,SecurityEmail,Phone,Cellphone,IdNumber,IsAdmin,IsUseStop,JobTitle,convert(nvarchar,convert(money,Ufts),2) as Ufts");
                    selCmd.AppendFormat(" FROM {0} a ", entity.SelectTable);

                    selCmd.AppendFormat(" where {0} ", filters);
                    selCmd.AppendFormat(")select * from tmp_table where tmp_Id between {0} and {1}", pageSize * pageIndex + 1, pageSize * (pageIndex + 1));
                } else {
                    selCmd.Append("SELECT ");
                    selCmd.Append("a.Id,LoginID,a.WorkID,LoginName,a.DeptCode,Password,(select FName from Sys_Factory where fCode=A.Factory) as FName,DeptName,a.Factory,Address,Email,SecurityEmail,Phone,Cellphone,IdNumber,IsAdmin,IsUseStop,JobTitle,convert(nvarchar,convert(money,Ufts),2) as Ufts");

                    selCmd.AppendFormat(" FROM {0} a ", entity.SelectTable);

                    selCmd.AppendFormat(" where {0} ", filters);
                }
                List<Sys_User> list = new List<Sys_User>();
                helper = new SQLHelper(strCon);
                using (SqlDataReader resultDataReader = helper.ExecuteReader(selCmd.ToString())) {
                    while (resultDataReader.Read()) {
                        entity = new Sys_User();
                        entity.LoginID = resultDataReader["LoginID"].ToString();

                        if (!Convert.IsDBNull(resultDataReader["Id"])) {
                            entity.Id = new Guid(resultDataReader["Id"].ToString());
                        } else {
                            string logId = "4C25E000-0000-0000-0000-000000000000";
                            entity.Id = new Guid(entity.LoginID + logId.Substring(entity.LoginID.Length));

                        }
                        entity.WorkID = Convert.ToString(resultDataReader["WorkID"]);
                        entity.Address = resultDataReader["Address"].ToString();
                        entity.Password = resultDataReader["Password"].ToString();
                        entity.Email = Convert.ToString(resultDataReader["Email"]);
                        entity.SecurityEmail = Convert.ToString(resultDataReader["SecurityEmail"]);
                        entity.Phone = Convert.ToString(resultDataReader["Phone"]);
                        entity.Cellphone = Convert.ToString(resultDataReader["Cellphone"]);
                        entity.IdNumber = Convert.ToString(resultDataReader["IdNumber"]);
                        entity.IdNumber = Convert.ToString(resultDataReader["IdNumber"]);
                        entity.IsAdmin = Convert.ToBoolean(resultDataReader["IsAdmin"]);
                        entity.IsUseStop = Convert.ToBoolean(resultDataReader["IsUseStop"]);
                        entity.LoginName = Convert.ToString(resultDataReader["LoginName"]);
                        entity.Factory = Convert.ToString(resultDataReader["Factory"]);
                        entity.DeptCode = Convert.ToString(resultDataReader["DeptCode"]);

                        entity.FactoryName = Convert.ToString(resultDataReader["FName"]);
                        entity.DeptName = Convert.ToString(resultDataReader["DeptName"]);
                        entity.JobTitle = Convert.ToString(resultDataReader["JobTitle"]);
                        entity.ConfirmPassword = entity.Password;
                        list.Add(entity);
                    }
                }

                return list;
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //  Lixtech.Utility.Files.WriteOperateLog(logPath, "GetSys_UserList", errMsg);
                return null;
            }
        }


        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>

        public bool InsertSys_User(Sys_User theObject, ref string errMsg) {
            try {
                SQLHelper helper = new SQLHelper(strCon);
                StringBuilder insCmd = new StringBuilder();
                insCmd.AppendFormat("Insert into {0}(Password,Address,Email,SecurityEmail", theObject.SaveTable);
                insCmd.AppendFormat(",Phone,Cellphone,IdNumber,IsAdmin,IsUseStop,LoginID,Id,LoginName,Factory,DeptCode,JobTitle)");
                insCmd.AppendFormat("values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}') ",
                                           theObject.Password, theObject.Address, theObject.Email
                                          , theObject.SecurityEmail, theObject.Phone, theObject.Cellphone
                                          , theObject.IdNumber, theObject.IsAdmin, theObject.IsUseStop, theObject.LoginID
                                          , theObject.Id, theObject.LoginName, theObject.Factory, theObject.DeptCode, theObject.JobTitle);
                helper.ExecuteNonQuery(insCmd.ToString());

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "InsertSys_User", errMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        ///  用户表数据更新
        /// </summary>
        /// <param name="theObject">数据实体</param>
        /// <returns>true:成功</returns>

        public bool ModifySys_User(Sys_User theObject, ref string errMsg) {
            try {
                SQLHelper helper = new SQLHelper(strCon);

                string selCmd = string.Format("Update {0} set Password='{1}',Address='{2}',Email='{3}',SecurityEmail='{4}'"
                                             + ",Phone='{5}',Cellphone='{6}',IdNumber='{7}',IsAdmin='{8}',IsUseStop='{9}',LoginID='{10}',LoginName='{11}',Factory='{12}',DeptCode='{13}',JobTitle='{14}' where Id='{15}' ",
                                             theObject.SaveTable, theObject.Password, theObject.Address, theObject.Email
                                             , theObject.SecurityEmail, theObject.Phone, theObject.Cellphone
                                             , theObject.IdNumber, theObject.IsAdmin, theObject.IsUseStop, theObject.LoginID, theObject.LoginName, theObject.Factory, theObject.DeptCode, theObject.JobTitle, theObject.Id);
                helper.ExecuteNonQuery(selCmd);


            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                // Lixtech.Utility.Files.WriteOperateLog(logPath, "ModifySys_User", errMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 创建批量新增用户与角色记录
        /// </summary>
        /// <param name="groupObject">角色组实体</param>
        /// <param name="roleObjects">角色实体集体</param>
        /// <returns>T:成功;F:失败</returns>

        public bool BatchUpdateUserAndRoleList(Sys_User theObject, List<Sys_RoleUser> roleObjects, ref string errMsg) {
            SQLHelper helper = new SQLHelper(strCon);

            try {
                Sys_RoleUser obj = new Sys_RoleUser();
                helper.SQLTransBegin();
                string selCmd = string.Format("Update {0} set Address='{1}',Email='{2}',SecurityEmail='{3}',Phone='{4}',Cellphone='{5}',IdNumber='{6}',IsAdmin='{7}',IsUseStop='{8}',LoginID='{9}',LoginName='{10}',Factory='{11}',DeptCode='{12}',JobTitle='{13}' where Id='{14}' ",
                                         theObject.SaveTable, theObject.Address, theObject.Email
                                         , theObject.SecurityEmail, theObject.Phone, theObject.Cellphone
                                          , theObject.IdNumber, theObject.IsAdmin, theObject.IsUseStop, theObject.LoginID, theObject.LoginName, theObject.Factory, theObject.DeptCode, theObject.JobTitle, theObject.Id);
                helper.ExecuteNonQuery(selCmd, true);

                if (roleObjects.Count > 0) {
                    //将删除插入第一条
                    string delCmd = string.Format("DELETE FROM {0} WHERE [UserID]='{1}'", obj.SaveTable, theObject.Id);
                    helper.ExecuteNonQuery(delCmd, true);
                }
                for (int i = 0; i < roleObjects.Count; i++) {
                    string insCmd = string.Format("Insert into {0}(Id,RoleID,UserID)Values('{1}','{2}','{3}')"
                                             , roleObjects[i].SaveTable, roleObjects[i].Id, roleObjects[i].RoleID, roleObjects[i].UserID);
                    helper.ExecuteNonQuery(insCmd, true);
                }
                helper.SQLTransCommit();

            } catch (Exception ex) {
                helper.SQLRollback();
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "BatchUpdateUserAndRoleList", errMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 用户表数据删除
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>true:成功</returns>

        public bool DeleteSys_Userfilters(List<string> filters, ref string errMsg) {
            SQLHelper helper = new SQLHelper(strCon);
            try {
                helper.SQLTransBegin();
                Sys_User tableName = new Sys_User();
                Sys_RoleUser tableName2 = new Sys_RoleUser();
                foreach (string s in filters) {
                    string delCmd = string.Format("Delete {0} where ID='{1}' ", tableName.SelectTable, s);
                    helper.ExecuteNonQuery(delCmd, true);
                    delCmd = string.Format("Delete {0} where [UserID]='{1}' ", tableName2.SelectTable, s);
                    helper.ExecuteNonQuery(delCmd, true);
                }
                helper.SQLTransCommit();

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                helper.SQLRollback();
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "DeleteSys_Userfilters", errMsg);
                return false;
            }
            return true;
        }
        #endregion

        #region 角色权限表表操作

        /// <summary>
        /// 删除角色权限信息.
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>

        public bool DeleteLixSysRoleRightByFilters(string filters, ref string errMsg) {
            try {
                SQLHelper helper = new SQLHelper(strCon);
                Sys_RoleRight theObj = new Sys_RoleRight();
                string delCmd = string.Format("delete {0} where {1}", theObj.SelectTable, filters);
                helper.ExecuteNonQuery(delCmd);
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                // Lixtech.Utility.Files.WriteOperateLog(logPath, "DeleteLixSysRoleRightByFilters", errMsg);
                return false;
            }
            return true;
        }

        /// <summary>
        /// LixSysRoleRight 表数据查询
        /// </summary>
        /// <param name="filters">过滤字段</param>
        /// <param name="parameters">过滤参数</param>
        /// <param name="fields">查询字段</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex"></param>
        /// <param name="sortExpression">排序</param>
        /// <returns>List Sys_RoleRight数据实体 </returns>

        public List<Sys_RoleRight> GetSysRoleRightList(string filters, int pageSize, int pageIndex, ref string errMsg) {
            List<Sys_RoleRight> list = null;
            try {
                Sys_RoleRight entity = new Sys_RoleRight();
                if (string.IsNullOrEmpty(filters)) filters = " 1=1";
                string cacheKey = "[" + entity.SelectTable + "][List](" + filters.Trim() + ")";

                list = new List<Sys_RoleRight>();
                SQLHelper helper = new SQLHelper(strCon);
                string selCmd = string.Format("select * from Sys_RoleRight where {0} ", filters);
                using (SqlDataReader resultDataReader = helper.ExecuteReader(selCmd, null)) {
                    if (resultDataReader != null && !resultDataReader.IsClosed) {

                        while (resultDataReader.Read()) {
                            entity = new Sys_RoleRight();

                            entity.Id = new Guid(resultDataReader["Id"].ToString());

                            entity.RoleID = new Guid(resultDataReader["RoleID"].ToString());

                            entity.MenuID = new Guid(resultDataReader["MenuID"].ToString());

                            entity.MenuVal = Convert.ToInt32(resultDataReader["MenuVal"]);


                            list.Add(entity);
                        }
                    }
                }

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);

                return null;
            }
            return list;
        }
        /// <summary>
        /// LixSysRoleRight 表数据新增
        /// </summary>
        /// <param name="theObject">数据实体</param>

        public bool InsertSysRoleRight(Sys_RoleRight theObject, ref string errMsg) {
            try {
                SQLHelper helper = new SQLHelper(strCon);
                string insCmd = string.Format(" Insert into {0}(RoleID,MenuID,MenuVal) values('{1}','{2}',{3})"
                    , theObject.SaveTable, theObject.RoleID, theObject.MenuID, theObject.MenuVal);
                helper.ExecuteNonQuery(insCmd);


            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "InsertSysRoleRight", errMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 创建批量新增角色与菜单
        /// </summary>
        /// <param name="listRoleRight">权限角色</param>
        /// <returns>T:成功;F:失败</returns>

        public bool BatchInsertSysRoleRight(List<Sys_RoleRight> listRoleRight, List<Sys_RoleRight> listUnRoleRight, string groudId, ref string errMsg) {
            SQLHelper helper = new SQLHelper(strCon);

            try {
                Sys_RoleRight theRoleR = new Sys_RoleRight();
                helper.SQLTransBegin();

                foreach (Sys_RoleRight model in listUnRoleRight) {
                    string insCmd = string.Format("delete from Sys_RoleRight where RoleID='{0}' and MenuID='{1}'", model.RoleID, model.MenuID);
                    helper.ExecuteNonQuery(insCmd, true, null);

                }

                foreach (Sys_RoleRight model in listRoleRight) {
                    string insCmd = string.Format(@"if exists(select 1 from Sys_RoleRight where RoleID='{0}'  and MenuID='{1}')
                                                     update Sys_RoleRight set MenuVal={2} where RoleID='{0}'  and MenuID='{1}'
                                                  else
                                                    insert into Sys_RoleRight(RoleID,MenuVal,MenuID) values('{0}',{2},'{1}')
                                                 "
                       , model.RoleID, model.MenuID, model.MenuVal);

                    helper.ExecuteNonQuery(insCmd, true, null);

                }
                helper.SQLTransCommit();

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                helper.SQLRollback();
                return false;
            }
            return true;
        }
        /// <summary>
        /// 根据查询条件返回LixSysRight实体
        /// </summary>
        /// <param name="relation">条件</param>
        /// <returns>LixSysRight  实体</returns>

        public Sys_RoleRight GetSysRoleRightByRelation(string relation, ref string errMsg) {
            if (string.IsNullOrEmpty(relation))
                relation = "1=1";
            try {
                List<Sys_RoleRight> list = GetSysRoleRightList(relation, -1, -1, ref errMsg);
                if (list == null || list.Count < 1) {
                    return null;
                }
                return list[0];

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);

                return null;
            }

        }
        /// <summary>
        /// 根据查询条件返回最大的权限值 (ljx)
        /// </summary>
        /// <param name="relation">条件</param>
        /// <returns>最大的权限值</returns>

        public int GetRightValueByRelation(string relation, ref string errMsg) {
            if (string.IsNullOrEmpty(relation)) {
                relation = "1=1";
            }
            try {

                SQLHelper con = new SQLHelper(strCon);
                object obj = con.ExecuteScalar("select Max(MenuVal) MenuVal from Sys_RoleRight where " + relation);
                if (obj == null || Convert.IsDBNull(obj)) {
                    return 0;
                }
                return Convert.ToInt32(obj.ToString());
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //  Lixtech.Utility.Files.WriteOperateLog(logPath, "GetRightValueByRelation", errMsg);
                return 0;
            } finally {

            }
            //List<LixSysRoleRight> list = SelectLixSysRoleRightList(new string[] { relation }, null, string.Empty, -1, -1, string.Empty);
            //if (list == null || list.Count < 1) {
            //    return 0;
            //}
            //int menuVal = LixDbBusinessEntity.ListToShadow<LixSysRoleRight, Sys_RoleRight>(list)[0].MenuVal;
            //return menuVal > 0 ? menuVal : 0;
        }
        /// <summary>
        /// LixSysRoleRight 数据修改
        /// </summary>
        /// <param name="theObj">数据实体</param>
        /// <param name="fields">修改的字段</param>
        /// <param name="filters">修改条件</param>
        /// <param name="parmeters">条件参数</param>
        /// <returns>T:成功;F失败</returns>

        public bool ModifySysRoleRightByFilters(Sys_RoleRight theObj, string fields, string filters, ref string errMsg) {
            if (theObj.Id == null || theObj.Id == Guid.Empty) {
                return false;
            }
            string modcmd = string.Format(" Update Sys_RoleRight set {0}='{1}' where {2}", fields, theObj.MenuVal, filters);
            try {
                SQLHelper helper = new SQLHelper(strCon);
                helper.ExecuteNonQuery(modcmd);
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //   Lixtech.Utility.Files.WriteOperateLog(logPath, "ModifySysRoleRightByFilters", errMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 修改角色权限值
        /// </summary>
        /// <param name="theObj"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>

        public bool ModifySysRoleRight(Sys_RoleRight theObj, ref string errMsg) {
            if (theObj.Id == null || theObj.Id == Guid.Empty) {
                return false;
            }
            string modcmd = string.Format(" Update {0} set RoleID='{1}',MenuID='{2}',MenuVal={3}  where Id='{4}'"
                , theObj.SaveTable, theObj.RoleID, theObj.MenuID, theObj.MenuVal, theObj.Id);

            try {
                SQLHelper helper = new SQLHelper(strCon);
                helper.ExecuteNonQuery(modcmd);
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);

                return false;
            }
            return true;
        }
        /// <summary>
        /// 批量修改角色权限值
        /// </summary>
        /// <param name="objRole"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>

        public bool BatchSysRoleInsertOrModify(List<Sys_RoleRight> objRole, ref string errMsg) {
            SQLHelper helper = new SQLHelper(strCon);
            helper.SQLTransBegin();
            try {
                Sys_RoleRight theObj = new Sys_RoleRight();
                StringBuilder insCmd = new StringBuilder();


                foreach (Sys_RoleRight model in objRole) {
                    insCmd.AppendFormat("if not exists ( select 1 from {0} where RoleID ='{0}' and MenuID='{1}')", theObj.SelectTable, model.RoleID);
                    insCmd.AppendFormat("\r\n insert into {0}(RoleID,MenuID) values('{1}','{2}')", theObj.SaveTable, model.RoleID, model.MenuID);
                    helper.ExecuteNonQuery(insCmd.ToString(), true, null);
                }
                helper.SQLTransCommit();

            } catch (Exception ex) {
                helper.SQLRollback();
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                // Lixtech.Utility.Files.WriteOperateLog(logPath, "BatchSysRoleInsertOrModify", errMsg);
                return false;
            }
            return true;
        }
        #endregion

        #region 角色表、角色与用户表 操作
        /// <summary>
        /// 权限转授
        /// </summary>
        /// <param name="UserId">被转授</param>
        /// <param name="listUser">转授接收用户集合</param>
        /// <returns>T:成功;F:失败</returns>
        public bool CopyRight(string groupId, List<Sys_Role> GroupObjects, ref string errMsg) {

            if (GroupObjects != null && GroupObjects.Count > 0) {
                SQLHelper helper = new SQLHelper(strCon);
                try {
                    //根据要被授的用户获取到其所有的角色
                    List<Sys_RoleRight> roleRightObjects = GetSysRoleRightList(string.Format("RoleID='{0}'", groupId), -1, -1, ref errMsg);


                    helper.SQLTransBegin();


                    foreach (Sys_Role role in GroupObjects) {
                        string inseCmd = string.Format("delete from Sys_RoleRight where RoleID='{0}'", role.Id);
                        helper.ExecuteNonQuery(inseCmd, true, null);

                        foreach (Sys_RoleRight roleRight in roleRightObjects) {
                            string insCmd = string.Format("Insert into {0}(ID,RoleID,menuID,menuVal) Values('{1}','{2}','{3}',{4})"
                                       , roleRight.SaveTable, Guid.NewGuid(), role.Id, roleRight.MenuID, roleRight.MenuVal);
                            helper.ExecuteNonQuery(insCmd, true, null);
                        }
                    }
                    helper.SQLTransCommit();

                } catch (Exception ex) {
                    helper.SQLRollback();
                    errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                    // Lixtech.Utility.Files.WriteOperateLog(logPath, "CopyRight", errMsg);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// LixSysRole 表数据查询
        /// </summary>
        /// <param name="filters">过滤字段</param>
        /// <param name="parameters">过滤参数</param>
        /// <param name="fields">查询字段</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex"></param>
        /// <param name="sortExpression">排序</param>
        /// <returns>List Sys_RoleUser数据实体 </returns>

        public List<Sys_RoleUser> GetSysRoleList(string filters, ref string errMsg) {
            List<Sys_RoleUser> list = null;
            try {
                if (string.IsNullOrEmpty(filters)) filters = " 1=1";
                Sys_RoleUser entity = new Sys_RoleUser();

                list = new List<Sys_RoleUser>();
                string selCmd = string.Format("select * from Sys_RoleUser a (nolock) where {0} ", filters);

                SQLHelper helper = new SQLHelper(strCon);
                using (SqlDataReader resultDataReader = helper.ExecuteReader(selCmd, null)) {
                    if (resultDataReader != null && !resultDataReader.IsClosed) {

                        while (resultDataReader.Read()) {
                            entity = new Sys_RoleUser();

                            entity.Id = new Guid(resultDataReader["Id"].ToString());
                            entity.UserID = resultDataReader["UserID"].ToString();

                            entity.RoleID = new Guid(resultDataReader["RoleID"].ToString());


                            list.Add(entity);
                        }
                    }
                }


            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "GetSysRoleList", errMsg);
                return null;
            }
            return list;
        }
        public List<Sys_Role> GetSysGroupList(string filters, int pageSize, int pageIndex, ref int rCount, ref string errMsg) {
            errMsg = string.Empty;
            List<Sys_Role> list = new List<Sys_Role>();
            Sys_Role model = new Sys_Role();
            if (string.IsNullOrEmpty(filters)) filters = " 1=1";
            StringBuilder selCmd = new StringBuilder();
            string appLang = ConfigurationManager.AppSettings["LANG_KEY"];
            string lang = string.Empty;
            SQLHelper helper;
            if (pageSize != -1) {
                helper = new SQLHelper(strCon);
                object objRecord = helper.ExecuteScalar(string.Format("select count(1) from {0}  where {1} ", model.SelectTable, filters));
                rCount = Convert.ToInt32(objRecord);
            }
            if (pageSize != -1) {
                selCmd.Append(";with tmp_table as ( ");
                selCmd.Append("SELECT ");
                selCmd.Append("ROW_NUMBER() OVER(ORDER BY Id DESC) AS tmp_Id,");
                selCmd.Append("Id,RoleCode,Factory,RoleName,RoleName_En,IsUse,Remark,convert(nvarchar,convert(money,Ufts),2) as Ufts");
                selCmd.AppendFormat(" FROM {0}  ", model.SelectTable);
                selCmd.AppendFormat(" where {0} ", filters);
                selCmd.AppendFormat(")select * from tmp_table where tmp_Id between {0} and {1}", pageIndex, pageSize * (pageIndex));
            } else {
                selCmd.Append("SELECT ");
                selCmd.Append("Id,RoleCode,Factory,RoleName,RoleName_En,IsUse,Remark,convert(nvarchar,convert(money,Ufts),2) as Ufts");
                selCmd.AppendFormat(" FROM {0}  ", model.SelectTable);
                selCmd.AppendFormat(" where {0} ", filters);
                selCmd.Append(" ORDER BY Id");
            }
            try {
                helper = new SQLHelper(strCon);
                using (SqlDataReader reader = helper.ExecuteReader(selCmd.ToString())) {
                    while (reader.Read()) {
                        model = new Sys_Role();
                        model.Id = new Guid(Convert.ToString(reader["Id"]));
                        if (!Convert.IsDBNull(reader["RoleCode"]))
                            model.RoleCode = Convert.ToString(reader["RoleCode"]);
                        if (!Convert.IsDBNull(reader["RoleName"]))
                            model.RoleName = Convert.ToString(reader["RoleName"]);
                        if (!Convert.IsDBNull(reader["RoleName_En"]))
                            model.RoleName_En = Convert.ToString(reader["RoleName_En"]);
                        if (!Convert.IsDBNull(reader["IsUse"]))
                            model.IsUse = Convert.ToBoolean(reader["IsUse"]);
                        if (!Convert.IsDBNull(reader["Remark"]))
                            model.Remark = Convert.ToString(reader["Remark"]);

                        model.Factory = Convert.ToString(reader["Factory"]);

                        //if (!Convert.IsDBNull(reader["Ufts"]))
                        //    model.Ufts = Convert.ToString(reader["Ufts"]);

                        list.Add(model);
                    }
                }
            } catch (Exception ex) {
                errMsg = ex.Message;
            }
            return list;
        }

        /// <summary>
        /// LixSysGroup 表数据查询
        /// </summary>
        /// <param name="filters">过滤字段</param>
        /// <param name="parameters">过滤参数</param>
        /// <param name="fields">查询字段</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex"></param>
        /// <param name="sortExpression">排序</param>
        /// <returns>List Sys_Role数据实体 </returns>

        public List<Sys_Role> GetSysGroupList(string filters, ref string errMsg) {
            List<Sys_Role> list = null;
            try {
                Sys_Role entity = new Sys_Role();
                if (string.IsNullOrEmpty(filters)) filters = " 1=1";

                list = new List<Sys_Role>();
                SQLHelper helper = new SQLHelper(strCon);
                string selCmd = string.Format("select * from {0} (nolock) where {1}"
                    //, System.Threading.Thread.CurrentThread.CurrentCulture.Name == "EN" ? "RoleName_En" : "RoleName"
                    , entity.SelectTable, filters);

                using (SqlDataReader resultDataReader = helper.ExecuteReader(selCmd)) {
                    if (resultDataReader != null && !resultDataReader.IsClosed) {

                        while (resultDataReader.Read()) {
                            entity = new Sys_Role();

                            entity.Id = new Guid(resultDataReader["Id"].ToString());
                            entity.RoleCode = Convert.ToString(resultDataReader["RoleCode"]);
                            entity.RoleName = Convert.ToString(resultDataReader["RoleName"]);
                            entity.RoleName_En = Convert.ToString(resultDataReader["RoleName_En"]);
                            entity.Remark = Convert.ToString(resultDataReader["Remark"]);
                            entity.IsUse = Convert.ToBoolean(resultDataReader["IsUse"]);
                            entity.Factory = Convert.ToString(resultDataReader["Factory"]);
                            list.Add(entity);
                        }
                    }

                }

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);

                //Lixtech.Utility.Files.WriteOperateLog(logPath, "GetSysGroupList", errMsg);
                return null;
            }
            return list;
        }
        /// <summary>
        /// 创建批量新增用户与角色记录
        /// </summary>
        /// <param name="groupObj">角色组实体</param>
        /// <param name="roleObjs">角色实体</param>
        /// <returns>T:成功;F:失败</returns>

        public bool BatchInsertUserAndRoleList(Sys_User theObject, List<Sys_RoleUser> roleObjects, ref string errMsg) {
            SQLHelper helper = new SQLHelper(strCon);
            helper.SQLTransBegin();
            try {
                StringBuilder insCmd = new StringBuilder();
                insCmd.AppendFormat("Insert into {0}(Password,Address,Email,SecurityEmail,Phone,Cellphone,IdNumber", theObject.SaveTable);
                insCmd.Append(",IsAdmin,IsUseStop,LoginID,Id,LoginName,Factory,DeptCode,JobTitle)");
                insCmd.AppendFormat("values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}') "
               , theObject.Password, theObject.Address, theObject.Email
               , theObject.SecurityEmail, theObject.Phone, theObject.Cellphone
               , theObject.IdNumber, theObject.IsAdmin, theObject.IsUseStop, theObject.LoginID, theObject.Id, theObject.LoginName, theObject.Factory, theObject.DeptCode, theObject.JobTitle);
                helper.ExecuteNonQuery(insCmd.ToString(), true, null);
                insCmd = new StringBuilder();
                for (int i = 0; i < roleObjects.Count; i++) {
                    insCmd.AppendFormat("insert into {0}(RoleID,UserID)Values('{1}','{2}')"
                            , roleObjects[i].SaveTable, roleObjects[i].RoleID, roleObjects[i].UserID);
                    helper.ExecuteNonQuery(insCmd.ToString(), true, null);
                }
                helper.SQLTransCommit();

            } catch (Exception ex) {
                helper.SQLRollback();
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //  Lixtech.Utility.Files.WriteOperateLog(logPath, "BatchInsertUserAndRoleList", errMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// LixSysGroup 表数据新增
        /// </summary>
        /// <param name="theObject">数据实体</param>

        public bool InsertSysGroup(Sys_Role theObject, ref string errMsg) {
            try {
                SQLHelper helper = new SQLHelper(strCon);
                string inscmd = string.Format("Insert into {0}(RoleCode,RoleName,RoleName_En,Remark,IsUse,Factory)values('{1}','{2}','{3}','{4}','{5}','{6}')"
                                                , theObject.SaveTable, theObject.RoleCode, theObject.RoleName, theObject.RoleName_En, theObject.Remark, theObject.IsUse, theObject.Factory);
                helper.ExecuteNonQuery(inscmd);

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //  Lixtech.Utility.Files.WriteOperateLog(logPath, "InsertSysGroup", errMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// LixSysRightValue 表数据查询
        /// </summary>
        /// <param name="filters">过滤字段</param>
        /// <param name="parameters">过滤参数</param>
        /// <param name="fields">查询字段</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex"></param>
        /// <param name="sortExpression">排序</param>
        /// <returns>List Sys_RightValue数据实体 </returns>

        public List<Sys_RightValue> GetSysRightValueList(string filters, ref string errMsg) {
            List<Sys_RightValue> list = null;
            if (string.IsNullOrEmpty(filters)) {
                filters = " 1=1";
            }
            Sys_RightValue entity = new Sys_RightValue();

            try {

                list = new List<Sys_RightValue>();
                string selCmd = string.Format("select * from {0} (nolock) where {1}  order by RightValue"
                    //  , System.Threading.Thread.CurrentThread.CurrentCulture.Name == "EN" ? "RightName_En" : "RightName"
                    , entity.SaveTable, filters);
                SQLHelper helper = new SQLHelper(strCon);
                using (SqlDataReader resultDataReader = helper.ExecuteReader(selCmd)) {
                    while (resultDataReader.Read()) {
                        entity = new Sys_RightValue();

                        entity.Id = new Guid(resultDataReader["Id"].ToString());
                        entity.MenuID = new Guid(resultDataReader["MenuID"].ToString());
                        entity.IsUse = Convert.ToBoolean(resultDataReader["IsUse"]);
                        entity.RightName = resultDataReader["RightName"].ToString();
                        entity.RightName_En = Convert.ToString(resultDataReader["RightName_En"]);
                        entity.RightValue = Convert.ToInt32(resultDataReader["RightValue"]);
                        entity.Factory = Convert.ToString(resultDataReader["Factory"]);

                        list.Add(entity);
                    }
                }
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //   Lixtech.Utility.Files.WriteOperateLog(logPath, "GetSysRightValueList", errMsg);
                return null;
            }
            return list;
        }
        /// <summary>
        /// 获取权限值
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>

        public List<Sys_RightValue> GetSysRightValueListByRightValueDesc(string filters, ref string errMsg) {
            List<Sys_RightValue> list = null;

            if (string.IsNullOrEmpty(filters)) {
                filters = " 1=1";
            }
            Sys_RightValue entity = new Sys_RightValue();
            try {

                list = new List<Sys_RightValue>();
                string selCmd = string.Format("select ROW_NUMBER() over(order by RightValue desc) RightID,* from {0} where {1} order by RightValue desc"
                    // , System.Threading.Thread.CurrentThread.CurrentCulture.Name == "EN" ? "RightName_En" : "RightName"
                    , entity.SaveTable, filters);
                SQLHelper helper = new SQLHelper(strCon);
                using (SqlDataReader resultDataReader = helper.ExecuteReader(selCmd)) {
                    while (resultDataReader.Read()) {
                        entity = new Sys_RightValue();
                        entity.RightID = Convert.ToInt32(resultDataReader["RightID"]);
                        entity.Id = new Guid(resultDataReader["Id"].ToString());
                        entity.RightName = resultDataReader["RightName"].ToString();
                        entity.RightName_En = Convert.ToString(resultDataReader["RightName_En"]);
                        entity.RightValue = Convert.ToInt32(resultDataReader["RightValue"]);
                        entity.Factory = Convert.ToString(resultDataReader["Factory"]);

                        list.Add(entity);
                    }
                }

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                // Lixtech.Utility.Files.WriteOperateLog(logPath, "GetSysRightValueListByRightValueDesc", errMsg);
                return null;
            }
            return list;
        }
        /// <summary>
        /// 创建批量新增角色组与角色记录
        /// </summary>
        /// <param name="groupObject">角色组实体</param>
        /// <param name="roleObjects">角色实体集体</param>
        /// <returns>T:成功;F:失败</returns>

        public bool BatchUpdateGroupAndRoleList(Sys_Role GroupObj, List<Sys_RoleUser> roleObjects, ref string errMsg) {
            SQLHelper helper = new SQLHelper(strCon);
            helper.SQLTransBegin();
            try {
                Sys_RoleUser theRoleU = new Sys_RoleUser();
                string insCmd = string.Empty;
                if(GroupObj != null) {
                    string modCmd = string.Format("Update {0} set [RoleCode]='{1}',[RoleName]='{2}',[Remark]='{3}',[RoleName_En]='{4}',IsUse='{5}',Factory='{6}' where [ID]='{7}'"
                                                , GroupObj.SaveTable, GroupObj.RoleCode, GroupObj.RoleName, GroupObj.Remark, GroupObj.RoleName_En, GroupObj.IsUse, GroupObj.Factory, GroupObj.Id);
                    helper.ExecuteNonQuery(modCmd, true, null);
                }
                //string delCmd = string.Format("DELETE FROM {0} WHERE [RoleID]='{1}'", theRoleU.SaveTable, GroupObj.Id.ToString());
                //helper.ExecuteNonQuery(delCmd, true, null);

                foreach (Sys_RoleUser roleObject in roleObjects) {
                    insCmd = string.Format("DELETE Sys_RoleUser where Id='{0}'", roleObject.RoleID);
                    helper.ExecuteNonQuery(insCmd, true, null);
                }

                helper.SQLTransCommit();
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                helper.SQLRollback();
                // Lixtech.Utility.Files.WriteOperateLog(logPath, "BatchUpdateGroupAndRoleList", errMsg);
                return false;


            }
            return true;
        }
        /// <summary>
        /// 创建批量新增角色组与角色记录
        /// </summary>
        /// <param name="groupObj">角色组实体</param>
        /// <param name="roleObjs">角色实体</param>
        /// <returns>T:成功;F:失败</returns>

        public bool BatchInsertGroupAndRoleList(Sys_Role GroupObj, List<Sys_RoleUser> roleObjects, ref string errMsg) {
            SQLHelper helper = new SQLHelper(strCon);

            try {
                //  if (roleObjects != null && roleObjects.Count > 0) {
                helper.SQLTransBegin();
                string insCmd = string.Empty;
                if (GroupObj != null) {
                    insCmd = string.Format("Insert into {0}([ID],[RoleCode],[RoleName],[RoleName_En],IsUse,Factory)values('{1}','{2}','{3}','{4}','{5}','{6}')"
                                                 , GroupObj.SaveTable, GroupObj.Id, GroupObj.RoleCode, GroupObj.RoleName, GroupObj.RoleName_En, GroupObj.IsUse, GroupObj.Factory);
                    helper.ExecuteNonQuery(insCmd, true, null);
                }
                foreach (Sys_RoleUser roleObject in roleObjects) {
                    insCmd = string.Format(" insert into {0}(ID,RoleID,UserID)values('{1}','{2}','{3}')"
                        , roleObject.SaveTable, roleObject.Id, roleObject.RoleID, roleObject.UserID);
                    helper.ExecuteNonQuery(insCmd, true, null);
                }
                helper.SQLTransCommit();

                //  }

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                helper.SQLRollback();
                // Lixtech.Utility.Files.WriteOperateLog(logPath, "BatchInsertGroupAndRoleList", errMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 批量删除角色集
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>

        public bool BatchDeleteSysGroup(List<string> groupId, ref string errMsg) {
            SQLHelper helper = new SQLHelper(strCon);
            helper.SQLTransBegin();
            try {
                Sys_RoleRight theRoleR = new Sys_RoleRight();
                Sys_RoleUser theRoleU = new Sys_RoleUser();
                Sys_Role theRole = new Sys_Role();
                foreach (string Id in groupId) {
                    string delcmd = string.Format("DELETE FROM {0} where [RoleID]='{1}'", theRoleR.SelectTable, Id);
                    helper.ExecuteNonQuery(delcmd, true, null);

                    delcmd = string.Format("DELETE FROM {0} where [RoleID]='{1}'", theRoleU.SelectTable, Id);
                    helper.ExecuteNonQuery(delcmd, true, null);

                    delcmd = string.Format("Delete FROM {0} where [Id]='{1}'", theRole.SelectTable, Id);
                    helper.ExecuteNonQuery(delcmd, true, null);
                }
                helper.SQLTransCommit();

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                helper.SQLRollback();
                // Lixtech.Utility.Files.WriteOperateLog(logPath, "BatchDeleteSysGroup", errMsg);
                return false;
            }
            return true;

        }
        #endregion

        #region 系统按钮
        /// <summary>
        /// 删除系统菜单节点
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>

        public bool DeleteSys_FunButton(Sys_FunButton theObject, ref string errMsg) {
            SQLHelper dbHelper = new SQLHelper(strCon);
            try {



                string selCmd = string.Format("Delete {0} where Id='{1}'", theObject.SelectTable, theObject.Id);
                dbHelper.ExecuteNonQuery(selCmd);


                return true;

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);

                // Lixtech.Utility.Files.WriteOperateLog(logPath, "DeleteFunModel", errMsg);
                return false;
            }
        }

        /// <summary>
        /// 新增系统菜单
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>

        public bool InsertSys_FunButton(Sys_FunButton theObject, ref string errMsg) {
            SQLHelper helper = new SQLHelper(strCon);
            try {
                StringBuilder insCmd = new StringBuilder();
                //insCmd.AppendFormat("Insert into {0}", theObject.SaveTable);
                //insCmd.AppendFormat("(Id,MenuCode,MenuName,AssemblyPath,RightValue,MenuParentCode,FunID,AssemblyType,ClassName,AssemblyUrl)");
                //insCmd.AppendFormat("values('{0}','{1}','{2}','{3}',{4},'{5}','{6}',{7},'{8}','{9}') "
                //                            , theObject.Id, theObject.MenuCode, theObject.MenuName, theObject.AssemblyPath
                //                            , theObject.RightValue, theObject.MenuParentCode, theObject.FunID
                //                            , theObject.AssemblyType, theObject.ClassName, theObject.AssemblyUrl);
                //helper.ExecuteNonQuery(insCmd.ToString());

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "InsertSysMenu", errMsg);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 修改系统菜单节点
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>

        public bool ModifySys_FunButton(Sys_FunButton theObject, ref string errMsg) {
            try {
                SQLHelper helper = new SQLHelper(strCon);

                //string selCmd = string.Format("Update {0} set MenuName='{1}',AssemblyPath='{2}',RightValue={3},AssemblyType={4},className='{5}',AssemblyUrl='{6}' where Id='{7}' ",
                //                            theObject.SaveTable, theObject.MenuName, theObject.AssemblyPath,
                //                            theObject.RightValue, theObject.AssemblyType, theObject.ClassName, theObject.AssemblyUrl, theObject.Id);
                //  helper.ExecuteNonQuery(selCmd);

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "ModifySysMenu", errMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取系统菜单节点表信息 返回DataTable 
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>

        public DataTable GetSys_FunButton(string filters, ref string errMsg) {

            if (string.IsNullOrEmpty(filters)) filters = " 1=1";
            string tableName = new Sys_Menu().SelectTable;


            DataTable list = null;

            try {

                list = new DataTable();
                SQLHelper helper = new SQLHelper(strCon);
                string selcmd = string.Format("select *,(select cButtonKey from Sys_Button where Id={0}.btnID) as cButtonKey,(select {0} as cButtonName from Sys_Button where Id={0}.btnID)) as cCaption from {0} where {1} order by iOrder"
                      , tableName, System.Threading.Thread.CurrentThread.CurrentCulture.Name == "EN" ? "cButtonName_En" : "cButtonName", filters);
                list = helper.ExecuteDataSet(selcmd).Tables[0];

                return list;
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "GetSysMenu", errMsg);
                return null;
            }

        }
        /// <summary>
        /// 获取系统表菜单节点信息.返回 List
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>

        public List<Sys_FunButton> GetSys_FunButtonList(string filters, ref string errMsg) {
            if (string.IsNullOrEmpty(filters)) filters = " 1=1";
            string tableName = new Sys_FunButton().SelectTable;

            List<Sys_FunButton> list = null;
            try {

                list = new List<Sys_FunButton>();
                SQLHelper helper = new SQLHelper(strCon);
                string selcmd = string.Format("select *,(select cButtonKey from Sys_Button where Id={0}.btnID) as cButtonKey,(select {1} cButtonName from Sys_Button where Id={0}.btnID) as cCaption,(select iStatus from Sys_Button where Id={0}.btnID) as iStatus from {0} where {2} and bActive=1 order by iOrder"
                    , tableName, System.Threading.Thread.CurrentThread.CurrentCulture.Name == "EN" ? "cButtonName_En" : "cButtonName", filters);
                using (SqlDataReader resultDataReader = helper.ExecuteReader(selcmd)) {
                    while (resultDataReader.Read()) {
                        Sys_FunButton entity = new Sys_FunButton();
                        entity.Id = new Guid(resultDataReader["Id"].ToString());
                        entity.cButtonKey = resultDataReader["cButtonKey"].ToString();
                        entity.bActive = Convert.ToBoolean(resultDataReader["bActive"]);
                        entity.Caption = resultDataReader["cCaption"].ToString();
                        entity.cButtonType = resultDataReader["cButtonType"].ToString();
                        entity.iStatus = Convert.ToInt32(resultDataReader["iStatus"]);
                        entity.cClassName = resultDataReader["cClassName"].ToString();
                        entity.cHotKey = resultDataReader["cHotKey"].ToString();
                        //    entity.cVoucherKey = Convert.ToString(resultDataReader["cVoucherKey"]);
                        entity.cFormKey = Convert.ToString(resultDataReader["cVoucherKey"]);
                        entity.iOrder = Convert.ToString(resultDataReader["iOrder"]);
                        entity.MenuID = new Guid(Convert.ToString(resultDataReader["MenuID"]));
                        entity.RightValueID = new Guid(Convert.ToString(resultDataReader["RightValueID"]));
                        //entity.cKeyBefore = Convert.ToString(resultDataReader["cKeyBefore"]);
                        list.Add(entity);
                    }
                }

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                //Lixtech.Utility.Files.WriteOperateLog(logPath, "GetSysMenuList", errMsg);
                return null;
            }
            return list;
        }
        #endregion

        /// <summary>
        /// 根据XX字段及表查询相对应的数据
        /// </summary>
        /// <param name="fileds"></param>
        /// <param name="tableName"></param>
        /// <param name="filters"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>

        public object GetFiledsByFilters(string fileds, string tableName, string filters, ref string errMsg) {
            try {
                if (string.IsNullOrEmpty(filters)) filters = " 1=1";
                SQLHelper dbHelper = new SQLHelper(strCon);
                string selCmd = string.Format("select {0} from {1} (nolock) where {2}", fileds, tableName, filters);
                return dbHelper.ExecuteScalar(selCmd);

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                // Lixtech.Utility.Files.WriteOperateLog(logPath, "GetFiledsByFilters", errMsg);
                return null;
            }
        }
        /// <summary>
        /// 根据表自动修改某表的数据
        /// </summary>
        /// <param name="fileds"></param>
        /// <param name="tableName"></param>
        /// <param name="filters"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool ModifyFiledsByFilters(string fileds, string tableName, string filters, ref string errMsg) {
            try {
                SQLHelper dbHelper = new SQLHelper(strCon);
                string selCmd = string.Format("Update {0} set {1} where {2}", tableName, fileds, filters);
                dbHelper.ExecuteNonQuery(selCmd);

                return true;
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
                // Lixtech.Utility.Files.WriteOperateLog(logPath, "ModifyFiledsByFilters", errMsg);
                return false;
            }
        }

        public void DeleteFactory(Sys_Factory model, ref string errMsg) {
            try {
                SQLHelper helper = new SQLHelper(strCon);
                string insCmd = string.Format(" Delete from Sys_Factory where Iden={0}", model.Iden);
                helper.ExecuteNonQuery(insCmd);

            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
            }
        }

        public void UpdateFactory(Sys_Factory model, ref string errMsg) {
            try {
                SQLHelper helper = new SQLHelper(strCon);
                string insCmd = string.Format(" Update Sys_Factory set FCode='{0}',FName='{1}',IsUse='{2}' where Iden={3}"
                    , model.FCode, model.FName, model.IsUse, model.Iden);
                helper.ExecuteNonQuery(insCmd);


            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
            }
        }

        public void AddFactory(Sys_Factory model, ref string errMsg) {
            try {
                SQLHelper helper = new SQLHelper(strCon);
                string insCmd = string.Format(" Insert into Sys_Factory(FCode,FName,IsUse) values('{0}','{1}','{2}')"
                    , model.FCode, model.FName, model.IsUse);
                helper.ExecuteNonQuery(insCmd);


            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty).Replace("\r\n", string.Empty);
            }
        }
        public bool ExecuteSQL(string sql, ref string errMsg) {
            return ExecuteSQL(sql, null, false, ref errMsg);
        }

        public bool ExecuteSQL(string sql, SQLHelper helper, bool isTran, ref string errMsg) {
            errMsg = string.Empty;
            try {
                if (helper == null)
                    helper = new SQLHelper(strCon);
                helper.ExecuteNonQuery(sql, isTran);
                return true;
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty);
            }
            return false;
        }


    }
}
