#light

namespace Pencil.Unit

open System
open System.Collections.Generic
open System.IO

type private Error = {Test: string; Message: string}
type IStopwatch = 
    abstract Elapsed : TimeSpan

type TextWriterTestResult (target:TextWriter, stopwatch : IStopwatch) =
    let mutable test = ""
    let mutable count = 0
    let failures = List<Error>()
    interface ITestResult with
        member this.Begin t =
            test <- t
            this :> ITestResult

        member this.Success() =
            count <- count + 1
            target.Write('.')
            this :> ITestResult

        member this.Failiure e =
            count <- count + 1
            failures.Add {Test = test; Message = e}
            target.Write('F')
            this :> ITestResult

    member this.ShowReport() =
        let time = stopwatch.Elapsed
        let (data:obj array) = [|
            box Environment.NewLine
            box count
            box failures.Count
            box time.TotalSeconds|]
        target.WriteLine("{0}Tests run: {1}, Failures: {2}, Time: {3:F3} seconds", data)
        failures |> Seq.iteri (fun n e -> target.WriteLine("    {0}) \"{1}\" failed with {2}", n + 1, e.Test, e.Message))

