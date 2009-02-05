#light

namespace Pencil.Test.Unit

open System
open Pencil.Unit
open NUnit.Framework

[<NUnitFixture>]
type SyntaxTests() =
    [<Test>]
    member this.Wont_should_use_MatchMessage() =
        (1 |> Wont Be 1) {new ITestResult with
            member this.Begin test = this
            member this.Success() = this
            member this.Failiure e = 
                e |> Should Be "Actual was 1"
                this} |> ignore
