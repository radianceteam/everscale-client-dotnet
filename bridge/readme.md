# TON SDK Dotnet Bridge

Bridge is aimed to solve issues with TON struct marshalling to managed code and vice versa.

Note: issues with marshalling include passing `tc_string_data_t` by value which looks
difficult ATM. So here, we're creating a different C API which can be transparently
mapped to .NET simple types, like, `IntPtr`.

## Requirements

### All platforms

1. CMake version 3.9 or higher (https://cmake.org/download/).
2. Rust version 1.47.0 or higher (https://www.rust-lang.org/).

### Windows

#### Visual Studio 2019

https://visualstudio.microsoft.com/downloads/

### Linux/MacOS

TBD: prerequisites; how to install them.

## Build

### Windows

1. Open `x86_x64 Cross Tools Command Prompt for VS 2019`
2. Change directory to `bridge` folder location.
3. Run `build.bat`

### Linux/MacOS

```
./build.sh
```

## TODO

Move to separate repo, create .github build scripts, setup matrix build, publish binaries on GitHub?