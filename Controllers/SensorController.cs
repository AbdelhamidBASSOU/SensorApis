using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SensorApis.Data;
using SensorApis.Models;

namespace SensorApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly SensorDbContext _context;

        public SensorController(SensorDbContext context)
        {
            _context = context;
        }

        // GET: api/Sensor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensors()
        {
            var sensors = await _context.Sensors.ToListAsync();
            return Ok(new { Message = "Sensors retrieved successfully", Data = sensors });
        }

        // GET api/Sensor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sensor>> GetSensor(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);
            if (sensor == null)
            {
                return NotFound(new { Message = "Sensor not found" });  // 404 if not found
            }
            return Ok(new { Message = "Sensor retrieved successfully", Data = sensor });
        }

        // POST api/Sensor
        [HttpPost]
        public async Task<ActionResult<Sensor>> PostSensor(Sensor sensor)
        {
            _context.Sensors.Add(sensor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSensor), new { id = sensor.Id }, new { Message = "Sensor created successfully", Data = sensor });
        }

        // PUT api/Sensor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSensor(int id, Sensor sensor)
        {
            if (id != sensor.Id)
            {
                return BadRequest(new { Message = "ID mismatch" });  // 400 if ID doesn't match
            }

            var existingSensor = await _context.Sensors.FindAsync(id);
            if (existingSensor == null)
            {
                return NotFound(new { Message = "Sensor not found" });  // 404 if not found
            }

            _context.Entry(existingSensor).State = EntityState.Detached;  // Detach existing entity
            _context.Entry(sensor).State = EntityState.Modified;  // Attach the updated entity

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Sensors.Any(e => e.Id == id))
                {
                    return NotFound(new { Message = "Sensor not found during concurrency check" });  // 404 if ID not found
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { Message = "Sensor updated successfully" });  // 204 No Content
        }

        // DELETE api/Sensor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensor(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);
            if (sensor == null)
            {
                return NotFound(new { Message = "Sensor not found" });  // 404 Not Found
            }

            _context.Sensors.Remove(sensor);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Sensor deleted successfully" });  // 204 No Content
        }
    }
}
