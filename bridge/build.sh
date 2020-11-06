#!/bin/sh

set -e

PROJECT_NAME="TON CLIENT DOTNET BRIDGE"

DO_CLEAN=0
FIND_LEAKS=0
SKIP_TESTS=0
BUILD_TYPE=Release
BUILD_DIR=cmake-build-release
SKIP_BUILDING_THIRD_PARTY_LIBS=0

while getopts ":SscTl" opt; do
  case ${opt} in
    S ) # skip building third party libs
      SKIP_BUILDING_THIRD_PARTY_LIBS=1
      ;;
    c ) # clean build directories
      DO_CLEAN=1
      ;;
    T ) # skip testing step
      SKIP_TESTS=1
      ;;
    s ) # enable debug symbols
      BUILD_TYPE=Debug
      BUILD_DIR=cmake-build-debug
      ;;
    l ) # find memory leaks
      FIND_LEAKS=1
      ;;
    \? ) echo "Usage: build.sh [-S] [-T] [-s] [-c] [-l]"
      ;;
  esac
done

CWD=$(pwd)

if [ "${SKIP_BUILDING_THIRD_PARTY_LIBS}" -ne "1" ]; then
  echo "Building third party libraries."
  cd vendor || exit
  if [ "${DO_CLEAN}" -ne "0" ]; then
    echo "Cleaning up build directory."
    rm -rf build
  fi
  mkdir -p build
  cd build || exit
  cmake ..
  make
else
  echo "Not building third party libraries."
fi

echo "Building ${PROJECT_NAME}..."
cd "${CWD}" || exit
if [ "${DO_CLEAN}" -ne "0" ]; then
  echo "Cleaning up build directory."
  rm -rf ${BUILD_DIR}
fi
mkdir -p ${BUILD_DIR}
cd ${BUILD_DIR} || exit

cmake .. \
  -DTON_SKIP_TESTS=${SKIP_TESTS} \
  -DCMAKE_BUILD_TYPE=${BUILD_TYPE} \
  -DTON_FIND_LEAKS=${FIND_LEAKS}

make

if [ "${SKIP_TESTS}" -ne "1" ]; then
  CTEST_COMMAND="ctest --output-on-failure"
  if [ "${FIND_LEAKS}" -ne "0" ]; then
    CTEST_COMMAND="${CTEST_COMMAND} -D ExperimentalMemCheck"
  fi
  bash -c "${CTEST_COMMAND}"
fi

echo "${PROJECT_NAME} build finished."
