## TON SDK .NET Wrapper - Debot Example

(Debot stands for Decentralized Bot.)

NOTE: this example is BROKEN as of 1.6.0.

## Requirements

.NET SDK 3.1 or later.

### Running debot example

#### Setting up environment

To run this demo, you should first start NodeSE locally by running this command:

```
docker run -d -p8888:80 -e USER_AGREEMENT=yes tonlabs/local-node
```

(requires [Docker](https://docs.docker.com/get-docker/)).

After starting NodeSE, set `TON_NETWORK_ADDRESS` environment vriable to `http://localhost:8888`.

#### Building project

```
dotnet restore
dotnet build
```

#### Running .exe

```
dotnet run bin/Debug/netcoreapp3.1/TonClient.DebotExample.exe
```