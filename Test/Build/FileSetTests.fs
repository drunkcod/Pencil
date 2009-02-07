#light

open System
open Pencil.IO
open Pencil.Build
open Pencil.Test.Stubs
open Pencil.Unit
open Pencil.Unit.Suite

let Test() = 
    Suite [
        Fact "ChangedAfter should return false if no file changed after given date"(
            let fs = FileSystemStub()
            let files = FileSet(fs)
            let lastChange = DateTime.Today
            files.Add(Path("MyFile.ext"))
            fs.GetLastWriteTimeHandler <- Converter(fun path -> lastChange) 
            files.ChangedAfter(lastChange.AddDays(1.0)) |> Should Be false)
            
        Fact "ChangedAfter should return true if modifed after given date"(
            let fs = FileSystemStub()
            let files = FileSet(fs)
            let lastChange = DateTime.Today
            files.Add(Path("MyFile.ext"))
            fs.GetLastWriteTimeHandler <- Converter(fun path -> lastChange) 
            files.ChangedAfter(lastChange.AddDays(-1.0)) |> Should Be true)
            
        Theory "Items should evaluate wildcard entries"
            [
                [Path("MyFile.fs")]
                [Path("A.fs"); Path("B.fs")]
            ]
            (fun expected -> 
                let fs = FileSystemStub()
                let files = FileSet(fs)
                files.Add(Path("*.fs")) 
                fs.GetFilesRecursiveHandler <- Func(fun path pattern -> expected |> List.to_seq)            
                files.Items |> Should Be expected)
]
