@echo ------------------------------------------
@echo Running DupFinder...
@echo ------------------------------------------

C:\ProgramData\chocolatey\lib\resharper-clt.portable\tools\dupfinder.exe /output=dupFinderReport.xml /show-text /exclude=**\*Test.cs ..\src\SampleProject.sln
rem BadgesSharp\BadgesSharpCmd.exe "https://github.com/giacomelli/SampleProject" DupFinder dupFinderReport.xml

@echo Done!
@pause
@exit