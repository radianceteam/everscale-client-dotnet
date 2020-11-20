# TON SDK .NET Wrapper - Development notes

## Requirements

.NET SDK 3.1 or later.

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

1. Run local NodeSE server for testing using [Docker](https://www.docker.com/products/docker-desktop).

```
docker run -d -p8888:80 tonlabs/local-node
```

2. Given that Docker machine host is `localhost`, set `TON_NETWORK_ADDRESS` environment 
variable to `http://localhost:8888`. Restart shell if needed (on Windows).

3. Run these commands:

```
dotnet restore
dotnet test
```

Tests will output something like this:

```
Test Run Successful.
Total tests: 214
     Passed: 214
 Total time: 1.2510 Minutes
```

to see more detailed output run tests using this command:

```
dotnet test --logger:"console;verbosity=detailed"
```

Note: you could run tests without having to setup Docker and starting NodeSE server. 
But if you do so, some tests will be skipped:

```
[xUnit.net 00:00:00.56]     TonSdk.Tests.Modules.ProcessingModuleTests.Should_Wait_For_Message [SKIP]
[xUnit.net 00:00:00.57]     TonSdk.Tests.Modules.TvmModuleTests.Should_Run_Executor [SKIP]
[xUnit.net 00:00:00.63]     TonSdk.Tests.Modules.TvmModuleTests.Should_Run_Tvm [SKIP]
  ! TonSdk.Tests.Modules.ProcessingModuleTests.Should_Wait_For_Message [1ms]
  ! TonSdk.Tests.Modules.TvmModuleTests.Should_Run_Executor [1ms]
  ! TonSdk.Tests.Modules.TvmModuleTests.Should_Run_Tvm [1ms]
[xUnit.net 00:00:02.02]     TonSdk.Tests.Modules.NetModuleTests.Should_Query_Collection_Block_Signature [SKIP]
  ! TonSdk.Tests.Modules.NetModuleTests.Should_Query_Collection_Block_Signature [1ms]
[xUnit.net 00:00:02.03]     TonSdk.Tests.Modules.NetModuleTests.Should_Query_Collection_Ranges [SKIP]
[xUnit.net 00:00:02.03]     TonSdk.Tests.Modules.NetModuleTests.Should_Subscribe_For_Messages [SKIP]
[xUnit.net 00:00:02.03]     TonSdk.Tests.Modules.NetModuleTests.Should_Subscribe_For_Transactions_With_Address [SKIP]
[xUnit.net 00:00:02.03]     TonSdk.Tests.Modules.NetModuleTests.Should_Query_Collection_All_Accounts [SKIP]
[xUnit.net 00:00:02.03]     TonSdk.Tests.Modules.NetModuleTests.Should_Wait_For_Collection [SKIP]
  ! TonSdk.Tests.Modules.NetModuleTests.Should_Query_Collection_Ranges [1ms]
  ! TonSdk.Tests.Modules.NetModuleTests.Should_Subscribe_For_Messages [1ms]
  ! TonSdk.Tests.Modules.NetModuleTests.Should_Subscribe_For_Transactions_With_Address [1ms]
  ! TonSdk.Tests.Modules.NetModuleTests.Should_Query_Collection_All_Accounts [1ms]
  ! TonSdk.Tests.Modules.NetModuleTests.Should_Wait_For_Collection [1ms]

Test Run Successful.
Total tests: 214
     Passed: 205
    Skipped: 9
 Total time: 7.2342 Seconds
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

## Versioning

Package versioning mirrors TON SDK releases. So for example package `1.1.1` works 
with TON SDK binaries of the same version, and contains all the functions from the 
corresponding `api.json`. 

## License

Apache License, Version 2.0.

## Troubleshooting

Fire any question to our [Telegram channel](https://t.me/RADIANCE_TON_SDK).