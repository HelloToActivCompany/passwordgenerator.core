using System;
using NUnit.Framework;
using Moq;
using System.Linq;
using System.Collections.Generic;

namespace PasswordGenerator.Core.Tests
{
    [TestFixture]
    public class SimpleUniversalCoderTests
    {
        [TestCase(new Byte[] { 0, 1, 2, 3, 38, 56, 44, 32 }, new char[] { 'a', 'b', 'c', 'A', 'B', 'C', '1', '3', '5' })]
        [TestCase(new Byte[] { 38, 56, 44, 32 }, new char[] { '1', '3', '5' })]
        [TestCase(new Byte[]{0, 1, 2, 3}, new char[]{'a', 'b', 'c'})]
        public void ConvertBytesToString_ForSameBytes_ReturnSameString(byte[] data, char[] alphabet)
        {
            //arrange
            var coder = new SimpleUniversalCoder();

            //assert
            Assert.AreEqual(coder.ConvertBytesToString(data, alphabet), coder.ConvertBytesToString(data, alphabet));
        }
    }
}
