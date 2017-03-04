using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PasswordGenerator.Core
{
    public class PasswordGenerator
    {
        private const int DEFAULT_PASSWORD_MIN_LENGTH = 4;
        private const int DEFAULT_PASSWORD_MAX_LENGTH = 40;

        private readonly string _key;

        private readonly IHashCryptographer _cryptographer;
        private UniversalAlphabetCoderBase _coder;
        private PasswordDescriptor _passwordDescriptor;
        public PasswordDescriptor PasswordDescriptor
        {
            get
            {
                return _passwordDescriptor;
            }

            set
            {
                _passwordDescriptor = value;

                if (_passwordDescriptor == null)
                    _passwordDescriptor = GetDefaultPasswordDescriptor();

                if (_passwordDescriptor.PasswordLength < _passwordMinLength)
                    _passwordDescriptor.PasswordLength = _passwordMinLength;
                else if (_passwordDescriptor.PasswordLength > _passwordMaxLength)
                    _passwordDescriptor.PasswordLength = _passwordMaxLength;
            }
        }

        private int _passwordMinLength = DEFAULT_PASSWORD_MIN_LENGTH;
        public int PasswordMinLength
        {
            get
            {
                return _passwordMinLength;
            }

            set
            {
                if (value < DEFAULT_PASSWORD_MIN_LENGTH)
                    _passwordMinLength = DEFAULT_PASSWORD_MIN_LENGTH;
                else
                {
                    _passwordMinLength = value;
                    if (_passwordMinLength > _passwordMaxLength)
                        _passwordMaxLength = _passwordMinLength;
                }
            }
        }

        private int _passwordMaxLength = DEFAULT_PASSWORD_MAX_LENGTH;
        public int PasswordMaxLength
        {
            get
            {
                return _passwordMaxLength;
            }

            set
            {
                if (value > DEFAULT_PASSWORD_MAX_LENGTH)
                    _passwordMaxLength = DEFAULT_PASSWORD_MAX_LENGTH;
                else
                {
                    _passwordMaxLength = value;
                    if (_passwordMaxLength < _passwordMinLength)
                        _passwordMinLength = _passwordMaxLength;
                }
            }
        }

        public PasswordGenerator(string key, IHashCryptographer cryptographer = null, PasswordDescriptor descriptor = null, UniversalAlphabetCoderBase coder = null)
        {
            PasswordDescriptor = descriptor; 
                 
            _key = key;

            _cryptographer = cryptographer ?? GetDefaultCryptographer();

            _coder = coder ?? GetDefaultCoder();      
        }

        public string Generate(PasswordDescriptor descriptor, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("input");

            string forEncrypt = _key + input;

            string password = GeneratePasswordForDescriptor(forEncrypt, descriptor);

            return password;
        }

        public string Generate(string input)
        {
            return Generate(PasswordDescriptor, input);
        }

        private string GeneratePasswordForDescriptor(string input, PasswordDescriptor descriptor)
        {
            ConfigurateCoderByDescriptor(_coder, descriptor);

            string password = _coder.ConvertBytesToString(_cryptographer.Encrypt(ConvertStringToBytes(input)));

            while (password.Length < descriptor.PasswordLength)
            {
                password += _coder.ConvertBytesToString(_cryptographer.Encrypt(ConvertStringToBytes(password)));
            }

            if (password.Length > descriptor.PasswordLength)
                password = password.Substring(0, descriptor.PasswordLength);

            password = AddDeficientFromDescriptor(password, descriptor);

            return password;
        }

        private void ConfigurateCoderByDescriptor(UniversalAlphabetCoderBase coder, PasswordDescriptor descriptor)
        {
            var alphabet = new List<char>();

            if (descriptor.LowerCase)
                alphabet.AddRange(PasswordDescriptor.Alphabet.LowerCase);
            if (descriptor.UpperCase)
                alphabet.AddRange(PasswordDescriptor.Alphabet.UpperCase);
            if (descriptor.Digits)
                alphabet.AddRange(PasswordDescriptor.Alphabet.Digits);
            if (descriptor.SpecialSymbols)
                alphabet.AddRange(PasswordDescriptor.Alphabet.SpecialSymbols);

            coder.Alphabet = alphabet.ToArray();
        }

        private byte[] ConvertStringToBytes(string str)
        {
            return Encoding.Unicode.GetBytes(str);
        }

        private string AddDeficientFromDescriptor(string password, PasswordDescriptor descriptor)
        {
            var lockedIndices = new List<int>();

            while (!IsPasswordSatisfiesDescriptor(password, descriptor))
            {
                if (descriptor.LowerCase && !AlphabetHelper.IsStringContainLowerCase(password, PasswordDescriptor.Alphabet))
                    password = AddSymbol(password, (IList<char>)PasswordDescriptor.Alphabet.LowerCase, lockedIndices);

                if (descriptor.UpperCase && !AlphabetHelper.IsStringContainUpperCase(password, PasswordDescriptor.Alphabet))
                    password = AddSymbol(password, (IList<char>)PasswordDescriptor.Alphabet.UpperCase, lockedIndices);

                if (descriptor.Digits && !AlphabetHelper.IsStringContainDigits(password, PasswordDescriptor.Alphabet))
                    password = AddSymbol(password, (IList<char>)PasswordDescriptor.Alphabet.Digits, lockedIndices);

                if (descriptor.SpecialSymbols && !AlphabetHelper.IsStringContainSpecialSymbols(password, PasswordDescriptor.Alphabet))
                    password = AddSymbol(password, (IList<char>)PasswordDescriptor.Alphabet.SpecialSymbols, lockedIndices);
            }

            return password;
        }

        private bool IsPasswordSatisfiesDescriptor(string password, PasswordDescriptor descriptor)
        {
            return (descriptor.LowerCase ? AlphabetHelper.IsStringContainLowerCase(password, PasswordDescriptor.Alphabet) : true) 
                    && (descriptor.UpperCase ? AlphabetHelper.IsStringContainUpperCase(password, PasswordDescriptor.Alphabet) : true)
                    && (descriptor.Digits ? AlphabetHelper.IsStringContainDigits(password, PasswordDescriptor.Alphabet) : true)
                    && (descriptor.SpecialSymbols ? AlphabetHelper.IsStringContainSpecialSymbols(password, PasswordDescriptor.Alphabet) : true);
        }

        private string AddSymbol(string str, IList<char> charSet, List<int> lockedIndices)
        {
            int aggregateHashValue = GetAggregateHashValue(str);

            int indexInStr = aggregateHashValue % str.Length;

            if (lockedIndices.Contains(indexInStr))
            {
                int minRelativelyPrime = Util.GetMinRelativelyPrimeNumber(str.Length);
               
                do
                {
                    indexInStr += minRelativelyPrime;
                    if (indexInStr >= str.Length)
                        indexInStr -= str.Length;
                }
                while (lockedIndices.Contains(indexInStr));
            }
            
            lockedIndices.Add(indexInStr);

            char adding = GetAddingChar(str, charSet);

            return str.Substring(0, indexInStr) + adding.ToString() + str.Substring(indexInStr + 1);
        }

        private int GetAggregateHashValue(string str)
        {
            var hash = _cryptographer.Encrypt(ConvertStringToBytes(str));

            int aggregateHash = hash.Aggregate(0, (s, i) => s + i);

            return aggregateHash;
        }

        private char GetAddingChar(string str, IList<char> charSet)
        {
            int aggregateHashValue = GetAggregateHashValue(str);

            int index = aggregateHashValue % charSet.Count;

            return charSet[index];
        }

        private IHashCryptographer GetDefaultCryptographer()
        {
            return new PCLCryptographer();
        }

        private PasswordDescriptor GetDefaultPasswordDescriptor()
        {
            return new PasswordDescriptor
            {
                LowerCase = true,
                UpperCase = true,
                Digits = true,
                SpecialSymbols = true,
                PasswordLength = 18,
            };
        }

        private UniversalAlphabetCoderBase GetDefaultCoder()
        {
            var coder = new SimpleUniversalCoder(null);
            ConfigurateCoderByDescriptor(coder, PasswordDescriptor);

            return coder;
        }        
    }

    public class PasswordDescriptor
    {
        private Alphabet _alphabet;
        public Alphabet Alphabet
        {
            get
            {
                if (_alphabet == null)                
                    _alphabet = new Alphabet();
                return _alphabet;
            }
            set
            {
                _alphabet = value;
            }
        }
        public bool LowerCase { get; set; }      
        public bool UpperCase { get; set; }
        public bool Digits { get; set; }
        public bool SpecialSymbols { get; set; }
        public int PasswordLength { get; set; }
    }
}
