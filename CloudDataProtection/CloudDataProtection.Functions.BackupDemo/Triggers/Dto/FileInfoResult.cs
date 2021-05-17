namespace CloudDataProtection.Functions.BackupDemo.Triggers.Dto
{
    public class FileInfoResult
    {
        public string Name { get; set; }

        public long Bytes { get; set; }
        
        public string ContentType { get; set; }
    }
}