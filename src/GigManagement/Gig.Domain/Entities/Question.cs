using System.Collections.Generic;

namespace Hive.Gig.Domain.Entities
{
    using Hive.Common.Domain.SeedWork;

    public class Question : ValueObject
    {
        private Question()
        {
        }
        
        public Question(string title, string answer) : this()
        {
            Title = title;
            Answer = answer;
        }
        
        public string Title { get; set; }

        public string Answer { get; set; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Title;
            yield return Answer;
        }
    }
}