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

Syntax.Result <- {new ITestResult with
    member this.Success() = ()
    member this.Failiure e = Assert.Fail(e.Message)}

[<TestFixture>]
type FSharpCompilerTests() =
    [<Test>]
    member this.Should_include_nologo_switch() =
        let platform = { new IExecutionEnvironment with
            member this.Run(program, arguments, handler) = 
                arguments.StartsWith("--nologo") |> Should Equal true
            member this.IsMono = false
            member this.StandardOut = TextWriter.Null
        }

        let fsc = FSharpCompilerTask(FileSystemStub(), platform)
        fsc.Execute()
