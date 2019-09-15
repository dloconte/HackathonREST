using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HackathonREST.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string ClientFullName { get; set; }
        public string Date { get; set; }
        public int CenterId { get; set; }
    }

    public class AppointmentResult
    {
        public int Id { get; set; }
        public string ClientFullName { get; set; }
        public string Date { get; set; }
        public Center Center { get; set; }
    }
}
