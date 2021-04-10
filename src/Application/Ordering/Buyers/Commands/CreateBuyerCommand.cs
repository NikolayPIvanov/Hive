using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Application.Billing.AccountHolder.Commands;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Ordering.Buyers.Commands
{
    public record CreateBuyerCommand(string UserId) : IRequest<int>;

    public class CreateBuyerCommandValidator : AbstractValidator<CreateBuyerCommand>
    {
        public CreateBuyerCommandValidator(IApplicationDbContext context)
        {
            RuleFor(x => x.UserId)
                .MustAsync(async (id, token) => await context.Buyers.AnyAsync(x => x.UserId == id, token));
        }
    }

    public class CreateBuyerCommandHandler : IRequestHandler<CreateBuyerCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateBuyerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<int> Handle(CreateBuyerCommand request, CancellationToken cancellationToken)
        {
            var buyer = new Buyer(request.UserId);

            _context.Buyers.Add(buyer);
            await _context.SaveChangesAsync(cancellationToken);

            return buyer.Id;
        }
    }
}