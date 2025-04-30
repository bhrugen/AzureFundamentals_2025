
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobProject.Services
{
    public class ContainerService : IContainerService
    {
        private readonly BlobServiceClient _blobClient;
        public ContainerService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }
        public async Task CreateContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
        }

        public async Task DeleteContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            await blobContainerClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllContainer()
        {
            List<string> containerName = new();

            await foreach(BlobContainerItem blobContainerItem in _blobClient.GetBlobContainersAsync())
            {
                containerName.Add(blobContainerItem.Name);
            }

            return containerName;
        }

        public async Task<List<string>> GetAllContainerAndBlobs()
        {
            List<string> containerAndBlobName = new();
            containerAndBlobName.Add("-----Account Name : "+_blobClient.AccountName +"-----");
            containerAndBlobName.Add("---------------------------------------------------------------");

            await foreach (BlobContainerItem blobContainerItem in _blobClient.GetBlobContainersAsync())
            {
                containerAndBlobName.Add("-----"+blobContainerItem.Name);
                BlobContainerClient _blobContainer = _blobClient.GetBlobContainerClient(blobContainerItem.Name);

                await foreach(BlobItem blobItem in _blobContainer.GetBlobsAsync())
                {
                    //get metadata

                    var blobClient = _blobContainer.GetBlobClient(blobItem.Name);
                    BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
                    string tempBlobToAdd = blobItem.Name;
                    if (blobProperties.Metadata.ContainsKey("title"))
                    {
                        tempBlobToAdd += "(" + blobProperties.Metadata["title"] + ")";
                    }

                    containerAndBlobName.Add(">>" + tempBlobToAdd);
                }
                containerAndBlobName.Add("---------------------------------------------------------------");
            }


            return containerAndBlobName;
        }
    }
}
