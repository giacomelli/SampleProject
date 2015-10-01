@echo ------------------------------------------
@echo Running FxCop...
@echo ------------------------------------------
rem @"C:\Program Files (x86)\Microsoft Fxcop 10.0\FxCopCmd.exe" /project:"..\SRC\SampleProject.FxCop" /out:fxcop-report.html /oXsl:FxCopReport.xsl /applyoutXsl
@"C:\Program Files (x86)\Microsoft Fxcop 10.0\FxCopCmd.exe" /project:"..\SRC\SampleProject.FxCop" /out:fxcop-report.xml
@echo fxcop-report.html file created.

DotBadge -b "FxCop:fxcop-report.xml" -output "FxCop-badge.svg"

@echo Done!
@pause
@exit