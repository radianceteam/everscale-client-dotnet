#include "tonclient_dotnet_bridge.h"
#include "tonclient.h"

#include <assert.h>
#include <stdint.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdarg.h>

void fail(const char *fmt, ...) {
    va_list args;
            va_start(args, fmt);
    vfprintf(stderr, fmt, args);
            va_end(args);
    exit(-1);
}

typedef struct tc_bridge_handler_data {
    tc_bridge_response_handler_t handler;
} tc_bridge_handler_data_t;

tc_bridge_handler_data_t *create_handler_data(tc_bridge_response_handler_t success_handler) {
    tc_bridge_handler_data_t *data = malloc(sizeof(tc_bridge_handler_data_t));
    data->handler = success_handler;
    return data;
}

void free_handler_data(tc_bridge_handler_data_t *data) {
    assert(data);
    if (!data) {
        fail("NULL passed to free_handler_data\n");
        return;
    }
    free(data);
}

void bridge_response_handler(
        void *request_ptr,
        tc_string_data_t params_json,
        uint32_t response_type,
        bool finished) {

    tc_bridge_handler_data_t *data = request_ptr;
    assert(data);

    if (!data) {
        fail("NULL ptr passed to response handler\n");
        return;
    }

    if (data->handler) {
        data->handler(response_type, params_json.content, params_json.len, finished);
    }

    if (finished) {
        free_handler_data(data);
    }
}

void tc_bridge_create_context(
        const char *config,
        uint32_t config_len,
        tc_bridge_json_callback_t response) {

    tc_string_data_t str = {config, config_len};
    tc_string_handle_t *result = tc_create_context(str);
    tc_string_data_t json = tc_read_string(result);
    response(json.content, json.len);
    tc_destroy_string(result);
}

void tc_bridge_request(
        uint32_t context,
        const char *function_name,
        uint32_t function_name_len,
        const char *function_params_json,
        uint32_t params_json_len,
        tc_bridge_response_handler_t handler) {

    assert(function_name);
    assert(function_name_len);
    assert(function_params_json);

    tc_bridge_handler_data_t *data = create_handler_data(handler);
    tc_string_data_t f_name = {function_name, function_name_len};
    tc_string_data_t f_params = {function_params_json, params_json_len};
    tc_request_ptr(context, f_name, f_params, data, &bridge_response_handler);
}

void tc_bridge_destroy_context(uint32_t context) {
    tc_destroy_context(context);
}
