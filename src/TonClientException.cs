using System;

namespace TonSdk
{
    public class TonClientException : Exception
    {
        public TonClientException(string message) : base(message)
        {
        }

        public TonClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
