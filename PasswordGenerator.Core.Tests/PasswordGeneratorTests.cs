﻿using System;
using NUnit.Framework;
using Moq;

namespace PasswordGenerator.Core.Tests
{
    [TestFixture]
    public class PasswordGeneratorTests
    {
        [Test]
        public void PasswordMinLenght_ForMinLengthBelow1_SetMinLength1()
        {
            //arrange
            var generator = GetGenerator();

            //act
            generator.PasswordMinLenght = -5;

            //assert
            Assert.AreEqual(1, generator.PasswordMinLenght);
        }

        [Test]
        public void PasswordMaxLenght_ForMaxLengthAbove40_SetMaxLength40()
        {
            //arrange
            var generator = GetGenerator();

            //act
            generator.PasswordMaxLenght = 90;

            //assert
            Assert.AreEqual(40, generator.PasswordMaxLenght);
        }

        [Test]
        public void PswdDescriptor_ForPasswordLengthBelow6_SetPasswordLength6()
        {
            //arrange
            var generator = GetGenerator();

            var shortPasswordDescriptor = new PasswordDescriptor()
            {
                PasswordLength = -10,
            };            

            //act
            generator.PasswordDescriptor = shortPasswordDescriptor;

            //assert
            Assert.AreEqual(generator.PasswordDescriptor.PasswordLength, 1);
        }

        [Test]
        public void PswdDescriptor_ForPasswordLengthAbove40_SetPasswordLength40()
        {
            //arrange
            var generator = GetGenerator();

            var longPasswordDescriptor = new PasswordDescriptor()
            {
                PasswordLength = 41,
            };            

            //act
            generator.PasswordDescriptor = longPasswordDescriptor;

            //assert
            Assert.AreEqual(generator.PasswordDescriptor.PasswordLength, 40);
        }


        [Test]
        public void Constructor_WithoutPasswordDescriptorParam_CreatePasswordDescriptorByDefault()
        {
            //arrange
            var generator = new PasswordGenerator(new HashCryptographer(), "key");
            var defaultDescriptor = new PasswordDescriptor
            {
                LowerCase = true,
                UpperCase = true,
                Digits = true,
                SpecialSymbols = true,
                PasswordLength = 18
            };

            //assert
            Assert.AreEqual(defaultDescriptor, generator.PasswordDescriptor);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("     ")]
        public void Generate_IfNullOrWhiteSpace_Throw(string input)
        {
            //arrange
            var generator = GetGenerator();

            //act + assert          
            Assert.That(() => generator.Generate(input), Throws.TypeOf<ArgumentException>());
        }

        [TestCase("sadqq", false, false, false, true, 11)]
        [TestCase("31r2qff", false, false, true, false, 12)]
        [TestCase("dsfewdfwqecw2f32ff23f2q", false, false, true, true, 13)]
        [TestCase("999", false, true, false, false, 14)]
        [TestCase("1", false, true, false, true, 15)]
        [TestCase("h65hg3", false, true, true, false, 16)]
        [TestCase("fsdf2", false, true, true, true, 17)]
        [TestCase("333ffz.com", true, false, false, false, 18)]
        [TestCase("0johufhd", true, false, false, true, 19)]
        [TestCase("thds4weufgfit6srtdu6s", true, false, true, false, 20)]
        [TestCase("joi08976rdxhfr63", true, false, true, true, 21)]
        [TestCase("uyuyuiyiuiyuio", true, true, false, false, 22)]
        [TestCase("czwsaxcxe", true, true, false, true, 23)]
        [TestCase("pl,mmjuugvvfeesz", true, true, true, false, 24)]
        [TestCase("7887!!yfjy54", true, true, true, true, 25)]
        public void Generate_WithDescriptor_ReturnPasswordMatchesDescriptor(string input, bool lowerCase, bool upperCase, 
                                                                                bool digits, bool specialSymbols, int passwordLength)
        {
            //arrange
            var descriptor = new PasswordDescriptor
            {
                LowerCase = lowerCase,
                UpperCase = upperCase,
                Digits = digits,
                SpecialSymbols = specialSymbols,
                PasswordLength = passwordLength
            };

            var generator = GetGenerator();

            //act
            var password = generator.Generate(descriptor, input);

            //assert
            Assert.IsTrue(IsPasswordMatchesDescription(password, descriptor));
        }

        private bool IsPasswordMatchesDescription(string password, PasswordDescriptor descriptor)
        {
            if (password.Length != descriptor.PasswordLength)
                return false;

            var currentDescriptor = new PasswordDescriptor
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
                if (Base91Coder.CharIsSpecial(c)) currentDescriptor.SpecialSymbols = true;

                if (currentDescriptor.LowerCase && currentDescriptor.UpperCase &&
                    currentDescriptor.Digits && currentDescriptor.SpecialSymbols)
                    break;
            }

            return currentDescriptor.Equals(descriptor);
        }

        private PasswordGenerator GetGenerator()
        {
            return new PasswordGenerator(new HashCryptographer(PCLCrypto.HashAlgorithm.Md5), "key");
        }
    }
}
