using Hive.Domain.Common;

namespace Hive.Domain.Entities.Gigs
{
    public class Question : AuditableEntity
    {
        private Question()
        {
        }
        
        public Question(string title, string answer, int gigId) : this()
        {
            Title = title;
            Answer = answer;
            GigId = gigId;
        }
        
        public string Title { get; set; }

        public string Answer { get; set; }

        public int GigId { get; private init; }
    }
}