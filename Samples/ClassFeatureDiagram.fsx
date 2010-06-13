#r "..\Build\Debug\Pencil.dll"

open System
open System.Drawing
open Pencil.Core
open Pencil.Dot

let loader = DefaultTypeLoader() :> ITypeLoader

let asMember (instruction:Instruction) =
    if instruction.IsMember then
        Some(instruction.Operand :?> IMember)
    else None

let dependencies : obj -> IMember seq = function
    | :? IMethod as m -> m.Body |> Seq.choose asMember|> Seq.distinct 
    | _ -> Seq.empty

let format : obj -> string= function
    | :? IField as x-> x.Name
    | :? IMethod as x -> 
        let args = String.Format("({0})", String.Join(", ", Seq.toArray ))
        x.Name + args
    | x -> x.ToString()

let sampleType = loader.FromNative(typeof<PencilMethod>)
let fields = sampleType.Fields
let methods = 
    let obj = loader.FromNative(typeof<obj>)
    sampleType.Methods |> Seq.filter (fun x -> x.DeclaringType <> obj)

Console.WriteLine "digraph { graph[overlap=false] node[fontname=Verdana,fontsize=9]"

let fieldStyle = DotNodeStyle(Shape = NodeShape.Box, FillColor = Color.LightBlue)
let methodStyle = DotNodeStyle(Shape = NodeShape.Oval)

let writeNodeStyles (style : DotNodeStyle) nodes =
    Console.WriteLine("  {{ node[{0}]", style)
    nodes |> Seq.iter (fun x -> Console.WriteLine("    \"{0}\"", format x))
    Console.WriteLine "  }"

writeNodeStyles fieldStyle fields
writeNodeStyles methodStyle methods

methods |> Seq.collect (fun m ->
    dependencies m
    |> Seq.filter (fun x -> x.DeclaringType.Equals(sampleType))
    |> Seq.map (fun x -> m,x))
|> Seq.iter (fun (m,x) -> Console.WriteLine("    \"{0}\" -> \"{1}\"", format m, format x))

Console.WriteLine "}"