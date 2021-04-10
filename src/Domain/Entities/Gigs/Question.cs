using System.Collections.Generic;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Gigs
{
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
        
        public string Title { get; init; }

        public string Answer { get; init; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Title;
            yield return Answer;
        }
    }
}