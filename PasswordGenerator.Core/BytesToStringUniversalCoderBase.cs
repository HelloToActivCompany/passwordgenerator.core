using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator.Core
{
    public abstract class BytesToStringUniversalCoderBase : IBytesToStringConvertable
    {
        public char[] Alphabet { get; set; }

        public BytesToStringUniversalCoderBase(char[] alphabet)
        {     
            Alphabet = alphabet;
        }

        public string ConvertBytesToString(byte[] data)
        {
            ValidationAlphabet();

            return ConvertBytesToStringImplementation(data);
        }

        private void ValidationAlphabet()
        {
            if (Alphabet == null)
                throw new NullReferenceException("Alphabet");
            if (Alphabet.Length == 0)
                throw new ArgumentException("Alphabet should'nt be empty");
        }

        protected abstract string ConvertBytesToStringImplementation(byte[] data);
    }
}
