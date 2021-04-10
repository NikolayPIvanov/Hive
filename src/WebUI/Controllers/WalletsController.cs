using System.Threading.Tasks;
using Hive.Application.Billing.Transactions.Queries;
using Hive.Application.Billing.Wallets.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    public class WalletsController : ApiControllerBase
    {
        [HttpGet("personal")]
        public async Task<IActionResult> GetMyWallet() => Ok(await Mediator.Send(new GetMyWalletCommand()));
        
        [HttpGet("{walletId:int}/transactions")]
        public async Task<IActionResult> GetWalletTransactions(int walletId) => Ok(await Mediator.Send(new GetTransactionsByWalletQuery(walletId)));
    }
}