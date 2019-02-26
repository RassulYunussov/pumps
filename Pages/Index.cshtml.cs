using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pumps.Application;
using pumps.Models;

namespace pumps.Pages
{
    public class IndexModel : PageModel
    {
        public Pump [] Pumps {get;}
        public IndexModel(ApplicationContext ctx)
        {
           Pumps = ctx.GetAllPumps();
        }
        public void OnGet()
        {

        }
    }
}
