using Project.Api;
using Project.Core.Options;

namespace Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().SeedData().Run();
        }
        /// <summary>
        /// 
        /// </summary>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}