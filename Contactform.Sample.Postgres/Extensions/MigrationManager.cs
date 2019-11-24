using ContactForm.Sample.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Contactform.Sample.Postgres.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        var logger = scope.ServiceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
                        if (logger != null) logger.LogError(ex, "MigrateDatabase");
                        //throw;
                    }
                }
            }

            return host;
        }
    }
}
