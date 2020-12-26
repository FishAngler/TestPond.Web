using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestPond.BusinessLayer.Models.DTO;
using TestPond.BusinessLayer.Services.CollectionRun;
using TestPond.Web.Attributes;

namespace TestPond.Web.API
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiKey]
    public class TestCaseAttachmentsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly CollectionRunService _service;
        private readonly ILogger<TestCaseAttachmentsController> _logger;

        public TestCaseAttachmentsController(IWebHostEnvironment env, CollectionRunService service, ILogger<TestCaseAttachmentsController> logger)
        {
            _env = env;
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromForm] TestCaseAttachmentsDTO attachments)
        {
            _logger.LogInformation($"{nameof(TestCaseAttachmentsController)}: Image Upload Received at {DateTime.Now.ToLocalTime()}!");

            try
            {
                if (attachments.TestFailureScreenshots.Count > 0)
                {
                    await _service.SaveTestCaseAttachments(attachments.TestFailureScreenshots);
                }
                else
                {
                    return BadRequest();
                }
                
                return Ok();

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var inner = ex.InnerException;

                return BadRequest();
            }
        }
    }
}