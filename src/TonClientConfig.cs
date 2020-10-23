namespace TonSdk
{
    public class TonClientConfig
    {
        public string BaseUrl { get; set; }
        public ILogger Logger { get; set; } = DummyLogger.Instance;
    }
}
