using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SensorApis.Data;  // Add the namespace for your DbContext
using SensorApis.Models;  // Assuming you have a Sensor model

namespace SensorApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly SensorDbContext _context;

        // Constructor that injects the SensorDbContext
        public SensorController(SensorDbContext context)
        {
            _context = context;
        }

        // GET: api/Sensor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensors()
        {
            // Return a list of all sensors from the database
            return await _context.Sensors.ToListAsync();
        }

        // GET api/Sensor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sensor>> GetSensor(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);
            if (sensor == null)
            {
                return NotFound();  // Return a 404 if the sensor is not found
            }

            return sensor;
        }

        // POST api/Sensor
        [HttpPost]
        public async Task<ActionResult<Sensor>> PostSensor(Sensor sensor)
        {
            _context.Sensors.Add(sensor);  // Add the new sensor to the database
            await _context.SaveChangesAsync();  // Save changes to the database

            // Return the created resource (status code 201) with the location of the new sensor
            return CreatedAtAction(nameof(GetSensor), new { id = sensor.Id }, sensor);
        }

        // PUT api/Sensor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSensor(int id, Sensor sensor)
        {
            if (id != sensor.Id)
            {
                return BadRequest();  // Return a 400 if the ID in the URL doesn't match the sensor object
            }

            _context.Entry(sensor).State = EntityState.Modified;  // Mark the sensor as modified
            await _context.SaveChangesAsync();  // Save the changes

            return NoContent();  // Return a 204 status code indicating the request was successful but has no content
        }

        // DELETE api/Sensor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensor(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);
            if (sensor == null)
            {
                return NotFound();  // Return a 404 if the sensor was not found
            }

            _context.Sensors.Remove(sensor);  // Remove the sensor from the database
            await _context.SaveChangesAsync();  // Save changes to the database

            return NoContent();  // Return a 204 status code indicating the deletion was successful
        }
    }
}
