#light

namespace Pencil.Unit

open System
open System.IO
open Pencil.Core
open Pencil.Unit
open Pencil.Unit.Suite

type TextRunner(resultTarget:TextWriter, stopwatch:IStopwatch) = 
    member this.Run(suite:ISuite) = 
        let result = TextWriterTestResult(resultTarget, stopwatch)
        suite.Tests |> Seq.fold (fun result t -> t result) (result :> ITestResult) |> ignore       
        result.ShowReport()

module TextWriterRunner =
    let inline private Types (x:^src) = (^src:(member Types: ^a)(x))
    let inline private Methods (x:^src) = (^src:(member Methods: ^a)(x))

    let Invoke (m:IMethod) = 
        try
            m.Invoke(null, null) :?> ISuite
        with e -> 
            Console.WriteLine(e.InnerException.Message)
            {new ISuite with 
                member this.Tests = []}
        
    let Run((testAssemblyPath:string),(resultTarget: TextWriter)) = 
        let startTime = DateTime.Now
        let result = TextWriterTestResult(resultTarget, {new IStopwatch with
            member this.Elapsed = DateTime.Now - startTime})
        AssemblyLoader.LoadFrom(testAssemblyPath).Modules
        |> Seq.map Types |> Seq.concat
        |> Seq.filter (fun x -> x.IsPublic)
        |> Seq.map Methods |> Seq.concat
        |> Seq.filter IsSuite
        |> Seq.map Invoke
        |> Seq.map (fun x -> x.Tests)
        |> Seq.concat
        |> Seq.fold (fun result t -> t result) (result :> ITestResult)
        |> ignore
        
        result.ShowReport()
