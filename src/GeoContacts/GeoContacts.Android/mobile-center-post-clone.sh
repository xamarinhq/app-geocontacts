#!/usr/bin/env bash

echo "Variables:"
printenv

echo "Arguments for updating:"
echo " - ACID: $AC_ANDROID"
echo " - ACLogin: $AC_SHOWLOGIN"
echo " - ACFace: $AC_FACE"
echo " - ACUseMSFT: $AC_USEMSFT"

# Updating manifest
sed -i '' "s/AC_ANDROID/$AC_ANDROID/g" $BUILD_REPOSITORY_LOCALPATH/src/GeoContacts/GeoContacts/Helpers/CommonConstants.cs

sed -i '' "s/AC_FACE/$AC_FACE/g" $BUILD_REPOSITORY_LOCALPATH/src/GeoContacts/GeoContacts/Helpers/CommonConstants.cs

sed -i '' "s/AC_USEMSFT_NO/$AC_USEMSFT/g" $BUILD_REPOSITORY_LOCALPATH/src/GeoContacts/GeoContacts/Helpers/CommonConstants.cs 

if [ "$APPCENTER_BRANCH" == "appstore" ]; then
  sed -i '' "s/AC_SHOWLOGIN/$AC_SHOWLOGIN/g" $BUILD_REPOSITORY_LOCALPATH/src/GeoContacts/GeoContacts/Helpers/CommonConstants.cs
fi


cat $BUILD_REPOSITORY_LOCALPATH/src/GeoContacts/GeoContacts/Helpers/CommonConstants.cs

echo "Manifest updated!"