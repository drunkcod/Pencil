#light

open System
open System.Collections.Generic
open System.IO
open Pencil.Core
open Pencil.Unit

let inline Types (x:^src) = (^src:(member Types: ^a)(x))
let inline Methods (x:^src) = (^src:(member Methods: ^a)(x))

AssemblyLoader.LoadFrom(fsi.CommandLineArgs.[1]).Modules
|> Seq.map Types |> Seq.concat
|> Seq.filter (fun x -> x.IsPublic)
|> Seq.iter (fun t ->
    Console.WriteLine(t.FullName)
    Methods t |> Seq.iter (fun m -> Console.WriteLine("    {0}", m)))

