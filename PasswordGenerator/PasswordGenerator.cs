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

        public PasswordGenerator() { }

        public PasswordGenerator(ICryptographer cryptographer)
        {
            this.cryptographer = cryptographer;
        }

        public string Generate(string url)
        {
            CheckNullOrEmptyURL(url);

            string preparedUrl = PrepaerURL(url);       

            string password = cryptographer.Encrypt(preparedUrl);

            return password;
        }
        private string PrepaerURL(string url)
        {
            string preparedUrl = url;

            preparedUrl = preparedUrl.Replace(@"http://", String.Empty);
            preparedUrl = preparedUrl.Replace(@"https://", String.Empty);
            preparedUrl = preparedUrl.Substring(0, preparedUrl.IndexOf(@"/"));

            return preparedUrl;
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
