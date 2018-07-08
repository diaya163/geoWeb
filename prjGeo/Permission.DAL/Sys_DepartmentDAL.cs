using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Esquel.WebBlock;
using Permission.Model;
using Esquel.Utility;
using Esquel.BaseManager;
namespace Permission.DAL {
    /// <summary>
    /// Sys_DepartmentDAL
    /// </summary>
    public partial class Sys_DepartmentDAL {
        private string conStr = string.Empty;
        private SQLHelper helper = null;
        public Sys_DepartmentDAL(string serverName) {
            string serverIP = ConfigurationManager.AppSettings["ServerIP"];
            conStr = DESEncrypt.Decrypt(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings[serverName]].ConnectionString);
            conStr = string.Format(conStr, string.IsNullOrEmpty(serverIP) ? "." : serverIP);
        }
        public Sys_DepartmentDAL() {
            string serverIP = ConfigurationManager.AppSettings["ServerIP"];
            conStr = DESEncrypt.Decrypt(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DefaultConnectionStringName"]].ConnectionString);
            conStr = string.Format(conStr, string.IsNullOrEmpty(serverIP) ? "." : serverIP);
        }

        /// <summary>
        /// 设置数据参数
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public List<SqlParameter> SetSqlParameter(Sys_Department model) {
            List<SqlParameter> lstParas = new List<SqlParameter>();
            SqlParameter pars = null;
            pars = new SqlParameter("@DeptCode", SqlDbType.VarChar);
            pars.Value = model.DeptCode;
            lstParas.Add(pars);
            pars = new SqlParameter("@DeptName", SqlDbType.NVarChar);
            pars.Value = model.DeptName;
            lstParas.Add(pars);
            pars = new SqlParameter("@DeptCodeParent", SqlDbType.VarChar);
            pars.Value = model.DeptCodeParent;
            lstParas.Add(pars);
            pars = new SqlParameter("@IsUse", SqlDbType.Bit);
            pars.Value = Convert.ToInt32(model.IsUse);
            lstParas.Add(pars);
            return lstParas;
        }


        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool IsExists(string filters, ref string errMsg) {
            errMsg = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select count(1) from {0}  (Nolock)  ", new Sys_Department().SelectTable);
            strSql.AppendFormat(" where {0} ", filters);
            try {
                helper = new SQLHelper(conStr);
                object obj = helper.ExecuteScalar(strSql.ToString());
                return Convert.ToInt32(obj) > 0 ? true : false;
            } catch (Exception ex) {
                errMsg = ex.Message;
            }
            return false;
        }


        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="rCount">总页数</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public DataTable GetDataTable(string filters, int pageSize, int pageIndex, ref int rCount, ref string errMsg) {
            errMsg = string.Empty;
            if (string.IsNullOrEmpty(filters)) filters = " 1=1";
            StringBuilder selCmd = new StringBuilder();
            if (pageSize != -1) {
                helper = new SQLHelper(conStr);
                object objRecord = helper.ExecuteScalar(string.Format("select count(1) from {0}  (Nolock)  where {1} ", new Sys_Department().SelectTable, filters));
                rCount = Convert.ToInt32(objRecord);
            }
            if (pageSize != -1) {
                selCmd.Append(";with tmp_table as ( ");
                selCmd.Append("SELECT ");
                selCmd.Append("ROW_NUMBER() OVER(ORDER BY ID DESC) AS tmp_Id,");
                selCmd.Append("ID,DeptCode,DeptName,DeptCodeParent,IsUse");
                selCmd.AppendFormat(" FROM {0}  (Nolock)  ", new Sys_Department().SelectTable);
                selCmd.AppendFormat(" where {0} ", filters);
                selCmd.AppendFormat(")select * from tmp_table where tmp_Id between {0} and {1}", pageSize * pageIndex + 1, pageSize * (pageIndex + 1));
            } else {
                selCmd.Append("SELECT ");
                selCmd.Append("ID,DeptCode,DeptName,DeptCodeParent,IsUse");
                selCmd.AppendFormat(" FROM {0}  (Nolock)  ", new Sys_Department().SelectTable);
                selCmd.AppendFormat(" where {0} ", filters);
            }
            try {
                helper = new SQLHelper(conStr);
                return helper.ExecuteDataTable(selCmd.ToString());
            } catch (Exception ex) {
                errMsg = ex.Message;
            }
            return null;
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public DataTable GetDataTable(string filters, ref string errMsg) {
            int rCount = 0;
            return GetDataTable(filters, -1, -1, ref rCount, ref errMsg);
        }


        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="orderBy">字段排序</param>
        /// <param name="TopNo">前几行</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="rCount">总页数</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<Sys_Department> GetList(string filters, string orderBy, int TopNo, int pageSize, int pageIndex, ref int rCount, ref string errMsg) {
            errMsg = string.Empty;
            IList<Sys_Department> list = new List<Sys_Department>();
            Sys_Department model = new Sys_Department();
            if (string.IsNullOrEmpty(filters)) filters = " 1=1";
            StringBuilder selCmd = new StringBuilder();
            if (pageSize != -1) {
                helper = new SQLHelper(conStr);
                object objRecord = helper.ExecuteScalar(string.Format("select count(1) from {0}  (Nolock)  where {1} ", model.SelectTable, filters));
                rCount = Convert.ToInt32(objRecord);
            }
            if (pageSize != -1) {
                selCmd.Append(";with tmp_table as ( ");
                if (TopNo > 0) {
                    selCmd.AppendFormat("SELECT Top {0}", TopNo);
                } else {
                    selCmd.Append("SELECT ");
                }
                if (string.IsNullOrEmpty(orderBy)) {
                    selCmd.Append("ROW_NUMBER() OVER(ORDER BY ID DESC) AS tmp_Id,");
                } else {
                    selCmd.AppendFormat("ROW_NUMBER() OVER(ORDER BY {0}) AS tmp_Id,", orderBy);
                }
                selCmd.Append("ID,DeptCode,DeptName,DeptCodeParent,IsUse");
                selCmd.AppendFormat(" FROM {0}  (Nolock)  ", model.SelectTable);
                selCmd.AppendFormat(" where {0} ", filters);
                selCmd.AppendFormat(")select * from tmp_table where tmp_Id between {0} and {1}", pageSize * pageIndex + 1, pageSize * (pageIndex + 1));
            } else {
                if (TopNo > 0) {
                    selCmd.AppendFormat("SELECT Top {0} ", TopNo);
                } else {
                    selCmd.Append("SELECT ");
                }
                selCmd.Append("ID,DeptCode,DeptName,DeptCodeParent,IsUse");
                selCmd.AppendFormat(" FROM {0}  (Nolock)  ", model.SelectTable);
                selCmd.AppendFormat(" where {0} ", filters);
                if (string.IsNullOrEmpty(orderBy)) {
                    selCmd.Append(" ORDER BY ID");
                } else {
                    selCmd.AppendFormat(" ORDER BY {0} ", orderBy);
                }
            }
            try {
                helper = new SQLHelper(conStr);
                using (SqlDataReader reader = helper.ExecuteReader(selCmd.ToString())) {
                    while (reader.Read()) {
                        model = new Sys_Department();
                        model.ID = new Guid(Convert.ToString(reader["ID"]));
                        model.DeptCode = Convert.ToString(reader["DeptCode"]);
                        model.DeptName = Convert.ToString(reader["DeptName"]);
                        model.DeptCodeParent = Convert.ToString(reader["DeptCodeParent"]);
                        model.IsUse = Convert.ToBoolean(reader["IsUse"]);

                        list.Add(model);
                    }
                }
            } catch (Exception ex) {
                errMsg = ex.Message;
            }
            return list;
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="orderBy">字段排序</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<Sys_Department> GetList(string filters, string orderBy, int pageSize, int pageIndex, ref string errMsg) {
            return GetList(filters, orderBy, -1, pageSize, pageIndex, ref errMsg);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="TopNo">Top 前几条</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<Sys_Department> GetList(string filters, int pageSize, int pageIndex, ref string errMsg) {
            return GetList(filters, string.Empty, -1, pageSize, pageIndex, ref errMsg);
        }


        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="orderBy">字段排序</param>
        /// <param name="TopNo">前几行</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<Sys_Department> GetList(string filters, string orderBy, int TopNo, ref string errMsg) {
            return GetList(filters, orderBy, TopNo, -1, -1, ref errMsg);
        }

        /// <summary>
        /// 自定义排序字段进行刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="orderBy">字段排序</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<Sys_Department> GetList(string filters, string orderBy, ref string errMsg) {
            return GetList(filters, orderBy, -1, -1, -1, ref errMsg);
        }
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="TopNo">Top 前几条</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<Sys_Department> GetList(string filters, int TopNo, ref string errMsg) {
            return GetList(filters, string.Empty, TopNo, -1, -1, ref errMsg);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<Sys_Department> GetList(string filters, ref string errMsg) {
            return GetList(filters, string.Empty, -1, -1, -1, ref errMsg);
        }



        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="orderBy">字段排序</param>
        /// <param name="TopNo">前几行</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="rCount">总页数</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<Sys_Department> GetList(string filters, string orderBy, int TopNo, int pageSize, int pageIndex, ref string errMsg) {
            errMsg = string.Empty;
            Sys_Department model = new Sys_Department();
            if (string.IsNullOrEmpty(filters)) filters = " 1=1";
            StringBuilder selCmd = new StringBuilder();
            string cacheKey = "[" + model.SelectTable + "][List](" + filters.Trim() + ")";
            IList<Sys_Department> list = HttpRuntime.Cache[cacheKey] as List<Sys_Department>;
            try {
                if (pageSize != -1) {
                    selCmd.Append(";with tmp_table as ( ");
                    if (TopNo > 0) {
                        selCmd.AppendFormat("SELECT Top {0}", TopNo);
                    } else {
                        selCmd.Append("SELECT ");
                    }
                    if (string.IsNullOrEmpty(orderBy)) {
                        selCmd.Append("ROW_NUMBER() OVER(ORDER BY ID DESC) AS tmp_Id,");
                    } else {
                        selCmd.AppendFormat("ROW_NUMBER() OVER(ORDER BY {0}) AS tmp_Id,", orderBy);
                    }
                    selCmd.Append("ID,DeptCode,DeptName,DeptCodeParent,IsUse");
                    selCmd.AppendFormat(" FROM {0}  (Nolock)  ", model.SelectTable);
                    selCmd.AppendFormat(" where {0} ", filters);
                    selCmd.AppendFormat(")select * from tmp_table where tmp_Id between {0} and {1}", pageSize * pageIndex + 1, pageSize * (pageIndex + 1));
                    selCmd.AppendFormat(";select count(1) as TotalCount from {0}  (Nolock)  where {1} ", model.SelectTable, filters);
                } else {
                    if (TopNo > 0) {
                        selCmd.AppendFormat("SELECT Top {0} ", TopNo);
                    } else {
                        selCmd.Append("SELECT ");
                    }
                    selCmd.Append("ID,DeptCode,DeptName,DeptCodeParent,IsUse");
                    selCmd.AppendFormat(" FROM {0}  (Nolock)  ", model.SelectTable);
                    selCmd.AppendFormat(" where {0} ", filters);
                    if (string.IsNullOrEmpty(orderBy)) {
                        selCmd.Append(" ORDER BY ID");
                    } else {
                        selCmd.AppendFormat(" ORDER BY {0} ", orderBy);
                    }
                }
                if (list == null) {
                    helper = new SQLHelper(conStr);
                    list = helper.SelectReader<Sys_Department>(typeof(Sys_Department), selCmd.ToString());
                    if (list != null) {
                        LixWebUiUtil.InsertRuntimeCash(cacheKey, list, LixWebUiUtil.CacheAbsoluteExpiration);
                    }

                }
            } catch (Exception ex) {
                errMsg = ex.Message;
            }
            return list;
        }


        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="helper">数据库类</param>
        /// <param name="isTran">是否为事务</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Add(Sys_Department model, SQLHelper helper, bool isTran, ref string errMsg) {
            errMsg = string.Empty;
            StringBuilder insCmd = new StringBuilder();
            insCmd.AppendFormat("insert into {0}", model.SaveTable);
            insCmd.Append("(ID,DeptCode,DeptName,DeptCodeParent,IsUse,Factory)");
            insCmd.Append("VALUES(@ID,@DeptCode,@DeptName,@DeptCodeParent,@IsUse,@Factory)");
            
            if (helper == null) {
                helper = new SQLHelper(conStr);
            }
            try {
                object obj = helper.ExecuteScalar(insCmd.ToString(), isTran, model);

                if (string.IsNullOrEmpty(model.SystemCode))
                    LixWebUiUtil.ClearRuntimeCash(model.SaveTable);
                else
                    LixWebUiUtil.ClearRuntimeCash(model.SaveTable + "@" + model.SystemCode);
                return Convert.ToInt32(obj);
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty);
            }
            return -1;
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Add(Sys_Department model, ref string errMsg) {
            return Add(model, null, false, ref errMsg);
        }


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="helper">数据库类</param>
        /// <param name="isTran">是否为事务</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool Update(Sys_Department model, SQLHelper helper, bool isTran, ref string errMsg) {
            errMsg = string.Empty;
            StringBuilder updCmd = new StringBuilder();
            updCmd.AppendFormat("Update {0} Set ", model.SaveTable);
            updCmd.Append("DeptCode=@DeptCode");
            updCmd.Append(",DeptName=@DeptName");
            updCmd.Append(",Factory=@Factory");
            updCmd.Append(",DeptCodeParent=@DeptCodeParent");
            updCmd.Append(",IsUse=@IsUse");

