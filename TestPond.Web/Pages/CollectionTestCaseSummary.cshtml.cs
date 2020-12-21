using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TestPond.BusinessLayer.Services.CollectionRun;
using TestPond.BusinessLayer.Models;

namespace TestPond.Web.Pages
{
    public class DeviceRunTestCaseSummaryModel : PageModel
    {
        private CollectionRunService service;
        private ILogger<DeviceRunTestCaseSummaryModel> _logger;

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        public DeviceTestSuiteCollectionRun CollectionRun { get; set; }

        public IEnumerable<IGrouping<string, TestCase>> TestCaseExecutionsGroupedByClassName { get; set; }

        public int PassedCount { get; set; }
        public int FailedCount { get; set; }
        public int SkippedCount { get; set; }

        public DeviceRunTestCaseSummaryModel(CollectionRunService service, ILogger<DeviceRunTestCaseSummaryModel> logger)
        {
            this.service = service;
            _logger = logger;
        }

        public async Task OnGet()
        {
            CollectionRun = await service.GetCollectionRun(Id);
            TestCaseExecutionsGroupedByClassName = await service.GetTestCaseSummary(Id);

            var results = CollectionRun.SingleDeviceTestSuiteRuns
                .SelectMany(x => x.TestCaseExecutions)
                .Select(x => x.Result);

            PassedCount = results.Count(x => x == TestResult.Passed);
            FailedCount = results.Count(x => x == TestResult.Failed);
            SkippedCount = results.Count(x => x == TestResult.Skipped);
        }
    }
}