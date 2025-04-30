
namespace AzureBlobProject.Services
{
    public class ContainerService : IContainerService
    {
        public Task CreateContainer(string containerName)
        {
            throw new NotImplementedException();
        }

        public Task DeleteContainer(string containerName)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetAllContainer()
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetAllContainerAndBlobs()
        {
            throw new NotImplementedException();
        }
    }
}
