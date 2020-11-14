using System;
using Serilog;
using TonSdk;

namespace TonClientDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("debug.log")
                .CreateLogger();

            using (var client = TonClient.Create(new DemoLogger()))
            {
                var version = client.Client.VersionAsync().Result;
                Console.WriteLine($"TON SDK client version: {version.Version}");
            }
        }
    }
}
