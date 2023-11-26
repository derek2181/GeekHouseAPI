using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using GeeekHouseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Services
{
    public class StorageService : IStorageService
    {
        public async Task<S3ResponseDTO> UploadFileAsync(S3Object s3Object, Models.AWSCredentials credentials)
        {
            //We add the aws credentials from
            var awsCredentials = new BasicAWSCredentials(credentials.AwsKey,credentials.AwsSecretKey);

            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast2
            };

            var response = new S3ResponseDTO();

            try
            {
                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = s3Object.InputStream,
                    Key = s3Object.Name,
                    BucketName = s3Object.BucketName,
                    CannedACL = S3CannedACL.NoACL
                };

                using var client = new AmazonS3Client(awsCredentials, config);
                var transferUtility = new TransferUtility(client);

                await transferUtility.UploadAsync(uploadRequest);

                response.StatusCode = 200;
                response.Message = $"{s3Object.Name} hass been uploaded succesfully";
            }
            catch(AmazonS3Exception e)
            {
                response.StatusCode = ((int)e.StatusCode);
                response.Message = e.Message;
            }
            catch(Exception e)
            {
                response.StatusCode = 500;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<S3ResponseDTO> DeleteFileIfExists(string src, Models.AWSCredentials credentials)
        {
            var awsCredentials = new BasicAWSCredentials(credentials.AwsKey, credentials.AwsSecretKey);

            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast2
            };

            var response = new S3ResponseDTO();

            try
            {
               
                using var client = new AmazonS3Client(awsCredentials, config);

                var segments = src.Split("/");
                var lastSegment = segments[segments.Length - 1];
                var deleteObjectRequest = new Amazon.S3.Model.DeleteObjectRequest
                {
                    BucketName = "geekhouse-bucket",
                    Key = lastSegment
                };

                var responseFromAWS = await client.DeleteObjectAsync(deleteObjectRequest);


                response.StatusCode = 200;
                response.Message = $"{lastSegment} hass been deleted succesfully";
            }
            catch (AmazonS3Exception e)
            {
                response.StatusCode = ((int)e.StatusCode);
                response.Message = e.Message;
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.Message = e.Message;
            }

            return response;
        }
    }
}
