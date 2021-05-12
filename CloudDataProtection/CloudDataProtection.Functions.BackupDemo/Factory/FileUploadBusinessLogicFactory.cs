using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Cryptography.Aes.Options;
using CloudDataProtection.Core.Environment;
using CloudDataProtection.Functions.BackupDemo.Business;

namespace CloudDataProtection.Functions.BackupDemo.Factory
{
    public class FileUploadBusinessLogicFactory
    {
        private static FileUploadBusinessLogicFactory _instance;

        public static FileUploadBusinessLogicFactory Instance =>
            _instance ?? (_instance = new FileUploadBusinessLogicFactory());

        public FileUploadBusinessLogic GetLogic()
        {
            AesOptions options = new AesOptions
            {
                KeySize = 256,
                BlockSize = 128,
                EncryptionKey = EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEMO_BLOB_AES_KEY"),
                EncryptionIv = EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEMO_BLOB_AES_IV")
            };

            AesStreamTransformer transformer = new AesStreamTransformer(options);
            
            return new FileUploadBusinessLogic(transformer);
        }
    }
}