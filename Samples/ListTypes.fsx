#light

open System
open Pencil.Core
open Pencil.Unit

let file = "FactAndTheory.dll"

let inline Types (x:^src) = (^src:(member Types: ^a)(x))
let inline Methods (x:^src) = (^src:(member Methods: ^a)(x))

let IsSuite (m:IMethod) = m.ReturnType.Equals(typeof<ISuite>) && m.Arguments.Count = 0

let result = {new ITestResult with
        member this.Success() = 
            Console.Write(".")
            this
        member this.Failiure e = this}

AssemblyLoader.LoadFrom(file).Modules
|> Seq.map Types |> Seq.concat
|> Seq.filter (fun x -> x.IsPublic)
|> Seq.map Methods |> Seq.concat
|> Seq.filter IsSuite
|> Seq.map (fun x -> x.Invoke(null, null) :?> ISuite)
|> Seq.map (fun x -> x.Tests)
|> Seq.concat
|> Seq.fold (fun result t -> t result) result
|> ignore
