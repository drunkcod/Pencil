#light

open System
open Pencil.Core

let file = "FactAndTheory.exe"

let inline Types (x:^src) = (^src:(member Types: seq<_>)(x))

let Methods (t:IType) =
    t.Methods |> Seq.iter (fun x -> Console.WriteLine("    {0} {1}", x.ReturnType, x.Name))

AssemblyLoader.LoadFrom(file).Modules
|> Seq.map Types |> Seq.concat
|> Seq.iter (fun x -> Console.WriteLine(x); Methods x)
