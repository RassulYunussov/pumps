using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using pumps.Application;
using pumps.Models;

namespace pumps.Services
{
    public class SensorsUpdateService : IHostedService, IDisposable
    {
        readonly ILogger _logger;
        readonly ApplicationContext _ctx;
        Timer _timer;
        Random _r;

        public SensorsUpdateService(ILogger<SensorsUpdateService> logger,ApplicationContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
            _r = new Random();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Data Generation Started");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, 
                TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Pump[] pumps = _ctx.GetAllPumps();
            foreach(var p in pumps)
            {
                 _ctx.PumpsState[p.Id] = p.UpdateState( _r.Next(1,100), _r.Next(1,100), _r.Next(1,100), _r.Next(1,100), _r.Next(1,100),DateTime.Now);
            }
            _ctx.RecordPumpsData().Wait();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Data Generation is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}