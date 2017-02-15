using System;
using System.Text.RegularExpressions;

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

            string password = Encrypt(forEncrypt, descriptor);                       

            return password;
        }

        public string Generate(string input)
        {
            return Generate(PswdDescriptor, input);
        }

        private string Encrypt(string data, PasswordDescriptor descriptor)
        {
            byte[] crypt =_cryptographer.Encrypt(data);
            
            string password = coder.ToBase91String(crypt);

            while (password.Length < descriptor.PasswordLength)
                password += _cryptographer.Encrypt(password);

            password = AdaptPasswordToThePasswordDescriptor(password, descriptor);

            return password;
        }

        private string AdaptPasswordToThePasswordDescriptor(string password, PasswordDescriptor descriptor)
        {
            if (password.Length > descriptor.PasswordLength)
                password = password.Substring(0, descriptor.PasswordLength);

            //TO DO:: realise the logic

            return password;
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
