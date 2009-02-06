#light

open System
open Pencil.Unit
open Pencil.Unit.Suite

let Tests() = 
    Suite [
        Fact "Contain should find substring"(
            (Contain "Bar" "FooBarBaz").IsMatch |> Should Be true)
        
        Fact "Contain should return false if expected not present"(
            (Contain "expected" "actual").IsMatch |> Should Be false)

        Fact "Format should have sensible message"(
            (Contain "expected" "actual").Message
            |> Should Be "\"actual\" doesn't contain \"expected\"")
]
