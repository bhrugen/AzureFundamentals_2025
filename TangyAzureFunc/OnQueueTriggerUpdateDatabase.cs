using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TangyAzureFunc.Data;
using TangyAzureFunc.Models;

namespace TangyAzureFunc
{
    public class OnQueueTriggerUpdateDatabase
    {
        private readonly ILogger<OnQueueTriggerUpdateDatabase> _logger;
        private readonly ApplicationDbContext _dbContext;

        public OnQueueTriggerUpdateDatabase(ILogger<OnQueueTriggerUpdateDatabase> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Function(nameof(OnQueueTriggerUpdateDatabase))]
        public void Run([QueueTrigger("SalesRequestInBound")] QueueMessage message)
        {
            string messageBody = message.Body.ToString();
            SalesRequest? salesRequest = JsonConvert.DeserializeObject<SalesRequest>(messageBody);
           
            if (salesRequest !=null)
            {
                salesRequest.Status = "";
                _dbContext.SalesRequests.Add(salesRequest);
                _dbContext.SaveChanges();
            }
            else
            {
                _logger.LogWarning("Failed to deserialize the message body into a SalesRequest object.");
            }
        }
    }
}
