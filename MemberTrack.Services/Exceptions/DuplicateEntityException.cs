using System;

namespace MemberTrack.Services.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException(long entityId) : base($"Entity with ID {entityId} already exists")
        {
            EntityId = entityId;
        }

        public long EntityId { get; private set; }
    }
}