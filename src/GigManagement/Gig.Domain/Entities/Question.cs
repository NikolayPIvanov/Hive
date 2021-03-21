using Hive.Common.Domain;

namespace Hive.Gig.Domain.Entities
{
    public class Question : AuditableEntity
    {
        private Question()
        {
        }
        
        public Question(string title, string answer, int gigId)
        {
            Title = title;
            Answer = answer;
            GigId = gigId;
        }
        
        public int Id { get; set; }

        public string Title { get; set; }

        public string Answer { get; set; }

        public int GigId { get; set; }
    }
}