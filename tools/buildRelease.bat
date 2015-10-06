@echo ------------------------------------------
@echo Building release...
@echo ------------------------------------------
call "C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat" x86

msbuild.exe ..\src\SampleProject.sln /p:DeployOnBuild=true /p:PublishProfile=PRD.pubxml

@echo Done!
@pause
@exit