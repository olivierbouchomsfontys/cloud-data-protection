using System;

namespace CloudDataProtection.Core.Cryptography.Aes.Options
{
    public class AesOptions
    {
        public string EncryptionKey { get; set; }
        
        public string EncryptionIv { get; set; }
        
        public int KeySize { get; set; }
        
        public int BlockSize { get; set; }

        public byte[] Key => Convert.FromBase64String(EncryptionKey);
        public byte[] Iv => Convert.FromBase64String(EncryptionIv);
    }
}