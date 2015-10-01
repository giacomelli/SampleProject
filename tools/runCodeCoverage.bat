del coverage.xml
rmdir coverage /S /Q

"..\src\packages\OpenCover.4.6.166\tools\OpenCover.Console.exe" -register:user  -output:coverage.xml -target:"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\MSTest.exe" -targetargs:"/testcontainer:\"%cd%\..\src\SampleProject.Infrastructure.Framework.UnitTests\bin\DEV\SampleProject.Infrastructure.Framework.UnitTests.dll\" /testcontainer:\"%cd%\..\src\SampleProject.Domain.UnitTests\bin\DEV\SampleProject.Domain.UnitTests.dll\"" -targetdir:"%cd%\..\src\SampleProject.Domain.UnitTests\bin\DEV" -filter:"+[*]SampleProject.* -[*.UnitTests]*"

"..\src\packages\ReportGenerator.2.3.2.0\tools\ReportGenerator.exe" coverage.xml ./coverage
