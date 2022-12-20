using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace AccessPointMap.API
{
    public class Program
    {
        private const string _appSettingsFilePath = "appsettings.json";
        private const string _appSettingsFilePathDevelopment = "appsettings.Development.json";

        public static void Main(string[] args)
        {
            string targetAppSettingsFile = _appSettingsFilePath;
#if DEBUG
            targetAppSettingsFile = _appSettingsFilePathDevelopment;
#endif

            try
            {
                if (!File.Exists(targetAppSettingsFile))
                {
                    Console.WriteLine("AccessPointMap settings file not found.");

                    throw new FileNotFoundException("AccessPointMap settings file not found.");
                }

                var configuration = new ConfigurationBuilder()
                    .AddJsonFile(targetAppSettingsFile)
                    .Build();

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

                Log.Information("AccessPointMap service starting up.");

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "AccessPointMap service startup fatal error.");
            }
            finally
            {
                Log.Information("AccessPointMap service quitting.");

                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
