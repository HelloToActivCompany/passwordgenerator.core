using System;
using NUnit.Framework;

namespace PasswordGenerator.Tests
{
    [TestFixture]
    public class PasswordGeneratorTests
    {
        IPasswordGenerator generator;
        IPasswordGenerator generatorWithCryptographer;
        ICryptographer cryptographer;

        [OneTimeSetUp]
        private void Initialize()
        {
            //arrange    
            generator = new PasswordGenerator();
                   
            cryptographer = new Md5Cryptographer();
            generatorWithCryptographer = new PasswordGenerator(cryptographer);
        }

        [Test]
        public void Check_instance_generator_whith_cryptografer()
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
