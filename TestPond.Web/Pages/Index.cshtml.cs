using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TestPond.BusinessLayer.Models;
using TestPond.BusinessLayer.Services.CollectionRun;

namespace TestPond.Web.Pages
{
    public class IndexRowModel : PageModel
    {
        private readonly CollectionRunService service;
        private readonly ILogger<IndexModel> _logger;

        public IList<DeviceTestSuiteCollectionRun> collectionRuns;

        [BindProperty]
        public DeviceTestSuiteCollectionRun CollectionRun { get; set; }

        public IndexRowModel(CollectionRunService service, ILogger<IndexModel> logger)
        {
            this.service = service;
            _logger = logger;
        }

        public async Task OnGet()
        {
            collectionRuns = await service.GetCollectionRuns();

            if (collectionRuns.Count() == 0)
            {
                return;
            }
        }

        public IActionResult OnPost()
        {
            //Console.WriteLine(CollectionRun.MobileAppBuild.BuildDefinitionName);

            return RedirectToPage("./Index");
        }
    }
}