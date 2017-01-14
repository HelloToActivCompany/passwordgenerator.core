using System;
using PCLCrypto;

namespace PasswordGenerator
{
    public class HashCryptographer: ICryptographer
    {
        public string Encrypt(string input)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(input);
            var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha1);
            var hash = hasher.HashData(data);
            var hashBase64 = Convert.ToBase64String(hash);
            return hashBase64;
        }
    }
}