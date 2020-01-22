#!/usr/bin/env bash

#exit if any command fails
set -e

export FrameworkPathOverride=$(dirname $(which mono))/../lib/mono/4.6.1-api/

mkdir testrunner
nuget install xunit.runner.console -Version 2.4.1 -OutputDirectory ./testrunner

# Ideally we would use the 'dotnet test' command to test netcoreapp and net451 so restrict for now 
# but this currently doesn't work due to https://github.com/dotnet/cli/issues/3073 so restrict to netcoreapp
echo Running netcoreapp3.1 tests
dotnet test ./tests/Nancy.Serialization.Hyperion.Tests -c Release -f netcoreapp3.1 --no-build
echo Running netcoreapp2.1 tests
dotnet test ./tests/Nancy.Serialization.Hyperion.Tests -c Release -f netcoreapp2.1 --no-build
echo Running net461 tests
mono ./testrunner/xunit.runner.console.2.4.1/tools/net461/xunit.console.exe ./tests/Nancy.Serialization.Hyperion.Tests/bin/Release/net461/Nancy.Serialization.Hyperion.Tests.dll

# # https://github.com/xunit/xunit/issues/1573
# dotnet build ./tests/Nancy.Serialization.Hyperion.Tests -c Release -f net461
# (cd ./tests/Nancy.Serialization.Hyperion.Tests && exec dotnet xunit -f net461)
