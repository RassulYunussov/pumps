using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using pumps.Database;
using pumps.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace pumps.Application
{
    public class ApplicationContext
    {
        readonly Dictionary<int,Pump> Pumps  = new Dictionary<int,Pump>();
        public ApplicationContext(IServiceProvider serviceProvider)
        {
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
        public Pump GetPump(int Id)
        {
            if(Pumps.ContainsKey(Id))
                return Pumps[Id];
            throw new Exception("Now Pump with selected Id");
        }
        public Pump[] GetAllPumps()
        {
            return Pumps.Values.ToArray();
        }
    }
}