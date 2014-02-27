using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;


// Add using statements to access AWS SDK for .NET services. 
// Both the Service and its Model namespace need to be added 
// in order to gain access to a service. For example, to access
// the EC2 service, add:
// using Amazon.EC2;
// using Amazon.EC2.Model;

namespace Project
{
    public class AmazonService
    {
        IAmazonS3 client;
        Log log = new Log();

        public AmazonService()
        {
            if (CheckRequiredFields())
            {
                client = Amazon.AWSClientFactory.CreateAmazonS3Client();
            }
        }

        public bool UploadFile(string bucketName, String fileName, string filePath)
        {
            if (CheckBucketAndFilePath(bucketName, fileName, filePath))
            {
                return WritingAnObject(bucketName, fileName, filePath);
            }
            return false;

        }

        private bool CheckRequiredFields()
        {
            NameValueCollection appConfig = ConfigurationManager.AppSettings;
            string filename = ConfigurationManager.AppSettings["LogFile"];
             using (StreamWriter writer = File.AppendText(filename))
             {
                 if (string.IsNullOrEmpty(appConfig["AWSAccessKey"]))
                 {
                     log.WriteLog("AWSAccessKey was not set in the App.config file.",writer);
                     return false;
                 }
                 if (string.IsNullOrEmpty(appConfig["AWSSecretKey"]))
                 {
                     log.WriteLog("AWSSecretKey was not set in the App.config file.",writer);
                     return false;
                 }
             }
            return true;
        }

        private bool CheckBucketAndFilePath(string bucketName, string fileName, string filePath)
        {
             string filename = ConfigurationManager.AppSettings["LogFile"];
             using (StreamWriter writer = File.AppendText(filename))
             {
                 if (string.IsNullOrEmpty(bucketName))
                 {
                     log.WriteLog("The bucketName is not set.",writer);
                     return false;
                 }

                 if (string.IsNullOrEmpty(fileName))
                 {
                     log.WriteLog("The fileName is not set.",writer);
                     return false;
                 }
                 if (string.IsNullOrEmpty(filePath))
                 {
                     log.WriteLog("The filePath is not set.",writer);
                     return false;
                 }
             }
            return true;
        }

        public bool WritingAnObject(string bucketName, string fileName, string filePath)
        {
            try
            {
                // simple object put
                PutObjectRequest request = new PutObjectRequest()
                {
                    FilePath = filePath,
                    BucketName = bucketName,
                    Key = fileName
                };
                request.Metadata.Add(fileName, fileName);
                client.PutObject(request);

                return true;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                string filename = ConfigurationManager.AppSettings["LogFile"];
                using (StreamWriter writer = File.AppendText(filename))
                {
                    if (amazonS3Exception.ErrorCode != null &&
                       (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                       amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    {
                        log.WriteLog("Please check the provided AWS Credentials.", writer);
                        log.WriteLog("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3", writer);
                        return false;
                    }
                    else
                    {
                        log.WriteLog("An error occurred with the message " + amazonS3Exception.Message + "when writing an object", writer);
                        return false;
                    }
                }
            }
        }

        public bool CheckIfBucketExists(string bucketname)
        {
            try
            {
                ListBucketsResponse response = client.ListBuckets();
                foreach (S3Bucket bucket in response.Buckets)
                {
                    if (bucketname.Equals(bucket.BucketName))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                string filename = ConfigurationManager.AppSettings["LogFile"];
                using (StreamWriter writer = File.AppendText(filename))
                {
                    if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    {
                        log.WriteLog("Please check the provided AWS Credentials.", writer);
                        log.WriteLog("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3", writer);
                        return false;
                    }
                    else
                    {
                        log.WriteLog("An Error, number " + amazonS3Exception.ErrorCode + ", occurred when listing buckets with the message " + amazonS3Exception.Message, writer);
                        return false;
                    }
                }
            }
        }

        public bool CreateABucket(string bucketname)
        {
            try
            {
                if (CheckIfBucketExists(bucketname))
                {
                    return false;
                }
                PutBucketRequest request = new PutBucketRequest();
                request.BucketName = bucketname;
                request.UseClientRegion = true;
                client.PutBucket(request);
                return true;
            }

            catch (AmazonS3Exception amazonS3Exception)
            {
                string filename = ConfigurationManager.AppSettings["LogFile"];
                using (StreamWriter writer = File.AppendText(filename))
                {

                    if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    {
                        log.WriteLog("Please check the provided AWS Credentials.", writer);
                        log.WriteLog("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3", writer);
                        return false;
                    }
                    else
                    {
                        log.WriteLog("An Error, number " + amazonS3Exception.ErrorCode + ", occurred when creating a bucket with the message, " + amazonS3Exception.Message, writer);
                        return false;
                    }
                }
            }
        }

        public bool CopyAObject(string bucketname, string oldkeyname, string newkeyname)
        {
            try
            {
                // Create a CopyObject request
                CopyObjectRequest request = new CopyObjectRequest
                {
                    SourceBucket = bucketname,
                    SourceKey = oldkeyname,
                    DestinationBucket = bucketname,
                    DestinationKey = newkeyname,
                    CannedACL = S3CannedACL.PublicRead
                };

                // Issue request
                client.CopyObject(request);
                return true;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                string filename = ConfigurationManager.AppSettings["LogFile"];
                using (StreamWriter writer = File.AppendText(filename))
                {
                    if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    {
                        log.WriteLog("Please check the provided AWS Credentials.", writer);
                        log.WriteLog("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3", writer);
                        return false;
                    }
                    else
                    {
                        log.WriteLog("An Error, number " + amazonS3Exception.ErrorCode + ", occurred when copying a object with the message " + amazonS3Exception.Message, writer);
                        return false;
                    }
                }
            }
        }

        public bool DeleteAObject(string bucketname, string keyname)
        {
            try
            {

                // Create a DeleteObject request
                DeleteObjectRequest request = new DeleteObjectRequest
                {
                    BucketName = bucketname,
                    Key = keyname
                };

                // Issue request
                client.DeleteObject(request);
                return true;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                string filename = ConfigurationManager.AppSettings["LogFile"];
                using (StreamWriter writer = File.AppendText(filename))
                {
                    if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    {
                        log.WriteLog("Please check the provided AWS Credentials.", writer);
                        log.WriteLog("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3", writer);
                        return false;
                    }
                    else
                    {
                        log.WriteLog("An Error, number " + amazonS3Exception.ErrorCode + ", occurred when deleting a object with the message " + amazonS3Exception.Message, writer);
                        return false;
                    }
                }
            }
        }

        public bool UpdateFile(string bucketname, string oldkeyname, string newkeyname)
        {
            if (!CopyAObject(bucketname, oldkeyname, newkeyname))
            {
                return false;
            }
            return DeleteAObject(bucketname, oldkeyname);

        }

    }
}