using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Cryptography.Aes.Options;
using CloudDataProtection.Core.Environment;
using CloudDataProtection.Functions.BackupDemo.Business;
using CloudDataProtection.Functions.BackupDemo.Service;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Functions.BackupDemo.Factory
{
    public class FileManagerLogicFactory
    {
        private static FileManagerLogicFactory _instance;

        public static FileManagerLogicFactory Instance => _instance ??= new FileManagerLogicFactory();

        public FileManagerLogic GetLogic()
        {
            AesOptions options = new AesOptions
            {
                KeySize = 256,
                BlockSize = 128,
                EncryptionKey = EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEMO_BLOB_AES_KEY"),
                EncryptionIv = EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEMO_BLOB_AES_IV")
            };

            IDataTransformer transformer = new AesStreamTransformer(options);
            ITransformer stringTransformer = new AesTransformer(Options.Create(options));
            IFileService fileService = new BlobStorageFileService();

            return new FileManagerLogic(fileService, transformer, stringTransformer);
        }
    }
}