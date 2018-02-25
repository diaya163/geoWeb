using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace prjGeo.Commons
{
    public class DataFunction
    {
        //#region GetFieldValue: Get a field value according to a table name and a condition
        ///// <summary>
        ///// Get a field value according to a table name and condition
        ///// </summary>
        ///// <param name="strTableName">Table name</param>
        ///// <param name="strFieldName"> Field name</param>
        ///// <param name="strCondition">Query condition</param>
        ///// <returns>string value</returns>
        //public static string GetFieldValue(string strTableName, string strFieldName, string strCondition)
        //{
        //    string strSql = "SELECT top 1 {0} FROM {1} WHERE {2}";
        //    string strReturn = "";

        //    if (strCondition.Trim() == "")
        //        strCondition = "1=1";

        //    strSql = String.Format(strSql, strFieldName, strTableName, strCondition);

        //    strReturn = DbHelperSQL.GetValue(strSql);

        //    if (strReturn == null)
        //        strReturn = "";

        //    return strReturn.TrimEnd();
        //}


        //////public static string GetFieldValue(string strTableName, string strFieldName, string strCondition)
        //////{
        //////    return GetFieldValue(strTableName, strFieldName, strCondition);
        //////}

        //public static object GetFieldObjectValue(string strTableName, string strFieldName, string strCondition)
        //{
        //    string strSql = "SELECT {0} FROM {1} WHERE {2}";

        //    strSql = String.Format(strSql, strFieldName, strTableName, strCondition);

        //    return DbHelperSQL.GetObjectValue(strSql);
        //}


        //#endregion
    }
}
