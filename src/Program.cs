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
                try
                {
                    result = MigrateDatabase();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    result = false;
                }

                if (!result)
                {
                    WaitTwentySeconds();
                }

            } while (!result);
        }

        private static bool MigrateDatabase()
        {
            var configuration = GetConfiguration();

            var connectionString = ResolveConnectionString(configuration);

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

        private static string ResolveConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration
                .GetSection("ConnectionStrings:MySqlDb")
                .Value;

            var dbPassword = Environment.GetEnvironmentVariable("MYSQL_ROOT_PASSWORD");
            if (string.IsNullOrWhiteSpace(dbPassword))
            {
                throw new InvalidOperationException("Error: MySql database password was not set!");
            }

            return string.Format(connectionString, dbPassword);
        }

        private static void WaitTwentySeconds()
        {
            var twentySeconds = 20000;
            Thread.Sleep(twentySeconds);
        }
    }
}
