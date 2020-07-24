using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SecretsManagerFacadeLib.Contracts;
using SecretsManagerFacadeLib.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GameClientApi.Interfaces.Clients.Aws
{
    public class S3Client
    {
        private IAmazonS3 _s3Client;
        private ITransferUtility _fileTransferUtility;
        private readonly RegionEndpoint _regionEndpoint;
        private readonly ICredentialsFacade<AwsCredentials> _credentialsFacade;

        private readonly ILogger<S3Client> _logger;
        private readonly IConfiguration _config;

        public S3Client(ICredentialsFacade<AwsCredentials> credentialsFacade, RegionEndpoint regionEndpoint, ILogger<S3Client> logger, IConfiguration config)
        {
            _credentialsFacade = credentialsFacade;
            _regionEndpoint = regionEndpoint;
            _logger = logger;
            _config = config;
            InitClient();
        }

        private void InitClient()
        {
            var awsCredentials = _credentialsFacade.GetCredentials();
            var BasicAwsCredentials = new BasicAWSCredentials(awsCredentials.AccessKey, awsCredentials.SecretKey);
            _s3Client = new AmazonS3Client(BasicAwsCredentials, _regionEndpoint);
            _fileTransferUtility = new TransferUtility(_s3Client);
            _logger.LogInformation("S3 Client created successfully");
        }

        public async Task UploadFile(string path)
        {
            try
            {
                var filename = Path.GetFileName(path);
                var s3Section = _config.GetSection("AWS:S3");
                var bucketName = s3Section["BucketName"];
                _logger.LogInformation($"Attempting to upload {filename} to S3 bucket {bucketName} ...");
                
                await _fileTransferUtility.UploadAsync(path, bucketName);
                _logger.LogInformation($"Upload successful.");
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Unknown error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
    }
}
