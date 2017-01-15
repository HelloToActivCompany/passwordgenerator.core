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

        public string Generate(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException();

            var prepare = TryParseHostNameUri(input);

            var password = _cryptographer.Encrypt(_key + prepare);

            return password;
        }

        private string TryParseHostNameUri(string input)
        {
            try
            {
                var hostName = new Uri(input).Host;
                return hostName.Replace("www.", "");
            }
            catch
            {
                return input;
            }
        }
    }
}
