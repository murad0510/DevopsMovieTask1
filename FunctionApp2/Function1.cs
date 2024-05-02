using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace FunctionApp2
{
    public class Function1
    {
        static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("redis-12939.c251.east-us-mz.azure.redns.redis-cloud.com:12939,password=gQEQnmuLQyxtwFApDk2TmWiYPyd6830N");

        public static dynamic SingleData { get; set; }
        public static dynamic Data { get; set; }

        [FunctionName("Function1")]
        public void Run([QueueTrigger("moviequeuq", Connection = "MyAzureStorage")] string myQueueItem, ILogger log)
        {

            Task.Run(async () =>
            {
                var db = redis.GetDatabase();
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync($@"http://www.omdbapi.com/?apikey=ab88e99a&s={myQueueItem}&plot=full");
                var str = await response.Content.ReadAsStringAsync();
                var searchData = JsonConvert.DeserializeObject<dynamic>(str);
                var posterUrl = "";
                if (searchData.Response == "True")
                {
                    var imdbID = searchData.Search[0].imdbID;
                    HttpResponseMessage posterResponse = await httpClient.GetAsync($@"http://www.omdbapi.com/?apikey=ab88e99a&i={imdbID}&plot=full");
                    var posterStr = await posterResponse.Content.ReadAsStringAsync();
                    var posterData = JsonConvert.DeserializeObject<dynamic>(posterStr);
                    posterUrl = posterData.Poster;
                    posterUrl = posterUrl.Replace("{", "").Replace("}", "");
                }

                HashEntry[] hashEntries = new HashEntry[]
                {
                    new HashEntry($"{myQueueItem}", $"{posterUrl}")
                };

                db.HashSet("movies", hashEntries);
            });
            //log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
