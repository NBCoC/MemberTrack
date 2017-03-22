using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Common.Quiz.Util
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items)
        {
            return items.Shuffle(new Randomizer());
        }

        //This overloaded method allows us to unit test that things are shuffled without any randomness.  We do this by us supplying the moq randomizer.
        public static IEnumerable<T> Shuffle<T, R>(this IEnumerable<T> items, IRandomizer<R> randomizer)
        {
            return items.OrderBy(i => randomizer.Next());
        }
    }
}
