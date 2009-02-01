#light

namespace Pencil.Test.Unit

open System
open Pencil.Unit
open NUnit.Framework

[<NUnitFixture>]
type SyntaxTests() =
    [<Test>]
    member this.Wont_should_use_MatchMessage() =
        (1 |> Wont Equal 1) {new ITestResult with
            member this.Success() = ()
            member this.Failiure e = (e.Message |> Should Equal "Actual was 1")} 
