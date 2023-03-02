using Amazon.S3;
using CSharp_Backend;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace UnitTestAWSUpload
{
    [TestClass]
    public class UnitTest1
    {
        private const string existingBucketName = "practicebucket-sahil";

        // Specify your bucket region (an example region is shown).
        private static IAmazonS3 s3Client;
        AWS_S3_upload upload = new AWS_S3_upload();
        string resourceFile = @"C:\Users\raisa\source\repos\CSharp-Backend\UnitTestAWSUpload\Ron Swanson, A Lifestyle (Vol. III) _ Parks and Recreation.mp4";

        /// <summary>
        /// We are testing that our upload is working asynchronously.
        /// </summary>
        [TestMethod]
        public void TestingAggregationException()
        {

            Exception exception =
            Assert.ThrowsException<AggregateException>(() =>
            {
                bool isUploaded = upload.UploadFileAsync(s3Client, existingBucketName, "Ron Swanson, A Lifestyle (Vol. III) _ Parks and Recreation.mp4", resourceFile).Result;
            });

            Assert.AreEqual("One or more errors occurred.", exception.Message);

        }


        /// <summary>
        /// Over here we are just checking that if our upload to S3 is working fine, in ideal condition 
        /// </summary>
        [TestMethod]
        public void UploadUsingLowLevelAPI()
        {
            string accessKey = "AKIA2MBFTIOO75DHUTNX";
            string secretAccessKey = "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";

            bool uploadUsingLowLevelAPI = upload.lowLevelAPIUpload(accessKey, secretAccessKey, existingBucketName, resourceFile);

            Assert.IsTrue(uploadUsingLowLevelAPI);
        }

        /// <summary>
        /// What if the file is not found by our API
        /// </summary>
        [TestMethod]
        public void FileNotFoundExceptionfromAWS()
        {
            string accessKey = "AKIA2MBFTIOO75DHUTNX";
            string secretAccessKey = "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";
            string fileName = @"C:\Users\raisa\source\repos\CSharp-Backend\UnitTestAWSUpload\April, A Lifestyle (Vol. III) _ Parks and Recreation.mp4";

            Exception exception =
            Assert.ThrowsException<FileNotFoundException>(() =>
            {
                bool uploadUsingLowLevelAPI = upload.lowLevelAPIUpload(accessKey, secretAccessKey, existingBucketName, fileName);
            });
            Assert.AreEqual(@"Could not find file 'C:\Users\raisa\source\repos\CSharp-Backend\UnitTestAWSUpload\April, A Lifestyle (Vol. III) _ Parks and Recreation.mp4'.", exception.Message);

        }

        /// <summary>
        /// What if we provided wrong bucket name to access our storage container
        /// </summary>
        [TestMethod]
        public void bucketValidationTest()
        {
            string accessKey = "AKIA2MBFTIOO75DHUTNX";
            string secretAccessKey = "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";
            string bucketName = "practicebucket";


            Exception exception =
            Assert.ThrowsException<AmazonS3Exception>(() =>
            {
                bool uploadUsingLowLevelAPI = upload.lowLevelAPIUpload(accessKey, secretAccessKey, bucketName, resourceFile);
            });
            Assert.AreEqual("The bucket you are attempting to access must be addressed using the specified endpoint. Please send all future requests to this endpoint.", exception.Message);

        }

        /// <summary>
        /// What if we provided wrong accesskeys to fetch our document 
        /// </summary>
        [TestMethod]
        public void accessKeyValidationTest()
        {
            string accessKey = "WRONG_ACCESS_KEY";
            string secretAccessKey = "G6IF4xODOxyDfFt66/n1FMJPHTuGdhlZxPyvPZgJ";


            Exception exception =
            Assert.ThrowsException<AmazonS3Exception>(() =>
            {
                bool uploadUsingLowLevelAPI = upload.lowLevelAPIUpload(accessKey, secretAccessKey, existingBucketName, resourceFile);
            });
            Assert.AreEqual("The AWS Access Key Id you provided does not exist in our records.", exception.Message);

        }

        /// <summary>
        /// What if we provided wrong secret access keys to fetch our document 
        /// </summary>
        [TestMethod]
        public void secretAccessKeyValidationTest()
        {
            string accessKey = "AKIA2MBFTIOO75DHUTNX";
            string secretAccessKey = "wrong_secret_access_key";


            Exception exception =
            Assert.ThrowsException<AmazonS3Exception>(() =>
            {
                bool uploadUsingLowLevelAPI = upload.lowLevelAPIUpload(accessKey, secretAccessKey, existingBucketName, resourceFile);
            });
            Assert.AreEqual("The request signature we calculated does not match the signature you provided. Check your key and signing method.", exception.Message);

        }

        /// <summary>
        /// . High level API’s are used because if our low level API found out that file is not in our local system, this function should create one.
        /// </summary>
        [TestMethod]
        public void highLevelAPIUpload()
        {
            string accessKey = "AKIA2MBFTIOO75DHUTNX";
            string secretAccessKey = "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";

            bool uploadUsingLowLevelAPI = upload.highLevelAPIUpload(accessKey, secretAccessKey, existingBucketName, resourceFile);

            Assert.IsTrue(uploadUsingLowLevelAPI);
        }

        /// <summary>
        /// . If any file is not found in local storage at the time of upload. High level API will just make an empty file,
        /// it will save it in my system and then it will provide the file. 
        /// </summary>
        [TestMethod]
        public void highLevelAPIUpload_fileNotFound()
        {
            string accessKey = "AKIA2MBFTIOO75DHUTNX";
            string secretAccessKey = "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";
            string fileName = @"C:\Users\raisa\source\repos\CSharp-Backend\UnitTestAWSUpload\April, A Lifestyle (Vol. III) _ Parks and Recreation.mp4";

            bool uploadUsingLowLevelAPI = upload.highLevelAPIUpload(accessKey, secretAccessKey, existingBucketName, fileName);

            Assert.IsTrue(uploadUsingLowLevelAPI);
        }
    }
}
