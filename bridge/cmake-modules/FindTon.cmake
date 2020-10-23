# Find the native TON Client headers and libraries.

# Result:
# TON_FOUND         - True if TON Client found.
# TON_INCLUDE_DIRS  - Location of tonclient.h.
# TON_LIBRARIES     - List of libraries when using ton client.

find_path(TON_INCLUDE_DIR tonclient.h)
find_library(TON_LIBRARY NAMES ton_client)

include(FindPackageHandleStandardArgs)
find_package_handle_standard_args(TON REQUIRED_VARS TON_LIBRARY TON_INCLUDE_DIR)

IF (TON_FOUND)
    set(TON_LIBRARIES ${TON_LIBRARY})
    set(TON_INCLUDE_DIRS ${TON_INCLUDE_DIR})
    if (NOT TARGET TON::Client)
        add_library(TON::Client UNKNOWN IMPORTED)
        set_target_properties(TON::Client PROPERTIES INTERFACE_INCLUDE_DIRECTORIES "${TON_INCLUDE_DIR}")
        set_property(TARGET TON::Client APPEND PROPERTY IMPORTED_LOCATION "${TON_LIBRARY}")
    endif ()
else (TON_FOUND)
    set(TON_LIBRARIES)
    set(TON_INCLUDE_DIRS)
endif (TON_FOUND)

mark_as_advanced(TON_INCLUDE_DIRS TON_LIBRARIES)
