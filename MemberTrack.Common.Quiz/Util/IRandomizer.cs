using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Common.Quiz.Util
{
    public interface IRandomizer<T>
    {
        T Next();
    }
}
