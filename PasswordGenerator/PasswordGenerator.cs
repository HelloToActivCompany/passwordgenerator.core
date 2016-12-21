using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator
{
    public class PasswordGenerator : IPasswordGenerator
    {
        public string Generate(string url)
        {
            CheckNullOrEmptyURL(url);

            var password = "";
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
