namespace CloudDataProtection.Functions.BackupDemo.Triggers.Dto
{
    public class FileDownloadResult
    {
        public byte[] Bytes { get; set; }
        
        public string Url { get; set; }
        
        public string FileName { get; set; }
        
        public FileDownloadResultType Type { get; set; }
        
        public string ContentType { get; set; }
    }

    public enum FileDownloadResultType
    {
        Bytes = 0,
        Url = 1
    }
}