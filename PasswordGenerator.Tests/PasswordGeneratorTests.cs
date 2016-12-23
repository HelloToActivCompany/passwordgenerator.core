using System;
using NUnit.Framework;
using Moq;

namespace PasswordGenerator.Tests
{
    [TestFixture]
    public class PasswordGeneratorTests
    {
        IPasswordGenerator generatorAsDefault;
        IPasswordGenerator generatorWithCryptographer;

        [OneTimeSetUp]
        public void Initialize()
        {
            //arrange    
            generatorAsDefault = new PasswordGenerator();
            
            Mock<ICryptographer> mock = new Mock<ICryptographer>();
            mock.Setup(gen => gen.Encrypt(It.IsAny<string>())).Returns(String.Empty);

            generatorWithCryptographer = new PasswordGenerator(mock.Object);
        }

        [Test]
        public void Check_instance_generator_with_cryptographer()
        {
            //assert
            Assert.That(generatorWithCryptographer != null);
        }

        [Test]        
        public void Generate_should_raise_exception_if_string_is_null()
        {
            // act + assert
            Assert.That(() => generatorWithCryptographer.Generate(null),
                Throws.TypeOf<ArgumentNullException>());            
        }

        [Test]        
        public void Generate_should_raise_exception_if_string_is_empty()
        {
            // act + assert        
            Assert.That(() => generatorWithCryptographer.Generate(""),
                Throws.TypeOf<ArgumentException>());
        }
    }
}
