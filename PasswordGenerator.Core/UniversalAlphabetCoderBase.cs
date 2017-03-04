using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator.Core
{
    public abstract class UniversalAlphabetCoderBase
    {
        public string ConvertBytesToString(byte[] data, char[] alphabet)
        {
            if (alphabet == null)
                throw new NullReferenceException("Alphabet");
            if (alphabet.Length == 0)
                throw new ArgumentException("Alphabet should'nt be empty");

            return ConvertBytesToStringImplementation(data, alphabet);
        }

        protected abstract string ConvertBytesToStringImplementation(byte[] data, char[] alphabet);
    }
}
