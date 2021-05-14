namespace CloudDataProtection.Functions.BackupDemo.Triggers.Dto
{
    public class FileInfoResult
    {
        public string EncryptedName { get; set; }

        public string Url { get; set; }
        
        public int Bytes { get; set; }
    }
}