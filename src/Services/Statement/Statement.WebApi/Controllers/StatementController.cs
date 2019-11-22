namespace Statement.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Statement.Framework.Services;
    using Statement.WebApi.Services;
    using System;
    using System.Threading.Tasks;

    [Route("api/statement")]
    [ApiController]
    public class StatementController : ControllerBase
    {
        private readonly IStatementService _statementService;
        private readonly IIdentityService _identityService;

        public StatementController(IStatementService statementService, IIdentityService identityService)
        {
            _statementService = statementService ?? throw new ArgumentNullException(nameof(statementService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        [HttpGet("{month}")]
        public async Task<IActionResult> Get(string month)
        {
            var identity = _identityService.GetIdentity();
            var result = await _statementService.GetAsync(identity.AccountNumber, month);
            return Ok(result);
        }
    }
}
