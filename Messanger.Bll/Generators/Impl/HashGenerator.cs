using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Bll.Generators.Impl
{
    public class HashGenerator : IHashGenerator
    {
        private readonly byte[] _salt;

        public HashGenerator(string salt)
        {
            _salt = Encoding.UTF8.GetBytes(salt);
        }

        public string GenerateHash(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: _salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}
