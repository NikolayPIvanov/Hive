using System;

namespace Hive.Common.Domain.SeedWork
{
    public record IntegrationEvent
    {
        public IntegrationEvent(string name)
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
            Name = name;
        }
        
        public Guid Id { get; private init; }
        
        public string Name { get; private init; }

        public DateTime CreationDate { get; private init; }
    }
}