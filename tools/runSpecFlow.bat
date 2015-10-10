@echo ------------------------------------------
@echo Running SpecFlow...
@echo ------------------------------------------

vstest.console.exe "..\src\SampleProject.Specs\bin\DEV\SampleProject.Specs.dll"       
BadgesSharp\BadgesSharpCmd.exe "https://github.com/giacomelli/SampleProject" SpecFlow %ERRORLEVEL%

@echo Done!
@pause
@exit