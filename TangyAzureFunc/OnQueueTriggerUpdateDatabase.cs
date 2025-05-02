using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TangyAzureFunc.Data;

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
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}
