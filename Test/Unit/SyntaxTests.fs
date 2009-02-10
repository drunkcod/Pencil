#light

open System
open Pencil.Unit
open Pencil.Unit.Suite

let Tests()  =
    Suite [
        Fact "Wont should use MatchMessage"(fun () ->
            fun result ->
            (1 |> Wont Be 1) {new ITestResult with
                member this.Begin test = this
                member this.Success() = this
                member this.Failiure e =
                    (e |> Should Be "Actual was 1") result })
]