using System;
using PCLCrypto;

namespace PasswordGenerator.Core
{
    public class PCLCryptographer: IHashCryptographer
    {
        private readonly IHashAlgorithmProvider _hasher;
        
        public PCLCryptographer(HashAlgorithm algorithm = HashAlgorithm.Md5)
        {
            _hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(algorithm);
        }

        public byte[] Encrypt(byte[] data)
        {
            return _hasher.HashData(data); 
        }
    }
}