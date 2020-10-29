enable_testing()

include_directories(. ../src)

find_package(TON REQUIRED)

add_executable(tonclient_dotnet_bridge_tests bridge.c)
target_link_libraries(tonclient_dotnet_bridge_tests tonclient_dotnet_bridge TON::Client)
add_test(bridge_tests tonclient_dotnet_bridge_tests)

if (WIN32)
    # Copy .dll files into tests folder
    set(LIB_RUNTIME_FILES
            ${TON_RUNTIME_LIBRARIES}
            ${CMAKE_BINARY_DIR}/src/tonclient_dotnet_bridge.dll)

    foreach (file IN LISTS LIB_RUNTIME_FILES)
        add_custom_command(
                TARGET tonclient_dotnet_bridge_tests POST_BUILD
                COMMAND ${CMAKE_COMMAND} -E copy_if_different ${file} ${CMAKE_BINARY_DIR}/tests
                COMMENT "Copying shared lib ${file} to ${CMAKE_BINARY_DIR}/tests")
    endforeach ()

endif ()
