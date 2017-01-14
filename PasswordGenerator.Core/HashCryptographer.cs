using System;
using PCLCrypto;

namespace PasswordGenerator.Core
{
    public class HashCryptographer: ICryptographer
    {
        private readonly IHashAlgorithmProvider _hasher;
        
        public HashCryptographer(HashAlgorithm algorithm = HashAlgorithm.Md5)
        {
            _hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(algorithm);
        }

        public string Encrypt(string input)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(input);
            var hash = _hasher.HashData(data);
            var hashBase64 = Convert.ToBase64String(hash);
            return hashBase64;
        }
    }
}