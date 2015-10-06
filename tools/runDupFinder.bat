@echo ------------------------------------------
@echo Running DupFinder...
@echo ------------------------------------------

C:\ProgramData\chocolatey\lib\resharper-clt.portable\tools\dupfinder.exe /output=dupFinder-report.xml /show-text /exclude=**\*Test.cs;**\*.feature.cs;**\BundleConfig.cs ..\src\SampleProject.sln
rem BadgesSharp\BadgesSharpCmd.exe "https://github.com/giacomelli/SampleProject" DupFinder dupFinder-report.xml

@echo Done!
@pause
@exit