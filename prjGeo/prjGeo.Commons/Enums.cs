using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjGeo.Commons
{
    public enum JsonStatu
    {
        ok = 1,
        err = 2,
        none = 3
    }
    public enum DateFormatOption
    {
        DateFmtMDY = 0,
        FmtDtp,
        DateFmtMD,
        DateFmtYMD,
        DateFmtMY
    }

    public enum DateInterval
    {
        Second,
        Minute,
        Hour,
        Day,
        Week,
        Month,
        Quarter,
        Year
    }
}
