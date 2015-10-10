@echo ------------------------------------------
@echo Running FxCop...
@echo ------------------------------------------
@"C:\Program Files (x86)\Microsoft Fxcop 10.0\FxCopCmd.exe" /project:"..\SRC\SampleProject.FxCop" /out:fxcop-report.xml
@echo fxcop-report.html file created.

BadgesSharp\BadgesSharpCmd.exe "https://github.com/giacomelli/SampleProject" FxCop "fxcop-report.xml"

@echo Done!
@pause
@exit