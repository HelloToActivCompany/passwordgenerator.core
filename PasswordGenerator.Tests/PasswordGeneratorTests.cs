using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PasswordGenerator.Tests
{
    [TestClass]
    public class PasswordGeneratorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateShouldRaiseExceptionIfStringIsNull()
        {
            //arrange

            //act
            var password = PasswordGenerator.Generate(null);

            //assert
        }
    }
}
