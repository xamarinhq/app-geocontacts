#!/usr/bin/env bash

echo "Variables:"
printenv

echo "Arguments for updating:"
echo " - ACID: $AC_ANDROID"

# Updating manifest
sed -i '' "s/AC_ANDROID/$AC_ANDROID/g" $BUILD_REPOSITORY_LOCALPATH/src/AwesomeContacts/AwesomeContacts/Helpers/CommonConstants.cs

cat $BUILD_REPOSITORY_LOCALPATH/src/AwesomeContacts/AwesomeContacts/Helpers/CommonConstants.cs

echo "Manifest updated!"