using System;
using NUnit.Framework;

namespace PasswordGenerator.Tests
{
    [TestFixture]
    public class PasswordGeneratorTests
    {
        [Test]        
        public void Generate_should_raise_exception_if_string_is_null()
        {
            // arrange
            IPasswordGenerator generator = new PasswordGenerator();

            // act + assert
            Assert.That(() => generator.Generate(null),
                Throws.TypeOf<ArgumentNullException>());            
        }

        [Test]        
        public void Generate_shoul_raise_exception_if_string_is_empty()
        {
            // arrange
            IPasswordGenerator generator = new PasswordGenerator();

            // act + assert        
            Assert.That(() => generator.Generate(""),
                Throws.TypeOf<ArgumentException>());
        }
    }
}
