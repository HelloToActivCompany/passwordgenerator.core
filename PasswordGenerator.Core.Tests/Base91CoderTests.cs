using System;
using NUnit.Framework;

namespace PasswordGenerator.Core.Tests
{
    [TestFixture]
    public class Base91CoderTests
    {
        [Test]
        public void Convert_ConvertStringToBytesAndThenConvertToString_GetEqualString()
        {
            //arrange
            Base91Coder coder = new Base91Coder();
            string forConvert = "StringForConvert";

            //act
            byte[] bytes = coder.ConvertStringToBytes(forConvert);
            string result = coder.ConvertBytesToString(bytes);
            
            //assert
            Assert.AreEqual(result, forConvert);
        }
    }
}
