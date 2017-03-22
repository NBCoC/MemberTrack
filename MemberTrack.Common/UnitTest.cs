using System;
using System.Linq;

namespace MemberTrack.Common
{
    public static class UnitTest
    {
        public static bool IsRunning
        {
            get
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                    .Any(assembly =>
                         assembly.FullName.StartsWith(
                             "Microsoft.VisualStudio.QualityTools.UnitTestFramework"));
            }
        }
    }
}
