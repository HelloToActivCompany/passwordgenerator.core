using PCLCrypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator.Core
{
    public class CompositePCLCryptographer : IHashCryptographer
    {
        private readonly IHashAlgorithmProvider[] _hashers;

        private readonly HashAlgorithm[] algorithms =
        {
            HashAlgorithm.Md5,
            HashAlgorithm.Sha1,
            HashAlgorithm.Sha256,
            HashAlgorithm.Sha384,
            HashAlgorithm.Sha512
        };

        public CompositePCLCryptographer()
        {
            _hashers = new IHashAlgorithmProvider[algorithms.Length];

            for (int i=0; i<algorithms.Length; i++)
            {
                _hashers[i] = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(algorithms[i]);
            } 
        }

        public byte[] Encrypt(byte[] data)
        {
            byte[] result = new byte[data.Length];
            data.CopyTo(result, 0);

            foreach (var hasher in _hashers)
                result = hasher.HashData(result);

            return result;
        }
    }
}
