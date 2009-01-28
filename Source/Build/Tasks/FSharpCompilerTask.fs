#light

namespace Pencil.Build.Tasks

open System.Text
open Pencil.Build
open Pencil.IO

type FSharpCompilerTask(fileSystem:IFileSystem, platform:IExecutionEnvironment) =
    inherit ExecTaskBase(platform)

    let sources = FileSet()
    let references = FileSet()
    let mutable outputType = OutputType.Library
    let mutable output = Path.Empty
    let mutable binPath = Path.Empty;

    member this.Sources = sources

    member this.References = references

    member this.OutputType
        with get() = outputType
        and set v = outputType <- v

    member this.Output
        with get() = output
        and set v = output <- v

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
        this.References.CopyTo(fileSystem, this.Output.GetDirectory())
        let args = StringBuilder()
        if this.IsRunningOnMono then
            args.AppendFormat("{0} ", this.CompilerPath) |> ignore
        args.Append("--nologo") |> ignore
        this.Sources.ForEach(fun x -> args.AppendFormat(" {0}", x) |> ignore)
        this.References.ForEach(fun x -> args.AppendFormat(" -r {0}", x) |> ignore)
        args.AppendFormat(" -o {0}", this.Output) |> ignore
        args.AppendFormat(" {0}", this.ArgumentOutputType).ToString()

    member private this.ArgumentOutputType : string =
            match this.OutputType with
            | _ -> "-a"
