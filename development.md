# TON SDK .NET Wrapper - Development notes

## Requirements

### Supported OS

 - Window x64, x86
 - Linux x64
 - macOS x64

### Supported runtimes

 - .NET Core 2.0 and later.
 - .NET Framework 4.6.1 and later.

## Source structure

### Native bridge library for .NET

Sources are in separate repository https://github.com/radianceteam/ton-client-dotnet-bridge. 
CI/CD was set up in that repository to generate binaries upon each build.
Windows, Linux and macOS binaries are pre-built and placed into [lib](lib/readme.md) 
directory located in the project root.

### .NET module code generator

Code generator sources are in [generator](generator/readme.md) directory. Proceed to the link 
to read more about the code generator feature.

### .NET interop code

Core classes and generated modules are in `src` dir.

### Tests

Tests are located in `tests` dir. To build project and run all tests, do the following:

1. Run local NodeSE server for testing.

```
docker run -d --name local-node -p8888:80 tonlabs/local-node
```

2. Given that Docker machine host is `localhost`, set `TON_NETWORK_ADDRESS` environment 
   variable to `http://localhost:8888`. Restart shell if needed.

3. Run these commands:

```
dotnet restore
dotnet test
```

## Updating procedure

Here's the procedure of upgrading .NET Wrapper to the new TON SDK version.

### Update binaries

1. Update `ton-client-dotnet-bridge` sources and push changes to GitHub.
2. Wait for the build, then go to https://github.com/radianceteam/ton-client-dotnet-bridge/actions and download build artifacts.
3. Unpack them, find binaries for Windows, Linux and Mac and copy them to the corresponding path inside `lib` directory (replacing the existing ones).

### Re-generate modules

1. Copy the latest `api.json` to `generator` directory.
2. Run `cd generator && npm install && npm run generate`.
3. Solve all compilation issues manually, if any.
4. Run tests; ensure that nothing is broken.
5. Write new tests for new functions, if any.

Note: test logic may alreay exist in TON SDK, see https://github.com/tonlabs/TON-SDK/tree/master/ton_client/client/src, 
look into the corresponding module directory and find `tests.rs` file. In this case, test can be easily converted 
from Rust programming language to C# (this is basically how all the tests are written here).

## Deployment

TBD

## TODO

### Deployment

 - run tests and publish NuGet package when creating new Release in this repo.
