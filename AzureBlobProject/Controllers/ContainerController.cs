using AzureBlobProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AzureBlobProject.Controllers
{
    public class ContainerController : Controller
    {
        private readonly IContainerService _containerService;
        public ContainerController(IContainerService containerService)
        {
            _containerService = containerService;
        }
        public async Task<IActionResult> Index()
        {
            var allContainer = await _containerService.GetAllContainer();
            return View();
        }
    }
}
