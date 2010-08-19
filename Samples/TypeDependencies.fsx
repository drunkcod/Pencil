#light

#r "..\Build\Debug\Pencil.dll"

open System
open System.Drawing
open System.IO
open Pencil.Core
open Pencil.Dot
open Pencil.NMeter

open System.ComponentModel
open System.Web.UI
open System.Web.UI.WebControls

let factory = DotNodeFactory()
let digraph = DirectedGraph(factory)

let isPrimitiveType (t:IType) =
    [typeof<bool>; typeof<Byte>; typeof<int>; typeof<Int64>
    ;typeof<Single>; typeof<Double>
    ;typeof<string>
    ] |> Seq.exists t.Equals

let isComponentModelType (t:IType) =
    [typeof<IComponent>
    ] |> Seq.exists (fun x -> t.Equals(x))

let isAspNetType (t:IType) =
    let name = t.FullName
    name <> null && name.StartsWith("System.Web.UI.")

type System.String with
    member this.IsStartOf (s:string) = s <> null && s.StartsWith(this)

let ignore = { new IFilter<IType> with
    member x.Include t =
        not (
            isPrimitiveType t
            || t.Equals(typeof<System.Type>)
            || t.Equals(typeof<ValueType>)
            || t.Equals(typeof<IDisposable>)
            || t.Equals(typeof<System.Runtime.Serialization.ISerializable>)
            || t.Equals(typeof<System.Text.StringBuilder>)
            || t.Equals(typeof<EventArgs>)
            || t.Equals(typeof<EventHandler>)
            || t.Equals(typeof<Guid>)
            || t.Name = "Nullable`1"
            || "System.Collections.".IsStartOf(t.FullName)
            || t.IsA<Delegate>()
            || t.IsA<Exception>()
            || t.Name = "_Exception"
            || isComponentModelType t
            || isAspNetType t)}

let dependencies = TypeDependencyGraph(digraph, ignore)
let fxStyle =
    DotNodeStyle(
        FillColor = Color.FromArgb(200, 255, 200),
        BorderColor = Color.FromArgb(100, 164, 100),
        FontColor = Color.FromArgb(32, 96, 32))

let mutable (currentNode:DotNode) = null
factory.NodeCreated.Add(fun e -> currentNode <- e.Item)
dependencies.NodeCreated.Add(fun e ->
    if "System.".IsStartOf e.Item.FullName then
        currentNode.Style <- fxStyle)

let IsAssembly fileName =
    let ext = Path.GetExtension(fileName)
    ext = ".dll" || ext = ".exe"

let includeType (x:IType) = true

fsi.CommandLineArgs
|> Seq.filter IsAssembly
|> Seq.iter (fun file ->
    AssemblyLoader.LoadFrom(file).Modules
    |> Seq.iter (fun x -> x.Types |> Seq.filter includeType |> Seq.iter (dependencies.Add)))

let dot = DotBuilder(Console.Out)
dot.RankSeparation <- 0.07
dot.NodeSeparation <- 0.1
dot.RankDirection <- RankDirection.LeftRight
dot.NodeStyle <-
    DotNodeStyle(
        FontSize = 8,
        Shape = NodeShape.Box,
        Height = 0.1,
        FillColor = Color.FromArgb(200, 200, 255),
        BorderColor = Color.FromArgb(100, 100, 164),
        FontColor = Color.FromArgb(32, 32, 96))
dot.EdgeStyle <-
    DotEdgeStyle(
        ArrowSize=0.7,
        Color = Color.FromArgb(32, 32, 32),
        PenWidth = 0.75)

dot.Write(digraph)