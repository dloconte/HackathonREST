using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackathonREST.Models
{
    public class Center
    {
        public int id { get; set; }
        public string name { get; set; }
        public string streetAddress { get; set; }
        public int centerTypeId { get; set; }
        public string centerTypeValue { get; set; }
    }
}
