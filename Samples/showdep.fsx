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

let digraph = DirectedGraph()
let loader = AssemblyLoader()
let dependencies = AssemblyDependencyGraph(digraph, loader, ignoreFilter)

let IsAssembly fileName =
    let ext = Path.GetExtension(fileName)
    ext = ".dll" || ext = ".exe"

Directory.GetFiles(".", "*.*")
|> Seq.filter IsAssembly
|> Seq.iter (AssemblyLoader.LoadFrom >> dependencies.Add)

DotBuilder(Console.Out).Write(digraph)