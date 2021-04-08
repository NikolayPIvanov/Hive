using System.Collections.Generic;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Gigs
{
    public class Question : ValueObject
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
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Title;
            yield return Answer;
            yield return GigId;
        }
    }
}