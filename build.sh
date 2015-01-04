nuget restore
xbuild
nuget install NUnit.Runners -Version 2.6.4 -o packages
mono --runtime=v4.5 packages/NUnit.Runners.2.6.4/tools/nunit-console.exe -noxml -nodots -labels -stoponerror ./tests/Tests/bin/Debug/Tests.dll