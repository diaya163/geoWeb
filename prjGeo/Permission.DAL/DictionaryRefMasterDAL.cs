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
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;

namespace Permission.DAL {
    /// <summary>
    /// DictionaryRefMasterDAL
    /// </summary>
    public partial class DictionaryRefMasterDAL {
        private string conStr = string.Empty;
        private SQLHelper helper = null;
        public DictionaryRefMasterDAL(string serverName) {
            string serverIP = ConfigurationManager.AppSettings["ServerIP"];
            conStr = DESEncrypt.Decrypt(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings[serverName]].ConnectionString);
            conStr = string.Format(conStr, string.IsNullOrEmpty(serverIP) ? "." : serverIP);
        }
        public DictionaryRefMasterDAL() {
            string serverIP = ConfigurationManager.AppSettings["ServerIP"];
            conStr = DESEncrypt.Decrypt(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DefaultConnectionStringName"]].ConnectionString);
            conStr = string.Format(conStr, string.IsNullOrEmpty(serverIP) ? "." : serverIP);
        }

        /// <summary>
        /// 设置数据参数
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public List<SqlParameter> SetSqlParameter(DictionaryRefMaster model) {
            List<SqlParameter> lstParas = new List<SqlParameter>();
            SqlParameter pars = null;
            pars = new SqlParameter("@RefKey", SqlDbType.NVarChar);
            pars.Value = model.RefKey;
            lstParas.Add(pars);
            pars = new SqlParameter("@RefCode", SqlDbType.NVarChar);
            pars.Value = model.RefCode;
            lstParas.Add(pars);
            pars = new SqlParameter("@RefCode2", SqlDbType.NVarChar);
            pars.Value = model.RefCode2 == null ? DBNull.Value : model.RefCode2 as object;
            lstParas.Add(pars);
            pars = new SqlParameter("@RefValue", SqlDbType.NVarChar);
            pars.Value = model.RefValue;
            lstParas.Add(pars);
            pars = new SqlParameter("@RefValue2", SqlDbType.NVarChar);
            pars.Value = model.RefValue2 == null ? DBNull.Value : model.RefValue2 as object;
            lstParas.Add(pars);
            pars = new SqlParameter("@RefIsUse", SqlDbType.Bit);
            pars.Value = model.RefIsUse == null ? DBNull.Value : model.RefIsUse as object;
            lstParas.Add(pars);
            pars = new SqlParameter("@RefSeq", SqlDbType.Int);
            pars.Value = model.RefSeq == null ? DBNull.Value : model.RefSeq as object;
            lstParas.Add(pars);
            pars = new SqlParameter("@RefRemark", SqlDbType.NVarChar);
            pars.Value = model.RefRemark == null ? DBNull.Value : model.RefRemark as object;
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
            strSql.AppendFormat("select count(1) from {0}  ", new DictionaryRefMaster().SelectTable);
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
                object objRecord = helper.ExecuteScalar(string.Format("select count(1) from {0}  where {1} ", new DictionaryRefMaster().SelectTable, filters));
                rCount = Convert.ToInt32(objRecord);
            }
            if (pageSize != -1) {
                selCmd.Append(";with tmp_table as ( ");
                selCmd.Append("SELECT ");
                selCmd.Append("ROW_NUMBER() OVER(ORDER BY ID DESC) AS tmp_Id,");
                selCmd.Append("ID,RefKey,RefCode,RefCode2,RefValue,RefValue2,RefIsUse,RefSeq,RefRemark");
                selCmd.AppendFormat(" FROM {0}  ", new DictionaryRefMaster().SelectTable);
                selCmd.AppendFormat(" where {0} ", filters);
                selCmd.AppendFormat(")select * from tmp_table where tmp_Id between {0} and {1}", pageSize * pageIndex + 1, pageSize * (pageIndex + 1));
            } else {
                selCmd.Append("SELECT ");
                selCmd.Append("ID,RefKey,RefCode,RefCode2,RefValue,RefValue2,RefIsUse,RefSeq,RefRemark");
                selCmd.AppendFormat(" FROM {0}  ", new DictionaryRefMaster().SelectTable);
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
        public IList<DictionaryRefMaster> GetList(string filters, string orderBy, int TopNo, int pageSize, int pageIndex, ref int rCount, ref string errMsg) {
            errMsg = string.Empty;
            IList<DictionaryRefMaster> list = new List<DictionaryRefMaster>();
            DictionaryRefMaster model = new DictionaryRefMaster();
            if (string.IsNullOrEmpty(filters)) filters = " 1=1";
            StringBuilder selCmd = new StringBuilder();
            if (pageSize != -1) {
                helper = new SQLHelper(conStr);
                object objRecord = helper.ExecuteScalar(string.Format("select count(1) from {0}  where {1} ", model.SelectTable, filters));
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
                selCmd.Append("ID,RefKey,RefCode,RefCode2,RefValue,RefValue2,RefIsUse,RefSeq,RefRemark");
                selCmd.AppendFormat(" FROM {0}  ", model.SelectTable);
                selCmd.AppendFormat(" where {0} ", filters);
                selCmd.AppendFormat(")select * from tmp_table where tmp_Id between {0} and {1}", pageSize * pageIndex + 1, pageSize * (pageIndex + 1));
            } else {
                if (TopNo > 0) {
                    selCmd.AppendFormat("SELECT Top {0} ", TopNo);
                } else {
                    selCmd.Append("SELECT ");
                }
                selCmd.Append("ID,RefKey,RefCode,RefCode2,RefValue,RefValue2,RefIsUse,RefSeq,RefRemark");
                selCmd.AppendFormat(" FROM {0}  ", model.SelectTable);
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
                        model = new DictionaryRefMaster();
                        model.ID = Convert.ToInt32(reader["ID"]);
                        model.RefKey = Convert.ToString(reader["RefKey"]);
                        model.RefCode = Convert.ToString(reader["RefCode"]);
                        if (!Convert.IsDBNull(reader["RefCode2"]))
                            model.RefCode2 = Convert.ToString(reader["RefCode2"]);
                        model.RefValue = Convert.ToString(reader["RefValue"]);
                        if (!Convert.IsDBNull(reader["RefValue2"]))
                            model.RefValue2 = Convert.ToString(reader["RefValue2"]);
                        if (!Convert.IsDBNull(reader["RefIsUse"]))
                            model.RefIsUse = Convert.ToBoolean(reader["RefIsUse"]);
                        if (!Convert.IsDBNull(reader["RefSeq"]))
                            model.RefSeq = Convert.ToInt32(reader["RefSeq"]);
                        if (!Convert.IsDBNull(reader["RefRemark"]))
                            model.RefRemark = Convert.ToString(reader["RefRemark"]);

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
        public IList<DictionaryRefMaster> GetList(string filters, string orderBy, int pageSize, int pageIndex, ref string errMsg) {
            return GetList(filters, orderBy, -1, pageSize, pageIndex, ref errMsg);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="TopNo">Top 前几条</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<DictionaryRefMaster> GetList(string filters, int pageSize, int pageIndex, ref string errMsg) {
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
        public IList<DictionaryRefMaster> GetList(string filters, string orderBy, int TopNo, ref string errMsg) {
            return GetList(filters, orderBy, TopNo, -1, -1, ref errMsg);
        }

        /// <summary>
        /// 自定义排序字段进行刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="orderBy">字段排序</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<DictionaryRefMaster> GetList(string filters, string orderBy, ref string errMsg) {
            return GetList(filters, orderBy, -1, -1, -1, ref errMsg);
        }
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="TopNo">Top 前几条</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<DictionaryRefMaster> GetList(string filters, int TopNo, ref string errMsg) {
            return GetList(filters, string.Empty, TopNo, -1, -1, ref errMsg);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<DictionaryRefMaster> GetList(string filters, ref string errMsg) {
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
        public IList<DictionaryRefMaster> GetList(string filters, string orderBy, int TopNo, int pageSize, int pageIndex, ref string errMsg) {
            errMsg = string.Empty;
            DictionaryRefMaster model = new DictionaryRefMaster();
            if (string.IsNullOrEmpty(filters)) filters = " 1=1";
            StringBuilder selCmd = new StringBuilder();
            string cacheKey = "[" + model.SelectTable + "][List](" + filters.Trim() + ")";
            IList<DictionaryRefMaster> list = null; // HttpRuntime.Cache[cacheKey] as List<DictionaryRefMaster>;
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
                    selCmd.Append("ID,RefKey,RefCode,RefCode2,RefValue,RefValue2,RefIsUse,RefSeq,RefRemark");
                    selCmd.AppendFormat(" FROM {0}  ", model.SelectTable);
                    selCmd.AppendFormat(" where {0} ", filters);
                    selCmd.AppendFormat(")select * from tmp_table where tmp_Id between {0} and {1}", pageSize * pageIndex + 1, pageSize * (pageIndex + 1));
                    selCmd.AppendFormat(";select count(1) as TotalCount from {0}  where {1} ", model.SelectTable, filters);
                } else {
                    if (TopNo > 0) {
                        selCmd.AppendFormat("SELECT Top {0} ", TopNo);
                    } else {
                        selCmd.Append("SELECT ");
                    }
                    selCmd.Append("ID,RefKey,RefCode,RefCode2,RefValue,RefValue2,RefIsUse,RefSeq,RefRemark");
                    selCmd.AppendFormat(" FROM {0}  ", model.SelectTable);
                    selCmd.AppendFormat(" where {0} ", filters);
                    if (string.IsNullOrEmpty(orderBy)) {
                        selCmd.Append(" ORDER BY ID");
                    } else {
                        selCmd.AppendFormat(" ORDER BY {0} ", orderBy);
                    }
                }
                if (list == null) {
                    helper = new SQLHelper(conStr);
                    list = helper.SelectReader<DictionaryRefMaster>(typeof(DictionaryRefMaster), selCmd.ToString());
                    if (list != null) {
                        LixWebUiUtil.InsertRuntimeCash(cacheKey, list, DictionaryRefMaster.CacheAbsoluteExpiration);
                    }

                }
            } catch (Exception ex) {
                errMsg = ex.Message;
            }
            return list;
        }

        // sql server数据类型（如：varchar）
        // 转换为SqlDbType类型
        public SqlDbType SqlTypeString2SqlType(string sqlTypeString) {
            SqlDbType dbType = SqlDbType.Variant;//默认为Object

            switch (sqlTypeString) {
                case "int":
                    dbType = SqlDbType.Int;
                    break;
                case "varchar":
                    dbType = SqlDbType.VarChar;
                    break;
                case "bit":
                    dbType = SqlDbType.Bit;
                    break;
                case "datetime":
                    dbType = SqlDbType.DateTime;
                    break;
                case "decimal":
                    dbType = SqlDbType.Decimal;
                    break;
                case "float":
                    dbType = SqlDbType.Float;
                    break;
                case "image":
                    dbType = SqlDbType.Image;
                    break;
                case "money":
                    dbType = SqlDbType.Money;
                    break;
                case "ntext":
                    dbType = SqlDbType.NText;
                    break;
                case "nvarchar":
                    dbType = SqlDbType.NVarChar;
                    break;
                case "smalldatetime":
                    dbType = SqlDbType.SmallDateTime;
                    break;
                case "smallint":
                    dbType = SqlDbType.SmallInt;
                    break;
                case "text":
                    dbType = SqlDbType.Text;
                    break;
                case "bigint":
                    dbType = SqlDbType.BigInt;
                    break;
                case "binary":
                    dbType = SqlDbType.Binary;
                    break;
                case "char":
                    dbType = SqlDbType.Char;
                    break;
                case "nchar":
                    dbType = SqlDbType.NChar;
                    break;
                case "numeric":
                    dbType = SqlDbType.Decimal;
                    break;
                case "real":
                    dbType = SqlDbType.Real;
                    break;
                case "smallmoney":
                    dbType = SqlDbType.SmallMoney;
                    break;
                case "sql_variant":
                    dbType = SqlDbType.Variant;
                    break;
                case "timestamp":
                    dbType = SqlDbType.Timestamp;
                    break;
                case "tinyint":
                    dbType = SqlDbType.TinyInt;
                    break;
                case "uniqueidentifier":
                    dbType = SqlDbType.UniqueIdentifier;
                    break;
                case "varbinary":
                    dbType = SqlDbType.VarBinary;
                    break;
                case "xml":
                    dbType = SqlDbType.Xml;
                    break;
            }
            return dbType;
        }

        // SqlDbType转换为C#数据类型
        public static Type SqlType2CsharpType(SqlDbType sqlType) {
            switch (sqlType) {
                case SqlDbType.BigInt:
                    return typeof(Int64);
                case SqlDbType.Binary:
                    return typeof(Object);
                case SqlDbType.Bit:
                    return typeof(Boolean);
                case SqlDbType.Char:
                    return typeof(String);
                case SqlDbType.DateTime:
                    return typeof(DateTime);
                case SqlDbType.Decimal:
                    return typeof(Decimal);
                case SqlDbType.Float:
                    return typeof(Double);
                case SqlDbType.Image:
                    return typeof(Object);
                case SqlDbType.Int:
                    return typeof(Int32);
                case SqlDbType.Money:
                    return typeof(Decimal);
                case SqlDbType.NChar:
                    return typeof(String);
                case SqlDbType.NText:
                    return typeof(String);
                case SqlDbType.NVarChar:
                    return typeof(String);
                case SqlDbType.Real:
                    return typeof(Single);
                case SqlDbType.SmallDateTime:
                    return typeof(DateTime);
                case SqlDbType.SmallInt:
                    return typeof(Int16);
                case SqlDbType.SmallMoney:
                    return typeof(Decimal);
                case SqlDbType.Text:
                    return typeof(String);
                case SqlDbType.Timestamp:
                    return typeof(Object);
                case SqlDbType.TinyInt:
                    return typeof(Byte);
                case SqlDbType.Udt://自定义的数据类型
                    return typeof(Object);
                case SqlDbType.UniqueIdentifier:
                    return typeof(Object);
                case SqlDbType.VarBinary:
                    return typeof(Object);
                case SqlDbType.VarChar:
                    return typeof(String);
                case SqlDbType.Variant:
                    return typeof(Object);
                case SqlDbType.Xml:
                    return typeof(Object);
                default:
                    return null;
            }
        }


        public static SqlDbType CsharpTypeToSqlType(Type type) {
            switch (type.Name) {
                case "int64":
                    return SqlDbType.BigInt;
                case "object":
                    return SqlDbType.Variant;
                case "boolean":
                    return SqlDbType.Bit;
                case "string":
                    return SqlDbType.NVarChar;
                case "dateTime":
                    return SqlDbType.DateTime;
                case "decimal":
                    return SqlDbType.Decimal;
                case "double":
                    return SqlDbType.Float;
                case "byte[]":
                    return SqlDbType.Binary;
                case "int32":
                    return SqlDbType.Int;
                case "float":
                    return SqlDbType.Float;

                //case SqlDbType.NText:
                //    return typeof(String);
                //case SqlDbType.NVarChar:
                //    return typeof(String);

                case "single":
                    return SqlDbType.Real;
                case "datetime":
                    return SqlDbType.DateTime;
                case "uniqueidentifier":
                    return SqlDbType.UniqueIdentifier;
                default:
                    return SqlDbType.Variant;
            }
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="helper">数据库类</param>
        /// <param name="isTran">是否为事务</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Add(DictionaryRefMaster model, SQLHelper helper, bool isTran, ref string errMsg) {
            errMsg = string.Empty;
            StringBuilder insCmd = new StringBuilder();
            insCmd.AppendFormat("insert into {0}", model.SaveTable);
            insCmd.Append("(RefKey,RefCode,RefCode2,RefValue,RefValue2,RefIsUse,RefSeq,RefRemark)");
            insCmd.Append("VALUES(@RefKey,@RefCode,@RefCode2,@RefValue,@RefValue2,@RefIsUse,@RefSeq,@RefRemark)");
            insCmd.Append(";select @@IDENTITY");

            /*
             
             propertyInfo.SetValue(tmp_Class, 5, null); //给对应属性赋值
int value_New = (int)propertyInfo.GetValue(tmp_Class, null);
Console.WriteLine(value_New);
             
             */
            //List<SqlParameter> parameters = SetSqlParameter(model);
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
        public int Add(DictionaryRefMaster model, ref string errMsg) {
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
        public bool Update(DictionaryRefMaster model, SQLHelper helper, bool isTran, ref string errMsg) {
            errMsg = string.Empty;
            StringBuilder updCmd = new StringBuilder();
            updCmd.AppendFormat("Update {0} Set ", model.SaveTable);
            updCmd.Append("RefKey=@RefKey,");
            updCmd.Append("RefCode=@RefCode");
            updCmd.Append(",RefCode2=@RefCode2");
            updCmd.Append(",RefValue=@RefValue");
            updCmd.Append(",RefValue2=@RefValue2");
            updCmd.Append(",RefIsUse=@RefIsUse");
            updCmd.Append(",RefSeq=@RefSeq");
            updCmd.Append(",RefRemark=@RefRemark");

            updCmd.Append(" where ID=@ID");

            //List<SqlParameter> parameters = SetSqlParameter(model);
            //SqlParameter parasAdd = null;
            //parasAdd = new SqlParameter("@ID", SqlDbType.Int);
            //parasAdd.Value = model.ID;
            //parameters.Add(parasAdd);

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
        public bool Update(DictionaryRefMaster model, ref string errMsg) {
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
        public bool Delete(DictionaryRefMaster model, SQLHelper helper, bool isTran, ref string errMsg) {
            errMsg = string.Empty;
            StringBuilder delCmd = new StringBuilder();
            delCmd.AppendFormat(" Delete {0} where ", model.SaveTable);
            delCmd.Append("ID=@ID");

            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter parasAdd = null;
            parasAdd = new SqlParameter("@ID", SqlDbType.Int);
            parasAdd.Value = model.ID;
            parameters.Add(parasAdd);

            if (helper == null) {
                helper = new SQLHelper(conStr);
            }
            try {
                int row = helper.ExecuteNonQuery(delCmd.ToString(), isTran, parameters.ToArray());
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
        public bool Delete(DictionaryRefMaster model, ref string errMsg) {
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
            updCmd.AppendFormat("Update {0} Set {1} where {2} ", new DictionaryRefMaster().SaveTable, fileds, filters);
            try {
                helper = new SQLHelper(conStr);
                helper.ExecuteNonQuery(updCmd.ToString());
                LixWebUiUtil.ClearRuntimeCash(new DictionaryRefMaster().SaveTable);
                return true;
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
            updCmd.AppendFormat("select top 1 {0} from {1}  where {2} ", filedName, new DictionaryRefMaster().SelectTable, filters);
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


