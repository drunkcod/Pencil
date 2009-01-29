#light

namespace Pencil.Test.Build.Tasks

open System
open System.IO
open Pencil.IO
open Pencil.Build
open Pencil.Build.Tasks
open NUnit.Framework
open Pencil.Unit
open Pencil.Test.Stubs


[<NUnitFixture>]
type FSharpCompilerTests() =

    let CheckArguments f =
        let environment = {new IExecutionEnvironment with
            member this.Run(program, arguments, handler) =
                f arguments
            member this.IsMono = false
            member this.StandardOut = TextWriter.Null}
        FSharpCompilerTask(FileSystemStub(), environment)

    [<Test>]
    member this.Arguments_should_include_nologo_switch() =
        let fsc = CheckArguments (Should Contain "--nologo")
        fsc.Execute()

    [<Test>]
    member this.Arguments_should_contain_references_assembly() =
        let fsc = CheckArguments (Should Contain "-r MyAssembly.dll")
        fsc.References.Add(Path("MyAssembly.dll"))
        fsc.Execute()

    [<Test>]
    member this.Should_support_Optimize_flag() =
        let fsc = CheckArguments (Should Contain "-O+")
        fsc.Optimize <- true
        fsc.Execute()

    [<Test>]
    member this.Should_support_Debug_flag() =
        let fsc = CheckArguments (Should Contain "-g")
        fsc.Debug <- true
        fsc.Execute()