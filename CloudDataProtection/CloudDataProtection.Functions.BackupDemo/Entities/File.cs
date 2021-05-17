namespace CloudDataProtection.Functions.BackupDemo.Entities
{
    public class File
    {
        public string StorageId { get; set; }
        
        public string Name { get; set; }
        
        public long Bytes { get; set; }
        
        public string ContentType { get; set; }
    }
}