using System.Linq;
using NUnit.Framework;

namespace Bhasha.Common.Tests
{
    [TestFixture]
    public class TokenTypeTests
    {
        [Test]
        public void IsWord_for_any_token_type([Values]TokenType tokenType)
        {
            Assert.That(tokenType.IsWord() == TokenTypeSupport.Words.Contains(tokenType));
        }
    }
}
