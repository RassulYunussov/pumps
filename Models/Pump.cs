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
        public bool UpdateState(float t,float p,float a,float vol,float vib,DateTime d)
        {
            bool changed = false;
            if(Temperature!=t)
            {
                Temperature = t;
                changed = true;
            }
            if(Pressure!=p)
            {
                Pressure = p;
                changed = true;
            }
            if(Ampers!=a)
            {
                Ampers = a;
                changed = true;
            }
            if(Volume!=vol)
            {
                Volume = vol;
                changed = true;
            }
            if(Vibration!=vib)
            {
                Vibration =vib;
                changed = true;
            }
            UpdateTime = d;
            return changed;
        }
    }
}