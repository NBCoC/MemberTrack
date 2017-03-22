using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Services.Exceptions
{
    public class PersonNotFoundException : Exception
    {
        public PersonNotFoundException(long personId) : base($"Person with ID {personId} not found")
        {
            PersonId = personId;
        }

        public PersonNotFoundException() : base("Person not found") { }

        public long? PersonId { get; private set; }

    }
}
