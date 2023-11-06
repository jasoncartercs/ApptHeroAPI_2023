using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ApptHeroAPI.Common.Utilities
{
    public static class HashHelper
    {
        public static string HashString(this string stringToHash)
        {
            try
            {
                using (SHA256 hasher = SHA256.Create())
                {
                    byte[] hasedBytes = hasher.ComputeHash(Encoding.Unicode.GetBytes(stringToHash));

                    StringBuilder hashString = new StringBuilder();
                    for (int i = 0; i < hasedBytes.Length; i++)
                    {
                        hashString.Append(hasedBytes[i].ToString("X2"));
                    }
                    return hashString.ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
