using System;
using NUnit.Framework;
using Moq;

namespace PasswordGenerator.Core.Tests
{
    [TestFixture]
    public class PasswordGeneratorTests
    {
        [Test]
        public void PswdDescriptorSet_ForPasswordLengthBelow_6_SetPasswordLength_6()
        {
            //arrange
            var generator = GetGenerator();

            var shortPasswordDescriptor = new PasswordGenerator.PasswordDescriptor();
            shortPasswordDescriptor.PasswordLength = 5;

            //act
            generator.PswdDescriptor = shortPasswordDescriptor;

            //assert
            Assert.AreEqual(generator.PswdDescriptor.PasswordLength, 6);
        }

        [Test]
        public void PswdDescriptorSet_ForPasswordLengthAbove_40_SetPasswordLength_40()
        {
            //arrange
            var generator = GetGenerator();

            var longPasswordDescriptor = new PasswordGenerator.PasswordDescriptor();
            longPasswordDescriptor.PasswordLength = 41;

            //act
            generator.PswdDescriptor = longPasswordDescriptor;

            //assert
            Assert.AreEqual(generator.PswdDescriptor.PasswordLength, 40);
        }


        [Test]
        public void Constructor_WithoutPasswordDescriptorParam_CreatePasswordDescriptorByDefault()
        {
            //arrange
            PasswordGenerator generator = new PasswordGenerator(new HashCryptographer(), "key");
            var defaultDescriptor = new PasswordGenerator.PasswordDescriptor
            {
                LowerCase = true,
                UpperCase = true,
                Digits = true,
                SpecialSymbols = true,
                PasswordLength = 18
            };

            //assert
            Assert.AreEqual(defaultDescriptor, generator.PswdDescriptor);
        }

        [TestCase("")]
        [TestCase(null)]       
        public void Generate_IfNullOrEmptyInput_Throw(string input)
        {
            //arrange
            var generator = GetGenerator();

            //act + assert          
            Assert.That(() => generator.Generate(input), Throws.TypeOf<ArgumentException>());
        }

        [TestCase(false, false, false, true, 11)]
        [TestCase(false, false, true, false, 12)]
        [TestCase(false, false, true, true, 13)]
        [TestCase(false, true, false, false, 14)]
        [TestCase(false, true, false, true, 15)]
        [TestCase(false, true, true, false, 16)]
        [TestCase(false, true, true, true, 17)]
        [TestCase(true, false, false, false, 18)]
        [TestCase(true, false, false, true, 19)]
        [TestCase(true, false, true, false, 20)]
        [TestCase(true, false, true, true, 21)]
        [TestCase(true, true, false, false, 22)]
        [TestCase(true, true, false, true, 23)]
        [TestCase(true, true, true, false, 24)]
        [TestCase(true, true, true, true, 25)]
        public void Generate_WithDescriptor_ReturnPasswordMatchesDescriptor(bool lowerCase, bool upperCase, 
                                                                                bool digits, bool specialSymbols, int passwordLength)
        {
            //arrange
            var descriptor = new PasswordGenerator.PasswordDescriptor
            {
                LowerCase = lowerCase,
                UpperCase = upperCase,
                Digits = digits,
                SpecialSymbols = specialSymbols,
                PasswordLength = passwordLength
            };

            var generator = GetGenerator();

            //act
            var password = generator.Generate(descriptor, "input");

            //assert
            Assert.IsTrue(IsPasswordMatchesDescription(password, descriptor));
        }

        private bool IsPasswordMatchesDescription(string password, PasswordGenerator.PasswordDescriptor descriptor)
        {
            if (password.Length != descriptor.PasswordLength)
                return false;

            PasswordGenerator.PasswordDescriptor currentDescriptor = new PasswordGenerator.PasswordDescriptor
            {
                LowerCase = false,
                UpperCase = false,
                Digits = false,
                SpecialSymbols = false,
                PasswordLength = password.Length
            };

            foreach (var c in password)
            {
                if (char.IsDigit(c))    currentDescriptor.Digits = true;
                if (char.IsLower(c))    currentDescriptor.LowerCase = true;
                if (char.IsUpper(c))    currentDescriptor.UpperCase= true;
                if (IsSpecialSymbol(c)) currentDescriptor.SpecialSymbols = true;

                if (currentDescriptor.LowerCase && currentDescriptor.UpperCase &&
                    currentDescriptor.Digits && currentDescriptor.SpecialSymbols)
                    break;
            }

            return currentDescriptor.Equals(descriptor);
        }

        private bool IsSpecialSymbol(char c)
        {
            return "!#$%&()*+,./:;<=>?@[]^_`{|}~\"".IndexOf(c) != -1;
        }

        private PasswordGenerator GetGenerator()
        {
            return new PasswordGenerator(new HashCryptographer(PCLCrypto.HashAlgorithm.Md5), "key");
        }        
    }
}
