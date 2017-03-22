using MemberTrack.Common.Quiz.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MemberTrack.Common.Quiz.Tests.Util
{
    public class FakeReverseOrderRandomizer : IRandomizer<Guid>
    {
        private Stack<Guid> _reverseOrderGuids = new Stack<Guid>();

        public FakeReverseOrderRandomizer(int guidsToUse)
        {
            for (int counter = 0; counter < guidsToUse; counter++)
            {
                _reverseOrderGuids.Push(CreateGuid());
            }
        }

        private class NativeMethods
        {
            [DllImport("rpcrt4.dll", SetLastError = true)]
            public static extern int UuidCreateSequential(out Guid guid);
        }

        private static Guid CreateGuid()
        {
            const int RPC_S_OK = 0;

            Guid guid;
            int result = NativeMethods.UuidCreateSequential(out guid);
            if (result == RPC_S_OK)
                return guid;
            else
                return Guid.NewGuid();
        }

        public Guid Next()
        {
            return _reverseOrderGuids.Pop();
        }
    }
}
