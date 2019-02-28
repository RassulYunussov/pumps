using System;

namespace pumps.Models 
{
    public class Pump
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Temperature { get; set; }
        public float Pressure {get;set;}
        public float Ampers { get; set; }
        public float Volume { get; set; }
        public float Vibration { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}