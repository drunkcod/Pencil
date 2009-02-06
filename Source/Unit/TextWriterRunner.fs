#light

namespace Pencil.Unit

open System
open System.IO
open Pencil.Core
open Pencil.Unit
open Pencil.Unit.Suite

module TextWriterRunner =
    let inline private Types (x:^src) = (^src:(member Types: ^a)(x))
    let inline private Methods (x:^src) = (^src:(member Methods: ^a)(x))

    let Run((testAssemblyPath:string),(resultTarget: TextWriter)) = 
        let result = TextWriterTestResult(resultTarget, DateTime.Now)
        AssemblyLoader.LoadFrom(testAssemblyPath).Modules
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
