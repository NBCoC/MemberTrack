using System;

namespace MemberTrack.Services.Exceptions
{
    public class UnauthorizeException : Exception
    {
        public UnauthorizeException() : base("You are not authorized to perform this action") { }
    }
}