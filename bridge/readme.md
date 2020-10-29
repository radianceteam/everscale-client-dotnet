# TON SDK Dotnet Bridge

Bridge is aimed to solve issues with TON struct marshalling to managed code and vice versa.

Note: issues with marshalling include passing `tc_string_data_t` by value which looks
difficult ATM. So here, we're creating a different C API which can be transparently
mapped to .NET simple types, like, `IntPtr`.

## Requirements

CMake version 3.18 or higher.

### Windows

#### CMake

https://cmake.org/download/

#### 7Zip

https://www.7-zip.org/download.html

#### Visual Studio 2019

https://visualstudio.microsoft.com/downloads/

### Linux/MacOS

TBD

## Build

Library files are built and installed into `lib` directory of the _project root_.

### Windows

1. Open `x86_x64 Cross Tools Command Prompt for VS 2019`
2. Change directory to `bridge` folder location.
3. Run `install.bat`

### Linux/MacOS

TBD

## TODO

Move to separate repo, create .github build scripts, setup matrix build, publish binaries on GitHub?