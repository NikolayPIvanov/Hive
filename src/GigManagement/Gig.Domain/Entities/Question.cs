using Hive.Common.Domain;

namespace Hive.Gig.Domain.Entities
{
    public class Question
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Answer { get; set; }

        public int GigId { get; set; }
    }
}