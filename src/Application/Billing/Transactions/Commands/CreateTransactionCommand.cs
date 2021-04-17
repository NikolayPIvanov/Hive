using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Billing;
using Hive.Domain.Enums;
using MediatR;

namespace Hive.Application.Billing.Transactions.Commands
{
    public record CreateTransactionCommand(decimal Amount, Guid? OrderNumber, int WalletId, TransactionType TransactionType = TransactionType.Fund) : IRequest<int>;

    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateTransactionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<int> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction(request.Amount, request.OrderNumber, request.TransactionType, request.WalletId);

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            return transaction.PublicId;
        }
    }
}