#!/bin/bash


while true; do
    echo "================ Configuration ================"
    echo '1. Debug'
    echo '2. Release'
    echo 'q. Quit'
    echo ""
    read -p "Select configuration [1]: " input_result

    case $input_result in
        "1" ) CONFIGURATION="Debug"; break;;
        "2" ) CONFIGURATION="Release"; break;;
        "" ) CONFIGURATION="Debug"; break;;
        [Qq] ) exit;;
    esac
    echo ""
done
echo ""


while true; do
    echo "================ Platform ================"
    echo '1. iOS'
    echo '2. Android'
    echo 'q. Quit'
    echo ""
    read -p "Select platform [1]: " input_result

    case $input_result in
        "1" ) PLATFORM="ios"; break;;
        "2" ) PLATFORM="android"; break;;
        "" ) PLATFORM="ios"; break;;
        [Qq] ) exit;;
    esac
    echo ""
done
echo ""


while true; do
    echo "================ Deploy Type ================"
    echo '1. Simulator/Emulator'
    echo '2. Physical device'
    echo 'q. Quit'
    echo ""
    read -p "Select deploy type [1]: " input_result

    case $input_result in
        "1" ) DEPLOY_TYPE="SIM"; break;;
        "2" ) DEPLOY_TYPE="DEVICE"; break;;
        "" ) DEPLOY_TYPE="SIM"; break;;
        [Qq] ) exit;;
    esac
    echo ""
done
echo ""

RUNTIME=""

if [[ $PLATFORM == "ios" ]]
then
  if [[ $DEPLOY_TYPE == "SIM" ]]
  then
    # Get this udid from "/Applications/Xcode.app/Contents/Developer/usr/bin/simctl list"
    RUNTIME="/p:_DeviceName=:v2:udid=3B1A13DA-677D-42CF-88A0-5E21CD4CA1EC;__DeployToDevice=false"
  else # Physical device
    RUNTIME="/p:__DeployToDevice=true"
  fi
else # Android
  if [[ $DEPLOY_TYPE == "SIM" ]]
  then
    RUNTIME="/p:__DeployToDevice=false"
  else
    RUNTIME="/p:__DeployToDevice=true"
  fi
fi

echo "dotnet build ImageSharp.Benchmarks.MAUI -t:Run -f net6.0-$PLATFORM $RUNTIME -c $CONFIGURATION" 

dotnet build ImageSharp.Benchmarks.MAUI -t:Run -f net6.0-$PLATFORM $RUNTIME -c $CONFIGURATION

# _PlatformName