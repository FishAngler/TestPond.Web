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
    public class SingleDeviceRunModel : PageModel
    {
        private readonly CollectionRunService service;
        private ILogger<SingleDeviceRunModel> _logger;

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        public DeviceTestSuiteCollectionRun CollectionRun{ get; set; }
        public SingleDeviceTestSuiteRun SingleDeviceRun { get; set; }
        public List<TestCaseExecution> TestCaseExecutions { get; set; }
        public MobileBuild MobileBuild { get; set; }
        public MobileDevice MobileDevice { get; set; }

        public int PassedCount { get; set; }
        public int FailedCount { get; set; }
        public int SkippedCount { get; set; }

        public SingleDeviceRunModel(CollectionRunService service, ILogger<SingleDeviceRunModel> logger)
        {
            this.service = service;
            _logger = logger;
        }

        public async Task OnGet()
        {
            SingleDeviceRun = await service.GetSingleDeviceTestSuiteRun(Id);
            MobileDevice = SingleDeviceRun.MobileDevice;

            CollectionRun = SingleDeviceRun.DeviceTestSuiteCollectionRun;
            MobileBuild = CollectionRun.MobileAppBuild;

            TestCaseExecutions = SingleDeviceRun.TestCaseExecutions;

            PassedCount = TestCaseExecutions.Select(x => x.Result == TestResult.Passed).Count();
            FailedCount = TestCaseExecutions.Select(x => x.Result == TestResult.Failed).Count();
            SkippedCount = TestCaseExecutions.Select(x => x.Result == TestResult.Skipped).Count();
        }
    }
}