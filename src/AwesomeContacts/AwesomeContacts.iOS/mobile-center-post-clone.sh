#!/usr/bin/env bash

echo "Variables:"
printenv

echo "Arguments for updating:"
echo " - ACID: $AC_IOS"
echo " - CURL: $CDC_URL"
echo " - CAUTH: $CDC_AUTH"

# Updating manifest
sed -i '' "s/AC_IOS/$AC_IOS/g" $BUILD_REPOSITORY_LOCALPATH/AwesomeContacts/AwesomeContacts/Helpers/CommonConstants.cs
sed -i '' "s/CDC_URL/$CDC_URL/g" $BUILD_REPOSITORY_LOCALPATH/AwesomeContacts/AwesomeContacts/Helpers/CommonConstants.cs
sed -i '' "s/CDC_AUTH/$CDC_AUTH/g" $BUILD_REPOSITORY_LOCALPATH/AwesomeContacts/AwesomeContacts/Helpers/CommonConstants.cs

cat $BUILD_REPOSITORY_LOCALPATH/AwesomeContacts/AwesomeContacts/Helpers/CommonConstants.cs

echo "Manifest updated!"