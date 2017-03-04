using System;
using NUnit.Framework;
using Moq;
using PCLCrypto;
using System.Text;
using System.Linq;

namespace PasswordGenerator.Core.Tests
{
    [TestFixture]
    public class CompositePCLCryptographerTests
    {
        [Test]
        public void Encrypt_ForEqualData_GenerateEqualCrypts()
        {
            //arrange
            var cryptographer = GetCryptographer();
            var data = Encoding.Unicode.GetBytes("forCryptographing");

            //act
            var crypt1 = cryptographer.Encrypt(data);
            var crypt2 = cryptographer.Encrypt(data);

            //assert            
            Assert.AreEqual(crypt1, crypt2);
        }

        private IHashCryptographer GetCryptographer()
        {
            return new CompositePCLCryptographer();
        }
    }
}