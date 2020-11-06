cmake_minimum_required(VERSION 3.9)

project(ton-client-dotnet-bridge-third-party-libs)

include(ExternalProject)

set(TON_SDK_VERSION 1.1.0-api-gen)
set(TON_SDK_PATH ${CMAKE_BINARY_DIR}/ton-sdk-${TON_SDK_VERSION})

ExternalProject_Add(
        ton-sdk
        GIT_REPOSITORY https://github.com/tonlabs/TON-SDK.git
        GIT_TAG ${TON_SDK_VERSION}
        PREFIX ${TON_SDK_PATH}
        CONFIGURE_COMMAND ""
        BUILD_COMMAND cargo build --release
        INSTALL_COMMAND ""
        BUILD_IN_SOURCE 1
        LOG_BUILD ON)
