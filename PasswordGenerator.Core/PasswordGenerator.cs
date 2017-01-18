using System;

namespace PasswordGenerator.Core
{
    public class PasswordGenerator : IPasswordGenerator
    {
        private readonly ICryptographer _cryptographer;
        private readonly string _key;

        public PasswordGenerator(ICryptographer cryptographer, string key)
        {
            _cryptographer = cryptographer;
            _key = key;
        }

        public string Generate(string input, string login = "")
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException();
            
            var temp = input;
            
            if (!string.IsNullOrEmpty(login))
                temp += "/" + login;

            var password = _cryptographer.Encrypt(_key + temp);

            return password;
        }

    }
}
