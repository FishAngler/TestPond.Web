using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TestPond.Web.Pages
{
    public class NUnitXmlModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Content("Hello World");
        }
    }
}