using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using System.Net;
using Videlo.Configuration;
using Videlo.Models;

namespace Videlo.Services
{
    public class S3Service
    {
        private readonly AWSConfiguration _options;
        private readonly AmazonS3Client _client;

        public S3Service(IOptions<AWSConfiguration> options)
        {
            _options = options.Value;

            var config = new AmazonS3Config()
            {
                ServiceURL = $"https://{_options.ServiceEndpoint}"
            };
            var credentials = new BasicAWSCredentials(_options.AccessKey, _options.SecretKey);

            _client = new AmazonS3Client(credentials, config);
        }

        public async Task<string> UploadVideoAsync(FormFileInfo file)
        {
            return await UploadFileAsync(file, "videos");
        }

        public async Task<string> UploadVideoThumbnailAsync(FormFileInfo file)
        {
            return await UploadFileAsync(file, "video-thumbnails");
        }

        private async Task<string> UploadFileAsync(FormFileInfo file, string folderName)
        {
            var path = $"{folderName}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var request = new PutObjectRequest()
            {
                BucketName = _options.BucketName,
                Key = path,
                InputStream = file.Stream
            };
            request.Metadata.Add("Content-Type", file.ContentType);
            var response = await _client.PutObjectAsync(request);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return path;
            }
            else
            {
                return string.Empty;
            }
        }


        public async Task<HttpStatusCode> DeleteFileAsync(string filePath)
        {
            var request = new DeleteObjectRequest()
            {
                BucketName = _options.BucketName,
                Key = filePath
            };
            var response = await _client.DeleteObjectAsync(request);
            return response.HttpStatusCode;
        }
    }
}
