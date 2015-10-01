@echo ------------------------------------------
@echo Running FxCop...
@echo ------------------------------------------
@"C:\Program Files (x86)\Microsoft Fxcop 10.0\FxCopCmd.exe" /project:"..\SRC\SampleProject.FxCop" /out:fxcop-report.html /oXsl:FxCopRichConsoleOutput.xsl /applyoutXsl
@echo fxcop-report.xml file created.

@echo Done!
@pause
@exit