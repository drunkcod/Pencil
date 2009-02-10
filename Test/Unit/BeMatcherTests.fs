#light

open System
open Pencil.Unit
open Pencil.Unit.Suite

let Tests() =
    Suite [
        Fact "Should compare sequence by comparing items"(fun () ->
            (Be [1; 2; 3] (seq{1..3})).IsMatch |> Should Be true)

        Fact "Should compare sequence by comparing lenghts"(fun () ->
            (Be [1; 2] (seq{1..3})).IsMatch |> Should Be false)

]