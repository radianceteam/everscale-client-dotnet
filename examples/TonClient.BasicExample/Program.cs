using System;
using Serilog;
using TonClient.Examples.Lib;

namespace TonClient.BasicExample
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

            using var client = TonSdk.TonClient.Create(new SerilogLogger());
            var version = client.Client.VersionAsync().Result;
            Console.WriteLine($"TON SDK client version: {version.Version}");
        }
    }
}
