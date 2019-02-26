using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pumps.Application;
using System.Text;
using pumps.Models;
using System;

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
        public async Task Values()
        {
            Random r = new Random();
            Response.ContentType = "text/event-stream";
            try 
            {
                 while(!Request.HttpContext.RequestAborted.IsCancellationRequested) {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("data:[");
                    Pump[] pumps = _ctx.GetAllPumps();
                    int counter=0;
                    foreach(var p in pumps)
                    {
                        p.Temperature = r.Next(1,100);
                        p.Volume = r.Next(1,100);
                        p.Pressure = r.Next(1,100);
                        p.Ampers = r.Next(1,100);
                        p.Vibration = r.Next(1,100);
                        if(counter++>0)
                            sb.Append(",");
                        sb.Append($@"{{ ""pump"":{p.Id}, ""temp"":{p.Temperature}, ""volume"": {p.Volume}, ""vibration"": {p.Vibration}, ""ampers"": {p.Ampers}, ""pressure"": {p.Pressure} }}");
                    }
                    sb.Append("]\n\n");
                    byte [] buff = Encoding.UTF8.GetBytes(sb.ToString());
                    await Response.Body.WriteAsync(buff,0,buff.Length);
                    await Response.Body.FlushAsync();
                    await Task.Delay(1000);
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