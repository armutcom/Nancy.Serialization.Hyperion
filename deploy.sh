#!/usr/bin/env bash

ApiKey=$1
Source=$2
BuildNumber=$3

echo $BuildNumber

if [ $TRAVIS_OS_NAME == 'linux' ]; then

(cd ./src/Nancy.Serialization.Hyperion && exec dotnet pack -c Release /p:TargetFramework=net452 /p:TargetFrameworks=net452 /p:BuildNumber=$3 /p:PackageVersion=1.0.1.$3 --verbosity normal)
(cd ./src/Nancy.Serialization.Hyperion && exec dotnet pack -c Release /p:TargetFramework=netstandard1.6 /p:TargetFrameworks=netstandard1.6 /p:BuildNumber=$3 /p:PackageVersion=2.0.1.$3-pre --verbosity normal)

(cd ./src/Nancy.Serialization.Hyperion/bin/Release && exec dotnet nuget push *.nupkg -k $ApiKey -s $Source)

fi