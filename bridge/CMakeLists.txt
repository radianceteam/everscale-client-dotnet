cmake_minimum_required(VERSION 3.9)

set(TC_VERSION_MAJOR 1)
set(TC_VERSION_MINOR 1)
set(TC_VERSION_PATCH 0)
set(TC_VERSION_SUFFIX "-api-gen")
set(TC_VERSION_STRING ${TC_VERSION_MAJOR}.${TC_VERSION_MINOR}.${TC_VERSION_PATCH})
set(TC_VERSION_FULL ${TC_VERSION_STRING}${TC_VERSION_SUFFIX})

set(CMAKE_MODULE_PATH ${CMAKE_MODULE_PATH} ${CMAKE_CURRENT_SOURCE_DIR}/cmake-modules)

set(TON_SDK_SRC "${CMAKE_CURRENT_SOURCE_DIR}/vendor/build/ton-sdk-${TC_VERSION_FULL}/src/ton-sdk")
set(TON_SDK_CLIENT_DIR "${TON_SDK_SRC}/ton_client/client")
set(TON_SDK_RELEASE_DIR "${TON_SDK_SRC}/target/release")
list(APPEND CMAKE_PREFIX_PATH "${TON_SDK_CLIENT_DIR}" "${TON_SDK_RELEASE_DIR}")

project(tonclient_dotnet_bridge VERSION ${TC_VERSION_STRING})

include_directories(include)
enable_testing()

add_subdirectory(include)
add_subdirectory(src)

if (NOT TON_SKIP_TESTS)
    enable_testing()
    add_subdirectory(tests)
endif ()

if (TON_FIND_LEAKS)
    include(CTest)
    find_program(MEMORYCHECK_COMMAND valgrind)
    set(MEMORYCHECK_COMMAND_OPTIONS "--trace-children=yes --leak-check=full")
    set(MEMORYCHECK_SUPPRESSIONS_FILE "${PROJECT_SOURCE_DIR}/valgrind_suppress.txt")
endif ()

