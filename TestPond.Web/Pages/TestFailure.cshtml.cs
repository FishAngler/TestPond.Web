using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TestPond.BusinessLayer.Services.CollectionRun;
using TestPond.BusinessLayer.Models;
using Microsoft.Extensions.Hosting;
using System.IO;
using TestPond.Web.Utilities;

namespace TestPond.Web.Pages
{
    public class TestFailureModel : PageModel
    {
        private readonly CollectionRunService _service;
        private ILogger<TestFailureModel> _logger;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public MobileBuild MobileAppBuild{ get; set; }
        public TestCaseExecution TestExecution{ get; set; }

        private readonly IHostEnvironment _env;
        public string FailScreenshotDir { get; private set; }

        public string FailScreenshot { get; private set; }

        public string FormattedStackTrace{ get; private set; }

        public TestFailureModel(CollectionRunService service, IHostEnvironment env, ILogger<TestFailureModel> logger)
        {
            _service = service;
            _env = env;
            _logger = logger;
        }

        public async Task OnGet()
        {
            TestExecution = await _service.GetTestCaseExecution(Id);

            MobileAppBuild = TestExecution.SingleDeviceTestSuiteRun.DeviceTestSuiteCollectionRun.MobileAppBuild;

            //Test Failure Screenshots
            var attachments = TestExecution.Attachments;
            var screenshotAttachment = attachments?.Where(x => x.Description.Contains("Screenshot")).FirstOrDefault() ?? null;

            if (screenshotAttachment != null)
            {
                FailScreenshotDir = Url.Content("~/Uploads/Screenshots/");
                FailScreenshot = Path.Combine(FailScreenshotDir, screenshotAttachment.FilePath);
            }

            //Test Failure Stack Trace
            if (!string.IsNullOrEmpty(TestExecution.FailureStackTrace))
            {
                FormattedStackTrace = FormatStackTrace(TestExecution.FailureStackTrace);
            }
        }

        public string FormatStackTrace(string stackTraceText)
        {
            
            if (string.IsNullOrWhiteSpace(stackTraceText))
            {
                return string.Empty;
            }

            string html;
            try
            {
                var formattedStackTrace = StackTraceFormatter.FormatHtml(stackTraceText,
                    new StackTraceHtmlFragments
                    {
                        BeforeFrame = "<span class='frame'>",
                        AfterFrame = "</span>",
                    });

                html = $"<pre><code>{formattedStackTrace}</code></pre>";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not parse Stack Trace for \n{stackTraceText}");
                html = $"Stack Trace could not be parsed/formatted:<br /> {ex.Message}";
            }

            return html;
        }
    }
}