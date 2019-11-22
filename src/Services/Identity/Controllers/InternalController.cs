namespace Identity.WebApi.Controllers
{
    using Identity.WebApi.Services;
    using Microsoft.AspNetCore.Mvc;
    using System;

    [Route("api/internal")]
    [ApiController]
    public class InternalController : ControllerBase
    {
        private IUserService _userService;

        public InternalController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet("accountnumbers")]
        public IActionResult GetAccountNumbers()
        {
            var result = _userService.GetAccountNumbers();
            return Ok(result);
        }

        [HttpGet("useraccounts")]
        public IActionResult GetUserAccounts()
        {
            var result = _userService.GetUserAccounts();
            return Ok(result);
        }

        [HttpGet("useraccount/{accountnumber}")]
        public IActionResult GetUserAccounts(int accountnumber)
        {
            var result = _userService.GetUserAccount(accountnumber);
            return Ok(result);
        }
    }
}