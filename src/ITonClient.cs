using System;
using System.Threading.Tasks;

namespace TonSdk
{
    public partial interface ITonClient : IDisposable
    {
        Task<string> CallFunction(
            string functionName,
            string functionParamsJson = "");
    }
}
