#light

open System
open Pencil.Unit

let Fact m f = fun (result:ITestResult) -> f result
    
let Theory m inputs f = fun (result:ITestResult) ->
    inputs |> Seq.fold (fun result x -> f x result) result

let Suite x = {new ISuite with member this.Tests = x }
    
let IntegerTests() =
    Suite [
        Fact "1+1 should equal 2"(
            1+1 |> Should Equal 2);

        Theory "Integer addition should work"
           [(1,1,2); (2,3,5);(3,7,10)]
            (fun (a, b, r) -> a + b |> Should Equal r)]
