using NUnit.Framework;
using System;
using System.Text;
using System.Linq;

namespace PasswordGenerator.Core.Tests
{
    /// <summary>
    /// Сводное описание для WhileTrueTest
    /// </summary>
    [TestFixture]
    [Ignore("this is while(true) for super test a password generator")]
    public class WhileTrueTest
    {
        static Random rnd = new Random((int)DateTime.Now.Ticks);

        [Test]
        [Ignore("this is while(true) for super test a password generator")]
        public void supertest()
        {
            long count = 0;

            while (true)
            {
                string key = GetRandomString();
                string input = GetRandomString();
                var descriptor = GetRandomDescriptor();

                var generator = new PasswordGenerator(key, new CompositePCLCryptographer(), descriptor);

                var password = generator.Generate(input);

                Assert.IsTrue(IsPasswordSatisfiesDescriptor(password, descriptor));

                count++;
            }
        }

        private PasswordDescriptor GetRandomDescriptor()
        {
            var res = new PasswordDescriptor();

            res.PasswordLength = rnd.Next(4, 32);

            res.LowerCase = rnd.Next(0, 2) == 0 ? true : false;
            res.UpperCase = rnd.Next(0, 2) == 0 ? true : false;
            res.Digits = rnd.Next(0, 2) == 0 ? true : false;
            res.SpecialSymbols = rnd.Next(0, 2) == 0 ? true : false;

            if (!(res.LowerCase && res.UpperCase && res.Digits && res.SpecialSymbols))
            {
                var select = rnd.Next(0, 4);

                switch (select)
                {
                    case 0:
                        res.LowerCase = true;
                        break;
                    case 1:
                        res.UpperCase = true;
                        break;
                    case 2:
                        res.Digits = true;
                        break;
                    case 3:
                        res.SpecialSymbols = true;
                        break;
                }
            }
            return res;
        }

        private string GetRandomString()
        {
            var res = "";

            do
            {
                var length = rnd.Next(4, 30);                

                var buffer = new byte[length];

                rnd.NextBytes(buffer);

                res = Encoding.UTF8.GetString(buffer);
            }
            while (string.IsNullOrWhiteSpace(res));

            return res;
        }

        private bool IsPasswordSatisfiesDescriptor(string password, PasswordDescriptor descriptor)
        {
            if (password.Length != descriptor.PasswordLength)
                return false;

            var tempPassword = password.ToCharArray();

            if (descriptor.LowerCase)
            {
                var tempPasswordLengthBefore = tempPassword.Length;
                tempPassword = tempPassword.Except(descriptor.Alphabet.LowerCase).ToArray();
                if (tempPassword.Length == tempPasswordLengthBefore)
                    return false;
            }
            if (descriptor.UpperCase)
            {
                var tempPasswordLengthBefore = tempPassword.Length;
                tempPassword = tempPassword.Except(descriptor.Alphabet.UpperCase).ToArray();
                if (tempPassword.Length == tempPasswordLengthBefore)
                    return false;
            }
            if (descriptor.Digits)
            {
                var tempPasswordLengthBefore = tempPassword.Length;
                tempPassword = tempPassword.Except(descriptor.Alphabet.Digits).ToArray();
                if (tempPassword.Length == tempPasswordLengthBefore)
                    return false;
            }
            if (descriptor.SpecialSymbols)
            {
                var tempPasswordLengthBefore = tempPassword.Length;
                tempPassword = tempPassword.Except(descriptor.Alphabet.SpecialSymbols).ToArray();
                if (tempPassword.Length == tempPasswordLengthBefore)
                    return false;
            }

            if (tempPassword.Length != 0)
                return false;

            return true;
        }
    }
}
