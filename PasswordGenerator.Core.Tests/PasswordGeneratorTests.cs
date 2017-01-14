using System;
using NUnit.Framework;
using Moq;
using PasswordGeneratorCore;

namespace PasswordGeneratorCore.Tests
{
    [TestFixture]
    public class PasswordGeneratorTests
    {
        IPasswordGenerator _generator;
        string _key;

        [OneTimeSetUp]
        public void Initialize()
        {
            //arrange            
            var mock = new Mock<ICryptographer>();
            mock.Setup(gen => gen.Encrypt(It.IsAny<string>())).Returns<string>(name => name);
            _key = "supersecretkey";
            _generator = new PasswordGeneratorCore.PasswordGenerator(mock.Object, _key);
        }

        [Test]
        public void Check_instance_generator_with_cryptographer()
        {
            //assert
            Assert.That(_generator != null);
        }

        [Test]        
        public void Generate_should_raise_exception_if_string_is_null()
        {
            // act + assert
            Assert.That(() => _generator.Generate(null),
                Throws.TypeOf<ArgumentException>());            
        }

        [Test]        
        public void Generate_should_raise_exception_if_string_is_empty()
        {
            // act + assert        
            Assert.That(() => _generator.Generate(""),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Generate_should_create_password_for_url_as_hostname()
        {
            //act + assert
            Assert.That(_generator.Generate(@"https://www.habrahabr.ru/post/150859/") == _key + "habrahabr.ru");
        }

        [Test]
        public void Generate_should_create_password_for_email()
        {
            //act + assert
            Assert.That(_generator.Generate(@"dmitriyanikin1991@gmail.com") == _key + "dmitriyanikin1991@gmail.com");
        }
    }
}
