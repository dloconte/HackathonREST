using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HackathonREST.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HackathonREST.Controllers
{ 
    [Route("[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentContext _context;
        private readonly CenterContext _cenContext;
        private readonly AppointmentResultContext _resultContext;

        public AppointmentController(AppointmentContext context, CenterContext cenContext, AppointmentResultContext resultContext)
        {
            CenterController _cenControl = new CenterController(cenContext); 
            _context = context;
            _cenContext = cenContext;
            _resultContext = resultContext;

            if(_context.Appointments.Count() == 0)
            {
                _context.Appointments.Add(new Appointment { ClientFullName = "ABC DEF", Date = "2019-09-13", CenterId = 1});
                _context.Appointments.Add(new Appointment { ClientFullName = "GHI JKL", Date = "2019-09-14", CenterId = 2});
              
                _context.SaveChanges();    
            }

            if(_resultContext.AppointmentResults.Count() == 0)
            {
                _resultContext.AppointmentResults.Add(new AppointmentResult { ClientFullName = "ABC DEF", Date = "2019-09-13", Center = _cenContext.Centers.Single(c => c.Id == 1) });
                _resultContext.AppointmentResults.Add(new AppointmentResult { ClientFullName = "GHI JKL", Date = "2019-09-14", Center = _cenContext.Centers.Single(c => c.Id == 2) });
                _resultContext.SaveChanges();
            }
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentResult>>> GetAppointments()
        {
            foreach(AppointmentResult result in _resultContext.AppointmentResults)
            {
                int cId = _context.Appointments.Single(a => a.Id == result.Id).CenterId;
                // Load in Center from _cenContext by grabbing the center that matches the CenterId of the appointment that matches the result's id
                result.Center = _cenContext.Centers.Single(c => c.Id == cId);
            }

            return await _resultContext.AppointmentResults.ToListAsync();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentResult>> GetAppointment(int id)
        {
            try
            {
                var appointment = await _resultContext.AppointmentResults.FindAsync(id);
                int cId = _context.Appointments.Single(a => a.Id == appointment.Id).CenterId;
                // Load in Center from _cenContext by grabbing the center that matches the CenterId of the appointment that matches the result's id
                appointment.Center = _cenContext.Centers.Single(c => c.Id == cId);
                return appointment;
            }
            catch
            {
                return NotFound("No appointment with ID " + id + " exists.");
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostTodoItem(Appointment appt)
        {
            // Validation: make sure date is in format yyyy-MM-dd
            Regex rgx = new Regex(@"(^[12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$)");
            if (rgx.IsMatch(appt.Date))
            {
                // Validation: only one appointment can be at a location per day
                foreach (Appointment apptMade in _context.Appointments)
                {
                    if (appt.Date.Equals(apptMade.Date) && appt.CenterId == apptMade.CenterId)
                    {
                        return BadRequest("An appointment at center " + apptMade.CenterId + " on " + apptMade.Date + " exists.");
                    }
                }
                _context.Appointments.Add(appt);
                await _context.SaveChangesAsync();

                // Create an Appointment Result object with the same information, but load in the center, using the ID from the appt added
                AppointmentResult result = new AppointmentResult
                {
                    Id = appt.Id,
                    Date = appt.Date,
                    ClientFullName = appt.ClientFullName,
                    Center = null
                };
                _resultContext.AppointmentResults.Add(result);

                await _resultContext.SaveChangesAsync();
                return Ok("Appointment created with ID " + appt.Id);
            }
            else
            {
                return BadRequest("Date is not in correct format. Please Re-Enter in the format: \"yyyy-MM-dd\".");
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, Appointment appt)
        {
            if (id != appt.Id)
            {
                return BadRequest("No request exists with that ID.");
            }

            // Validation: make sure date is in format yyyy-MM-dd
            Regex rgx = new Regex(@"(^[12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$)");
            if (rgx.IsMatch(appt.Date))
            {
                // Validation: only one appointment can be at a location per day
                foreach (Appointment apptMade in _context.Appointments)
                {
                    if (appt.Date.Equals(apptMade.Date) && appt.CenterId == apptMade.CenterId)
                    {
                        return BadRequest("An appointment at center " + apptMade.CenterId + " on " + apptMade.Date + " exists.");
                    }
                }

                _context.Entry(appt).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Create an Appointment Result object with the same information, but load in the center, using the ID from the appt added
                AppointmentResult result = _resultContext.AppointmentResults.Single(r => r.Id == id);
                result.Date = appt.Date;
                result.ClientFullName = appt.ClientFullName;
                _resultContext.Entry(result).State = EntityState.Modified;

                await _resultContext.SaveChangesAsync();
            }
            else
            {
                return BadRequest("Date is not in correct format. Please Re-Enter in the format: \"yyyy-MM-dd\".");
            }

            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            var result = await _resultContext.AppointmentResults.FindAsync(id);

            if (appointment == null)
            {
                return NotFound("No appointment with ID " + id + "exists.");
            }

            _context.Appointments.Remove(appointment);
            _resultContext.AppointmentResults.Remove(result);
            await _context.SaveChangesAsync();
            await _resultContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
