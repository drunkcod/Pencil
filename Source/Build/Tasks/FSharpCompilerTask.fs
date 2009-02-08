#light

namespace Pencil.Build.Tasks

open System.Text
open Pencil.Build
open Pencil.Core
open Pencil.IO

type FSharpCompilerTask(fileSystem:IFileSystem, platform:IExecutionEnvironment) =
    inherit CompilerBaseTask(fileSystem, platform)

    let mutable outputType = OutputType.Library
    let mutable binPath = Path.Empty;
    let mutable optimize = false
    let mutable debug = false

    member this.OutputType
        with get() = outputType
        and set v = outputType <- v
        
    member this.Optimize
        with get() = optimize
        and set v = optimize <- v

    member this.Debug
        with get() = debug
        and set v = debug <- v

    member this.BinPath
        with get() = binPath
        and set v = binPath <- v

    member private this.CompilerPath = this.BinPath + "fsc.exe"

    override this.GetProgramCore() =
        if this.IsRunningOnMono then
            Path("mono")
        else
            this.CompilerPath
            
    override this.GetArgumentsCore() =
        this.References.CopyTo(this.Output.GetDirectory())
        let args = StringBuilder()
        let append (x:string) = args.Append(x) |> ignore
        and appendFormat format (arg:obj) = args.AppendFormat(format, arg) |> ignore
        
        if this.IsRunningOnMono then
            appendFormat "{0} " this.CompilerPath
        append("--nologo")

        if this.Optimize then
            append(" -O+")

        if this.Debug then
            append(" -g")

        this.Sources |> Iter (appendFormat " {0}")
        this.References |> Iter (appendFormat " -r {0}")
        args.AppendFormat(" -o {0} {1}", this.Output, this.ArgumentOutputType) 
        |> string
        
    member private this.ArgumentOutputType : string =
            match this.OutputType with
            | _ -> "-a"

