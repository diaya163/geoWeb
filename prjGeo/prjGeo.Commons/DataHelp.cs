using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace prjGeo.Commons
{
    public class DataHelp
    {
        static JavaScriptSerializer jss = new JavaScriptSerializer();
        public static string objToJson(object obj)
        {
            return jss.Serialize(obj);
        }
    }
}
