using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Common.Quiz.Util
{
    public class Randomizer : IRandomizer<Guid>
    {
        public Guid Next()
        {
            return Guid.NewGuid();
        }
    }
}
