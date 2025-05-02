using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TangyAzureFunc.Data;
using TangyAzureFunc.Models;

namespace TangyAzureFunc
{
    public class BlobResizeUpdateDbStatus
    {
        private readonly ILogger<BlobResizeUpdateDbStatus> _logger;
        private ApplicationDbContext _dbContext; 

        public BlobResizeUpdateDbStatus(ILogger<BlobResizeUpdateDbStatus> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        [Function(nameof(BlobResizeUpdateDbStatus))]
        public async Task Run([BlobTrigger("functionsalesrep-final/{name}")] Byte[] myBlobByte, string name)
        {
            
            var fileName= Path.GetFileNameWithoutExtension(name);

            SalesRequest? salesRequest = await _dbContext.SalesRequests.FirstOrDefaultAsync(u=>u.Id== fileName);

            if (salesRequest != null)
            {
                salesRequest.Status="Image Processed";
                _dbContext.SaveChanges();
            }

            _logger.LogInformation($"BlobResize Update DB Status has been completed");
        }
    }
}
