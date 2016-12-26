using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator
{
    public class PasswordGenerator : IPasswordGenerator
    {
        private readonly string key;

        private readonly ICryptographer cryptographer;

        public PasswordGenerator(ICryptographer cryptographer, string key)
        {
            this.key = key;
            this.cryptographer = cryptographer;
        }

        public string Generate(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException();

            string password = "";

            if (IsValidUri(input))
            {
                var hostName = new Uri(input).Host;
                password = cryptographer.Encrypt(hostName.Replace("www.", ""));
            }
            else
            {
                password = cryptographer.Encrypt(input);
            }

            password = key + password;

            password = cryptographer.Encrypt(password);

            return password;
        }

        private bool IsValidUri(String uri)
        {
            try
            {
                new Uri(uri);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
