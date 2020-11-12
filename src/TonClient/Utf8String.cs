using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TonSdk
{
    internal class Utf8String : IDisposable
    {
        private readonly string _str;

        public IntPtr Ptr { get; }
        public byte[] Bytes { get; }
        public int Length => Bytes.Length;

        public Utf8String(string str)
        {
            _str = str;
            Bytes = Encoding.UTF8.GetBytes(str);
            Ptr = Marshal.AllocHGlobal(Bytes.Length);
            Marshal.Copy(Bytes, 0, Ptr, Bytes.Length);
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(Ptr);
        }

        public override string ToString()
        {
            return _str;
        }

        public static string ToString(IntPtr ptr, int len)
        {
            var bytes = new byte[len];
            Marshal.Copy(ptr, bytes, 0, bytes.Length);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}