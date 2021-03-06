﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DotNetCore.CAP;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Contracts.IntegrationEvents;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Gigs.Commands.CreateGig
{
    public record CreateGigCommand(
        string Title, 
        string Description, 
        int CategoryId, 
        int? PlanId,
        ICollection<string> Tags, 
        ICollection<QuestionModel> Questions,
        ICollection<PackageModel> Packages) : IRequest<int>;

    public class CreateGigCommandValidator : AbstractValidator<CreateGigCommand>
    {
        public CreateGigCommandValidator(IGigManagementDbContext dbContext, ICurrentUserService currentUserService)
        {
            RuleFor(x => x.Title)
                .MaximumLength(100).WithMessage("Title length must not be above 100 characters.")
                .MinimumLength(3).WithMessage("Title length must not be below 3 characters.")
                .NotEmpty().WithMessage("Title should be provided.");

            RuleFor(x => x.CategoryId)
                .MustAsync(async (id, token) => await dbContext.Categories.AnyAsync(x => x.Id == id && x.ParentId != null, cancellationToken: token))
                .WithMessage("Must provide an existing category id.");
            
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
                .SetValidator(_ => new QuestionValidator()).WithMessage("Question is not in correct format");
            
            RuleForEach(x => x.Packages)
                .SetValidator(_ => new PackageModelValidator(dbContext)).WithMessage("Package is not in correct format");

            RuleFor(x => x)
                .MustAsync(async (_, token) =>
                    await dbContext.Sellers.AnyAsync(s => s.UserId == currentUserService.UserId, token))
                .WithMessage("Could not find seller account for logged in user.");
        }    
    }
    
    public class CreateGigCommandHandler : IRequestHandler<CreateGigCommand, int>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICapPublisher _capPublisher;

        public CreateGigCommandHandler(IGigManagementDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService, ICapPublisher capPublisher)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _capPublisher = capPublisher ?? throw new ArgumentNullException(nameof(capPublisher));
        }
        
        public async Task<int> Handle(CreateGigCommand request, CancellationToken cancellationToken)
        {
            var sellerAccount = await _dbContext.Sellers.Select(x => new {x.UserId, x.Id})
                .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken: cancellationToken);

            if (sellerAccount == null)
            {
                throw new NotFoundException(nameof(Seller), _currentUserService.UserId);
            }
            
            var tags = (request.Tags).Select(t => new Tag(t)).ToHashSet();
            var questions = request.Questions.Select(q => new Question(q.Title, q.Answer)).ToHashSet();
            var packages = _mapper.Map<ICollection<Package>>(request.Packages);
            
            var gig = new Domain.Entities.Gig(request.Title, request.Description, sellerAccount.Id, request.CategoryId, tags, questions, packages, request.PlanId);

            _dbContext.Gigs.Add(gig);
            await _dbContext.SaveChangesAsync(cancellationToken);

            if (gig.PlanId.HasValue)
            {
                var @event = new PlanGigCreatedIntegrationEvent(gig.Id, gig.PlanId.Value);
                await _capPublisher.PublishAsync(@event.Name, @event, cancellationToken: cancellationToken);
            }

            return gig.Id;
        }
    }
}
