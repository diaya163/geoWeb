using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace prjGeo.Commons
{

    #region Convert Class
    public class CvtFunc
    {
        /// <summary>                               
        /// 替换字符串中的单引号为双引号            
        /// </summary>                              
        /// <param name="strValue"></param>         
        /// <returns></returns>                     
        public static string CvtStrInSQL(string strValue)
        {
            strValue = strValue.Replace("\'", "\''");
            return strValue;
        }
        public static string ToString(object obj)
        {
            if (obj == null) return "";

            return obj.ToString().TrimEnd();
        }

        public static string ToDateString(object obj)
        {
            if (obj == null || obj.ToString().TrimEnd() == "") return "";

            return BaseFunctions.FormatDate(obj.ToString());
        }

        public static DateTime ToDateTime(object obj)
        {
            if (obj == null || obj.ToString().TrimEnd() == "") return new DateTime(1900, 1, 1);

            return DateTime.Parse(obj.ToString());
        }

        public static double ToDouble(object obj)
        {
            return ToDouble(obj, -1);
        }
        public static double ToDouble(object obj, int intDecimalPoint)
        {
            string strFormatStyle = "";

            if (obj == null || obj.ToString().TrimEnd() == "") return 0;

            if (intDecimalPoint >= 0)
            {
                strFormatStyle = "N." + intDecimalPoint.ToString();
                return double.Parse(BaseFunctions.FormatNumeric(obj.ToString(), strFormatStyle));
            }
            else
            {
                //return double.Parse(obj.ToString());

                return double.Parse(BaseFunctions.FormatNumeric(obj, "n.n"));

            }
        }

        public static int ToInt(object obj)  //Int32
        {
            if (obj == null || obj.ToString().TrimEnd() == "" || !BaseFunctions.IsNumeric(obj.ToString())) return 0;

            return int.Parse(BaseFunctions.FormatNumeric(obj, "n.0"));
        }

        public static long ToLong(object obj) //Int64
        {
            if (obj == null || obj.ToString().TrimEnd() == "") return 0;

            return long.Parse(obj.ToString());
        }

        public static bool ToBoolean(object obj)
        {
            if (obj == null || obj.ToString().TrimEnd() == "") return false;

            if (obj.ToString().ToUpper() == "YES") return true;
            if (obj.ToString().ToUpper() == "NO") return false;

            return Convert.ToBoolean(obj.ToString());
        }


        //*************以下是判断方法********************************
        #region deal with data type

        #region Check whether a string value is date value
        /// <summary>
        /// Check whether a string value is date value  
        /// </summary>
        /// <param name="Value">Checked string</param>
        public static bool IsDate(object Value)
        {
            if (Value == null)
                return false;
            else
                return IsDate(Value.ToString());
        }
        public static bool IsDate(string Value)
        {
            if (Value == null || Value == string.Empty)
            {
                return false;
            }
            else
            {
                try
                {
                    DateTime.Parse(Value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        #endregion

        #region Check whether a string value is numeric value
        /// <summary>
        /// Check whether a string value is numeric value  
        /// </summary>
        /// <param name="Value">Checked string</param>
        public static bool IsNumeric(string Value)
        {
            return IsNumeric(Value, false);
        }

        public static bool IsNumeric(string Value, bool bIncludeSpecString)
        {
            if (bIncludeSpecString)
            {
                Value = Value.Replace("$", "").Replace("(", "").Replace(")", "").Trim();
            }

            string strValue = CvtFunc.ToString(Value);

            int iLength = strValue.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Length;

            if (strValue == null || strValue == string.Empty || iLength == 0 || iLength > 2)
            {
                return false;
            }
            else
            {
                strValue = strValue.Replace(",", "");

                if (strValue.Substring(0, 1) == "-")
                {
                    strValue = strValue.Substring(1, strValue.Length - 1);
                    if (strValue == "") return false;
                }

                foreach (char c in strValue.ToCharArray())
                {
                    if (!(char.IsNumber(c) || c == '.'))
                    {
                        return false;
                    }
                }

                if (strValue.IndexOf("..") > 0) return false;
                return true;
            }

        }
        #endregion

        #region Check whether a string value is double value
        /// <summary>
        /// Check whether a string value is double value  
        /// </summary>
        /// <param name="Value">Checked string</param>
        public static bool IsDouble(string Value)
        {


            if (Value == null || Value == string.Empty)
            {
                return false;
            }
            else
            {
                if (Value.Substring(0, 1) == "-")
                {
                    Value = Value.Substring(1, Value.Length - 1);
                }

                foreach (char c in Value.ToCharArray())
                {
                    if (!(char.IsNumber(c) || c == '.' || c == ','))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        #endregion

        #region Check whether a string value is inteter value
        /// <summary>
        /// Check whether a string value is inteter value  
        /// </summary>
        /// <param name="Value">Checked string</param>
        public static bool IsInt(string Value)
        {
            if (Value == null || Value == string.Empty)
            {
                return false;
            }
            else
            {

                foreach (char c in Value.ToCharArray())
                {
                    if (!char.IsNumber(c))
                        return false;
                }
                return true;
            }
        }
        #endregion

        #region Cast object to duble,check the null and DBNull
        /// <summary>
        /// Cast object to duble,check the null and DBNull
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double CastToDouble(object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.ToString() == "")
            {
                return 0;
            }
            else
            {
                return double.Parse(obj.ToString());
            }
        }
        #endregion

        #region Cast object to int,check the null and DBNull
        /// <summary>
        /// Cast object to int,check the null and DBNull
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int CastToInt(object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.ToString() == "")
            {
                return 0;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        #endregion

        #region split a string with separator
        public static string[] Split(string strData, string strSeparator)
        {
            int iCount = 0;
            int iPos1 = 0, iPos2 = 0;
            int iSepLength = 0;
            string[] strSplit = null;

            if (strData == null || strSeparator == null)
                return null;
            else
            {
                iSepLength = strSeparator.Length;

                while (iPos1 >= 0)
                {
                    iPos1 = strData.IndexOf(strSeparator, iPos2);
                    if (iPos1 >= 0)
                    {
                        iPos2 = iPos1 + iSepLength;
                    }
                    if (iPos1 >= 0)
                    {
                        iCount++;
                    }
                }

                if (iPos2 < strData.Length)
                {
                    iCount++;
                }
                strSplit = new string[iCount];

                iCount = 0; iPos1 = 0; iPos2 = 0;

                while (iPos1 >= 0)
                {
                    iPos1 = strData.IndexOf(strSeparator, iPos2);
                    if (iPos1 >= 0)
                    {
                        strSplit[iCount] = strData.Substring(iPos2, iPos1 - iPos2);
                        iPos2 = iPos1 + iSepLength;
                    }

                    if (iPos1 >= 0)
                    {
                        iCount++;
                    }
                }

                if (iPos2 < strData.Length)
                {
                    strSplit[iCount] = strData.Substring(iPos2, strData.Length - iPos2);
                }

                return strSplit;
            }
        }
        #endregion

        #region Convert Array to DataTable
        public static DataTable ArrayToDataTable(string[] FieldNameArray, string[] ValueArray1, string[] ValueArray2)
        {
            DataTable dt = null;
            DataRow dr = null;

            if ((FieldNameArray.Length >= 2) && (ValueArray1.Length == ValueArray2.Length))
            {
                dt = new DataTable("NewTable");
                dt.Columns.Add(FieldNameArray[0].ToString());
                dt.Columns.Add(FieldNameArray[1].ToString());

                for (int i = 0; i < ValueArray1.Length; i++)
                {
                    dr = dt.NewRow();

                    dr[FieldNameArray[0].ToString()] = ValueArray1[i].ToString();
                    dr[FieldNameArray[1].ToString()] = ValueArray2[i].ToString();
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        #endregion

        #region FormatDate
        //format date type data to long date type
        public static string FormatDate(DateTime dt)
        {
            string strFormatDate = "";
             
            strFormatDate = dt.ToString(SystemDateOption.optDateFmtMDY, System.Globalization.DateTimeFormatInfo.InvariantInfo);

            return strFormatDate;
        }

        public static string FormatDate(string dt)
        {
            string strFormatDate = "";

            if (dt != "" && dt.Trim() != "/  /")
                strFormatDate = DateTime.Parse(dt).ToString(SystemDateOption.optDateFmtMDY, System.Globalization.DateTimeFormatInfo.InvariantInfo);

            return strFormatDate;
        }

        public static string FormatDate(string dt, DateFormatOption optFormat)
        {
            string strFormatDate = "";
            if (dt.Trim() == "/  /" || dt.Trim() == "") return strFormatDate;
            strFormatDate = DateTime.Parse(dt).ToString(SystemDateOption.GetFormat(optFormat), System.Globalization.DateTimeFormatInfo.InvariantInfo);

            return strFormatDate;
        }
        #endregion

        #region DateDiff
        public static long DateDiff(DateInterval Interval, System.DateTime StartDate, System.DateTime EndDate, bool bIsAddStartDay)
        {
            long lngDateDiffValue = 0;

            if (bIsAddStartDay)
                EndDate = EndDate.AddDays(1); //for need add the start day

            System.TimeSpan TS = new System.TimeSpan(EndDate.Ticks - StartDate.Ticks);
            switch (Interval)
            {
                case DateInterval.Second:
                    lngDateDiffValue = (long)TS.TotalSeconds;
                    break;
                case DateInterval.Minute:
                    lngDateDiffValue = (long)TS.TotalMinutes;
                    break;
                case DateInterval.Hour:
                    lngDateDiffValue = (long)TS.TotalHours;
                    break;
                case DateInterval.Day:
                    lngDateDiffValue = (long)TS.Days;
                    break;
                case DateInterval.Week:
                    lngDateDiffValue = (long)(TS.Days / 7);
                    break;
                case DateInterval.Month:
                    lngDateDiffValue = (long)(TS.Days / 30);
                    break;
                case DateInterval.Quarter:
                    lngDateDiffValue = (long)((TS.Days / 30) / 3);
                    break;
                case DateInterval.Year:
                    lngDateDiffValue = (long)(TS.Days / 365);
                    break;
            }
            return (lngDateDiffValue);
        }//end of DateDiff
        public static long DateDiff(DateInterval Interval, System.DateTime StartDate, System.DateTime EndDate)
        {
            return DateDiff(Interval, StartDate, EndDate, false);
        }
        #endregion

        #region Input string:'Yes' or 'No',"True" or 'False', Return bool: True or False
        public static bool GetBoolValue(string strValue)
        {
            bool bReturn = false;
            if (strValue.ToUpper().Trim() == "YES" || strValue.ToUpper().Trim() == "TRUE")
            {
                bReturn = true;
            }
            return bReturn;
        }
        #endregion

        #region Get Days
        /// <summary>
        /// Get Days: if strEndDate > strStartDate, than return value >0 else  if strEndDate = strStartDate, than return value =0 else return value <0
        /// </summary>
        /// <param name="strStartDate"></param>
        /// <param name="strEndDate"></param>
        /// <returns></returns>
        public static int CompareDate(string strStartDate, string strEndDate)
        {
            DateTime dtStartDate, dtEndDate;
            TimeSpan timeSpan;

            dtStartDate = Convert.ToDateTime(strStartDate);
            dtEndDate = Convert.ToDateTime(strEndDate);

            timeSpan = dtEndDate.Subtract(dtStartDate);

            return timeSpan.Days;
        }
        #endregion

        #region StringUtil
        public static string Space(int i)
        {
            string s = "";
            for (int j = 0; j < i; j++)
            {
                s += " ";
            }
            return s;
        }

        public static int ToInt32(string a_strPara, Int32 a_intDefault)
        {
            if (a_strPara == null) return a_intDefault;
            if (!IsInt(a_strPara)) return a_intDefault;
            try
            {
                return Convert.ToInt32(a_strPara);
            }
            catch
            {
                return a_intDefault;
            }
        }

        #endregion

        #region InStrEx
        public static bool InStrEx(string strInCompStr, string strCompStr)
        {
            return InStrEx(strInCompStr, strCompStr, ",");
        }
        public static bool InStrEx(string strInCompStr, string strCompStr, string strSplit)
        {
            string[] strArrSplit = new string[1];

            strArrSplit[0] = strSplit;

            string[] a = strInCompStr.Split(strArrSplit, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < a.Length; i++)
            {
                if (string.Compare(a[i], strCompStr, StringComparison.CurrentCultureIgnoreCase) == 0) return true;
            }
            return false;
        }
        #endregion

        #region Convert Number to Roma
        public static string CnvNumberToRoma(int intNumber)
        {
            if (intNumber <= 0) return "";

            string[] strRomaArray = new string[] { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };
            string strCnvNumberToRoma = "";
            int intRoma = intNumber % 10;
            if (intRoma != 0)
            {
                strCnvNumberToRoma = strRomaArray[intRoma];
            }
            intRoma = intNumber / 10;
            for (int i = 1; i <= intRoma; i++)
            {
                strCnvNumberToRoma = strRomaArray[10] + strCnvNumberToRoma;
            }

            return strCnvNumberToRoma;
        }
        #endregion



        #region Get MultiTextBox Current Line No
        public static int GetMultiTextBoxCurrentLineNo(string strTextBoxValue)
        {
            string strFind = "\r\n";
            int intLastFind;
            int intNumber;
            intNumber = 1;
            intLastFind = strTextBoxValue.IndexOf(strFind);
            while (intLastFind >= 0)
            {
                intNumber++;
                intLastFind = strTextBoxValue.IndexOf(strFind, intLastFind + 1);
            }
            return intNumber;
        }
        #endregion


        #endregion


    }
    #endregion Convert Class
}
