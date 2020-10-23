#pragma once

// Bridge is aimed to solve issues with TON struct marshalling to managed code and vice versa.
//
// Note: issues with marshalling include passing tc_string_data_t by value which looks
// difficult ATM. So here, we're creating a different C API which can be transparently
// mapped to .NET simple types, like, IntPtr.

#include <stdint.h>
#include <stdbool.h>

#if defined _WIN32 || defined __CYGWIN__
    #define TC_BRIDGE_HELPER_DLL_IMPORT __declspec(dllimport)
    #define TC_BRIDGE_HELPER_DLL_EXPORT __declspec(dllexport)
    #define TC_BRIDGE_HELPER_DLL_LOCAL
#else
#if __GNUC__ >= 4
    #define TC_BRIDGE_HELPER_DLL_IMPORT __attribute__ ((visibility ("default")))
    #define TC_BRIDGE_HELPER_DLL_EXPORT __attribute__ ((visibility ("default")))
    #define TC_BRIDGE_HELPER_DLL_LOCAL  __attribute__ ((visibility ("hidden")))
#else
    #define TC_BRIDGE_HELPER_DLL_IMPORT
    #define TC_BRIDGE_HELPER_DLL_EXPORT
    #define TC_BRIDGE_HELPER_DLL_LOCAL
#endif // __GNUC__ >= 4
#endif // defined _WIN32 || defined __CYGWIN__

// Now we use the generic helper definitions above to define TC_BRIDGE_API and TC_BRIDGE_INTERNAL.
// TC_BRIDGE_API is used for the public API symbols. It either DLL imports or DLL exports (or does nothing for static build)
// TC_BRIDGE_INTERNAL is used for non-api symbols.

#ifdef TC_BRIDGE_DLL // defined if TON is compiled as a DLL
#ifdef TC_BRIDGE_DLL_EXPORTS // defined if we are building the TON DLL (instead of using it)
    #define TC_BRIDGE_API TC_BRIDGE_HELPER_DLL_EXPORT
#else
    #define TC_BRIDGE_API TC_BRIDGE_HELPER_DLL_IMPORT
#endif // TC_BRIDGE_DLL_EXPORTS
    #define TC_BRIDGE_INTERNAL TC_BRIDGE_HELPER_DLL_LOCAL
#else // TC_BRIDGE_DLL is not defined: this means TON is a static lib.
#define TC_BRIDGE_API
    #define TC_BRIDGE_INTERNAL
#endif // TC_BRIDGE_DLL

#if defined(__FreeBSD__) || defined(__FreeBSD)
#define TON_FREE_BSD
#elif defined(__linux) || defined(__linux__) || defined(linux)
#define TON_LINUX
#elif defined(_WIN32) || defined(__WIN32__) || defined(WIN32)
#define TON_WINDOWS
#elif defined(__APPLE__)
#define TON_APPLE
#endif


#ifdef __cplusplus
extern "C"
{
#endif

// The main change to the original TON API is the callback func which
// accepts non-null terminated string and its length, as an arguments
// (instead of struct which seems to be hard to map to the C# code).
// Note: the client doesn't have to free the given str, it's done by the lib.

typedef void (*tc_bridge_json_callback_t)(
        const char *str,
        uint32_t len);

typedef void (*tc_bridge_response_handler_t)(
        const char *response_json,
        uint32_t response_json_len);

TC_BRIDGE_API void tc_bridge_create_context(
        const char *config,
        uint32_t config_len,
        tc_bridge_json_callback_t response);

TC_BRIDGE_API void tc_bridge_request(
        uint32_t context,
        const char *function_name,
        uint32_t function_name_len,
        const char *function_params_json,
        uint32_t params_json_len,
        tc_bridge_response_handler_t success_handler,
        tc_bridge_response_handler_t error_handler);

TC_BRIDGE_API void tc_bridge_destroy_context(uint32_t context);

#ifdef __cplusplus
} // extern "C"
#endif