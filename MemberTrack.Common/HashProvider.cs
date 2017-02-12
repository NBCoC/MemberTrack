using System;
using System.Security.Cryptography;
using System.Text;
using MemberTrack.Common.Contracts;

namespace MemberTrack.Common
{
    public class HashProvider : IHashProvider
    {
        private const string PostSalt = "_buffer_9#00!#8423-12834)*@$920*";
        private const string PreSalt = "member_track_salt_";

        public string Hash(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var data = Encoding.UTF8.GetBytes($"{PreSalt}{value}{PostSalt}");

            data = new SHA256CryptoServiceProvider().ComputeHash(data);

            return Convert.ToBase64String(data);
        }
    }
}