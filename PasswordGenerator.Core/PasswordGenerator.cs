using System;

namespace PasswordGeneratorCore
{
    public class PasswordGenerator : IPasswordGenerator
    {
        private readonly string _key;

        private readonly ICryptographer _cryptographer;

        public PasswordGenerator(ICryptographer cryptographer, string key)
        {
            this._cryptographer = cryptographer;
            this._key = key;
        }

        public string Generate(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException();

            var prepare = TryParseHostNameUri(input) ?? input;

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
                return null;
            }
        }
    }
}
