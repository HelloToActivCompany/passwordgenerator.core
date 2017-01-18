using System;
using NUnit.Framework;
using Moq;

namespace PasswordGenerator.Core.Tests
{
    [TestFixture]
    public class PasswordGeneratorTests
    {
        private IPasswordGenerator _generator;
        private string _key;
        private string _login;

        [OneTimeSetUp]
        public void Initialize()
        {
            //arrange            
            var mock = new Mock<ICryptographer>();
            mock.Setup(gen => gen.Encrypt(It.IsAny<string>())).Returns<string>(name => name);
            _key = "supersecretkey";
            _generator = new PasswordGenerator(mock.Object, _key);
            _login = "anylogin";
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
            Assert.That(() => _generator.Generate(null), Throws.TypeOf<ArgumentException>());
            Assert.That(() => _generator.Generate(null, _login), Throws.TypeOf<ArgumentException>());
        }

        [Test]        
        public void Generate_should_raise_exception_if_string_is_empty()
        {
            // act + assert        
            Assert.That(() => _generator.Generate(""), Throws.TypeOf<ArgumentException>());
            Assert.That(() => _generator.Generate("", _login), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Generate_should_create_password_for_any_string()
        {
            //act + assert
            Assert.That(_generator.Generate("habrahabr.ru") == _key + "habrahabr.ru");
            Assert.That(_generator.Generate("habrahabr.ru", _login) == _key + "habrahabr.ru" + "/" + _login);
        }
    }
}
