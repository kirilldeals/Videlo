using Microsoft.Extensions.Options;

namespace Videlo.Configuration
{
    public class AWSConfiguration
    {
        public string ServiceEndpoint { get; set; } = null!;
        public string BucketName { get; set; } = null!;
        public string AccessKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;

        public string BaseURL 
        {
            get 
            {
                return $"https://{BucketName}.{ServiceEndpoint}/";
            }
        }
    }
}
