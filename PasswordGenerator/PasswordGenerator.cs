using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator
{
    public class PasswordGenerator
    {
        public static string Generate(string url)
        {
            if (url == null)
                throw new ArgumentNullException();

            if (url == "")
                throw new ArgumentException();

            var password = "";


            return password;
        }
    }
}
