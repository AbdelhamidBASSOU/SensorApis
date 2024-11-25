using System.ComponentModel.DataAnnotations;

namespace SensorApis.Models
   
{
    public class Sensor
    {
        public int Id { get; set; }

        [Required]
        public  string Name { get; set; }

        [Required]
        public  string Type { get; set; }

        public double Value { get; set; }
    }


}
