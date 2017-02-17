using System;
using NUnit;
using NUnit.Framework;

namespace PasswordGenerator.Core.Tests
{
    [TestFixture]
    public class UtilTests
    {        
        [TestCase(10, 3)]
        [TestCase(18, 5)]
        [TestCase(40, 3)]
        public void GetMinRelativelyPrimeNumber_IfNumberMoreThen1_GetRelativelyPrime(int number, int expected)
        {
            //arrange + act
            var minRelativelyPrime = Util.GetMinRelativelyPrimeNumber(number);

            //assert
            Assert.AreEqual(minRelativelyPrime, expected);
        }

        [TestCase(1, -1)]
        [TestCase(0, -1)]
        [TestCase(-1, -1)]
        public void GetMinRelativelyPrimeNumber_IfNumberBelow2_ReturnMinusOne(int number, int expected)
        {
            //arrange + act
            var minRelativelyPrime = Util.GetMinRelativelyPrimeNumber(number);

            //assert
            Assert.AreEqual(minRelativelyPrime, expected);
        }
    }
}
