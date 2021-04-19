using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Questions.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Gigs.Commands
{
    public record UpdateGigCommand : IRequest
    {
        public int Id { get; set; }
        
        public string Title { get; set; }

        public string Description { get; set; }
        
        public bool IsDraft { get; set; } = true;
        
        public int CategoryId { get; set; }
        
        public HashSet<string> Tags { get; init; }
        
        public UpdateGigCommand(int id, string title, string description, int categoryId, bool isDraft, HashSet<string> tags)
            => (Id, Title, Description, CategoryId, IsDraft, Tags) = 
                (id, title, description, categoryId, isDraft, tags ?? new HashSet<string>(5));
    }

    public class UpdateGigCommandValidator : AbstractValidator<UpdateGigCommand>
    {
        public UpdateGigCommandValidator(IGigManagementDbContext dbContext)
        {
            RuleFor(x => x.Title)
                .MaximumLength(50).WithMessage("Title length must not be above 50 characters.")
                .MinimumLength(3).WithMessage("Title length must not be below 3 characters.")
                .NotEmpty().WithMessage("Title should be provided.");

            RuleFor(x => x.CategoryId)
                .MustAsync(async (id, token) => await dbContext.Categories.AnyAsync(x => x.Id == id, cancellationToken: token))
                .WithMessage("Must provide an existing category id.");

            RuleFor(x => x.Tags)
                .Must(tags => tags.Count <= 5).WithMessage("Can provide up to 5 tags.");

            RuleForEach(x => x.Tags)
                .MinimumLength(3).WithMessage("Tag length must not be below 3 characters.")
                .MaximumLength(20).WithMessage("Tag length must not be above 20 characters.");
        }
    }

    public class UpdateGigCommandHandler : IRequestHandler<UpdateGigCommand>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateGigCommandHandler(IGigManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<Unit> Handle(UpdateGigCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Gigs
                .Include(g => g.GigScope)
                .Include(g => g.Tags)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            
            if (entity is null)
            {
                throw new NotFoundException(nameof(Gig), request.Id);
            }

            _mapper.Map(request, entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}