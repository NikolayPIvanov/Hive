using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security.Handlers;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Gigs.Commands
{
    using Domain.Entities;
    
    public record UpdateGigCommand(int Id, string Title, string Description, bool IsDraft, int? PlanId,
        ICollection<string> Tags, ICollection<QuestionModel> Questions) : IRequest;

    public class UpdateGigCommandValidator : AbstractValidator<UpdateGigCommand>
    {
        public UpdateGigCommandValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(50).WithMessage("Title length must not be above 50 characters.")
                .MinimumLength(3).WithMessage("Title length must not be below 3 characters.")
                .NotEmpty().WithMessage("Title should be provided.");
            
            RuleFor(x => x.Description)
                .MaximumLength(2500).WithMessage("Description length must not be above 2500 characters.")
                .MinimumLength(10).WithMessage("Description length must not be below 10 characters.")
                .NotEmpty().WithMessage("Description should be provided.");
            RuleFor(x => x.Tags)
                .Must(tags => tags.Count <= 5).WithMessage("Can provide up to 5 tags.");

            RuleForEach(x => x.Tags)
                .MinimumLength(3).WithMessage("Tag length must not be below 3 characters.")
                .MaximumLength(20).WithMessage("Tag length must not be above 20 characters.");
            
            RuleForEach(x => x.Questions)
                .SetValidator(_ => new QuestionValidator());
        }
    }

    public class UpdateGigCommandHandler : AuthorizationRequestHandler<Gig>, IRequestHandler<UpdateGigCommand>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateGigCommand> _logger;

        public UpdateGigCommandHandler(IGigManagementDbContext dbContext, IMapper mapper, 
            ICurrentUserService currentUserService, IAuthorizationService authorizationService,
            ILogger<UpdateGigCommand> logger) 
            : base(currentUserService, authorizationService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        public async Task<Unit> Handle(UpdateGigCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Gigs.FindAsync(request.Id);
            
            if (entity is null)
            {
                _logger.LogWarning("Gig with id: {Id} was not found", request.Id);
                throw new NotFoundException(nameof(Gig), request.Id);
            }
            
            var result = await base.AuthorizeAsync(entity,  new [] {"OnlyOwnerPolicy"});
            
            if (!result.All(s => s.Succeeded))
            {
                throw new ForbiddenAccessException();
            }

            _mapper.Map(request, entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Gig with id: {Id} was updated", request.Id);

            return Unit.Value;
        }
    }
}