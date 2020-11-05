#include "tonclient_dotnet_bridge.h"
#include "tonclient.h"

#include <assert.h>
#include <stdint.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdarg.h>

#ifdef TON_WINDOWS
#define WIN32_LEAN_AND_MEAN      // Exclude rarely-used stuff from Windows headers

#include <windows.h>
    #include <winnt.h>

    #define ATOMIC_INCREMENT(p) InterlockedIncrement(p)
#else
    #define ATOMIC_INCREMENT(p) __sync_add_and_fetch(p, 1)
#endif

void fail(const char *fmt, ...) {
    va_list args;
            va_start(args, fmt);
    vfprintf(stderr, fmt, args);
            va_end(args);
    exit(-1);
}

static uint32_t next_request_id = 0;

uint32_t get_next_request_id() {

    // NOTE: it's theoretically possible that we exceed the number of unique request ids.
    // FIXME: re-start counter in case of exceeding UINT32_MAX?
    if (next_request_id + 1 >= UINT32_MAX) {
        fail("request ID %d is exceeding UINT32_MAX value %d\n", next_request_id, UINT32_MAX);
        return 0;
    }

    return ATOMIC_INCREMENT(&next_request_id);
}

// Below is the primitive hash map impl we use to map request ids to the handler data.
//
// NOTE: it's almost impossible that the two different requests will have the same
// index at the same moment. (It's only possible when more than REQUEST_DATA_MAP_SIZE
// requests are generated in a single moment of time).
//
// So we are assuming that the client lib is not called so frequently, otherwise we'll need
// to add synchronization here, or use some existing concurrent hash map library.
//
// Anyway the best solution here is: `tonclient.h` should be modified so instead of passing
// `uint32_t request_id` we could use `void* ptr`. This would eliminate the need of transforming
// callback data structures to `uint32_t` and back, thus removing the need of hash table logic
// as all is needed in this case is to pass the pointer to the `struct` containing all the
// callback info and then free it when request processing is finished.
//
// (For now, waiting for the SDK updates.)

typedef struct tc_bridge_handler_data {
    uint32_t request_id;
    tc_bridge_response_handler_t handler;
    struct tc_bridge_handler_data *next;
} tc_bridge_handler_data_t;

#define REQUEST_DATA_MAP_SIZE 4096

static tc_bridge_handler_data_t *request_data_map[REQUEST_DATA_MAP_SIZE];

tc_bridge_handler_data_t *create_handler_data(
        uint32_t request_id,
        tc_bridge_response_handler_t success_handler) {

    tc_bridge_handler_data_t *data = calloc(1, sizeof(tc_bridge_handler_data_t));
    data->request_id = request_id;
    data->handler = success_handler;
    data->next = NULL;

    uint32_t index = request_id % REQUEST_DATA_MAP_SIZE;
    tc_bridge_handler_data_t *prev = request_data_map[index];
    if (prev) {
        while (prev->next) {
            prev = prev->next;
        }
        prev->next = data;
    } else {
        request_data_map[index] = data;
    }

    return data;
}

/**
 * @param request_id Unique request id.
 * @return Request handler data or <code>NULL</code> if not found.
 */
tc_bridge_handler_data_t *lookup_handler_data(uint32_t request_id) {
    uint32_t index = request_id % REQUEST_DATA_MAP_SIZE;
    tc_bridge_handler_data_t *data = request_data_map[index];
    while (data && data->request_id != request_id) {
        data = data->next;
    }
    return data;
}

void free_handler_data(tc_bridge_handler_data_t *data) {
    assert(data);

    if (!data) {
        fail("NULL passed to free_handler_data\n");
        return;
    }

    uint32_t index = data->request_id % REQUEST_DATA_MAP_SIZE;
    tc_bridge_handler_data_t *p = request_data_map[index], *prev = NULL;
    while (p && p != data) {
        prev = p;
        p = p->next;
    }
    if (prev) {
        prev->next = data->next;
    } else {
        request_data_map[index] = data->next;
    }
    free(data);
}

void bridge_response_handler(
        uint32_t request_id,
        tc_string_data_t params_json,
        tc_response_types_t response_type,
        bool finished) {

    tc_bridge_handler_data_t *data = lookup_handler_data(request_id);
    assert(data);

    if (!data) {
        fail("request %d has no response handler\n", request_id);
        return;
    }

    if (data->handler) {
        data->handler(response_type, params_json.content, params_json.len);
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

    uint32_t request_id = get_next_request_id();

    create_handler_data(request_id, handler);

    tc_string_data_t f_name = {function_name, function_name_len};
    tc_string_data_t f_params = {function_params_json, params_json_len};
    tc_request(context, f_name, f_params, request_id, &bridge_response_handler);
}

void tc_bridge_destroy_context(uint32_t context) {
    tc_destroy_context(context);
}
