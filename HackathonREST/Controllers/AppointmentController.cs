using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HackathonREST.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HackathonREST.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentContext _context;

        public AppointmentController(AppointmentContext context)
        {
            _context = context;

            if(_context.Appointments.Count() == 0)
            {
                _context.Appointments.Add(new Appointment { clientFullName = "ABC DEF", date = "2019-09-13", centerId = 1});
                _context.Appointments.Add(new Appointment { clientFullName = "GHI JKL", date = "2019-09-14", centerId = 2});
                _context.SaveChanges();
            }
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return await _context.Appointments.ToListAsync();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostTodoItem(Appointment appt)
        {
            // Validation: make sure date is in format yyyy-MM-dd
            Regex rgx = new Regex(@"(^[12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$)");
            if (rgx.IsMatch(appt.date))
            {
                _context.Appointments.Add(appt);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAppointment), new { id = appt.id }, appt);
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, Appointment appt)
        {
            if (id != appt.id)
            {
                return BadRequest();
            }

            // Validation: make sure date is in format yyyy-MM-dd
            Regex rgx = new Regex(@"(^[12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$)");
            if (rgx.IsMatch(appt.date))
            {
                _context.Entry(appt).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
