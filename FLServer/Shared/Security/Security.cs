﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Security
{
    public static class Security
    {

        internal static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
