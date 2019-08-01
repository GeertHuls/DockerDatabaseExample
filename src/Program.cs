using System;
using System.IO;
using System.Reflection;
using System.Threading;
using DbUp;
using Microsoft.Extensions.Configuration;

namespace DockerDatabaseExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Wait10SecondsBeforeMigrationStarts();

            var configuration = GetConfiguration();

            var connectionString = configuration
                .GetSection("ConnectionStrings:MySqlDb")
                .Value;

            EnsureDatabase.For.MySqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .MySqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.WriteLine("Database migration failed.");
            }
            else
            {
                Console.WriteLine("Database migration was successful!");
            }
        }

        private static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
        }

        private static void Wait10SecondsBeforeMigrationStarts()
        {
            // ** Warning: hack ahead...
            // The migration of the database will run in the background and
            // can only work before the mysql database is up and running.
            // This happens in the docker entrypoint at the bottom of the script.
            // This delay allows the mysql database to be up and running BEFORE
            // the migration starts, otherwise the migration will fail.
            var tenSeconds = 10000;
            Thread.Sleep(tenSeconds);
        }
    }
}
