#r "..\Build\Pencil.dll"

open System
open System.IO
open Pencil.Core
open Pencil.Dot
open Pencil.NMeter

let SourceDirectory = Path.GetDirectoryName(fsi.CommandLineArgs.[0])

let ignoreFilter =
    let path = Path.Combine(SourceDirectory, "Ignore.xml")
    let configuration = XmlConfiguration.FromFile(path).Read<IgnoreFilterConfiguration>()
    IgnoreFilter.From(configuration)

let digraph = DirectedGraph(DotNodeFactory())
let loader = AssemblyLoader()
let dependencies = AssemblyDependencyGraph(digraph, loader, ignoreFilter)

let IsAssembly fileName =
    let ext = Path.GetExtension(fileName)
    ext = ".dll" || ext = ".exe"

AppDomain.CurrentDomain.add_AssemblyResolve(fun s e -> raise(new Exception()))

fsi.CommandLineArgs.[1] |> (loader.LoadFrom >> dependencies.Add)


let dot = DotBuilder(Console.Out)
dot.EmitGraphDefinition <- false
dot.Write(digraph)
