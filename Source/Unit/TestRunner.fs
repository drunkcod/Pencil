#light

namespace Pencil.Unit

open System
open Pencil.Core
open Pencil.Unit
open Pencil.Unit.Suite

type IStopwatch =
    abstract Elapsed : TimeSpan

type ITestRunner =
        abstract Run : seq<ISuite> -> ITestResult
    
type TestRunner(result:ITestResult) =
    static member NewDefaultStopwatch() =
        let start = DateTime.Now
        {new IStopwatch with member this.Elapsed = DateTime.Now - start}
            
    interface ITestRunner with
        member this.Run suites =
                suites
                |> Seq.map_concat (fun (x:ISuite) -> x.Tests)
                |> Seq.fold (|>) result
    member this.Run(suite:ISuite) = (this :> ITestRunner).Run (Seq.singleton suite)
