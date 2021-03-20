using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Hive.Gig.Application.Gigs.Commands.CreateGig
{
    public class CreateGigCommand : IRequest<int>
    {
        public string Title { get; set; }
    }

    public class CreateGigCommandHandler : IRequestHandler<CreateGigCommand, int>
    {
        public Task<int> Handle(CreateGigCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}