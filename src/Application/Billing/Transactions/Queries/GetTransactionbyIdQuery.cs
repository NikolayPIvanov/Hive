using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Billing;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Billing.Transactions.Queries
{
    public record GetTransactionByIdQuery(int PublicId) : IRequest<TransactionDto>;

    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTransactionByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
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