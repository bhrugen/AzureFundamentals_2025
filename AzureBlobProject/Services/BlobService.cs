using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using AzureBlobProject.Models;

namespace AzureBlobProject.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobClient;
        public BlobService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }
        public async Task<bool> CreateBlob(string name, IFormFile file, string containerName, BlobModel blobModel)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);

            var blobClient = blobContainerClient.GetBlobClient(name);

            var httpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            IDictionary<string, string> metaData = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(blobModel.Title))
            {
                metaData.Add("title", blobModel.Title);
            }
            if (!string.IsNullOrEmpty(blobModel.Comment))
            {
                metaData.Add("comment", blobModel.Comment);
            }

            var result = await blobClient.UploadAsync(file.OpenReadStream(),httpHeaders,metaData);

            //IDictionary<string,string> removeMetaData = new Dictionary<string, string>();
            
            //metaData.Remove("title");
            //await blobClient.SetMetadataAsync(metaData);

            if (result != null)
            {
                return true;
            }
            return false;

        }

        public async Task<bool> DeleteBlob(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);

            var blobClient = blobContainerClient.GetBlobClient(name);

            return await blobClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllBlobs(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobsAsync();

            List<string> blobNames = new List<string>();
            await foreach (var blob in blobs)
            {
                blobNames.Add(blob.Name);
            }

            return blobNames;
        }

        public async Task<List<BlobModel>> GetAllBlobsWithUri(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobsAsync();

            List<BlobModel> blobList = new List<BlobModel>();


            await foreach (var blob in blobs)
            {
                var blobClient = blobContainerClient.GetBlobClient(blob.Name);

                BlobModel blobModel = new()
                {
                    Uri = blobClient.Uri.AbsoluteUri
                };

                if (blobClient.CanGenerateSasUri)
                {
                    BlobSasBuilder blobSasBuilder = new()
                    {
                        BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                        BlobName = blobClient.Name,
                        Resource="b",
                        ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
                    };

                    blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

                    blobModel.Uri = blobClient.GenerateSasUri(blobSasBuilder).AbsoluteUri;

                }


                BlobProperties properties = await blobClient.GetPropertiesAsync();
                if (properties.Metadata.ContainsKey("title"))
                {
                    blobModel.Title = properties.Metadata["title"];
                }
                if (properties.Metadata.ContainsKey("comment"))
                {
                    blobModel.Comment = properties.Metadata["comment"];
                }
                blobList.Add(blobModel);
            }

            return blobList;
        }

        public async Task<string> GetBlob(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);

            var blobClient = blobContainerClient.GetBlobClient(name);

            if (blobClient != null)
            {
                return blobClient.Uri.AbsoluteUri;
            }
            return "";
        }
    }
}
