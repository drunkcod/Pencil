#light

#r "Pencil.dll"

open System
open System.Drawing;
open System.IO
open Pencil.Core
open Pencil.NMeter

let factory = DotNodeFactory()
let digraph = DirectedGraph(factory)
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
            || t.Equals(typeof<System.Type>)
            || t.Equals(typeof<ValueType>)
            || t.Equals(typeof<Exception>)
            || t.Equals(typeof<IDisposable>)
            || t.Equals(typeof<System.Runtime.Serialization.ISerializable>)
            || t.Equals(typeof<System.Text.StringBuilder>)
            || t.Name = "List`1"
            || t.Name = "Stack`1"
            || t.Name = "ICollection`1"
            || t.Name = "IEnumerable`1"
            || t.Name = "IEnumerator"
            || t.Name = "IEnumerator`1"
            || t.Name = "IDictionary`2"
            || t.Name = "Dictionary`2"
            || t.Name = "KeyValuePair`2"
            || t.Name = "Action`1"
            || t.Name = "Converter`2"
            || t.Name = "Func`2"
            || t.Name = "Func`4"
            || t.Name = "_Exception")}

let dependencies = TypeDependencyGraph(digraph, ignore)


let mutable (currentNode:DotNode) = null
factory.NodeCreated.Add(fun e -> currentNode <- e.Item)

dependencies.NodeCreated.Add(fun e ->
    let name = e.Item.FullName
    if name <> null && name.StartsWith("System.") then
        currentNode.FillColor <- Color.FromArgb(200, 255, 200)
        currentNode.BorderColor <- Color.FromArgb(133, 196, 133)
        currentNode.FontColor <- Color.FromArgb(50, 64, 50))

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
dot.RankDirection <- RankDirection.LeftRight;

dot.Write(digraph)
