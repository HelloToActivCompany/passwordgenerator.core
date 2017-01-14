using System;
using PCLCrypto;

namespace PasswordGeneratorCore
{
    public class HashCryptographer: ICryptographer
    {
        private readonly HashAlgorithm _algorithm;

        public HashCryptographer()
        {
            _algorithm = HashAlgorithm.Md5;
        }

        public HashCryptographer(HashAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }

        public string Encrypt(string input)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(input);
            var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(_algorithm);
            var hash = hasher.HashData(data);
            var hashBase64 = Convert.ToBase64String(hash);
            return hashBase64;
        }
    }
}