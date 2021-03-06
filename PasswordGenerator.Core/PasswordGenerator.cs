﻿using System;
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
        private CoderBase _coder;
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

        public PasswordGenerator(string key, IHashCryptographer cryptographer = null, PasswordDescriptor descriptor = null, CoderBase coder = null)
        {
            PasswordDescriptor = descriptor; 
                 
            _key = key;

            _cryptographer = cryptographer ?? new PCLCryptographer();

            _coder = coder ?? new Coder();      
        }

        public string Generate(string input, PasswordDescriptor descriptor)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("input");

            string forEncrypt = _key + input;

            string password = GeneratePasswordForDescriptor(forEncrypt, descriptor);

            return password;
        }

        public string Generate(string input)
        {
            return Generate(input, PasswordDescriptor);
        }

        private string GeneratePasswordForDescriptor(string input, PasswordDescriptor descriptor)
        {
            string password = _coder.ConvertBytesToString(_cryptographer.Encrypt(ConvertStringToBytes(input)), descriptor.GetCharArray());

            while (password.Length < descriptor.PasswordLength)
            {
                password += _coder.ConvertBytesToString(_cryptographer.Encrypt(ConvertStringToBytes(password)), descriptor.GetCharArray());
            }

            if (password.Length > descriptor.PasswordLength)
                password = password.Substring(0, descriptor.PasswordLength);

            password = AddDeficientsFromDescriptor(password, descriptor);

            return password;
        }

        private byte[] ConvertStringToBytes(string str)
        {
            return Encoding.Unicode.GetBytes(str);
        }

        private string AddDeficientsFromDescriptor(string password, PasswordDescriptor descriptor)
        {
            var lockedIndices = new List<int>();

            if (descriptor.LowerCase)
            {
                password = AddDeficientIfNessesary(password, descriptor.Alphabet.LowerCase, lockedIndices);
            }

            if (descriptor.UpperCase)
            {
                password = AddDeficientIfNessesary(password, descriptor.Alphabet.UpperCase, lockedIndices);
            }

            if (descriptor.Digits)
            {
                password = AddDeficientIfNessesary(password, descriptor.Alphabet.Digits, lockedIndices);
            }

            if (descriptor.SpecialSymbols)
            {
                password = AddDeficientIfNessesary(password, descriptor.Alphabet.SpecialSymbols, lockedIndices);
            }

            return password;
        }

        private string AddDeficientIfNessesary(string password, IReadOnlyList<char> set, List<int> lockedIndices)
        {           
            if (!password.ToCharArray().ContainsElementFrom(set))
            {
                password = AddSymbol(password, set, lockedIndices);                
            }
            else
            {
                for (int i = 0; i < set.Count(); i++)
                {
                    int indx;
                    if ((indx = password.IndexOf(set[i])) != -1)
                    {
                        lockedIndices.Add(indx);
                        break;
                    }
                }
            }      

            return password;
        }

        private string AddSymbol(string str, IReadOnlyList<char> set, List<int> lockedIndices)
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

            char adding = GetAddingChar(str, set);

            return str.Substring(0, indexInStr) + adding.ToString() + str.Substring(indexInStr + 1);
        }

        private int GetAggregateHashValue(string str)
        {
            var hash = _cryptographer.Encrypt(ConvertStringToBytes(str));

            int aggregateHash = hash.Aggregate(0, (s, i) => s + i);

            return aggregateHash;
        }

        private char GetAddingChar(string str, IReadOnlyList<char> charSet)
        {
            int aggregateHashValue = GetAggregateHashValue(str);

            int index = aggregateHashValue % charSet.Count;

            return charSet[index];
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

        public char[] GetCharArray()
        {
            var alphabet = new List<char>();

            if (LowerCase)
                alphabet.AddRange(Alphabet.LowerCase);
            if (UpperCase)
                alphabet.AddRange(Alphabet.UpperCase);
            if (Digits)
                alphabet.AddRange(Alphabet.Digits);
            if (SpecialSymbols)
                alphabet.AddRange(Alphabet.SpecialSymbols);

            return alphabet.ToArray();
        }
    }
}
