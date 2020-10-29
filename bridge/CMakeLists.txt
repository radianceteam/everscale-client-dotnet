cmake_minimum_required(VERSION 3.18)

set(TC_VERSION_MAJOR 1)
set(TC_VERSION_MINOR 0)
set(TC_VERSION_PATCH 0)
set(TC_VERSION_SUFFIX "-rc")
set(TC_VERSION_STRING ${TC_VERSION_MAJOR}.${TC_VERSION_MINOR}.${TC_VERSION_PATCH})
set(TC_VERSION_FULL ${TC_VERSION_STRING}${TC_VERSION_SUFFIX})
set(TC_LIB_DIR "${CMAKE_CURRENT_SOURCE_DIR}/../lib")

set(CMAKE_MODULE_PATH ${CMAKE_MODULE_PATH} ${CMAKE_CURRENT_SOURCE_DIR}/cmake-modules)
set(CMAKE_INSTALL_PREFIX ${CMAKE_CURRENT_SOURCE_DIR}/..)
list(APPEND CMAKE_PREFIX_PATH ${TC_LIB_DIR})

project(tonclient_dotnet_bridge VERSION ${TC_VERSION_STRING})

include_directories(include lib)
enable_testing()

add_subdirectory(include)
add_subdirectory(lib)
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

