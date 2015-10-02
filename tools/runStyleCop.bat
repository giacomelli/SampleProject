@echo ------------------------------------------
@echo Running StyleCop...
@echo ------------------------------------------

StyleCopCmd\Net.SF.StyleCopCmd.Console\StyleCopCmd.exe -sf ..\src\SampleProject.sln -of stylecop-report.xml

@echo stylecop-report.html file created.

BadgesSharp\BadgesSharpCmd.exe "https://github.com/giacomelli/SampleProject" StyleCop "stylecop-report.violations.xml"

@echo Done!
@pause
@exit