#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Accounts;
using MediatR;

namespace Hive.Application.UserProfiles.Commands.UpdateUserProfile
{
    public static class UpdateProfileCommand
    {
        public record Command(int ProfileId, string? FirstName, string? LastName, string? Description, string? Education,
            ICollection<string> Languages, ICollection<string> Skills) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _context.Profiles.FindAsync(new object[] { request.ProfileId }, cancellationToken: cancellationToken);

                if (entity is null)
                {
                    throw new NotFoundException(nameof(UserProfile), request.ProfileId);
                }

                entity.FirstName = request.FirstName;
                entity.LastName = request.LastName;
                entity.Description = request.Description;
                entity.Languages = request.Languages.Select(x => new Language(x)).ToHashSet();
                entity.Skills = request.Skills.Select(x => new Skill(x)).ToHashSet();
                entity.Education = request.Education;
                
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}