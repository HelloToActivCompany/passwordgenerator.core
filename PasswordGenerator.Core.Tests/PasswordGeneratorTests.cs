using System;
using NUnit.Framework;
using Moq;

namespace PasswordGenerator.Core.Tests
{
    [TestFixture]
    public class PasswordGeneratorTests
    {
        [TestCase("")]
        [TestCase(null)]       
        public void Generate_IfNullOrEmptyInput_Throw(string input)
        {
            //arrange
            var generator = GetGenerator();

            //act + assert          
            Assert.That(() => generator.Generate(input), Throws.TypeOf<ArgumentException>());
        }

        [TestCase("habrahabr.ru", "alksar")]
        [TestCase("habrahabr.ru", "")]
        [TestCase("habrahabr.ru", null)]
        [TestCase("vk.com", "alksar")]
        [TestCase("vk.com", "")]
        [TestCase("vk.com", null)]
        public void Generate_ForAnyNotNullOrEmptyInputAndAnyLogin_GeneratePassword(string input, string login)
        {
            //arrange
            var stubCryptographer = new Mock<ICryptographer>(); 
            stubCryptographer.Setup(m => m.Encrypt(It.IsAny<string>())).Returns("12345");

            var generator = new PasswordGenerator(stubCryptographer.Object, "secret");

            //act + assert            
            Assert.That(generator.Generate(input, login) == "12345");
        }

        private IPasswordGenerator GetGenerator()
        {
            return new PasswordGenerator(new HashCryptographer(PCLCrypto.HashAlgorithm.Md5), "key");
        }
    }
}
