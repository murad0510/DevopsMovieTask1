using Azure.Storage.Queues;
using DevopsMovieTask1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DevopsMovieTask1.Controllers
{
    public class HomeController : Controller
    {
        private readonly QueueClient _queueClient;

        public HomeController()
        {
            _queueClient = new QueueClient(ConnectionStrings.AzureStorageConnectionString, "moviequeuq");
            _queueClient.CreateIfNotExists();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(string movieName)
        {
            await _queueClient.SendMessageAsync(movieName);

            return View();
        }
    }
}
