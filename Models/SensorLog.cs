using System;

namespace pumps.Models
{
    public enum SensorType { Temperature, Pressure, Ampers, Volume, Vibration}
    public class SensorLog
    {
        public int Id { get; set; }
        public Pump Pump { get; set; }
        public SensorType Sensor { get; set; }
        public float Value { get; set; }
        public DateTime Date { get; set; }
    }
}