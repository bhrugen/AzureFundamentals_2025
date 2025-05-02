using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TangyAzureFunc.Data;
using TangyAzureFunc.Models;

namespace TangyAzureFunc
{
    public class GroceryAPI
    {
        private readonly ILogger<GroceryAPI> _logger;
        private readonly ApplicationDbContext _dbContext;

        public GroceryAPI(ILogger<GroceryAPI> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Function("GetGrocery")]
        public IActionResult GetGrocery([HttpTrigger(AuthorizationLevel.Function, "get", Route ="GroceryList")] HttpRequest req)
        {
            _logger.LogInformation("Getting Grocery List Items.");


            return new OkObjectResult(_dbContext.GroceryItems.ToList());
        }

        [Function("CreateGrocery")]
        public async Task<IActionResult> CreateGrocery([HttpTrigger(AuthorizationLevel.Function, "post", Route = "GroceryList")] HttpRequest req)
        {
            _logger.LogInformation("Creating Grocery List Item.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            GroceryItem_Upsert? data = JsonConvert.DeserializeObject<GroceryItem_Upsert>(requestBody);

            GroceryItem groceryItem = new GroceryItem
            {
                Name = data.Name
            };
            _dbContext.GroceryItems.Add(groceryItem);
            _dbContext.SaveChanges();

            return new OkObjectResult(_dbContext.GroceryItems.ToList());
        }
    }
}
