using MemberTrack.Common.Quiz.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace MemberTrack.Common.Quiz.Tests
{
    [TestClass]
    public class IEnumerableExtensionsTests
    {

        [TestMethod]
        public void Shuffle_ObeysRandomizer()
        {
            var randomizer = new Mock<IRandomizer<int>>();

            
            randomizer.SetupSequence(m => m.Next()).Returns(4).Returns(3).Returns(1).Returns(2);

            List<string> jumbledDaveCrockettQuote = new List<string> { "and will quickly fall", ",grew up quickly", "The party in power,", "like Jonah's gourd" };

            List<string> daveCrockettQuote = jumbledDaveCrockettQuote.Shuffle(randomizer.Object).ToList();

            //QUOTE: The party in power, like Jonah's gourd, grew up quickly, and will quickly fall. -Dave Crockett
            Assert.AreEqual(4, daveCrockettQuote.Count);
            Assert.AreSame(daveCrockettQuote[0], jumbledDaveCrockettQuote[2]);
            Assert.AreSame(daveCrockettQuote[1], jumbledDaveCrockettQuote[3]);
            Assert.AreSame(daveCrockettQuote[2], jumbledDaveCrockettQuote[1]);
            Assert.AreSame(daveCrockettQuote[3], jumbledDaveCrockettQuote[0]);

        }


    }
}
