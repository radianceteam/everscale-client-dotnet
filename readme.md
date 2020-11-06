# TON SDK Dotnet Wrapper

## Native bridge library for .NET

Sources are in `https://github.com/radianceteam/ton-client-dotnet-bridge` repository. 
Windows, Linux and macOS binaries are pre-built and placed into `lib` directory located in the project root.

## .NET module code generator

Code generator sources are in `generator` directory.

## .NET interop code

Core classes and generated modules are in `src` dir.

## Tests

Tests are located in `tests` dir.

NOTE: Code was generated for all 57 functions, and 29 of them are covered with tests ATM.
Some of others may have bugs, so to be covered later. Still TODOs there for Step 3.

Functions (methods) covered with tests:

1. `client.version`
2. `client.get_api_reference`
3. `client.build_info`
4. `crypto.factorize`
5. `crypto.modular_power`
6. `crypto.ton_crc16`
7. `crypto.generate_random_bytes`
8. `crypto.convert_public_key_to_ton_safe_format`
9. `crypto.generate_random_sign_keys`
10. `crypto.sign`
11. `crypto.verify_signature`
12. `crypto.sha256`
13. `crypto.sha512`
14. `crypto.scrypt`
15. `crypto.nacl_sign_keypair_from_secret_key`
16. `crypto.nacl_sign`
17. `crypto.nacl_sign_open`
18. `crypto.nacl_sign_detached`
19. `crypto.nacl_box_keypair`
20. `crypto.nacl_box`
21. `crypto.nacl_box_open`
22. `crypto.nacl_secret_box`
23. `crypto.nacl_secret_box_open`
24. `crypto.mnemonic_words`
25. `crypto.mnemonic_from_random`
26. `crypto.mnemonic_from_entropy`
27. `crypto.mnemonic_verify`
28. `crypto.mnemonic_derive_sign_keys`
29. `crypto.hdkey_xprv_from_mnemonic`

## Build

To build project and run all tests, clone this repo and then run:

```
dotnet restore
dotnet test
```

## Usage example

### Basic usage

TBD

### Advanced usage

TBD

## TODO

### Tests

 - 100% function test coverage
 - More negative tests

### Deployment

 - run tests and publish NuGet package when creating new Release in this repo.

### Documentation

 - usage readme
 - development readme
