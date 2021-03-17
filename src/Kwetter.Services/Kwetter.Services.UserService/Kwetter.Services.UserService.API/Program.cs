using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Kwetter.Services.UserService.API
{
    /// <summary>
    /// Represents the <see cref="Program"/> class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The start up method.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates the host builder.
        /// </summary>
        /// <param name="args">The command line arguments</param>
        /// <returns>Returns the host builder.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
