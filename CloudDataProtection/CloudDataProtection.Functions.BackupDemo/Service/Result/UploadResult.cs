namespace CloudDataProtection.Functions.BackupDemo.Service.Result
{
    public class UploadResult
    {
        public bool Success { get; }
        public string Id { get; set; }

        public UploadResult(bool success)
        {
            Success = success;
        }
    }
}