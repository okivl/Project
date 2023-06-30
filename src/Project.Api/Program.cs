using Project.Api;
using Project.Core.Extensions;

namespace Api
{
    /// <summary/>
    public class Program
    {
        /// <summary/>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().MigrateDb().SeedData().Run();
        }
        /// <summary/>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}