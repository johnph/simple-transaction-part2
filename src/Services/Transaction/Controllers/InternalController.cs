namespace Transaction.WebApi.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using Transaction.Framework.Services.Interface;
    using Transaction.WebApi.Models;

    [Route("api/internal")]
    [ApiController]
    public class InternalController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public InternalController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{accountNumber}/statement/{month}")]
        public async Task<IActionResult> GetStatement(int accountNumber, string month)
        {
            var transactionResult = await _transactionService.Statement(accountNumber, month);
            return Ok(_mapper.Map<StatementResultModel>(transactionResult));
        }
    }
}
