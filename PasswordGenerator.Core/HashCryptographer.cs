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

        public byte[] Encrypt(string input)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(input);
            var hash = _hasher.HashData(data);            
            return hash;
        }
    }
}