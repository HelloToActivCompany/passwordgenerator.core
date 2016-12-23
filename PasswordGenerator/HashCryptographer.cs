using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace PasswordGenerator
{
    public class HashCryptographer : ICryptographer
    {
        HashAlgorithm algorithm = new MD5Cng();

        public string Encrypt(string input)
        {
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            var result = data.Select<byte, string>(x => x.ToString("x2")).Aggregate("", (str, chr) => str + chr);
            
            return result;
        }
    }
}
