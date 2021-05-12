using System;

namespace CloudDataProtection.Functions.BackupDemo.Entities
{
    public class File
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public string StorageId { get; set; }
        
        public string Url { get; set; }
    }
}