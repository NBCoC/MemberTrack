using System;

namespace MemberTrack.Services.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(long entityId) : base($"Entity with ID {entityId} not found")
        {
            EntityId = entityId;
        }

        public EntityNotFoundException() : base("Entity not found") { }

        public long? EntityId { get; private set; }
    }
}