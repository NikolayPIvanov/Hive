using System.Threading;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using Billing.Domain;
using Hive.Common.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.PaymentMethods.Commands
{
    public record DeletePaymentMethodCommand(int PaymentMethodId) : IRequest;
    
    public class DeletePaymentMethodCommandHandler : IRequestHandler<DeletePaymentMethodCommand>
    {
        private readonly IBillingContext _context;

        public DeletePaymentMethodCommandHandler(IBillingContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(DeletePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var paymentMethod = await _context.PaymentMethods
                .FirstOrDefaultAsync(x => x.Id == request.PaymentMethodId, cancellationToken);
            
            if (paymentMethod is null)
            {
                throw new NotFoundException(nameof(PaymentMethod), request.PaymentMethodId);
            }
            
            _context.PaymentMethods.Remove(paymentMethod);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}