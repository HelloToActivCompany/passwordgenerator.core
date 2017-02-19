using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PasswordGenerator.Core
{
    public class PasswordGenerator
    {
        const int DEFAULT_PASSWORD_MIN_LENGTH = 1;
        const int DEFAULT_PASSWORD_MAX_LENGTH = 40;

        private readonly ICryptographer _cryptographer;
        private readonly string _key;
        private readonly Base91Coder coder = new Base91Coder();

        private int _passwordMinLenght = DEFAULT_PASSWORD_MIN_LENGTH;
        public int PasswordMinLenght
        {
            get
            {
                return _passwordMinLenght;
            }

            set
            {
                if (value < DEFAULT_PASSWORD_MIN_LENGTH)
                    _passwordMinLenght = DEFAULT_PASSWORD_MIN_LENGTH;
                else
                    _passwordMinLenght = value;
            }
        }

        private int _passwordMaxLenght = DEFAULT_PASSWORD_MAX_LENGTH;
        public int PasswordMaxLenght
        {
            get
            {
                return _passwordMaxLenght;
            }

            set
            {
                if (value > DEFAULT_PASSWORD_MAX_LENGTH)
                    _passwordMaxLenght = DEFAULT_PASSWORD_MAX_LENGTH;
                else
                    _passwordMaxLenght = value;
            }
        }

        private PasswordDescriptor _pswdDescriptor;
        public PasswordDescriptor PswdDescriptor
        {
            get { return _pswdDescriptor; }

            set
            {
                if (value.PasswordLength < _passwordMinLenght)
                    value.PasswordLength = _passwordMinLenght;
                else if (value.PasswordLength > _passwordMaxLenght)
                    value.PasswordLength = _passwordMaxLenght;

                _pswdDescriptor = value;
            }
        }

        public PasswordGenerator(ICryptographer cryptographer, string key, PasswordDescriptor? descriptor = null)
        {
            _cryptographer = cryptographer;
            _key = key;

            PswdDescriptor = descriptor ?? GetDefaultPasswordDescriptor();
        }

        public string Generate(PasswordDescriptor descriptor, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("input");

            string forEncrypt = _key + input;

            string password = GeneratePasswordInAccordanceWithDescriptor(forEncrypt, descriptor);

            return password;
        }

        public string Generate(string input)
        {
            return Generate(PswdDescriptor, input);
        }

        private string GeneratePasswordInAccordanceWithDescriptor(string input, PasswordDescriptor descriptor)
        {
            byte[] crypt = _cryptographer.Encrypt(input);

            string password = coder.ToBase91String(crypt);

            password = FilterPassword(password, descriptor);

            while (password.Length < descriptor.PasswordLength)
            {
                var nextIteration = password;
                var nextFiltred = "";

                do
                {
                    nextIteration = coder.ToBase91String(_cryptographer.Encrypt(nextIteration));
                    nextFiltred = FilterPassword(nextIteration, descriptor);
                }
                while (nextFiltred.Length == 0);

                password += nextFiltred;
            }

            if (password.Length > descriptor.PasswordLength)
                password = password.Substring(0, descriptor.PasswordLength);

            password = AddDeficientAccordingWithDescriptor(password, descriptor);

            return password;
        }

        private string FilterPassword(string password, PasswordDescriptor descriptor)
        {
            return
                new String(password.ToCharArray().Where(c =>
                {
                    return (descriptor.Digits || !char.IsDigit(c))
                            && (descriptor.LowerCase || !char.IsLower(c))
                            && (descriptor.UpperCase || !char.IsUpper(c))
                            && (descriptor.SpecialSymbols || !Base91Coder.CharIsSpecial(c));
                }).ToArray());
        }

        private string AddDeficientAccordingWithDescriptor(string password, PasswordDescriptor descriptor)
        {
            var lockedIndices = new List<int>();

            while (!IsPasswordAccordingToDescription(password, descriptor))
            {
                if (descriptor.LowerCase && !Util.IsStringContainLowerCase(password))
                    password = AddSymbol(password, Base91Coder.GetLowerCaseChars(), lockedIndices);

                if (descriptor.UpperCase && !Util.IsStringContainUpperCase(password))
                    password = AddSymbol(password, Base91Coder.GetUpperCaseChars(), lockedIndices);

                if (descriptor.Digits && !Util.IsStringContainDigits(password))
                    password = AddSymbol(password, Base91Coder.GetDigits(), lockedIndices);

                if (descriptor.SpecialSymbols && !IsStringContainSpecialSymbols(password))
                    password = AddSymbol(password, Base91Coder.GetSpecialChars(), lockedIndices);
            }

            return password;
        }

        private bool IsPasswordAccordingToDescription(string password, PasswordDescriptor descriptor)
        {
            return (descriptor.LowerCase ? Util.IsStringContainLowerCase(password) : true) 
                    && (descriptor.UpperCase ? Util.IsStringContainUpperCase(password) : true)
                    && (descriptor.Digits ? Util.IsStringContainDigits(password) : true)
                    && (descriptor.SpecialSymbols ? IsStringContainSpecialSymbols(password) : true);
        }

        private bool IsStringContainSpecialSymbols(string str)
        {
            return str.ToCharArray().Any(c => Base91Coder.CharIsSpecial(c));
        }

        private string AddSymbol(string str, char[] charSet, List<int> lockedIndices)
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
            var hash = _cryptographer.Encrypt(str);

            int aggregateHash = hash.Aggregate(0, (s, i) => s + i);

            return aggregateHash;
        }

        private char GetAddingChar(string str, char[] charSet)
        {
            int aggregateHashValue = GetAggregateHashValue(str);

            int index = aggregateHashValue % charSet.Length;

            return charSet[index];
        }

        public struct PasswordDescriptor
        {
            public bool LowerCase;
            public bool UpperCase;
            public bool Digits;
            public bool SpecialSymbols;
            public int PasswordLength;
        }

        private PasswordDescriptor GetDefaultPasswordDescriptor()
        {
            return new PasswordDescriptor
            {
                LowerCase = true,
                UpperCase = true,
                Digits = true,
                SpecialSymbols = true,
                PasswordLength = 18
            };
        }
    }
}
