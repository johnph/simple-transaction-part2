namespace Transaction.WebApi.Controllers
{
    using Transaction.WebApi.Models;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Transaction.Framework.Domain;
    using Transaction.Framework.Services.Interface;
    using Transaction.WebApi.Services;
    using System;

    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public AccountController(ITransactionService transactionService, IIdentityService identityService, IMapper mapper)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("balance")]
        public async Task<IActionResult> Balance()
        {
            var accountNumber = _identityService.GetIdentity().AccountNumber;
            var transactionResult = await _transactionService.Balance(accountNumber);
            return Ok(_mapper.Map<TransactionResultModel>(transactionResult));
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] TransactionModel accountTransactionModel)
        {
            var accountTransaction = _mapper.Map<AccountTransaction>(accountTransactionModel);            
            var result = await _transactionService.Deposit(accountTransaction);
            return Created(string.Empty, _mapper.Map<TransactionResultModel>(result));
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] TransactionModel accountTransactionModel)
        {
            var accountTransaction = _mapper.Map<AccountTransaction>(accountTransactionModel);            
            var result = await _transactionService.Withdraw(accountTransaction);
            return Created(string.Empty, _mapper.Map<TransactionResultModel>(result));
        }
    }
}
