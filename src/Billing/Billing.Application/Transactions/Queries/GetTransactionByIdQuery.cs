using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Billing.Application.Interfaces;
using Hive.Billing.Domain.Entities;
using Hive.Common.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.Transactions.Queries
{
    public record GetTransactionByIdQuery(int PublicId) : IRequest<TransactionDto>;

    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDto>
    {
        private readonly IBillingDbContext _context;
        private readonly IMapper _mapper;

        public GetTransactionByIdQueryHandler(IBillingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<TransactionDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction =
                await _context.Transactions.FirstOrDefaultAsync(t => t.PublicId == request.PublicId, cancellationToken);

            if (transaction == null)
            {
                throw new NotFoundException(nameof(Transaction), request.PublicId);
            }

            return _mapper.Map<TransactionDto>(transaction);
        }
    }
}