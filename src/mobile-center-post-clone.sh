#!/usr/bin/env bash

echo "Variables:"
printenv

echo "Arguments for updating:"
echo " - ACID: $AC_IOS"
echo " - ACSECRET: $APP_SECRET"
echo " - ACLogin: $AC_SHOWLOGIN"

# Updating manifest
sed -i '' "s/AC_IOS/$AC_IOS/g" $BUILD_REPOSITORY_LOCALPATH/src/AwesomeContacts/AwesomeContacts/Helpers/CommonConstants.cs
sed -i '' "s/APP_SECRET/$APP_SECRET/g" $BUILD_REPOSITORY_LOCALPATH/src/AwesomeContacts/AwesomeContacts.iOS/Info.plist

if [ "$APPCENTER_BRANCH" == "appstore" ]; then
  /usr/libexec/PlistBuddy -c "Set :CFBundleIdentifier com.microsoft.advocates" "$BUILD_REPOSITORY_LOCALPATH/src/AwesomeContacts/AwesomeContacts.iOS/Info.plist"
  sed -i '' "s/AC_SHOWLOGIN/$AC_SHOWLOGIN/g" $BUILD_REPOSITORY_LOCALPATH/src/AwesomeContacts/AwesomeContacts/Helpers/CommonConstants.cs
fi

cat $BUILD_REPOSITORY_LOCALPATH/src/AwesomeContacts/AwesomeContacts/Helpers/CommonConstants.cs
cat $BUILD_REPOSITORY_LOCALPATH/src/AwesomeContacts/AwesomeContacts.iOS/Info.plist

echo "Manifest updated!"