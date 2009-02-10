#light

open System
open Pencil.IO
open Pencil.Build
open Pencil.Test.Stubs
open Pencil.Unit
open Pencil.Unit.Suite

let Test() =
    Suite [
        Fact "ChangedAfter should return false if no file changed after given date"(fun () ->
            let fs = FileSystemStub()
            let files = FileSet(fs).Add(Path("MyFile.ext"))
            let lastChange = DateTime.Today
            fs.GetLastWriteTimeHandler <- Converter(fun path -> lastChange)
            files.ChangedAfter(lastChange.AddDays(1.0)) |> Should Be false)

        Fact "ChangedAfter should return true if modifed after given date"(fun () ->
            let fs = FileSystemStub()
            let files = FileSet(fs).Add(Path("MyFile.ext"))
            let lastChange = DateTime.Today
            fs.GetLastWriteTimeHandler <- Converter(fun path -> lastChange)
            files.ChangedAfter(lastChange.AddDays(-1.0)) |> Should Be true)

        Fact "ChangedAfter with path should query file system"(fun () ->
            let fs = FileSystemStub()
            let files = FileSet(fs).Add(Path("MyFile.ext"))
            let didQueryFileSystem = ref false
            fs.GetLastWriteTimeHandler <- Converter(fun path ->
                didQueryFileSystem := !didQueryFileSystem || (path = Path("SomeOtherFile"))
                DateTime.Today)
            files.ChangedAfter(Path("SomeOtherFile")) |> ignore
            !didQueryFileSystem |> Should Be true)

        Theory "Items should evaluate wildcard entries"
            [
                [Path("MyFile.fs")]
                [Path("A.fs"); Path("B.fs")]
            ]
            (fun expected ->
                let fs = FileSystemStub()
                let files = FileSet(fs).Add(Path("*.fs"))
                fs.GetFilesRecursiveHandler <- Func(fun path pattern -> expected |> List.to_seq)
                files.Items |> Should Be expected)
]