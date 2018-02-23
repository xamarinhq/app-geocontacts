#!/usr/bin/env bash

echo "Variables:"
printenv

echo "Arguments for updating:"
echo " - ACID: $AC_IOS"
echo " - ACSECRET: $APP_SECRET"

# Updating manifest
sed -i '' "s/AC_IOS/$AC_IOS/g" $BUILD_REPOSITORY_LOCALPATH/src/AwesomeContacts/AwesomeContacts/Helpers/CommonConstants.cs
sed -i '' "s/APP_SECRET/$ACPP_SECRET/g" $BUILD_REPOSITORY_LOCALPATH/src/AwesomeContacts/AwesomeContacts.iOS/Info.plist

cat $BUILD_REPOSITORY_LOCALPATH/src/AwesomeContacts/AwesomeContacts/Helpers/CommonConstants.cs
cat $BUILD_REPOSITORY_LOCALPATH/src/AwesomeContacts/AwesomeContacts.iOS/Info.plist

echo "Manifest updated!"
