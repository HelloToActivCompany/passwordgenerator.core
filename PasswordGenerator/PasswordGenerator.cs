using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator
{
    public class PasswordGenerator : IPasswordGenerator
    {
        private ICryptographer cryptographer;

        public PasswordGenerator(ICryptographer cryptographer)
        {
            this.cryptographer = cryptographer;
        }

        public string Generate(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException();

            string password = "";

            try
            {
                var hostName = new Uri(input).Host;
                password = cryptographer.Encrypt(hostName);
            }
            catch
            {
                password = cryptographer.Encrypt(input);
            }

            return password;
        }
    }
}
