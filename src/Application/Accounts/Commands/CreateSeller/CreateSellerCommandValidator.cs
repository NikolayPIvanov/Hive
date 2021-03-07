using System.Data;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Accounts.Commands.CreateSeller
{
    public class CreateSellerCommandValidator : AbstractValidator<CreateSellerCommand.Command>
    {
        private readonly IApplicationDbContext _context;

        public CreateSellerCommandValidator(IApplicationDbContext context)
        {
            _context = context;
            
            RuleFor(c => c.UserId)
                .MustAsync(NotExistAccountAlreadyAsync).WithMessage("A seller account already exists.")
                .NotEmpty();
        }

        private async Task<bool> NotExistAccountAlreadyAsync(string userId, CancellationToken cancellationToken)
        {
            return !(await _context.Sellers.AnyAsync(c => c.UserId == userId, cancellationToken));
        }
    }
}