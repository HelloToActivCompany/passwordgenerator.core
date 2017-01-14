using System;
using PCLCrypto;

namespace PasswordGenerator
{
    public class HashCryptographer: ICryptographer
    {
        private HashAlgorithm algorithm;

        public HashCryptographer()
        {
            algorithm = HashAlgorithm.Md5;
        }

        public HashCryptographer(HashAlgorithm algorithm)
        {
            this.algorithm = algorithm;
        }

        public string Encrypt(string input)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(input);
            var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(algorithm);
            var hash = hasher.HashData(data);
            var hashBase64 = Convert.ToBase64String(hash);
            return hashBase64;
        }
    }
}