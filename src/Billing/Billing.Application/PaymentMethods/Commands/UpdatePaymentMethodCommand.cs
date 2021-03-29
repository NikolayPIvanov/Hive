using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using Billing.Domain;
using Hive.Common.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.PaymentMethods.Commands
{
    public record UpdatePaymentMethodCommand(int PaymentMethodId, string Type) : IRequest;
    
    public class UpdatePaymentMethodCommandHandler : IRequestHandler<UpdatePaymentMethodCommand>
    {
        private readonly IBillingContext _context;

        public UpdatePaymentMethodCommandHandler(IBillingContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(UpdatePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var paymentMethod = await _context.PaymentMethods
                .FirstOrDefaultAsync(x => x.Id == request.PaymentMethodId, cancellationToken);
            // TODO: Validation
            if (paymentMethod is null)
            {
                throw new NotFoundException(nameof(PaymentMethod), request.PaymentMethodId);
            }

            paymentMethod.Type = request.Type;
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}