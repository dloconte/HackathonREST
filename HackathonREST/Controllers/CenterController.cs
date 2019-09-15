using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HackathonREST.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HackathonREST.Controllers
{
    [Route("centers")]
    [ApiController]
    public class CenterController : ControllerBase
    {
        private readonly CenterContext _context;

        public CenterController(CenterContext context)
        {
            if (context.Centers.Count() == 0)
            {
                var file = System.IO.File.ReadAllText(@"C:\Users\dloconte\Downloads\RESTapp\HackathonREST\HackathonREST\App-Data\centers.json");

                var jObject = JObject.Parse(file);

                if (jObject != null)
                {
                    JArray centersArray = (JArray)jObject["Centers"];
                    JArray centerTypesArray = (JArray)jObject["CenterTypes"];

                    List<String> centerTypes = new List<string>();

                    foreach (var type in centerTypesArray)
                    {

                        var centerType = type["Value"];
                        centerTypes.Add(centerType.ToString());
                    }

                    foreach (var center in centersArray)
                    {
                        Center c = new Center();

                        c.Id = (int)center["Id"];
                        c.Name = center["Name"].ToString();
                        c.StreetAddress = center["StreetAddress"].ToString();
                        c.CenterTypeId = (int)center["CenterTypeId"];

                        for (int i = 1; i <= centerTypesArray.Count; i++)
                        {

                            if ((int)center["CenterTypeId"] == i)
                            {
                                c.CenterTypeValue = centerTypes[i - 1];
                            }
                        }

                        context.Centers.Add(c);
                    }
                }
                context.SaveChangesAsync();
            }

            _context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Center>>> GetCenters()
        {
            return await _context.Centers.ToListAsync();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Center>> GetCenter(int id)
        {
            var appointment = await _context.Centers.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<Center>> PostCenter(Center c)
        {
            _context.Centers.Add(c);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCenter), new { id = c.Id }, c);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCenter(int id, Center c)
        {
            if (id != c.Id)
            {
                return BadRequest();
            }

            _context.Entry(c).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Centers.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            _context.Centers.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
