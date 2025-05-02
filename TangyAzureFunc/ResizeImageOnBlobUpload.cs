using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace TangyAzureFunc
{
    public class ResizeImageOnBlobUpload
    {
        private readonly ILogger<ResizeImageOnBlobUpload> _logger;

        public ResizeImageOnBlobUpload(ILogger<ResizeImageOnBlobUpload> logger)
        {
            _logger = logger;
        }

        [Function(nameof(ResizeImageOnBlobUpload))]
        [BlobOutput("functionsalesrep-final/{name}")]
        public async Task<Byte[]> Run([BlobTrigger("functionsalesrep/{name}", Connection = "AzureWebJobsStorage")] Byte[] myBlobByte, string name)
        {
            
            using var memoryStream = new MemoryStream(myBlobByte);

            using var image = Image.Load(memoryStream);

            image.Mutate(x => x.Resize(100, 100));
            using var outputStream = new MemoryStream();
            image.SaveAsJpeg(outputStream);
            outputStream.Position = 0; // Reset the stream position to the beginning
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name}");

            return outputStream.ToArray();

        }
    }
}
