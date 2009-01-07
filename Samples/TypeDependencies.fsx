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
            || t.Equals(typeof<Byte>)
            || t.Equals(typeof<Int64>)
            || t.Equals(typeof<int>)
            || t.Equals(typeof<Single>)
            || t.Equals(typeof<Double>)
            || t.Equals(typeof<string>)
            || t.Equals(typeof<Type>)
            || t.Equals(typeof<ValueType>)
            || t.Name = "List`1"
            || t.Name = "IEnumerable`1"
            || t.Name = "IEnumerator"
            || t.Name = "IEnumerator`1"
            || t.Name = "Dictionary`2"
            || t.Name = "_Exception")}

let dependencies = TypeDependencyGraph(digraph, ignore)

let IsAssembly fileName =
    let ext = Path.GetExtension(fileName)
    ext = ".dll" || ext = ".exe"

fsi.CommandLineArgs
|> Seq.filter IsAssembly
|> Seq.iter (fun file ->
    AssemblyLoader.LoadFrom(file).Modules
    |> Seq.iter (fun x -> x.Types |> Seq.iter (dependencies.Add)))

let dot = DotBuilder(Console.Out)
dot.FontSize <- 8
dot.RankSeparation <- 0.07
dot.NodeSeparation <- 0.1
dot.NodeShape <- NodeShape.Box
dot.NodeHeight <- 0.1

dot.Write(digraph)