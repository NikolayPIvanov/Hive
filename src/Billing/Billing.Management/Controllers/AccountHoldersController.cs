using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Billing.Application.Wallets.Commands;
using Billing.Application.Wallets.Queries;
using Hive.Common.Core.Models;
using Hive.Common.Core.Security;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Stripe;

namespace Billing.Management.Controllers
{
    [Authorize]
    public class AccountHoldersController : ApiControllerBase
    {
        [HttpGet("wallet")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(WalletDto), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        public async Task<ActionResult<WalletDto>> GetWallet([FromQuery] int pageSize = 5) => 
            Ok(await Mediator.Send(new GetMyWalletCommand(pageSize)));
        
        [HttpGet("{accountHolderId:int}/wallets/{walletId:int}/transactions")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PaginatedList<TransactionDto>), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        public async Task<ActionResult<PaginatedList<TransactionDto>>> GetWalletTransactions(int walletId, [FromQuery] int pageIndex = 1, int pageSize = 5) 
            => Ok(await Mediator.Send(new GetWalletTransactionsQuery(walletId, pageIndex, pageSize)));

        [HttpPost("{accountHolderId:int}/wallets/{walletId:int}/transactions")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(int), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestObjectResult), Description = "Bad Request operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        public async Task<IActionResult> DepositInWallet([FromRoute] int accountHolderId, int walletId, [FromBody] CreateTransactionCommand command)
        {
            if (walletId != command.WalletId)
            {
                return BadRequest();
            }
            
            var transactionId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetTransactionById), new {accountHolderId, walletId, transactionNumber = transactionId},
                new {accountHolderId, walletId, transactionId});
        }
        
        [HttpGet("{accountHolderId:int}/wallets/{walletId:int}/transactions/{transactionNumber:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(TransactionDto), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        public async Task<IActionResult> GetTransactionById([FromRoute] int accountHolderId, int walletId, int transactionNumber)
        {
            var transaction = await Mediator.Send(new GetTransactionByIdQuery(transactionNumber));
            return Ok(transaction);
        }
    }
}