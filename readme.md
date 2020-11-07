# TON SDK .NET Wrapper

## Supported Platforms

 - Windows x86, x64
 - Linux x64
 - macOS x64
 
### Supported runtimes

 - .NET Core 2.0 and newer.
 - .NET Framework 4.6.1 and newer.

## Installation

### NuGet package

```
Install-Package TonClient
```

## Usage example

### Basic usage

```
using TonSdk.TonClient;

...

    var client = TonClient.Create();
    var result = await _client.Client.VersionAsync();
    // use result.Version...

### Advanced usage

#### Configuring client

```
    var client = TonClient.Create(new ClientConfig
        {
            Network = new NetworkConfig
            {
                ServerAddress = "http://localhost",
                MessageRetriesCount = 10,
                OutOfSyncThreshold = 2500
            },
            Abi = new AbiConfig
            {
                MessageExpirationTimeout = 10000
            }
        });
```

#### Logging

By default, wrapper uses `DummyLogger` which is an implementation of `ILogger` interface.

To configure custom logging, create own `ILogger` implementation and pass it to `TonClient.Create()`:

```
    public class MyLogger : ILogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger _delegate;

        public MyLogger(Microsoft.Extensions.Logging.ILogger logger)
        {
            _delegate = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Debug(string message)
        {
            _delegate.LogDebug(message);
        }

        public void Information(string message)
        {
            _delegate.LogInformation(message);
        }

        public void Warning(string message)
        {
            _delegate.LogWarning(message);
        }

        public void Error(string message, Exception ex = null)
        {
            _delegate.LogError(ex, message);
        }
    }

    ...

    var logger = ... ;

    var client = TonClient.Create(null, new MyLogger(logger));
    
``` 

## Development

See [Development documentation](development.md)