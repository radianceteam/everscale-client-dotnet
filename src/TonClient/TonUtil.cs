using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using TonSdk.Modules;

namespace TonSdk
{
    public static class TonUtil
    {
        public static Abi LoadAbi(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException(nameof(path));
            }
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return new Abi.Contract
                    {
                        Value = new JsonSerializer().Deserialize<AbiContract>(new JsonTextReader(reader))
                    };
                }
            }
        }

        public static string LoadTvc(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException(nameof(path));
            }
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }
    }
}
