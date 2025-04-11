using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Configuration;

namespace ExternalAPI
{
    public class ExternalApi
    {
        private readonly string _bucketName;
        private readonly IAmazonS3 _s3Client;

        public ExternalApi()
        {
            _bucketName = ConfigurationManager.AppSettings["AWSBucketName"];
            string accessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
            string secretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
            string region = ConfigurationManager.AppSettings["AWSRegion"];

            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
            {
                throw new Exception("AWS credentials are missing in web.config.");
            }

            _s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.GetBySystemName(region));
        }

        public async Task<bool> UploadFileAsync(string fileName, object data)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    ContentBody = jsonData,
                    ContentType = "application/json"
                };

                var response = await _s3Client.PutObjectAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file: {ex.Message}");
                return false;
            }
        }

        public async Task<string> ReadFileAsync(string fileName)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName
                };

                var response = await _s3Client.GetObjectAsync(request);
                var reader = new StreamReader(response.ResponseStream);
                return await reader.ReadToEndAsync();
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"S3 Error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName
                };

                var response = await _s3Client.DeleteObjectAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
                return false;
            }
        }

        public async Task<List<string>> ListFilesAsync()
        {
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = _bucketName
                };

                var response = await _s3Client.ListObjectsV2Async(request);
                var files = new List<string>();
                foreach (var obj in response.S3Objects)
                {
                    files.Add(obj.Key);
                }
                return files;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listing files: {ex.Message}");
                return new List<string>();
            }
        }
    }
}