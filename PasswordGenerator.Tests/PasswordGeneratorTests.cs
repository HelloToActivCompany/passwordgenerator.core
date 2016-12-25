using System;
using NUnit.Framework;
using Moq;

namespace PasswordGenerator.Tests
{
    [TestFixture]
    public class PasswordGeneratorTests
    {
        IPasswordGenerator generator;

        [OneTimeSetUp]
        public void Initialize()
        {
            //arrange            
            Mock<ICryptographer> mock = new Mock<ICryptographer>();
            mock.Setup(gen => gen.Encrypt(It.IsAny<string>())).Returns<string>(name => name);
            generator = new PasswordGenerator(mock.Object);
        }

        [Test]
        public void Check_instance_generator_with_cryptographer()
        {
            //assert
            Assert.That(generator != null);
        }

        [Test]        
        public void Generate_should_raise_exception_if_string_is_null()
        {
            // act + assert
            Assert.That(() => generator.Generate(null),
                Throws.TypeOf<ArgumentException>());            
        }

        [Test]        
        public void Generate_should_raise_exception_if_string_is_empty()
        {
            // act + assert        
            Assert.That(() => generator.Generate(""),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Generate_should_create_password_for_url_as_hostname()
        {
            //act + assert
            Assert.That(generator.Generate(@"https://www.habrahabr.ru/post/150859/") == "habrahabr.ru");
        }

        [Test]
        public void Generate_should_create_password_for_email()
        {
            //act + assert
            Assert.That(generator.Generate(@"dmitriyanikin1991@gmail.com") == "dmitriyanikin1991@gmail.com");
        }
    }
}
