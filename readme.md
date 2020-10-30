# TON SDK Dotnet Wrapper

## Native bridge library for .NET

Sources are in `bridge` directory. 
Windows and Linux binaries are pre-built and placed into `lib` directory located in the project root. 
Build script is written in `bridge/install.bat` (Windows) and `bridge/install.sh` (Linux/macOS).

## .NET module code generator

Code generator sources are in `generator` directory.

## .NET interop code

Core classes and generated modules are in `src` dir.

## Tests

Tests are located in `tests` dir.

NOTE: Code was generated for all functions, but only 13 of them are covered with tests ATM. 
Some of others may have bugs, so to be covered later. Still TODOs there for Step 2 and Step 3.

Functions (methods) covered with tests:
1. client.version
2. client.get_api_reference
3. client.build_info
4. crypto.factorize
5. crypto.modular_power
6. crypto.ton_crc16
7. crypto.generate_random_bytes
8. crypto.convert_public_key_to_ton_safe_format
9. crypto.generate_random_sign_keys
10. crypto.sign
11. crypto.verify_signature
12. crypto.sha256
13. crypto.sha512

## Build

To build project and run all tests, clone this repo and then run:

```
dotnet restore
dotnet test
```

## TODO
 - More Tests.
 - Mac support.
 - GitHub workflow script which runs tests and publishes NuGet packages.

