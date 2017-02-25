using System;
using NUnit.Framework;
using Moq;
using PCLCrypto;
using System.Text;
using System.Linq;

namespace PasswordGenerator.Core.Tests
{
    [TestFixture]
    public class PCLCryptographerTests
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

        [Test]
        public void Constructor_ByDefault_CreateMd5Cryptographer()
        {
            //arrange
            var cryptographerByDefault = GetCryptographer();
            var md5Cryptographer = new PCLCryptographer(HashAlgorithm.Md5);

            var data = Encoding.Unicode.GetBytes("forCryptographing");


            //act
            var cryptByDefault = cryptographerByDefault.Encrypt(data);
            var md5Crypt = md5Cryptographer.Encrypt(data);

            //assert
            Assert.AreEqual(cryptByDefault, md5Crypt);
        }

        private IHashCryptographer GetCryptographer()
        {
            return new PCLCryptographer();
        }
    }
}