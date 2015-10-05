@echo ------------------------------------------
@echo Running SpecFlow...
@echo ------------------------------------------

vstest.console.exe "..\src\SampleProject.Specs\bin\DEV\SampleProject.Specs.dll"       

@echo Done!
@pause
@exit