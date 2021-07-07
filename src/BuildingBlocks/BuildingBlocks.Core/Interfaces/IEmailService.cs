using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BuildingBlocks.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(IEnumerable<string> recipients, string subject, string message,
            CancellationToken cancellationToken = default);
    }
}