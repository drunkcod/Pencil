#light

namespace Pencil.Unit

open System
open System.IO
open Pencil.Core
open Pencil.Unit
open Pencil.Unit.Suite

type TextRunner(resultTarget:TextWriter, stopwatch:IStopwatch) =
    [<OverloadID("RunMultiple")>]
    member this.Run suites =
        let result = TextWriterTestResult(resultTarget, stopwatch)
        suites
        |> Seq.map_concat (fun (x:ISuite) -> x.Tests)
        |> Seq.fold (|>) (result :> ITestResult) |> ignore
        result.ShowReport()

    [<OverloadID("RunSingle")>]
    member this.Run(suite:ISuite) = this.Run (Seq.singleton suite)

module TextWriterRunner =
    let inline private Types (x:^src) = (^src:(member Types: ^a)(x))
    let inline private Methods (x:^src) = (^src:(member Methods: ^a)(x))
    let private GetStopwatch() =
        let start = DateTime.Now
        {new IStopwatch with member this.Elapsed = DateTime.Now - start}

    let Invoke (m:IMethod) =
        try
            m.Invoke(null, null) :?> ISuite
        with e ->
            Console.WriteLine(e.InnerException)
            {new ISuite with member this.Tests = []}

    let Run((testAssemblyPath:string),(resultTarget: TextWriter)) =
        let result = TextRunner(resultTarget, GetStopwatch())
        AssemblyLoader.LoadFrom(testAssemblyPath).Modules
        |> Seq.map_concat Types
        |> FilterMap (fun x -> x.IsPublic) Methods
        |> Seq.concat
        |> FilterMap IsSuite Invoke
        |> result.Run