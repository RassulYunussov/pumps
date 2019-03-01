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
        public readonly Dictionary<int,bool> PumpsState = new Dictionary<int,bool>();
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
                    PumpsState.Add(p.Id,false);
                }
            }
        }

        public async Task<Pump> GetPump(int pumpId,bool createNew)
        {
            Pump pump = null;
            if(Pumps.ContainsKey(pumpId))
                pump = Pumps[pumpId];
            else if(createNew) 
            {
                using(var scope = _sp.CreateScope())
                {
                    var ctx = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    pump = new Pump(){Name="Unnamed Pump",Pressure = 0,Temperature = 0,Vibration=0,Ampers=0,Volume=0};
                    ctx.Pumps.Add(pump);
                    Pumps.Add(pumpId,pump);
                    PumpsState.Add(pumpId,false);
                    await ctx.SaveChangesAsync();
                }
            }
            else {
                System.Console.WriteLine("Nu such pump");
            }
            return await Task.FromResult(pump);
        }

        public async Task RecordPumpData(Pump pump)
        {
            using(var scope = _sp.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetService<ApplicationDbContext>();
                ctx.Pumps.Update(pump);
                LogSensorData(ctx,pump);
                await ctx.SaveChangesAsync();
            }
        }

        internal void ResetPumpsState()
        {
           foreach (var k in PumpsState.Keys.ToList())
               PumpsState[k]=false;
        }

        private void LogSensorData(ApplicationDbContext ctx, Pump p)
        {
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
        public async Task RecordPumpsData()
        {
            using( var scope = _sp.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetService<ApplicationDbContext>();
                foreach(var p in Pumps.Values)
                {
                    ctx.Update(p);
                    LogSensorData(ctx,p);
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