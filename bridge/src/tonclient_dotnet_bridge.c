#include "tonclient_dotnet_bridge.h"
#include "tonclient.h"

#include <assert.h>

#define REQUEST_POOL_SIZE 4096

// The idea is that the number of concurrent requests can't be more (can it?),
// so we can temporarily store pointers to the success/error handlers here,
// in the static memory storage, and free them once the request is finished.

tc_bridge_response_handler_t success_handlers[REQUEST_POOL_SIZE];
tc_bridge_response_handler_t error_handlers[REQUEST_POOL_SIZE];

uint32_t next_request_id = 1;

void bridge_response_handler(
        uint32_t request_id,
        tc_string_data_t params_json,
        uint32_t response_type,
        bool finished) {

    assert(success_handlers[request_id]);
    assert(error_handlers[request_id]);

    if (response_type == tc_response_success) {
        success_handlers[request_id](params_json.content, params_json.len);
    } else if (response_type == tc_response_error) {
        error_handlers[request_id](params_json.content, params_json.len);
    } else if (response_type == tc_response_custom) {
        // TODO: what??
    } else if (response_type == tc_response_nop && finished) {
        // Just in case
        success_handlers[request_id] = NULL;
        error_handlers[request_id] = NULL;
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
        tc_bridge_response_handler_t success_handler,
        tc_bridge_response_handler_t error_handler) {

    assert(function_name);
    assert(function_name_len);
    assert(function_params_json);

    int request_id = next_request_id++;
    if (request_id >= REQUEST_POOL_SIZE) {
        request_id = 1; // re-start request counter
    }

    success_handlers[request_id] = success_handler;
    error_handlers[request_id] = error_handler;

    tc_string_data_t f_name = {function_name, function_name_len};
    tc_string_data_t f_params = {function_params_json, params_json_len};
    tc_request(context, f_name, f_params, request_id, &bridge_response_handler);
}

void tc_bridge_destroy_context(uint32_t context) {
    tc_destroy_context(context);
}