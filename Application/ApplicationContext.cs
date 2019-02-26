using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using pumps.Database;
using pumps.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace pumps.Application
{
    public class ApplicationContext
    {
        readonly Dictionary<int,Pump> Pumps  = new Dictionary<int,Pump>();
        readonly IServiceProvider _sp;
        public ApplicationContext(IServiceProvider serviceProvider)
        {
            _sp = serviceProvider;
            using( var scope = serviceProvider.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var pumps = from p in ctx.Pumps.AsNoTracking() select p;
                foreach (var p in pumps)
                {
                    Pumps.Add(p.Id,p);
                }
            }
        }
        public async Task RecordPumpsData()
        {
            using( var scope = _sp.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetService<ApplicationDbContext>();
                foreach(var p in Pumps.Values)
                {
                    ctx.Attach(p);
                    SensorLog slTemp = new SensorLog();
                    slTemp.Pump = p;
                    slTemp.Sensor = SensorType.Temperature;
                    slTemp.Value = p.Temperature;
                    slTemp.Date = DateTime.Now;
                    ctx.SensorLogs.Add(slTemp);

                    SensorLog slVol = new SensorLog();
                    slVol.Pump = p;
                    slVol.Sensor = SensorType.Volume;
                    slVol.Value = p.Volume;
                    slVol.Date = DateTime.Now;
                    ctx.SensorLogs.Add(slVol);

                    SensorLog slPress = new SensorLog();
                    slPress.Pump = p;
                    slPress.Sensor = SensorType.Pressure;
                    slPress.Value = p.Pressure;
                    slPress.Date = DateTime.Now;
                    ctx.SensorLogs.Add(slPress);

                    SensorLog slAmp = new SensorLog();
                    slAmp.Pump = p;
                    slAmp.Sensor = SensorType.Ampers;
                    slAmp.Value = p.Ampers;
                    slAmp.Date = DateTime.Now;
                    ctx.SensorLogs.Add(slAmp);

                    SensorLog slVibr = new SensorLog();
                    slVibr.Pump = p;
                    slVibr.Sensor = SensorType.Vibration;
                    slVibr.Value = p.Vibration;
                    slVibr.Date = DateTime.Now;
                    ctx.SensorLogs.Add(slVibr);
                }
                await ctx.SaveChangesAsync();
            } 
        }
        public Pump[] GetAllPumps()
        {
            return Pumps.Values.ToArray();
        }
    }
}