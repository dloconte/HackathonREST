using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackathonREST.Models
{
    public class Center
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public int CenterTypeId { get; set; }
        public string CenterTypeValue { get; set; }
    }
}
