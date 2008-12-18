#light

#r "..\Build\Debug\Pencil.dll"

open System
open System.IO
open Pencil.Core
open Pencil.NMeter

let digraph = DirectedGraph()
let ignore = { new IFilter<IType> with
    member x.Include t =
        not (
            t.Equals(typeof<bool>)
            || t.Equals(typeof<int>)
            || t.Equals(typeof<string>)
            || t.Equals(typeof<Type>)
            || t.Name = "List`1")
}
let dependencies = TypeDependencyGraph(digraph, ignore)

let IsAssembly fileName =
    let ext = Path.GetExtension(fileName)
    ext = ".dll" || ext = ".exe"

fsi.CommandLineArgs
|> Seq.filter IsAssembly
|> Seq.iter (fun file ->
    AssemblyLoader.LoadFrom(file).Modules
    |> Seq.iter (fun x -> x.Types |> Seq.iter (dependencies.Add)))
DotBuilder(Console.Out).Write(digraph)