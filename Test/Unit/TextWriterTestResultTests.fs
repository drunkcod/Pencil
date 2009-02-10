#light

open System
open System.IO
open System.Text
open Pencil.Unit
open Pencil.Unit.Suite

let Tests() = 
    Suite [
        Fact "Should write summary to target"(fun () ->
            let output = StringBuilder()
            let target = new StringWriter(output)
            let result = TextWriterTestResult(target, {new IStopwatch with
                    member this.Elapsed = TimeSpan.FromMilliseconds(1.0)})
            result.ShowReport()
            let expected = "Tests run: 0, Failures: 0, Time: 0.001 seconds"
            (string output).Trim() |> Should Be expected)
]
