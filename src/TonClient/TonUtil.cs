using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
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
            using (var stream = new FileStream(GetAbsolutePath(path), FileMode.Open, FileAccess.Read, FileShare.Read))
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
            using (var stream = new FileStream(GetAbsolutePath(path), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        /// <summary>
        /// This method converts relative file path to the absolute one,
        /// based on the assembly location. This allows to load assets
        /// similarly in both VS and command line runtime environments.
        /// </summary>
        private static string GetAbsolutePath(string fileName)
        {
            var codeBaseUrl = new Uri(typeof(TonUtil).GetTypeInfo().Assembly.CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            return Path.Combine(dirPath, fileName);
        }
    }
}