            updCmd.Append(" where ID=@ID");

            if (helper == null) {
                helper = new SQLHelper(conStr);
            }
            try {
                int row = helper.ExecuteNonQuery(updCmd.ToString(), isTran, model);
                if (string.IsNullOrEmpty(model.SystemCode))
                    LixWebUiUtil.ClearRuntimeCash(model.SaveTable);
                else
                    LixWebUiUtil.ClearRuntimeCash(model.SaveTable + "@" + model.SystemCode);
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool Update(Sys_Department model, ref string errMsg) {
            return Update(model, null, false, ref errMsg);
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="helper">数据库类</param>
        /// <param name="isTran">是否为事务</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool Delete(Sys_Department model, SQLHelper helper, bool isTran, ref string errMsg) {
            errMsg = string.Empty;
            StringBuilder delCmd = new StringBuilder();
            delCmd.AppendFormat(" Delete {0} where ", model.SaveTable);
            delCmd.Append("ID=@ID");


            if (helper == null) {
                helper = new SQLHelper(conStr);
            }
            try {
                int row = helper.ExecuteNonQuery(delCmd.ToString(), isTran, model);
                if (string.IsNullOrEmpty(model.SystemCode))
                    LixWebUiUtil.ClearRuntimeCash(model.SaveTable);
                else
                    LixWebUiUtil.ClearRuntimeCash(model.SaveTable + "@" + model.SystemCode);
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool Delete(Sys_Department model, ref string errMsg) {
            return Delete(model, null, false, ref errMsg);
        }


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="fileds">需要更新的表达式</param>
        /// <param name="filters">更新条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool UpdateFiledsByFilters(string fileds, string filters, ref string errMsg) {
            errMsg = string.Empty;
            StringBuilder updCmd = new StringBuilder();
            updCmd.AppendFormat("Update {0} Set {1} where {2} ", new Sys_Department().SaveTable, fileds, filters);
            try {
                helper = new SQLHelper(conStr);
                helper.ExecuteNonQuery(updCmd.ToString());
                return true;
                LixWebUiUtil.ClearRuntimeCash(new Sys_Department().SaveTable);
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty);
            }
            return false;
        }


        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="fileds">需要更新的表达式</param>
        /// <param name="filters">更新条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public string SelectFiledsByFilters(string filedName, string filters, ref string errMsg) {
            errMsg = string.Empty;
            StringBuilder updCmd = new StringBuilder();
            updCmd.AppendFormat("select top 1 {0} from {1}  (Nolock)  where {2} ", filedName, new Sys_Department().SelectTable, filters);
            try {
                helper = new SQLHelper(conStr);
                object obj = helper.ExecuteScalar(updCmd.ToString());
                if (!Convert.IsDBNull(obj) && !string.IsNullOrEmpty(Convert.ToString(obj))) {
                    return Convert.ToString(obj);
                }
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty);
            }
            return string.Empty;
        }


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="fileds">需要更新的表达式</param>
        /// <param name="filters">更新条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool UpdateSQL(string sql, ref string errMsg) {
            errMsg = string.Empty;
            try {
                helper = new SQLHelper(conStr);
                helper.ExecuteNonQuery(sql);
                return true;
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty);
            }
            return false;
        }


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="fileds">需要更新的表达式</param>
        /// <param name="filters">更新条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool ExecuteSQL(string sql, ref string errMsg) {
            return ExecuteSQL(sql, null, false, ref errMsg);
        }


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="fileds">需要更新的表达式</param>
        /// <param name="filters">更新条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool ExecuteSQL(string sql, SQLHelper helper, bool isTran, ref string errMsg) {
            errMsg = string.Empty;
            try {
                if (helper == null)
                    helper = new SQLHelper(conStr);
                helper.ExecuteNonQuery(sql, isTran);
                return true;
            } catch (Exception ex) {
                errMsg = ex.Message.Replace("'", string.Empty);
            }
            return false;
        }

    }
}


