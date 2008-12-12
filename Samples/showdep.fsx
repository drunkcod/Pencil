#light

#r "..\Build\Debug\Pencil.dll"

open System
open System.IO
open Pencil.Core
open Pencil.NMeter

let ignoreFilter =
    let path = Path.Combine(__SOURCE_DIRECTORY__, "Ignore.xml")
    let configuration = IgnoreFilterConfiguration.FromFile(path)
    IgnoreFilter.From(configuration)

let IsAssembly fileName =
    let ext = Path.GetExtension(fileName)
    let file = Path.GetFileName(fileName)
    ext = ".dll" || ext = ".exe"

let digraph = DirectedGraph()
let loader = AssemblyLoader()
let dependencies = AssemblyDependencyGraph(digraph, loader, ignoreFilter)

Directory.GetFiles(".", "*.*")
|> Seq.filter IsAssembly
|> Seq.filter (fun x -> not (x.Contains("tests")))
|> Seq.iter (fun file -> dependencies.Add(loader.LoadFrom(file)))

Console.WriteLine(DotBuilder().ToString(digraph))