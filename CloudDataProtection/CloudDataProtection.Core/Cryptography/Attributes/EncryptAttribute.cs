using System;

namespace CloudDataProtection.Core.Cryptography.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EncryptAttribute : Attribute
    {
        public DataType DataType { get; set; }
    }

    public enum DataType
    {
        Unknown = 0,
        EmailAddress = 100
    }
}