@echo off
color
@Tools\Pencil.Build.exe Pencil.cs -r:Tools\Pencil.Build.FSharpCompilerTask.dll %*
goto %ERRORLEVEL%
rem Fail!, paint it red.
:1
	color 4F
	goto done
rem Success, paint it green.
:0
	color 2F
:done