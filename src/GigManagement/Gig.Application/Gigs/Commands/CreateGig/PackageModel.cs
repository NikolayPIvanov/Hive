using Hive.Gig.Domain.Enums;

namespace Hive.Gig.Application.Gigs.Commands
{
    public record PackageModel(string Title, string Description, decimal Price, PackageTier PackageTier,
        double DeliveryTime, DeliveryFrequency DeliveryFrequency, int? Revisions, RevisionType RevisionType, int GigId);
}