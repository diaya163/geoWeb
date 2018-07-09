using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; 

namespace prjGeo.Models.Buss
{
    public class HSAllSeq
    {
        public int id { get; set; }

        public string HSID { get; set; }
        public string HSResult { get; set; }
        public decimal sumNAP { get; set; }
        public int SumSeqNO { get; set; }
        public string GeCata { get; set; }
        public string ValueCata { get; set; }

    }
}
