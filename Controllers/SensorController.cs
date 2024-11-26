using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SensorApis.Data;
using SensorApis.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly SensorDbContext _context;
        private readonly IMemoryCache _cache;

        public SensorController(SensorDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: api/Sensor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensors()
        {
            if (!_cache.TryGetValue("sensors", out List<Sensor>? sensors))
            {
                var sensorList = await _context.Sensors.ToListAsync();
                sensors = sensorList ?? new List<Sensor>();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set("sensors", sensors, cacheEntryOptions);
            }

            return Ok(new { Message = "Sensors retrieved successfully", Data = sensors });
        }

        // GET api/Sensor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sensor>> GetSensor(int id)
        {
            if (!_cache.TryGetValue($"sensor_{id}", out Sensor? sensor))
            {
                var sensorFromDb = await _context.Sensors.FindAsync(id);
                if (sensorFromDb == null)
                {
                    return NotFound(new { Message = "Sensor not found" });
                }
                sensor = sensorFromDb;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set($"sensor_{id}", sensor, cacheEntryOptions);
            }

            return Ok(new { Message = "Sensor retrieved successfully", Data = sensor });
        }

        // POST api/Sensor
        [HttpPost]
        public async Task<ActionResult<Sensor>> PostSensor(Sensor sensor)
        {
            _context.Sensors.Add(sensor);
            await _context.SaveChangesAsync();
            _cache.Remove("sensors");

            return CreatedAtAction(nameof(GetSensor), new { id = sensor.Id }, new { Message = "Sensor created successfully", Data = sensor });
        }

        // PUT api/Sensor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSensor(int id, Sensor sensor)
        {
            if (id != sensor.Id)
            {
                return BadRequest(new { Message = "ID mismatch" });
            }

            var existingSensor = await _context.Sensors.FindAsync(id);
            if (existingSensor == null)
            {
                return NotFound(new { Message = "Sensor not found" });
            }

            _context.Entry(existingSensor).State = EntityState.Detached;
            _context.Entry(sensor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Sensors.Any(e => e.Id == id))
                {
                    return NotFound(new { Message = "Sensor not found during concurrency check" });
                }
                else
                {
                    throw;
                }
            }

            _cache.Remove("sensors");
            _cache.Remove($"sensor_{id}");

            return Ok(new { Message = "Sensor updated successfully" });
        }

        // DELETE api/Sensor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensor(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);
            if (sensor == null)
            {
                return NotFound(new { Message = "Sensor not found" });
            }

            _context.Sensors.Remove(sensor);
            await _context.SaveChangesAsync();

            _cache.Remove("sensors");
            _cache.Remove($"sensor_{id}");

            return Ok(new { Message = "Sensor deleted successfully" });
        }
    }
}
