using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using luftborn.Api.Helper;
using luftborn.Presistance;
using luftborn.Service.Features.Common.Interfaces;

namespace luftborn.Api
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CurrentDirectoryHelpers.SetCurrentDirectory();
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                using (var concreteContext = (luftbornEntities)scope.ServiceProvider.GetService<IluftbornEntities>())
                {
                    concreteContext.Database.SetCommandTimeout(600);
                    concreteContext.Database.Migrate();

                    var environment = scope.ServiceProvider.GetService<IWebHostEnvironment>();
                    luftbornEntitiesSeed.ApiInitialize(concreteContext);
                }
                using (var concreteContext = (luftbornAuditDbContext)scope.ServiceProvider.GetService<IluftbornAuditDbContext>())
                {
                    concreteContext.Database.Migrate();
                }
            }
            host.Run();
        }

        /// <summary>
        /// CreateWebHostBuilder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>();
    }
}