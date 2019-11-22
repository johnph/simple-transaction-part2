namespace Publisher.Service.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Publisher.Framework.Services;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/publish")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        private readonly IPublishService _publishService;

        public PublishController(IPublishService publishService)
        {
            _publishService = publishService ?? throw new ArgumentNullException(nameof(publishService));
        }

        [HttpPost("statement")]
        public async Task<IActionResult> Post()
        {
            await _publishService.PublishAsync(DateTime.Now.AddMonths(-1).ToString("MMM-yyyy"));
            return NoContent();
        }

        [HttpPost("statement/{month}")]
        public async Task<IActionResult> Post([FromRoute] string month)
        {
            await _publishService.PublishAsync(month);
            return NoContent();
        }

        [HttpPost("statement/{month}/accountnumbers")]
        public async Task<IActionResult> Post([FromRoute] string month, [FromBody] List<int> accountNumbers)
        {
            await _publishService.PublishAsync(month, accountNumbers);
            return NoContent();
        }
    }
}