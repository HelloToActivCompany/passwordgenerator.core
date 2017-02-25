using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator.Core
{
    public class Alphabet
    {
        public char[] LowerCase { get; set; }
        public char[] UpperCase { get; set; }
        public char[] Digits { get; set; }
        public char[] SpecialSymbols { get; set; }

        public Alphabet()
        {
            LowerCase = new char[0];
            UpperCase = new char[0];
            Digits = new char[0];
            SpecialSymbols = new char[0];
        }

        public bool IsLowerCase(char c)
        {
            return LowerCase.Contains(c);
        }

        public bool IsUpperCase(char c)
        {
            return UpperCase.Contains(c);
        }

        public bool IsDigit(char c)
        {
            return Digits.Contains(c);
        }

        public bool IsSpecialSymbol(char c)
        {
            return SpecialSymbols.Contains(c);
        }
    }
}
