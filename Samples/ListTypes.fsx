#light

open System
open Pencil.Core
open Pencil.Unit

let file = "FactAndTheory.dll"

let inline Types (x:^src) = (^src:(member Types: seq<_>)(x))
let inline Methods (x:^src) = (^src:(member Methods: seq<_>)(x))

AssemblyLoader.LoadFrom(file).Modules
|> Seq.map Types |> Seq.concat
|> Seq.filter (fun x -> x.IsPublic)
|> Seq.map Methods |> Seq.concat
|> Seq.filter (fun x -> x.ReturnType.Name.Equals("ISuite"))
|> Seq.iter (fun x -> Console.WriteLine(x))
