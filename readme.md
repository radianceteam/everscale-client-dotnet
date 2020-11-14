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

## Usage examples

### Basic usage

```cs
using TonSdk;

    using (var client = TonClient.Create()) {
        var version = await client.Client.VersionAsync();
        Console.WriteLine($"TON SDK client version: {version.Version}");
    }
```

### Advanced usage

#### Configuring client

```cs
    using (var client = TonClient.Create(new ClientConfig
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
        }))
    {
        // ...
    }

```

#### Logging

By default, wrapper uses `DummyLogger` which is an implementation of `ILogger` interface.

To configure custom logging, create own `ILogger` implementation and pass it to `TonClient.Create()`:

```cs 
using System;
using Serilog;
using ILogger = TonSdk.ILogger;

...

    public class MyLogger : ILogger
    {
        public void Debug(string message)
        {
            Log.Debug(message);
        }

        public void Information(string message)
        {
            Log.Information(message);
        }

        public void Warning(string message)
        {
            Log.Warning(message);
        }

        public void Error(string message, Exception ex = null)
        {
            Log.Error(ex, message);
        }
    }   
``` 

then call `TonClient.Create` method with logger argument:

```cs 
using System;
using Serilog;
using TonSdk;
   
    Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                // ... other logging setup
                .CreateLogger();

    using (var client = TonClient.Create(new MyLogger())) {
        // ...
    }   
```

or with both config and logger:
   
```cs 
    using (var client = TonClient.Create(new ClientConfig { 
        // ... 
    }, new MyLogger()))
    {
        // ...
    }
```

Note: see [TonClientDemo](src/TonClientDemo) for the complete working demo.

## Development

See [Development documentation](development.md).