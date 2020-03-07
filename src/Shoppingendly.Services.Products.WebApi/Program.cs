using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Shoppingendly.Services.Products.Infrastructure.Logger.Extensions;

namespace Shoppingendly.Services.Products.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseLogging()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
