using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestPond.BusinessLayer.Models.DTO;
using TestPond.BusinessLayer.Services.CollectionRun;
using TestPond.Web.Attributes;

namespace TestPond.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiKey]
    public class CollectionRunController : Controller
    {
        private readonly CollectionRunService _collectionRunService;
        private readonly ILogger<CollectionRunController> _logger;

        public CollectionRunController(CollectionRunService collectionRunService,  ILogger<CollectionRunController> logger)
        {
            _collectionRunService = collectionRunService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _collectionRunService.GetCollectionRun(id));
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Put([FromBody] NUnitXMLCollectionRunDTO dto)
        {
            _logger.LogInformation($"{nameof(CollectionRunController)}: Collection Run Received at {DateTime.Now.ToLocalTime()}!");

            _collectionRunService.AddCollectionRun(dto);

            //TODO: return the realID
            //return CreatedAtAction("PostDeviceTestRun", new { id = 22 });
            //return Created("URI TBD", dto);
            return Ok("Thank you, please upload again");
        }
    }
}
