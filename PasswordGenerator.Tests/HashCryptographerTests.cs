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
        public void Check_instance_hashcryptographer()
        {
            //assert
            Assert.That(hashCryptographer != null);
        }

        [Test]
        public void Check_md5_as_default_cryptographer()
        {
            //arrange   
            System.Security.Cryptography.HashAlgorithm md5 = new System.Security.Cryptography.MD5Cng();
                       
            //act
            byte[] data = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes("lalala"));

            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            string md5result = sBuilder.ToString();
            
            //assert
            Assert.That(hashCryptographer.Encrypt("lalala") == md5result);
        }
    }
}
