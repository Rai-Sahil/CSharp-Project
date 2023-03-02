using Amazon;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CSharp_Backend
{
    public class AWS_S3_upload
    {

        public async Task<bool> UploadFileAsync(
            IAmazonS3 client,
            string bucketName,
            string objectName,
            string filePath)
        {
            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = objectName,
                    FilePath = filePath
                };

                var response = await client.PutObjectAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine($"Successfully uploaded {objectName} to {bucketName}.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Could not upload {objectName} to {bucketName}.");
                    return false;
                }
            }
            catch (AmazonS3Exception)
            {
                Console.WriteLine("Could not connect AWS S3");
                return false;

            }
            catch (Exception)
            {
                Console.WriteLine("File Not Found");
                return false;
            }
        }

        public bool lowLevelAPIUpload(string accessKey, string secretAccessKey, string bucketName, string fileName)
        {
            IAmazonS3 client = new AmazonS3Client(accessKey, secretAccessKey, RegionEndpoint.USWest2);

            FileInfo file = new FileInfo(fileName);
            string destPath = "Ron Swanson, A Lifestyle (Vol. III) _ Parks and Recreation.mp4"; // <-- low-level s3 path uses /
            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = file.OpenRead(),
                BucketName = bucketName,
                Key = destPath // <-- in S3 key represents a path  
            };

            PutObjectResponse response = client.PutObject(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool highLevelAPIUpload(string accessKey, string secretAccessKey, string bucketName, string fileName)
        {
            IAmazonS3 client = new AmazonS3Client(accessKey, secretAccessKey, RegionEndpoint.USWest2);

            FileInfo localFile = new FileInfo(fileName);
            string destPath = @"Ron Swanson, A Lifestyle (Vol. III) _ Parks and Recreation.mp4"; // <-- high-level s3 path uses \

            S3FileInfo s3File = new S3FileInfo(client, bucketName, destPath);
            if (!s3File.Exists)
            {
                using (var s3Stream = s3File.Create()) // <-- create file in S3  
                {
                    localFile.OpenRead().CopyTo(s3Stream); // <-- copy the content to S3  
                    return true;
                }
            }
            else if (s3File.Exists)
            {
                return true;
            }

            return false;
        }
    }
}
