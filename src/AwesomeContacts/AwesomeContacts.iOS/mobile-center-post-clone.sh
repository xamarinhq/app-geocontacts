#!/usr/bin/env bash

echo "Variables:"
printenv

echo "Arguments for updating:"
echo " - ACID: $AC_IOS"

# Updating manifest
sed -i '' "s/AC_IOS/$AC_IOS/g" $BUILD_REPOSITORY_LOCALPATH/src/AwesomeContacts/AwesomeContacts/Helpers/CommonConstants.cs

cat $BUILD_REPOSITORY_LOCALPATH/src/AwesomeContacts/AwesomeContacts/Helpers/CommonConstants.cs

echo "Manifest updated!"