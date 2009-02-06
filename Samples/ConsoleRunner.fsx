#light

open System
open Pencil.Core
open Pencil.Unit
open Pencil.Unit.Suite

let inline Types (x:^src) = (^src:(member Types: ^a)(x))
let inline Methods (x:^src) = (^src:(member Methods: ^a)(x))

let result = TextWriterTestResult(Console.Out, DateTime.Now)
Console.WriteLine()

let target = "FactAndTheory.dll"//fsi.CommandLineArgs.[1]
AssemblyLoader.LoadFrom(target).Modules
|> Seq.map Types |> Seq.concat
|> Seq.filter (fun x -> x.IsPublic)
|> Seq.map Methods |> Seq.concat
|> Seq.filter IsSuite
|> Seq.map (fun x -> x.Invoke(null, null) :?> ISuite)
|> Seq.map (fun x -> x.Tests)
|> Seq.concat
|> Seq.fold (fun result t -> t result) (result :> ITestResult)
|> ignore

result.ShowReport()
