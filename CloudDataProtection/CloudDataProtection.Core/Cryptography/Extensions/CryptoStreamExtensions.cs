using System;
using System.Security.Cryptography;
using System.Text;

namespace CloudDataProtection.Core.Cryptography.Extensions
{
    public static class CryptoStreamExtensions
    {
        /// <summary>
        /// Writes a UTF8 string to a CryptoStream, flushes the data and closes the stream.
        /// </summary>
        /// <param name="cryptoStream">CryptoStream</param>
        /// <param name="input">UTF8 string</param>
        public static void WriteUtf8String(this CryptoStream cryptoStream, string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);

            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            cryptoStream.Close();
        }

        /// <summary>
        /// Writes a Base64 string to a CryptoStream, flushes the data and closes the stream.
        /// </summary>
        /// <param name="cryptoStream">CryptoStream</param>
        /// <param name="input">Base64 string</param>
        public static void WriteBase64String(this CryptoStream cryptoStream, string input)
        {
            byte[] bytes = Convert.FromBase64String(input);
            
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            cryptoStream.Close();
        } 
    }
}