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
    public class CollectionTestCaseSummaryModel : PageModel
    {
        private CollectionRunService service;
        private ILogger<CollectionTestCaseSummaryModel> _logger;

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        public DeviceTestSuiteCollectionRun CollectionRun { get; set; }

        public IEnumerable<IGrouping<string, TestCase>> TestCaseExecutionsGroupedByClassName { get; set; }

        public int PassedCount { get; set; }
        public int FailedCount { get; set; }
        public int SkippedCount { get; set; }

        public CollectionTestCaseSummaryModel(CollectionRunService service, ILogger<CollectionTestCaseSummaryModel> logger)
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

        public List<TestCaseExecution> GetPassingExecutions(TestCase tc)
        {
            return tc.TestCaseRuns.Where(x => x.Result == TestResult.Passed).ToList();
        }

        public List<TestCaseExecution> GetFailingExecutions(TestCase tc)
        {
            return tc.TestCaseRuns.Where(x => x.Result == TestResult.Failed).ToList();
        }
    }
}