using System.Collections.Generic;
using Hive.Application.Common.Mappings;
using Hive.Application.Gigs.Commands.CreateGig;
using Hive.Domain.Entities.Gigs;

namespace Hive.Application.Gigs.Queries.GetGig
{
    public class GigDto : IMapFrom<Gig>
    {
        public int Id { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string? Metadata { get; set; }
        
        public string Tags { get; set; }

        public int CategoryId { get; set; }
        
        public List<QuestionDto> Questions { get; set; }

        public int SellerId { get; set; }
    }
}