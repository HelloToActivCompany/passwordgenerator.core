using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordGenerator.Core
{
    public class PasswordGenerator
    {
        private readonly ICryptographer _cryptographer;
        private readonly string _key;
        private readonly Base91Coder coder = new Base91Coder();

        private PasswordDescriptor _pswdDescriptor;
        public PasswordDescriptor PswdDescriptor
        {
            get { return _pswdDescriptor; }

            set
            {
                const int PASSWORD_MIN_LENGTH = 6;
                const int PASSWORD_MAX_LENGTH = 40;

                if (value.PasswordLength < PASSWORD_MIN_LENGTH)
                    value.PasswordLength = PASSWORD_MIN_LENGTH;
                else if (value.PasswordLength > PASSWORD_MAX_LENGTH)
                    value.PasswordLength = PASSWORD_MAX_LENGTH;

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
            if (string.IsNullOrEmpty(input))
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

            password = AddDeficientAccordingToDescription(password, descriptor);
            
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
        
        private string AddDeficientAccordingToDescription(string password, PasswordDescriptor descriptor)
        {
            var lockedIndices = new List<int>();

            if (descriptor.LowerCase && (String.Equals(password.ToUpper(), password, StringComparison.Ordinal)))            
                password = AddSymbol(password, Base91Coder.GetLowerCaseChars(), lockedIndices);                
            
            if (descriptor.UpperCase && (String.Equals(password.ToLower(), password, StringComparison.Ordinal)))            
                password = AddSymbol(password, Base91Coder.GetUpperCaseChars(), lockedIndices);
            
            if (descriptor.Digits && (!password.ToCharArray().Any(c => char.IsDigit(c))))            
                password = AddSymbol(password, Base91Coder.GetDigits(), lockedIndices);
            
            if (descriptor.SpecialSymbols && (!password.ToCharArray().Any(c => Base91Coder.CharIsSpecial(c))))            
                password = AddSymbol(password, Base91Coder.GetSpecialChars(), lockedIndices);
            

            return password;
        }

        private string AddSymbol(string str, char[] charSet, List<int> lockedIndices)
        {
            int aggregateHashValue = GetAggregateHashValue(str);

            int indexInStr = aggregateHashValue % str.Length;

            
            while (!lockedIndices.All(i => i != indexInStr))
                indexInStr = (indexInStr + aggregateHashValue) % str.Length;
            
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
