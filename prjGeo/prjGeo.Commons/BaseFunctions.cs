using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace prjGeo.Commons
{
    public class BaseFunctions
    {
        public BaseFunctions()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        #region FormatNumeric:Format numeric according format style ,like as "NN","N.S","NN.N" etc
        public static string FormatNumeric(object dblData, string strFormatStyle)
        {
            if (dblData == null) dblData = "";
            return FormatNumeric(dblData.ToString(), strFormatStyle);
        }
        public static string FormatNumeric(double dblData, string strFormatStyle)
        {
            return FormatNumeric(dblData.ToString(), strFormatStyle);
        }
        public static string FormatNumeric(string strData, string strFormatStyle)
        {
            int intDigitPos = 0;
            bool blnTrimEndZeroChar = false;
            string strValue1 = "", strValue2 = "";

            if (strFormatStyle.ToUpper().Trim() == "N.?")
            {
                blnTrimEndZeroChar = true;
                strFormatStyle = "N.N";
            }

            if (strFormatStyle.ToUpper().Trim() == "NS.?")
            {
                blnTrimEndZeroChar = true;
                strFormatStyle = "NS.N";
            }

            strData = strData.Replace(",", "").Trim();

            //Format the strData to NumerricType
            if (!BaseFunctions.IsNumeric(strData))
            {
                strData = "0";
            }
            else
            {
                if (strData.Substring(0, 1) == ".")
                    strData = "0" + strData;
                if (strData.Substring(strData.Length - 1, 1) == ".")
                    strData = strData.Substring(0, strData.Length - 1);
            }
            switch (strFormatStyle.ToUpper())
            {
                case "NN"://no format , remove "," in strField_Char  and remove left "0"
                    strData = strData.Replace(",", "");
                    while (strData.Substring(0, 1) == "0" && strData.Length > 1 && strData.Substring(1, 1) != ".")
                    {
                        strData = strData.Substring(1, strData.Length - 1);
                    }
                    return (strData);
                case "N.N"://no format , remove "," in strField_Char  and remove left "0"
                    strData = strData.Replace(",", "");
                    while (strData.Substring(0, 1) == "0" && strData.Length > 1 && strData.Substring(1, 1) != ".")
                    {
                        strData = strData.Substring(1, strData.Length - 1);
                    }
                    if (blnTrimEndZeroChar)
                    {
                        while (strData.IndexOf(".") >= 0 && strData.Length > 0 && strData.Substring(strData.Length - 1, 1) == "0")
                        {
                            strData = strData.Substring(0, strData.Length - 1);
                        }
                        if (strData.Length > 0 && strData.Substring(strData.Length - 1, 1) == ".") strData = strData.Substring(0, strData.Length - 1);
                    }
                    if (strData.Trim() == "") strData = "0";
                    return (strData);
                case "NS.N"://'number do not format ,but need add number separator ","
                    strData = strData.Replace(",", "");

                    intDigitPos = strData.IndexOf(".", 0);//get the position of radix point
                    if (intDigitPos > 0)
                    {
                        strValue1 = FormatSeparatorNumeric(strData.Substring(0, intDigitPos));
                        strValue2 = strData.Substring(intDigitPos + 1, strData.Length - intDigitPos - 1);
                        strData = strValue1 + "." + strValue2;
                    }
                    else
                        strData = FormatSeparatorNumeric(strData);
                    if (blnTrimEndZeroChar)
                    {
                        while (strData.IndexOf(".") >= 0 && strData.Length > 0 && strData.Substring(strData.Length - 1, 1) == "0")
                        {
                            strData = strData.Substring(0, strData.Length - 1);
                        }
                        if (strData.Length > 0 && strData.Substring(strData.Length - 1, 1) == ".") strData = strData.Substring(0, strData.Length - 1);
                    }
                    if (strData.Trim() == "") strData = "0";
                    return (strData);
                case "NS"://'number do not format ,but need add number separator ","
                    strData = strData.Replace(",", "");

                    intDigitPos = strData.IndexOf(".", 0);//get the position of radix point
                    if (intDigitPos > 0)
                    {
                        strValue1 = FormatSeparatorNumeric(strData.Substring(0, intDigitPos));
                        strValue2 = strData.Substring(intDigitPos + 1, strData.Length - intDigitPos - 1);
                        strData = strValue1 + "." + strValue2;
                    }
                    else
                        strData = FormatSeparatorNumeric(strData);
                    return (strData);
                default:
                    if (strFormatStyle.IndexOf(".") >= 0 && BaseFunctions.IsNumeric(strFormatStyle.Substring(strFormatStyle.IndexOf(".") + 1)))
                    {

                        strData = Round(strData, Int32.Parse(strFormatStyle.Substring(strFormatStyle.IndexOf(".") + 1)));

                        //strData = decimal.Round(decimal.Parse(strData, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowLeadingSign), Int32.Parse(strFormatStyle.Substring(strFormatStyle.IndexOf(".") + 1)), MidpointRounding.ToEven  ).ToString();

                    }
                    break;
            }

            strFormatStyle = strFormatStyle.Substring(1, strFormatStyle.Length - 1).ToUpper();

            string strChar = string.Empty;
            if (strFormatStyle != string.Empty)
                strChar = strFormatStyle.Substring(0, 1);

            bool bSp = false;

            if (strChar == "S")
            {
                bSp = true;
                strFormatStyle = strFormatStyle.Substring(1, strFormatStyle.Length - 1);
            }
            else if ((strChar == ".") || (strChar == "") || (Char.IsNumber(strChar, 0)))
            {
                bSp = false;
            }
            else if (((char.Parse(strChar)) >= 'A' && (char.Parse(strChar)) <= 'R') || ((char.Parse(strChar)) >= 'T' && (char.Parse(strChar)) <= 'Z'))
            {
                bSp = false;
                strFormatStyle = strFormatStyle.Substring(1, strFormatStyle.Length - 1);
            }
            else
            {
                return (strData);
            }

            int intWidth = 0;
            int intScaleWidth = 0;
            // string strAddChar = "";
            int intForLen = 0;

            intDigitPos = strFormatStyle.IndexOf(".");

            if (intDigitPos < 0) //"N" , "NS"
            {
                if (strFormatStyle == string.Empty)
                    intWidth = 0;
                else
                    intWidth = Int32.Parse(strFormatStyle);
                intScaleWidth = 0;
            }
            else if (intDigitPos == 0) //"N.2" ,"NS.2"
            {
                intWidth = 0;
                intScaleWidth = Int32.Parse(strFormatStyle.Substring(1, strFormatStyle.Length - 1)); ;
            }
            else if (intDigitPos >= 1) //"NS10.2" , "NS10.2B" , "NS10.2Z"
            {
                intWidth = Int32.Parse(strFormatStyle.Substring(0, intDigitPos));
                intScaleWidth = Int32.Parse(strFormatStyle.Substring(intDigitPos + 1));
            }


            //Add scale
            if (intScaleWidth > 0)
            {
                intDigitPos = strData.IndexOf(".");
                if (intDigitPos >= 0)
                {
                    strValue2 = strData.Substring(intDigitPos + 1, strData.Length - intDigitPos - 1);
                }
                else
                {
                    strData = strData + ".";
                    strValue2 = "";
                }
                intForLen = intScaleWidth - strValue2.Length;
                if (intForLen > 0)
                {
                    for (int i = 0; i < intForLen; i++)
                    {
                        strData += "0";
                    }
                }

            }

            //Add spearater
            if (bSp)
            {
                strData = strData.Replace(",", "");
                intDigitPos = strData.IndexOf(".");
                if (intDigitPos >= 0)
                {
                    strValue1 = FormatSeparatorNumeric(strData.Substring(0, intDigitPos));
                    strValue2 = strData.Substring(intDigitPos + 1, strData.Length - intDigitPos - 1);
                    strData = strValue1 + "." + strValue2;

                }
                else
                {
                    strData = FormatSeparatorNumeric(strData);
                }
            }

            return strData;

        }

        private static string FormatSeparatorNumeric(string strData)
        {
            string StrTemp = "", strSign = "";
            int iIndex = 0;

            if (strData.Length > 0 && strData.Substring(0, 1) == "-")
            {
                strSign = "-";
                strData = strData.Substring(1);

            }
            int intLen = strData.Length;
            for (int i = intLen - 1; i >= 0; i--)
            {
                iIndex++;
                StrTemp = strData.Substring(i, 1) + StrTemp;
                if (iIndex % 3 == 0)
                    StrTemp = "," + StrTemp;
            }

            if (StrTemp != "" && StrTemp[0] == ',')
            {
                StrTemp = StrTemp.Substring(1, StrTemp.Length - 1);
            }
            return (strSign + StrTemp);
        }

        private static int GetIntFromStr(string strData)
        {
            string strTemp = "";
            int intNum = 0;
            int intLen = strData.Length;
            for (int i = 0; i < intLen; i++)
            {
                if (Char.IsNumber(strData[i]))
                {
                    strTemp += strData.Substring(i, 1);
                }
            }

            if (strTemp != "")
            {
                intNum = int.Parse(strTemp);
            }
            return (intNum);
        }

        public static string Round(string strdata, int iDecimal)
        {
            strdata = strdata.Replace(",", "").Trim();
            int iPointLocate = strdata.IndexOf('.');
            bool bIsNegative = (strdata.IndexOf('-') != -1);
            if (iPointLocate <= 0) return strdata;

            int iDecimalScale = strdata.Length - iPointLocate - 1;
            if (iDecimalScale <= iDecimal) return strdata;
            int iDigit = int.Parse(strdata.Substring(iPointLocate + iDecimal + 1, 1));
            if (iDigit >= 5)
            {
                if (bIsNegative)
                    strdata = Convert.ToString(double.Parse(strdata.Substring(0, iPointLocate + iDecimal + 1)) - 1 / Math.Pow(10, iDecimal));
                else
                    strdata = Convert.ToString(double.Parse(strdata.Substring(0, iPointLocate + iDecimal + 1)) + 1 / Math.Pow(10, iDecimal));
            }
            else
            {
                strdata = Convert.ToString(double.Parse(strdata.Substring(0, iPointLocate + iDecimal + 1)));
            }
            return strdata;
        }

        #endregion

        #region public static string FormatWithZero()
        public static string FormatWithZero(string strField_Char, int intField_length)
        {

            string strForm_char = strField_Char.Trim();
            int iDiff = intField_length - strForm_char.Length;

            if (BaseFunctions.IsNumeric(strForm_char))
            {
                for (int i = 0; i < iDiff; i++)
                {
                    strForm_char = "0" + strForm_char;
                }
            }
            return strForm_char.ToUpper();
        }
        #endregion
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

        #endregion

        public static bool SendEmail(string strToEmail, string strSubject, string strBody)
        {
            try
            {
                //Microsoft.Office.Interop.Outlook.Application a = new Microsoft.Office.Interop.Outlook.Application();
                //Microsoft.Office.Interop.Outlook.MailItem oMail = (Microsoft.Office.Interop.Outlook.MailItem)a.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                //oMail.Subject = strSubject;
                //oMail.To = strToEmail;
                //oMail.Body = strBody;
                //oMail.Send();
                return true;
            }
            catch (Exception e)
            {
                //MsgBoxEx.Show(e.Message, SysVar.WarnTitle);
                return false;
            }
        }

    }
}
