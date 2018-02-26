using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using CarsCatalog.BBL.Models;
using CarsCatalog.DDL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsCatalog.BBL.Services
{
    public static class AWSImagesService
    {
        private static string AWSAccessKey = "AWSAccessKey";
        private static string AWSSecretAccesKey = "AWSSecretAccesKey";
        private static string BucketName = "BucketName";
        private static IAmazonS3 client = new AmazonS3Client(AWSAccessKey, AWSSecretAccesKey, Amazon.RegionEndpoint.USEast1);

        /// <summary>
        /// This method request to S3 a presigned URL of a file with the key reference
        /// </summary>
        /// <param name="Key">File reference</param>
        /// <returns>string with the presigned file url</returns>
        public static string GetPresignedUrl(string Key)
        {
            string preSignedUrl = "";
            using (client = new AmazonS3Client(AWSAccessKey, AWSSecretAccesKey, Amazon.RegionEndpoint.USEast1))
            {
                S3FileInfo s3FileInfo = new S3FileInfo(client, BucketName, Key);
                if (s3FileInfo.Exists)
                {
                    var expiryUrlRequest = new GetPreSignedUrlRequest()
                    {
                        BucketName = BucketName,
                        Key = Key,
                        Expires = DateTime.Now.AddMinutes(5)
                    };
                    preSignedUrl = client.GetPreSignedURL(expiryUrlRequest);
                }
            }

                return preSignedUrl;
        }

        /// <summary>
        /// This method calls the presigned url method foreach file on the list
        /// </summary>
        /// <param name="Images">List with image entities which needs presigned urls</param>
        /// <returns></returns>
        public static List<ImageDisplayModel> GetImagesListUrls(ICollection<Images> Images)
        {
            List<ImageDisplayModel> ImageModels = new List<ImageDisplayModel>();
            if (Images != null)
            {
                foreach (Images img in Images)
                {
                    ImageModels.Add(new ImageDisplayModel {
                        ImageId = img.ImageId,
                        ImageName = img.FileName,
                        PreSignedUrl = AWSImagesService.GetPresignedUrl(img.FileReference)
                    });
                }
            }

            return ImageModels;
        }

        /// <summary>
        /// Mehotd that uploads images to the AmazonS3 bucket
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static SaveMessagesModel UploadImages(ImageSaveModel model)
        {
            SaveMessagesModel savedModel = new SaveMessagesModel();

            using (client = client = new AmazonS3Client(AWSAccessKey, AWSSecretAccesKey, Amazon.RegionEndpoint.USEast1))
            {
                try
                {
                    TransferUtility fileTransferUtility = new TransferUtility(client);
                    fileTransferUtility.Upload(model.FileToUpload, BucketName, model.FileReference);
                    savedModel.Saved = true;
                }

                catch (AmazonS3Exception s3Exception)
                {
                    Console.WriteLine(s3Exception.Message, s3Exception.InnerException);
                    savedModel.Saved = false;
                    savedModel.ErrorMessage = s3Exception.Message;
                }
            }                

            return savedModel;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static SaveMessagesModel DeleteImages(string fileReference)
        {
            SaveMessagesModel savedModel = new SaveMessagesModel();

            using(client = client = new AmazonS3Client(AWSAccessKey, AWSSecretAccesKey, Amazon.RegionEndpoint.USEast1))
                {
                DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = BucketName,
                    Key = fileReference,
                };
                try
                {
                    DeleteObjectResponse response = client.DeleteObject(deleteObjectRequest);
                    savedModel.Saved = true;
                }
                catch (AmazonS3Exception s3Exception)
                {
                    Console.WriteLine(s3Exception.Message, s3Exception.InnerException);
                    savedModel.Saved = false;
                    savedModel.ErrorMessage = s3Exception.Message;
                }
            }

            return savedModel;
        }
    }
}
