using System;
using System.Collections.Generic;
using MediatR;

namespace Hive.Common.Core.SeedWork
{
    public abstract class DomainEntity
    {
        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();
        
        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents ??= new List<INotification>();
            _domainEvents.Add(eventItem);
        }
        
        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }
        
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

    }
    
    public abstract class Entity : DomainEntity
    {
        public virtual int Id { get; protected set; }

        public DateTime Created { get; set;  }

        public string CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public string LastModifiedBy { get; set; }
        
        public bool IsTransient()
        {
            return Id == default;
        }
    }
}