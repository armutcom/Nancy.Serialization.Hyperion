#!/usr/bin/env bash

#exit if any command fails
set -e

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then  
  rm -R $artifactsFolder
fi

dotnet restore

# Ideally we would use the 'dotnet test' command to test netcoreapp and net451 so restrict for now 
# but this currently doesn't work due to https://github.com/dotnet/cli/issues/3073 so restrict to netcoreapp

dotnet test ./src/Nancy.Serialization.Hyperion.Tests -c Release -f netcoreapp2.0

# Instead, run directly with mono for the full .net version 
dotnet build ./src/Nancy.Serialization.Hyperion.Tests -c Release -f net452

mono \  
./src/Nancy.Serialization.Hyperion.Tests/bin/Release/net452/*/dotnet-test-xunit.exe \
./src/Nancy.Serialization.Hyperion.Tests/bin/Release/net452/*/Nancy.Serialization.Hyperion.Tests.dll

revision=${TRAVIS_JOB_ID:=1}  
revision=$(printf "%04d" $revision)

dotnet pack ./src/Nancy.Serialization.Hyperion -c Release -o ./artifacts --version-suffix=$revision  