using System;

namespace MemberTrack.Common
{
    public static class ExceptionExtensions
    {
        public static string ToDetail(this Exception exception)
        {
            var message = exception.Message;

            if (exception.InnerException == null)
            {
                return message;
            }

            if (!exception.InnerException.Message.Equals(message))
            {
                message = $"{Environment.NewLine}{ToDetail(exception.InnerException)}";
            }

            return message;
        }
    }
}