#light

open System
open System.IO
open System.Text
open Pencil.Unit
open Pencil.Unit.Suite

let Tests() =
    Suite [
        Fact "Should write summary to target"(
            let output = StringBuilder()
            let target = new StringWriter(output)
            let runner = TextRunner(target, {new IStopwatch with 
                member this.Elapsed = TimeSpan.FromMilliseconds(1.0)})
            runner.Run({new ISuite with member this.Tests = [] })
            let expected = "Tests run: 0, Failures: 0, Time: 0.001 seconds"
            output.ToString().Trim() |> Should Be expected)
            
        Fact "Should add 'F' for failed test"(
            let output = StringBuilder()
            let target = new StringWriter(output)
            let runner = TextRunner(target, {new IStopwatch with 
                member this.Elapsed = TimeSpan.Zero})
            runner.Run({new ISuite with member this.Tests = [1 |> Should Be 2] }) 
            output.ToString().Substring(0, 1) |> Should Be "F")

]
