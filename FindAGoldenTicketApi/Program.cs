using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FindAGoldenTicketApi
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            while (true)
            {
                var ticketManager = new TicketManager();
                Console.WriteLine("Press any key to start...");
                Console.ReadLine();
                await ticketManager.GetAsync();
                Console.WriteLine("Exit?");
                var exit = Console.ReadLine();
                if (exit != null && exit.ToLowerInvariant() == "y")
                {
                    return;
                }
            }
        }
    }

    public class TicketManager
    {

        private readonly List<TimeSpan> timeSpans = new List<TimeSpan>();
        public async Task GetAsync()
        {
            var httpClient = new HttpClient();

            var ticketsResponse = await httpClient.GetAsync("https://localhost:44312/api/tickets");

            var ticketIds = JsonConvert.DeserializeObject<List<int>>(await ticketsResponse.Content.ReadAsStringAsync());

            var i = 0;
            var threads = new List<Thread>();
            foreach (var ticketId in ticketIds)
            {
                var i1 = i;
                var thread1 = new Thread(async () => await PrintResponseAsync(httpClient, ticketId, i1, 1));
                threads.Add(thread1);
                thread1.Start();

                i++;

                while (threads.Count(c => c.IsAlive)>100)
                {
                    System.Threading.Thread.Sleep(10);
                }

            }

            while (threads.Any(c => c.IsAlive))
            {
                System.Threading.Thread.Sleep(10);
            }

            //while (i < ticketIds.Count)
            //{
            //    var i1 = i;
            //    var thread1 = new Thread(async () => await PrintResponseAsync(httpClient, ticketIds[i1], i1, 1));
            //    var thread2 = new Thread(async () => await PrintResponseAsync(httpClient, ticketIds[i1], i1 + 1, 2));
            //    var thread3 = new Thread(async () => await PrintResponseAsync(httpClient, ticketIds[i1], i1 + 2, 3));
            //    var thread4 = new Thread(async () => await PrintResponseAsync(httpClient, ticketIds[i1], i1 + 3, 4));

            //    thread1.Start();
            //    thread2.Start();
            //    thread3.Start();
            //    thread4.Start();
            //    //var request1 = PrintResponseAsync(httpClient, ticketIds[i], i, 1);
            //    //var request2 = PrintResponseAsync(httpClient, ticketIds[i], i + 1, 2);
            //    //var request3 = PrintResponseAsync(httpClient, ticketIds[i], i + 2, 3);
            //    //var request4 = PrintResponseAsync(httpClient, ticketIds[i], i + 3, 4);
            //    //var request2 = httpClient.GetAsync($"https://localhost:44312/api/tickets/sync/{ticketIds[i + 1]}");
            //    //var request3 = httpClient.GetAsync($"https://localhost:44312/api/tickets/sync/{ticketIds[i + 2]}");
            //    //var request4 = httpClient.GetAsync($"https://localhost:44312/api/tickets/sync/{ticketIds[i + 3]}");

            //    //await request1;
            //    //await request2;
            //    //await request3;
            //    //await request4;

            //    //await PrintResponseAsync(request1, i, 1);
            //    //await PrintResponseAsync(request1, i + 1, 2);
            //    //await PrintResponseAsync(request1, i + 2, 3);
            //    //await PrintResponseAsync(request1, i + 3, 4);

            //    i += 4;
            //}

            Console.WriteLine($"Average Response Time: {timeSpans.Average(c=>c.TotalSeconds)}");
            Console.WriteLine("Press any key to exit....");

        }

        private async Task PrintResponseAsync(HttpClient httpClient, int ticketId, int i, int responseNumber)
        {
            var startTime = DateTime.Now;
            var response = await httpClient.GetAsync($"https://localhost:44312/api/tickets/async/{ticketId}");
            timeSpans.Add(DateTime.Now-startTime);
            Console.WriteLine(response.IsSuccessStatusCode
                ? $"Element {i} Response {responseNumber}: {await response.Content.ReadAsStringAsync()}"
                : $"Element {i} Response {responseNumber}:  Response Code: {response.StatusCode}");
        }
    }
}
