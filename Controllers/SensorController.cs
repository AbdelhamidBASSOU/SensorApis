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
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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

        // v1.0: GET api/v1/Sensor
        [HttpGet, MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensorsV1()
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

        // v2.0: GET api/v2/Sensor
        [HttpGet, MapToApiVersion("2.0")]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensorsV2()
        {
            var sensors = await _context.Sensors.ToListAsync();

            return Ok(new
            {
                Message = "Sensors retrieved successfully",
                TotalCount = sensors.Count,
                Data = sensors
            });
        }

        // v1.0: GET api/v1/Sensor/5
        [HttpGet("{id}"), MapToApiVersion("1.0")]
        public async Task<ActionResult<Sensor>> GetSensorV1(int id)
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

        // v2.0: GET api/v2/Sensor/5
        [HttpGet("{id}"), MapToApiVersion("2.0")]
        public async Task<ActionResult<Sensor>> GetSensorV2(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);
            if (sensor == null)
            {
                return NotFound(new { Message = "Sensor not found", SensorId = id });
            }

            return Ok(new
            {
                Message = "Sensor retrieved successfully",
                Data = sensor,
                Timestamp = DateTime.UtcNow
            });
        }

        // Shared: POST api/Sensor
        [HttpPost]
        public async Task<ActionResult<Sensor>> PostSensor(Sensor sensor)
        {
            _context.Sensors.Add(sensor);
            await _context.SaveChangesAsync();
            _cache.Remove("sensors");

            return CreatedAtAction(nameof(GetSensorV1), new { id = sensor.Id }, new { Message = "Sensor created successfully", Data = sensor });
        }

        // Shared: PUT api/Sensor/5
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

        // Shared: DELETE api/Sensor/5
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
