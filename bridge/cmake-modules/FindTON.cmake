# Find the native TON Client headers and libraries.

# Result:
# TON_FOUND             - True if TON Client found.
# TON_INCLUDE_DIRS      - Location of tonclient.h.
# TON_LIBRARIES         - List of libraries when using ton client.
# TON_RUNTIME_LIBRARIES - List of runtime libraries when using ton client.

set(TON_LIB_NAMES ton_client tonclient)

find_path(TON_INCLUDE_DIR tonclient.h)
find_library(TON_LIBRARY NAMES ${TON_LIB_NAMES})

include(FindPackageHandleStandardArgs)
find_package_handle_standard_args(TON REQUIRED_VARS TON_LIBRARY TON_INCLUDE_DIR)

IF (TON_FOUND)
    set(TON_LIBRARIES ${TON_LIBRARY})
    set(TON_INCLUDE_DIRS ${TON_INCLUDE_DIR})
    if (NOT TARGET TON::Client)
        add_library(TON::Client UNKNOWN IMPORTED)
        set_target_properties(TON::Client PROPERTIES INTERFACE_INCLUDE_DIRECTORIES "${TON_INCLUDE_DIR}")
        set_property(TARGET TON::Client APPEND PROPERTY IMPORTED_LOCATION "${TON_LIBRARY}")
        if (WIN32)
            foreach (name ${TON_LIB_NAMES})
                find_file(TON_RUNTIME_LIBRARIES ${name}${CMAKE_SHARED_LIBRARY_SUFFIX})
                if (TON_RUNTIME_LIBRARIES)
                    break()
                endif ()
            endforeach ()
        else ()
            set(TON_RUNTIME_LIBRARIES ${TON_LIBRARY})
        endif ()
    endif ()
else (TON_FOUND)
    set(TON_LIBRARIES)
    set(TON_INCLUDE_DIRS)
endif (TON_FOUND)

mark_as_advanced(TON_INCLUDE_DIRS TON_LIBRARIES)
