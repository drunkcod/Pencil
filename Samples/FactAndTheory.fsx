#light

open System
open Pencil.Unit

let Fact m f = fun result -> ()
let Theory m inputs f = fun result -> ()

let Suite x = fun () -> Console.WriteLine("Great Success")

module Samples =
    let IntegerTests() = 
        Suite [
            Fact "1+1 should equal 2"(
                1+1 |> Should Equal 2);
                
            Theory "Integer addition should work"
               [(1,1,2); (2,3,5);(3,7,10)]
                (fun (a, b, r) -> a + b |> Should Equal r)]
