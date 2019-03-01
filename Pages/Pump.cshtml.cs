using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pumps.Application;
using pumps.Models;

namespace pumps.Pages
{
    public class PumpModel: PageModel
    {
        ApplicationContext _ctx;
        public Pump Pump { get; set; }
        public PumpModel(ApplicationContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<ActionResult> OnGetAsync(int id)
        {
            Pump = await _ctx.GetPump(id,false);
            if(Pump==null)
                return RedirectToPage("Index");
            return Page();
        }
    }
}