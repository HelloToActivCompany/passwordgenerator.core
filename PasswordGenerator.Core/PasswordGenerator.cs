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

            var prepare = TryParseHostNameUri(input);

            //prepare += !string.IsNullOrEmpty(login) ? "/" + login : login;
            if (!string.IsNullOrEmpty(login) && !prepare.Contains(login + "@"))
                prepare += "/" + login;


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
