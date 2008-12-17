#light

#r "..\Build\Debug\Pencil.dll"

open System
open System.IO
open Pencil.Core
open Pencil.NMeter

let digraph = DirectedGraph()
let dependencies = TypeDependencyGraph(digraph)

let IsAssembly fileName =
    let ext = Path.GetExtension(fileName)
    ext = ".dll" || ext = ".exe"

AssemblyLoader.LoadFrom("Pencil.Build.exe").Modules
|> Seq.iter (fun x -> x.Types |> Seq.iter (dependencies.Add))

DotBuilder(Console.Out).Write(digraph)