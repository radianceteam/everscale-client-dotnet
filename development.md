# TON SDK .NET Wrapper - Development notes

## Requirements

.NET SDK 3.1 SDK or later.

## Native library for .NET

Native binaries are built in a separate repository https://github.com/radianceteam/ton-client-dotnet-bridge,
CI/CD was set up to generate binaries upon each new tag push. Windows, Linux and macOS binaries are pre-built 
that way and placed into [runtimes](runtimes/readme.md) directory located in the project root.

## Source structure

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

1. Push new tag to `ton-client-dotnet-bridge` to trigger TON SDK build (using the same tag as the SDK version, example: `1.1.0`).
2. Wait for the build, then go to https://github.com/radianceteam/ton-client-dotnet-bridge/actions and download build artifacts.
3. Unpack them, find binaries for Windows, Linux and Mac and copy them to the corresponding path inside `runtimes` directory (replacing the existing ones).

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

Create tag named after TON SDK version and push it. GitHub CI/CD will take care of the rest, 
like, running tests, creating Release, pushing to NuGet. Enjoy!
