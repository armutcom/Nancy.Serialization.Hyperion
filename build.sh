#!/usr/bin/env bash

#exit if any command fails
set -e

export FrameworkPathOverride=$(dirname $(which mono))/../lib/mono/4.5.2-api/

dotnet restore Nancy.Serialization.Hyperion.sln

# Ideally we would use the 'dotnet test' command to test netcoreapp and net451 so restrict for now 
# but this currently doesn't work due to https://github.com/dotnet/cli/issues/3073 so restrict to netcoreapp
dotnet test ./tests/Nancy.Serialization.Hyperion.Tests -c Release -f netcoreapp3.0

# https://github.com/xunit/xunit/issues/1573
dotnet build ./tests/Nancy.Serialization.Hyperion.Tests -c Release -f net452
(cd ./tests/Nancy.Serialization.Hyperion.Tests && exec dotnet xunit -f net452)
