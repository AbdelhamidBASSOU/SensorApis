namespace SensorApis.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public double Value { get; set; }
    }


}
