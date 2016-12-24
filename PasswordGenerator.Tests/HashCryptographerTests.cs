using System;
using NUnit.Framework;
using Moq;

namespace PasswordGenerator.Tests
{
    [TestFixture]
    public class HashCryptographerTests
    {
        ICryptographer hashCryptographer;

        [OneTimeSetUp]
        public void Initialize()
        {
            //arrange    
            hashCryptographer = new HashCryptographer();            
        }

        [Test]
        public void Check_instance_HashCryptographer()
        {
            //assert
            Assert.That(hashCryptographer != null);
        }

        [Test]
        public void Check_md5_as_default_cryptographer()
        {
            //act + assert
            Assert.That(hashCryptographer.Encrypt("lalala") == "9aa6e5f2256c17d2d430b100032b997c");
        }
    }
}
