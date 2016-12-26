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
            this.cryptographer = cryptographer;
            this.key = key;
        }

        public string Generate(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException();

            var prepare = TryParseHostNameUri(input) ?? input;

            string password = cryptographer.Encrypt(key + prepare);

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
