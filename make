#!/bin/bash
mono Tools/Pencil.Build.exe Pencil.cs -r:Tools/Pencil.dll -r:Tools/Pencil.Unit.dll -r:Tools/Pencil.Build.FSharpCompilerTask.dll $*
