using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace GoldenTicketApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //System.Threading.ThreadPool.GetMaxThreads(out var existingMaxWorkerThreads, out var existingMaxIocpThreads);
            //System.Threading.ThreadPool.SetMaxThreads(1, 1);
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseStartup<Startup>();
    }
}
