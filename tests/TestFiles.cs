using System;
using System.IO;
using System.Reflection;

namespace TonSdk.Tests
{
    public static class TestFiles
    {
        public static string GetFilePath(string fileName)
        {
            var codeBaseUrl = new Uri(typeof(TestFiles).GetTypeInfo().Assembly.CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            return Path.Combine(dirPath, fileName);
        }

        public static Stream OpenStream(string fileName)
        {
            return new FileStream(GetFilePath(fileName), FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }
}
