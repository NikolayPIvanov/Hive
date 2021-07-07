using System.Collections.Generic;
using Hive.Common.Domain.SeedWork;

namespace Hive.Gig.Domain.Entities
{
    public class ImagePath : ValueObject
    {
        private ImagePath()
        {
        }
        
        public ImagePath(string path) : this()
        {
            Path = path;
        }
        
        public string Path { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Path;
        }
    }
    
}