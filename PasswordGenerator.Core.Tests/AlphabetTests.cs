using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace PasswordGenerator.Core.Tests
{
    [TestFixture]
    public class AlphabetTests
    {
        [Test]
        public void Constructor_ByDefault_CreateDefaultAlphabet()
        {
            //arrange
            var alphabet = new Alphabet();

            var defaultLower = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            var defaultUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var defaultDigits = "0123456789".ToCharArray();
            var defaultSpecial = "!#$%&()*+,./:;<=>?@[]^_\"{|}~ ".ToCharArray();


            //assert
            Assert.AreEqual(0, defaultLower.Except(alphabet.LowerCase).Count());
            Assert.AreEqual(0, defaultUpper.Except(alphabet.UpperCase).Count());
            Assert.AreEqual(0, defaultDigits.Except(alphabet.Digits).Count());
            Assert.AreEqual(0, defaultSpecial.Except(alphabet.SpecialSymbols).Count());
        }

        [Test]
        public void Constructor_WithStringParam_SetAlphabet()
        {
            //arrange
            var alphabet = new Alphabet("abcABC123!@#");

            //assert
            Assert.AreEqual(0, "abc".Except(alphabet.LowerCase).Count());
            Assert.AreEqual(0, "ABC".Except(alphabet.UpperCase).Count());
            Assert.AreEqual(0, "123".Except(alphabet.Digits).Count());
            Assert.AreEqual(0, "!@#".Except(alphabet.SpecialSymbols).Count());
        }

        [Test]
        public void Add_NewSymbol_ExtendAlphabet()
        {
            //arrange
            var alphabet = new Alphabet("a");

            //act
            alphabet.Add('b');

            //assert
            Assert.AreEqual(0, "ab".Except(alphabet.LowerCase).Count());
        }

        [Test]
        public void Add_NewSymbols_ExtendAlphabet()
        {
            //arrange
            var alphabet = new Alphabet("a");

            //act
            alphabet.Add("bc");

            //assert
            Assert.AreEqual(0, "abc".Except(alphabet.LowerCase).Count());
        }

        [Test]
        public void Add_ExistsSymbols_NotExtendAlphabet()
        {
            //arrange
            var alphabet = new Alphabet("a");

            //act
            alphabet.Add("aaaaa");

            //assert
            Assert.AreEqual(0, alphabet.LowerCase.Except("a").Count());
        }
    }
}
