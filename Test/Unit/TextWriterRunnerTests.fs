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
    
let Tests() =
    Suite [
        Fact "Should write summary to target"(
            let result = CaptureOutput(fun target ->
                let runner = TextRunner(target, {new IStopwatch with 
                    member this.Elapsed = TimeSpan.FromMilliseconds(1.0)})
                runner.Run({new ISuite with member this.Tests = [] }))
            let expected = "Tests run: 0, Failures: 0, Time: 0.001 seconds"
            result.Trim() |> Should Be expected)
            
        Fact "Should add 'F' for failed test"(
            let result = CaptureOutput(fun target ->
                let runner = TextRunner(target, {new IStopwatch with 
                    member this.Elapsed = TimeSpan.Zero})
                runner.Run({new ISuite with member this.Tests = [1 |> Should Be 2] }))
            result.Substring(0, 1) |> Should Be "F")
]
