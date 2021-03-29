using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using Billing.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.PaymentMethods.Commands
{
    public record CreatePaymentMethodCommand(string Type) : IRequest<int>;

    public class CreatePaymentMethodCommandHandler : IRequestHandler<CreatePaymentMethodCommand, int>
    {
        private readonly IBillingContext _context;

        public CreatePaymentMethodCommandHandler(IBillingContext context)
        {
            _context = context;
        }
        
        public async Task<int> Handle(CreatePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = "src";
            var account = await _context.AccountHolders.Select(x => new {x.UserId, x.AccountId})
                .FirstOrDefaultAsync(x => x.UserId == currentUserId, cancellationToken);
            
            // TODO: Validation
            var paymentMethod = new PaymentMethod(request.Type, account.AccountId);
            _context.PaymentMethods.Add(paymentMethod);
            await _context.SaveChangesAsync(cancellationToken);

            return paymentMethod.Id;
        }
    }
}