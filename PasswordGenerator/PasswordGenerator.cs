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

        public string Generate(string url)
        {
            CheckNullOrEmptyURL(url);

            string hostName = new Uri(url).Host;

            string password = cryptographer.Encrypt(hostName);

            return password;
        }

        private void CheckNullOrEmptyURL(string url)
        {
            if (url == null)
                throw new ArgumentNullException();

            if (url == "")
                throw new ArgumentException();
        }
    }
}
