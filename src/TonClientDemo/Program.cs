using System;
using TonSdk;

namespace TonClientDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var client = TonClient.Create())
            {
                var version = client.Client.VersionAsync().Result;
                Console.WriteLine($"TON SDK client version: {version.Version}");
            }
        }
    }
}
