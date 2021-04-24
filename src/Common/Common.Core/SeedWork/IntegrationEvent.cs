using System;

namespace Hive.Common.Core.SeedWork
{
    public record IntegrationEvent
    {
        public IntegrationEvent(string name)
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
            Name = name;
            Callback = null;
        }
        
        public IntegrationEvent(string name, string callback) : this(name)
        {
            Callback = callback;
        }

        public string Callback { get; private init; }

        public Guid Id { get; private init; }
        
        public string Name { get; private init; }

        public DateTime CreationDate { get; private init; }
    }
}