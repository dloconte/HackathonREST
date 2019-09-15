using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackathonREST.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HackathonREST.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CenterTypeController : Controller
    {
        private CenterTypeContext _context;

        public CenterTypeController(CenterTypeContext context)
        {
            if(context.CenterTypes.Count() == 0)
            { 
                var file = System.IO.File.ReadAllText(@"C:\Users\dloconte\Downloads\RESTapp\HackathonREST\HackathonREST\App-Data\centers.json");

                var jObject = JObject.Parse(file);

                if (jObject != null)
                {
                    JArray centerTypesArray = (JArray)jObject["CenterTypes"];

                    foreach (var centerType in centerTypesArray)
                    {
                        CenterType ctype = new CenterType();

                        ctype.Id = (int)centerType["Id"];
                        ctype.Value = centerType["Value"].ToString();

                        context.CenterTypes.Add(ctype);
                    }
                }
                context.SaveChangesAsync();
            }
            _context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CenterType>>> GetCenterTypes()
        {
            return await _context.CenterTypes.ToListAsync();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CenterType>> GetCenterType(int id)
        {
            var appointment = await _context.CenterTypes.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<CenterType>> PostCenterType(CenterType ctype)
        {
            _context.CenterTypes.Add(ctype);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCenterType), new { id = ctype.Id }, ctype);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCenterType(int id, CenterType ctype)
        {
            if (id != ctype.Id)
            {
                return BadRequest();
            }

            _context.Entry(ctype).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.CenterTypes.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            _context.CenterTypes.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
