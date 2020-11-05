using System;
using System.Runtime.InteropServices;

namespace TonSdk
{
    internal static class Interop
    {
        private const string DllName = "tonclient_dotnet_bridge";

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void tc_bridge_json_callback_t(
            IntPtr str,
            int len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void tc_bridge_create_context(
            IntPtr config,
            int config_len,
            tc_bridge_json_callback_t response);

        public enum tc_response_types_t
        {
            tc_response_success = 0,
            tc_response_error = 1,
            tc_response_nop = 2,
            tc_response_custom = 100,
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void tc_bridge_response_handler_t(
            tc_response_types_t type,
            IntPtr params_json,
            int response_json_len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void tc_bridge_request(
            uint context,
            IntPtr function_name,
            int function_name_len,
            IntPtr function_params_json,
            int params_json_len,
            tc_bridge_response_handler_t handler);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void tc_bridge_destroy_context(uint context);
    }
}
