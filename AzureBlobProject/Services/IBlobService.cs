using AzureBlobProject.Models;
using Microsoft.AspNetCore.Http;

namespace AzureBlobProject.Services
{
    public interface IBlobService
    {
        Task<List<string>> GetAllBlobs(string containerName);
        Task<List<BlobModel>> GetAllBlobsWithUri(string containerName);
        Task<string> GetBlob(string name, string containerName);
        Task CreateBlob(string name, IFormFile file, string containerName, BlobModel blobModel);
        Task DeleteBlob(string name, string containerName);
    }
}
