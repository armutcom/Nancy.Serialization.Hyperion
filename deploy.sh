ApiKey=$1
Source=$2
BuildNumber$3

dotnet pack -c Release /p:TargetFramework=net452 /p:TargetFrameworks=net452 /p:BuildNumber=$3 --verbosity normal
dotnet pack -c Release /p:TargetFramework=netstandard1.6 /p:TargetFrameworks=netstandard1.6 /p:BuildNumber=$3 --verbosity normal

dotnet nuget push *.nupkg -k $ApiKey -s $Source