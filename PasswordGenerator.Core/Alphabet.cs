using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator.Core
{
    public class Alphabet
    {
        private static readonly char[] _defaultLowerCase = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private static readonly char[] _defaultUpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private static readonly char[] _defaultDigits = "0123456789".ToCharArray();
        private static readonly char[] _defaultSpecialSymbols = "!#$%&()*+,./:;<=>?@[]^_\"{|}~ ".ToCharArray();
        
        private List<char> _lowerCase;
        private List<char> _upperCase;
        private List<char> _digits;
        private List<char> _specialSymbols;
        private List<char> _unsupported;

        public IReadOnlyList<char> LowerCase
        {
            get { return _lowerCase; }
        }

        public IReadOnlyList<char> UpperCase
        {
            get { return _upperCase; }
        }

        public IReadOnlyList<char> Digits
        {
            get { return _digits; }
        }

        public IReadOnlyList<char> SpecialSymbols
        {
            get { return _specialSymbols; }
        }

        public IReadOnlyList<char> Unsupported
        {
            get { return _unsupported; }
        }

        public Alphabet()
        {
            _lowerCase = new List<char>(_defaultLowerCase);
            _upperCase = new List<char>(_defaultUpperCase);
            _digits = new List<char>(_defaultDigits);
            _specialSymbols = new List<char>(_defaultSpecialSymbols);
            _unsupported = new List<char>();
        }

        public Alphabet(string alphabet)
        {
            _lowerCase = new List<char>();
            _upperCase = new List<char>();
            _digits = new List<char>();
            _specialSymbols = new List<char>();
            _unsupported = new List<char>();

            foreach (char c in alphabet)
            {
                Add(c);
            }
        }

        public void Add(char symbol)
        {
            if (char.IsLower(symbol))
                _lowerCase.Add(symbol);
            else if (char.IsUpper(symbol))
                _upperCase.Add(symbol);
            else if (char.IsDigit(symbol))
                _digits.Add(symbol);
            else if (_defaultSpecialSymbols.Contains(symbol))
                _specialSymbols.Add(symbol);
            else _unsupported.Add(symbol);
        }
        public void Add(string symbols)
        {
            foreach (char c in symbols)
            {
                Add(c);
            }
        }

        public bool IsLowerCase(char c)
        {
            return _lowerCase.Contains(c);
        }

        public bool IsUpperCase(char c)
        {
            return _upperCase.Contains(c);
        }

        public bool IsDigit(char c)
        {
            return _digits.Contains(c);
        }

        public bool IsSpecialSymbol(char c)
        {
            return _specialSymbols.Contains(c);
        }
    }
}
