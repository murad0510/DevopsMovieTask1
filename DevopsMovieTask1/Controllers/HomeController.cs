using Azure.Storage.Queues;
using DevopsMovieTask1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

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
            var movoe=Convert.ToBase64String(Encoding.UTF8.GetBytes(movieName));
            await _queueClient.SendMessageAsync(movoe);

            return View();
        }
    }
}
