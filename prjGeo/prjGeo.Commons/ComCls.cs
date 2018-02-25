using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjGeo.Commons
{
    #region SystemDateOption class
    public class SystemDateOption
    {
        public static string optDateFmtMDY
        {
            get { return "MM/dd/yyyy"; }
        }
        public static string optFmtDtp
        {
            get { return "MM/dd/yyy"; }
        }
        public static string optDateFmtYMD
        {
            get { return "yyyy/MM/dd"; }
        }
        public static string optDateFmtMY
        {
            get { return "MM/yyyy"; }
        }
        public static string GetFormat(DateFormatOption opt)
        {
            switch (opt)
            {
                case DateFormatOption.DateFmtMDY:
                    return SystemDateOption.optDateFmtMDY;
                case DateFormatOption.FmtDtp:
                    return SystemDateOption.optFmtDtp;
                case DateFormatOption.DateFmtYMD:
                    return SystemDateOption.optDateFmtYMD;
                case DateFormatOption.DateFmtMY:
                    return SystemDateOption.optDateFmtMY;

            }
            return "";

        }
    }
    #endregion
}
