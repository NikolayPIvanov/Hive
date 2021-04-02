namespace Hive.Gig.Domain.Entities
{
    using Hive.Common.Domain.SeedWork;

    public class Question : Entity
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