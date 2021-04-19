using System;
using System.Collections.Generic;
using MediatR;

namespace Hive.Common.Core.SeedWork
{
    public abstract class Entity
    {
        private List<INotification> _domainEvents;
        
        public virtual int Id { get; protected set; }

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();
        
        public DateTime Created { get; set;  }

        public string CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public string LastModifiedBy { get; set; }
        
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
        
        public bool IsTransient()
        {
            return Id == default;
        }
    }
}