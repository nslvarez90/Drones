using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Drones.Models;

namespace Drones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DronsController : ControllerBase
    {
        private readonly DronContext _context;

        public DronsController(DronContext context)
        {
            _context = context;
        }

        // GET: api/Drons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dron>>> Drons()
        {
          if (_context.DronItems == null)
          {
              return NotFound();
          }
            return await _context.DronItems.ToListAsync();
        }

        // GET: api/Drons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dron>> GetDron(int id)
        {
          if (_context.DronItems == null)
          {
              return NotFound();
          }
            var dron = await _context.DronItems.FindAsync(id);

            if (dron == null)
            {
                return NotFound();
            }

            return dron;
        }

        // PUT: api/Drons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDron(int id, Dron dron)
        {
            if (id != dron.Id)
            {
                return BadRequest();
            }

            _context.Entry(dron).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DronExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Drons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Dron>> PostDron(Dron dron)
        {
          if (_context.DronItems == null)
          {
              return Problem("Entity set 'DronContext.DronItems'  is null.");
          }
            _context.DronItems.Add(dron);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDron", new { id = dron.Id }, dron);
        }

        // DELETE: api/Drons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDron(int id)
        {
            if (_context.DronItems == null)
            {
                return NotFound();
            }
            var dron = await _context.DronItems.FindAsync(id);
            if (dron == null)
            {
                return NotFound();
            }

            _context.DronItems.Remove(dron);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DronExists(int id)
        {
            return (_context.DronItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // POST: api/AddMedicineDron
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddMedicineDron/{id}")]
        public async Task<ActionResult<Dron>> AddMedicineDron(List<Medicine> medicines,int dronId)
        {
            if (_context.DronItems == null)
            {
                return Problem("Entity set 'DronContext.DronItems'  is null.");
            }
            var dron = await _context.DronItems.FindAsync(dronId);
            if( dron == null)
            {
                return Problem("Entity set 'DronContext.DronItems'  is null.");
            }
            int medicinesWeight = weightTotal(medicines);
            if (medicinesWeight > dron.maxWeight ) {
                return Problem("Medicine weight is more of Dron weight support.");
            }
            dron.medicines=medicines.ToList();
            _context.Entry(dron).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DronExists(dronId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }
        private int weightTotal(List<Medicine>  medicines) {
            int total = 0;
            medicines.ForEach(medicine =>
            {
                total += medicine.weight;
            });
            return total;
        }

        // GET: api/GetAllDronsAviable/
        [HttpGet("GetAllDronsAviable")]
        public async Task<ActionResult<IEnumerable<Dron>>> GetAllDronsAviable()
        {
            if (_context.DronItems == null)
            {
                return NotFound();
            }
            List<Dron> drons = await _context.DronItems.ToListAsync();
            List<Dron> aviables = new List<Dron>();
            drons.ForEach(dron =>
            {
                if (weightTotal(dron.medicines) < dron.maxWeight)
                {
                    aviables.Add(dron);
                }
            });            
            return  aviables.ToList();
        }

        //GET: api/DronBatteryStatus
        [HttpGet("GetAllDronsAviable/{id}")]
        public async Task<ActionResult<float>> GetAllDronsAviable(int id)
        {
            if (_context.DronItems == null)
            {
                return NotFound();
            }
            var dron = await _context.DronItems.FindAsync(id);
            if (dron == null)
            {
                return NotFound();
            }
            return Ok(dron.batteryCapacity);
        }

        //GET: api/DronWeight
        [HttpGet("DronWeight/{id}")]
        public async Task<ActionResult<float>> DronWeight(int id)
        {
            if (_context.DronItems == null)
            {
                return NotFound();
            }
            var dron = await _context.DronItems.FindAsync(id);
            if (dron == null)
            {
                return NotFound();
            }
            return Ok(weightTotal(dron.medicines));
        }
    }
}
