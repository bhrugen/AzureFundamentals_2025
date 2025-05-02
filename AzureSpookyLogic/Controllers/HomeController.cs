using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureSpookyLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;

namespace AzureSpookyLogic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BlobServiceClient _blobServiceClient;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, BlobServiceClient blobServiceClient)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(SpookyRequest spookyRequest, IFormFile file)
        {
            spookyRequest.Id = Guid.NewGuid().ToString();
            using var client = _httpClientFactory.CreateClient();
            using (var content = new StringContent(JsonConvert.SerializeObject(spookyRequest), System.Text.Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage response = await client.PostAsync("LOGIC APP URL", content);
                string returnValue = await response.Content.ReadAsStringAsync();

            }

            if (file != null)
            {
                var fileName = spookyRequest.Id + Path.GetExtension(file.FileName);
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("logic-app-holder");
                var blobClient = containerClient.GetBlobClient(fileName);

                var httpheaders = new BlobHttpHeaders()
                {
                    ContentType = file.ContentType
                };
                await blobClient.UploadAsync(file.OpenReadStream(), httpheaders);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
