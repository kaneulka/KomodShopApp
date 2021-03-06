using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Repo;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Komod
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = /*CreateHostBuilder(args).Build();//*/CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var context = services.GetRequiredService<ApplicationContext>();
                    await DbInit.InitializeAsync(userManager, rolesManager, context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        //??? IIS
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args);

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
