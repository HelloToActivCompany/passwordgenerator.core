using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PasswordGenerator.Core
{
    public static class AlphabetHelper
    {
        public static bool IsStringContainLowerCase(string str, Alphabet alphabet)
        {
            return str.ToCharArray().Any(c => alphabet.IsLowerCase(c));
        }

        public static bool IsStringContainUpperCase(string str, Alphabet alphabet)
        {
            return str.ToCharArray().Any(c => alphabet.IsUpperCase(c));
        }

        public static bool IsStringContainDigits(string str, Alphabet alphabet)
        {
            return str.ToCharArray().Any(c => alphabet.IsDigit(c));
        }

        public static bool IsStringContainSpecialSymbols(string str, Alphabet alphabet)
        {
            return str.ToCharArray().Any(c => alphabet.IsSpecialSymbol(c));
        }
    }
}
