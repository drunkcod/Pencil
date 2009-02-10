#light

open System
open System.IO
open Pencil.IO
open Pencil.Build
open Pencil.Build.Tasks
open Pencil.Test.Stubs
open Pencil.Unit
open Pencil.Unit.Suite

let GetCompiler() =
    let args = ref ""
    let environment = {new IExecutionEnvironment with
        member this.Run(program, arguments, handler) = args := arguments
        member this.IsMono = false
        member this.StandardOut = TextWriter.Null}
    args, FSharpCompilerTask(FileSystemStub(), environment)

let Tests() =
    Suite [
        Fact "Arguments should include nologo"(fun () ->
            let args, fsc = GetCompiler()
            fsc.Execute()
            !args |> Should Contain "--nologo")

        Fact "Arguments should contain referenced assemby"(fun () ->
            let args, fsc = GetCompiler()
            fsc.References.Add(Path("MyAssembly.dll")) |> ignore
            fsc.Execute()
            !args |> Should Contain "-r MyAssembly.dll")

        Fact "Should support Optimize flag"(fun () ->
            let args, fsc = GetCompiler()
            fsc.Optimize <- true
            fsc.Execute()
            !args |> Should Contain "-O+")

        Fact "Should support Debug flag"(fun () ->
            let args, fsc = GetCompiler()
            fsc.Debug <- true
            fsc.Execute()
            !args |> Should Contain "-g")
]