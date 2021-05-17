using System.Collections.Generic;

namespace CloudDataProtection.Functions.BackupDemo.Service.Result
{
    public class InfoResult
    {
        public bool Success { get; }
        
        public IDictionary<string, string> Tags { get; set; }
        
        public long Bytes { get; set; }
        
        public bool IsNotFoundError { get; set; }

        public InfoResult(bool success)
        {
            Success = success;
        }
    }
}