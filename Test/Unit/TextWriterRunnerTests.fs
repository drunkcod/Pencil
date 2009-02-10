#light

open System
open System.IO
open System.Text
open Pencil.Unit
open Pencil.Unit.Suite

let CaptureOutput f =
    let output = StringBuilder()
    let target = new StringWriter(output)
    f target
    output.ToString()

let NewRunner(target,stopwatch) = TestRunner(TextWriterTestResult(target, stopwatch))    

let Tests() =
    Suite [
        Fact "Should add 'F' for failed test"(fun () ->
            let result = CaptureOutput(fun target ->
                let runner = NewRunner(target, {new IStopwatch with
                    member this.Elapsed = TimeSpan.Zero})
                runner.Run({new ISuite with member this.Tests = [1 |> Should Be 2] }) |> ignore)
            result.Substring(0, 1) |> Should Be "F")
]
