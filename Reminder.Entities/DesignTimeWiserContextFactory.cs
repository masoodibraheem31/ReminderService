using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Reminder.Entities
{
    class DesignTimeWiserContextFactory : IDesignTimeDbContextFactory<ReminderContext>
    {
        public ReminderContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../ReminderAPI/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<ReminderContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new ReminderContext(builder.Options);
        }
    }
}
