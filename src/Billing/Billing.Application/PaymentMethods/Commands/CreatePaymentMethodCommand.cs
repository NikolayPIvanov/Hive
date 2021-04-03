using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using Hive.Billing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.PaymentMethods.Commands
{
    public record CreatePaymentMethodCommand(string Type) : IRequest<int>;

    public class CreatePaymentMethodCommandHandler : IRequestHandler<CreatePaymentMethodCommand, int>
    {
        private readonly IBillingDbContext _dbContext;

        public CreatePaymentMethodCommandHandler(IBillingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<int> Handle(CreatePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = "src";
            var account = await _dbContext.AccountHolders.Select(x => new {x.UserId, x.AccountId})
                .FirstOrDefaultAsync(x => x.UserId == currentUserId, cancellationToken);
            
            // TODO: Validation
            var paymentMethod = new PaymentMethod(request.Type, account.AccountId);
            _dbContext.PaymentMethods.Add(paymentMethod);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return paymentMethod.Id;
        }
    }
}