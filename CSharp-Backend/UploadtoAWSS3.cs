using Amazon;
using Amazon.S3;
using MongoDB.Driver;
using System;
using System.Windows.Forms;

namespace CSharp_Backend
{
    public partial class UploadtoAWSS3 : Form
    {

        private const string existingBucketName = "practicebucket-sahil";
        private const string directoryPath = @"C:\Users\raisa\OneDrive\Desktop";
        // The example uploads only .txt files.
        private const string wildCard = "*.asm";
        private const string V = "key1=value1";

        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest2;
        private static IAmazonS3 s3Client;
        AWS_S3_upload upload = new AWS_S3_upload();

        public UploadtoAWSS3()
        {
            InitializeComponent();

        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "mp4 | *.mp4";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtInputFile.Text = openFileDialog1.FileName;
            }
        }

        private void splitButton_Click(object sender, EventArgs e)
        {
            s3Client = new AmazonS3Client(bucketRegion);
            upload.UploadFileAsync(s3Client, existingBucketName,  $"{outputFileNameTextBox.Text}.mp4", txtInputFile.Text).Wait();
            
        }
    }
}

//namespace CSharp_Project_Uplaod_Videos
//{
//    public partial class Form1 : Form
//    {



//public async void UploadFile()
//{
//    var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);

//    try
//    {
//        PutObjectRequest putRequest = new PutObjectRequest
//        {
//            BucketName = bucketName,
//            Key = keyName,
//            FilePath = filePath,
//            ContentType = "text/plain"
//        };

//        PutObjectResponse response = await client.PutObjectAsync(putRequest);
//    }
//    catch (AmazonS3Exception amazonS3Exception)
//    {
//        if (amazonS3Exception.ErrorCode != null &&
//            (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
//            ||
//            amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
//        {
//            throw new Exception("Check the provided AWS Credentials.");
//        }
//        else
//        {
//            throw new Exception("Error occurred: " + amazonS3Exception.Message);
//        }
//    }
//}



//    }
//}
