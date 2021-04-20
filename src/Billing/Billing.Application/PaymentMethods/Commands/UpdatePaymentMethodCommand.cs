using System.Threading;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using Hive.Billing.Domain.Entities;
using Hive.Common.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.PaymentMethods.Commands
{
    public record UpdatePaymentMethodCommand(int PaymentMethodId, string Alias) : IRequest;
    
    public class UpdatePaymentMethodCommandHandler : IRequestHandler<UpdatePaymentMethodCommand>
    {
        private readonly IBillingDbContext _dbContext;

        public UpdatePaymentMethodCommandHandler(IBillingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Unit> Handle(UpdatePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var paymentMethod = await _dbContext.PaymentMethods
                .FirstOrDefaultAsync(x => x.Id == request.PaymentMethodId, cancellationToken);
            // TODO: Validation
            if (paymentMethod is null)
            {
                throw new NotFoundException(nameof(PaymentMethod), request.PaymentMethodId);
            }

            paymentMethod.Alias = request.Alias;
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}