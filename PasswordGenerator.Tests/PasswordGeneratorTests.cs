using System;
using NUnit.Framework;

namespace PasswordGenerator.Tests
{
    [TestFixture]
    public class PasswordGeneratorTests
    {
        IPasswordGenerator generator;

        [OneTimeSetUp]
        private void Initialize()
        {
            //arrange
            generator = new PasswordGenerator();
        }

        [Test]        
        public void Generate_should_raise_exception_if_string_is_null()
        {
            // act + assert
            Assert.That(() => generator.Generate(null),
                Throws.TypeOf<ArgumentNullException>());            
        }

        [Test]        
        public void Generate_should_raise_exception_if_string_is_empty()
        {
            // act + assert        
            Assert.That(() => generator.Generate(""),
                Throws.TypeOf<ArgumentException>());
        }
    }
}
