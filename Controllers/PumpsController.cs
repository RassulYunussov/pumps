using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pumps.Application;
using System.Text;
using pumps.Models;
using System;
using System.Linq;
using System.Collections.Generic;


namespace pumps.Controllers
{
    [Route("{controller}/{action}")]
    public class PumpsController: ControllerBase
    {
        ApplicationContext _ctx;
        public PumpsController(ApplicationContext ctx)
        {
            _ctx = ctx;
        }
        public async Task AddPumpData(int pumpId,float temp,float press,float amps,float vol,float vibr)
        {
            Pump pump = await _ctx.GetPump(pumpId,true);
            
            _ctx.PumpsState[pumpId] = pump.UpdateState(temp,press,amps,vol,vibr,DateTime.Now);
           
            await _ctx.RecordPumpData(pump);
        }
        public async Task<List<object[]>> Temperature(int pumpId)
        {
            List<object []> values = await _ctx.GetPumpTemperature(pumpId);
            return values;
        }
        public async Task<List<object[]>> Vibration(int pumpId)
        {
            List<object []> values = await _ctx.GetPumpVibration(pumpId);
            return values;
        }
        public async Task<List<object[]>> Ampers(int pumpId)
        {
            List<object []> values = await _ctx.GetPumpAmpers(pumpId);
            return values;
        }
        public async Task<List<object[]>> Volume(int pumpId)
        {
            List<object []> values = await _ctx.GetPumpVolume(pumpId);
            return values;
        }
        public async Task<List<object[]>> Pressure(int pumpId)
        {
            List<object []> values = await _ctx.GetPumpPressure(pumpId);
            return values;
        }
        public async Task Values()
        {
            Response.ContentType = "text/event-stream";
            try 
            {
                 StringBuilder sb = new StringBuilder();
                 while(!Request.HttpContext.RequestAborted.IsCancellationRequested) {
                    sb.Append("data:[");
                    Pump[] pumps = _ctx.GetAllPumps();
                    int counter=0;
                    foreach(var p in pumps)
                    {
                        if(_ctx.PumpsState[p.Id])
                        {
                            if(counter++>0)
                                sb.Append(",");
                            sb.Append($@"{{ ""pump"":{p.Id}, ""temp"":{p.Temperature}, ""volume"": {p.Volume}, ""vibration"": {p.Vibration}, ""ampers"": {p.Ampers}, ""pressure"": {p.Pressure} ,""ut"":""{p.UpdateTime.ToShortTimeString()}""}}");
                        }
                    }
                    sb.Append("]\n\n");
                    if(!_ctx.PumpsState.Values.All(s=>s==false))
                    {
                          byte [] buff = Encoding.UTF8.GetBytes(sb.ToString());
                          await Response.Body.WriteAsync(buff,0,buff.Length);
                          await Response.Body.FlushAsync();
                          _ctx.ResetPumpsState();
                    }
                    await Task.Delay(1000);
                    sb.Clear();
                }
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            finally
            {
                Response.Body.Close();
            }
           
        }
    }
}