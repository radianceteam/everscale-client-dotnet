#!/bin/sh

set -e

PROJECT_NAME="TON CLIENT DOTNET BRIDGE"

DO_CLEAN=0
FIND_LEAKS=0
SKIP_INSTALL=0
SKIP_TESTS=0
BUILD_TYPE=Release
BUILD_DIR=cmake-build-release

while getopts ":scITl" opt; do
  case ${opt} in
    c ) # clean build directories
      DO_CLEAN=1
      ;;
    I ) # skip installation step
      SKIP_INSTALL=1
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
    \? ) echo "Usage: install.sh [-T] [-I] [-s] [-c] [-l]"
      ;;
  esac
done

CWD=$(pwd)
INSTALL_DIR=${CWD}/..

if [ ! -w "${INSTALL_DIR}" ]; then
  echo "Directory ${INSTALL_DIR} is not writable. Trying to install into system directlory by non-root user?"
  exit 1
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
  -DCMAKE_INSTALL_PREFIX="${INSTALL_DIR}" \
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

if [ "${SKIP_INSTALL}" -ne "1" ]; then
  echo "Installing ${PROJECT_NAME} into ${INSTALL_PREFIX}."
  make install
  echo "${PROJECT_NAME} is successfully installed into ${INSTALL_PREFIX}."
else
  echo "Skipping installation."
fi

echo "${PROJECT_NAME} build finished."
