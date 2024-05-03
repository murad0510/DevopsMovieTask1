using Microsoft.AspNetCore.Mvc;
using ShowMovie.Entities;
using ShowMovie.Models;
using StackExchange.Redis;
using System.Diagnostics;

namespace ShowMovie.Controllers
{
    public class HomeController : Controller
    {
        static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("redis-12939.c251.east-us-mz.azure.redns.redis-cloud.com:12939,password=gQEQnmuLQyxtwFApDk2TmWiYPyd6830N");

        public async Task<IActionResult> Index()
        {
            var db = redis.GetDatabase();

            var hashEntries = await db.HashGetAllAsync("movies");

            List<Movie> movieList = new List<Movie>();
            foreach (var hashEntry in hashEntries)
            {
                string title = hashEntry.Name;
                string poster = hashEntry.Value;

                var movie = new Movie
                {
                    Name = title,
                    Poster = poster,
                };

                movieList.Add(movie);
            }

            var model = new MovieViewModel
            {
                Movies = movieList
            };

            return View(model);
        }

        public async Task<IActionResult> DeleteMovie()
        {
            var db = redis.GetDatabase();
            var hashEntries = await db.HashGetAllAsync("movies");

            List<Movie> movieList = new List<Movie>();


            foreach (var hashEntry in hashEntries)
            {
                string title = hashEntry.Name;
                string poster = hashEntry.Value;

                var movie = new Movie
                {
                    Name = title,
                    Poster = poster,
                };

                movieList.Add(movie);
            }

            if (movieList.Count > 0)
            {
                db.HashDelete("movies", movieList[0].Name);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
