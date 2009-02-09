#light

open System
open System.IO
open Pencil.Build
open Pencil.Build.Tasks
open Pencil.IO
open Pencil.Unit
open Pencil.Unit.Suite
open Pencil.Test.Stubs

let NullEnvironment = {new IExecutionEnvironment with
                member this.Run(program, arguments, handler) = ()
                member this.IsMono = false
                member this.StandardOut = TextWriter.Null}

type TestCompiler =
    class
        inherit CompilerBaseTask
        val mutable hasCompiled : bool
        new(fileSystem:IFileSystem) = {
            inherit CompilerBaseTask(fileSystem, NullEnvironment);
            hasCompiled = false }

        override this.GetProgramCore() = Path("TestCompiler")
        override this.GetArgumentsCore() = ""
        override this.CompileCore() = this.hasCompiled <- true
        member this.HasCompiled = this.hasCompiled
    end

let Tests() =
    Suite [
        Fact "Execute should Compile if Sources have changed"(
            let fileSystem = FileSystemStub()
            let compiler = TestCompiler(fileSystem)
            compiler.Sources.Add(Path("MyThingy.fs"))
            compiler.Output <- Path("MyOutput.dll")
            let changeTime = DateTime.Now
            fileSystem.GetLastWriteTimeHandler <- Converter(fun path ->
                if path = compiler.Output then
                    changeTime.AddMinutes(-1.0)
                else changeTime)
            compiler.Compile()
            compiler.HasCompiled |> Should Be true)

        Fact "Execute shouldn't Compile if no changes to source"(
            let fileSystem = FileSystemStub()
            let compiler = TestCompiler(fileSystem)
            compiler.Sources.Add(Path("MyThingy.fs"))
            compiler.Output <- Path("MyOutput.dll")
            let changeTime = DateTime.Now
            fileSystem.GetLastWriteTimeHandler <- Converter(fun path ->
                if path = compiler.Output then
                    changeTime.AddMinutes(1.0)
                else changeTime)
            compiler.Compile()
            compiler.HasCompiled |> Should Be false)

        Fact "Execute should Compile if References have changed"(
            let fileSystem = FileSystemStub()
            let compiler = TestCompiler(fileSystem)
            compiler.References.Add(Path("Pencil.dll"))
            compiler.Output <- Path("MyOutput.dll")
            let changeTime = DateTime.Now
            fileSystem.GetLastWriteTimeHandler <- Converter(fun path ->
                if path = compiler.Output then
                    changeTime.AddMinutes(-1.0)
                else changeTime)
            compiler.Compile()
            compiler.HasCompiled |> Should Be true)

]
