using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AwsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class s3Controller : Controller
    {
        private readonly IAmazonS3 _s3Client;
        public s3Controller()
        { }





        [HttpGet("get-data")]
        public async Task<string> getbucketdata()
        {
            var credentials = new BasicAWSCredentials("AKIAWKM76GM3HE7UORXU", "Ftjr1Zjjyr2Wdg4r8eJyGSOJbB5Vhw4kU2GEtwwf");



            var client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast1); ;
            var buckets = await client.ListBucketsAsync();



            foreach (var bucket in buckets.Buckets)
            {
                var objects = await client.ListObjectsAsync(bucket.BucketName);
                if (objects != null)
                {
                    Console.WriteLine(string.Join(",", objects.S3Objects.Select(x => x.Key)));



                    foreach (var s3object in objects.S3Objects)
                    {

                        var response = await client.GetObjectAsync(new GetObjectRequest
                        {
                            BucketName = bucket.BucketName,
                            Key = s3object.Key
                        });

                        var bytes = new byte[response.ResponseStream.Length];
                        response.ResponseStream.Read(bytes, 0, bytes.Length);
                        Console.WriteLine(Encoding.UTF8.GetString(bytes));
                        return Encoding.UTF8.GetString(bytes);
                    }



                }
            }



            Console.WriteLine(string.Join(",", buckets.Buckets.Select(x => x.BucketName)));
            return "";
        }
    }
}
