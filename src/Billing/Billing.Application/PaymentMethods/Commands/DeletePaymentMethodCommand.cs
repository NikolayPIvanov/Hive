using System.Threading;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using Hive.Billing.Domain.Entities;
using Hive.Common.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.PaymentMethods.Commands
{
    public record DeletePaymentMethodCommand(int PaymentMethodId) : IRequest;
    
    public class DeletePaymentMethodCommandHandler : IRequestHandler<DeletePaymentMethodCommand>
    {
        private readonly IBillingDbContext _dbContext;

        public DeletePaymentMethodCommandHandler(IBillingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Unit> Handle(DeletePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var paymentMethod = await _dbContext.PaymentMethods
                .FirstOrDefaultAsync(x => x.Id == request.PaymentMethodId, cancellationToken);
            
            if (paymentMethod is null)
            {
                throw new NotFoundException(nameof(PaymentMethod), request.PaymentMethodId);
            }
            
            _dbContext.PaymentMethods.Remove(paymentMethod);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}