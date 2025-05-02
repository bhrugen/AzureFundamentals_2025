using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using TangyAzureFunc.Data;
using TangyAzureFunc.Models;

namespace TangyAzureFunc
{
    public class UpdateStatusToComplete
    {
        private readonly ILogger _logger;
        private ApplicationDbContext _dbContext;
        public UpdateStatusToComplete(ILoggerFactory loggerFactory, ApplicationDbContext context)
        {
            _logger = loggerFactory.CreateLogger<UpdateStatusToComplete>();
            _dbContext = context;
        }

        [Function("UpdateStatusToComplete")]
        public void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            IEnumerable<SalesRequest> salesRequests = _dbContext.SalesRequests.Where(u => u.Status == "Image Processed").ToList();
            foreach(var salesRequest in salesRequests)
            {
                salesRequest.Status = "Completed";
            }
            _dbContext.SaveChanges();


            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
