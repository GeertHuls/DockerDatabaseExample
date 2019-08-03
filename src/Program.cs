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
            // TODO: implement circuit breaker using Polly instead...
            bool result;
            do
            {
                result = MigrateDatabase();
                if (!result)
                {
                    WaitTwentySeconds();
                }

            } while (!result);
        }

        private static bool MigrateDatabase()
        {
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

            return result.Successful;
        }

        private static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
        }

        private static void WaitTwentySeconds()
        {
            var twentySeconds = 20000;
            Thread.Sleep(twentySeconds);
        }
    }
}
