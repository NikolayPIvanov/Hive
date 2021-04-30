using System.Threading.Tasks;
using Billing.Application.Wallets.Commands;
using Billing.Application.Wallets.Queries;
using Hive.Common.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Management.Controllers
{
    [Authorize]
    public class AccountHoldersController : ApiControllerBase
    {
        [HttpGet("{accountHolderId:int}/wallets")]
        public async Task<IActionResult> GetMyWallet() => Ok(await Mediator.Send(new GetMyWalletCommand()));
        
        [HttpGet("{accountHolderId:int}/wallets/{walletId:int}")]
        public async Task<IActionResult> GetWalletTransactions(int walletId) => Ok(await Mediator.Send(new GetWalletByIdQuery(walletId)));

        [HttpPost("{accountHolderId:int}/wallets/{walletId:int}/transactions")]
        public async Task<IActionResult> CreateTransaction([FromRoute] int walletId, [FromBody] CreateTransactionCommand command)
        {
            if (walletId != command.WalletId)
            {
                return BadRequest();
            }
            var transactionId = await Mediator.Send(command);
            return Ok(transactionId);
        }
    }
}