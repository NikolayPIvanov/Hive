using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Hive.Application.Orders.Commands.AcceptOrder
{
    public class AcceptOrderCommand : IRequest
    {
        public Guid OrderNumber { get; set; }
    }
    
    public class AcceptOrderCommandHandler : IRequestHandler<AcceptOrderCommand>
    {
        public Task<Unit> Handle(AcceptOrderCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}