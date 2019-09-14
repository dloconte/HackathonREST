using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackathonREST.Models
{
    public class Appointment
    {
        public int id { get; set; }
        public string clientFullName { get; set; }
        public string date { get; set; }
        public int centerId { get; set; }
    }
}
