#light

open Pencil.Unit
open Pencil.Unit.Suite

let Tests() =
    Suite [
        Fact "1+1 should equal 2"(
            1+1 |> Should Be 2)

        Fact "Failing test"(
            true |> Should Be false)

        Theory "Integer addition should work"
           [(1,1,2); (2,3,5);(3,7,10)]
           (fun (a, b, r) -> a + b |> Should Be r)

        Theory "Flawed theory"
            [("Pizza", "Cheese"); ("Team", "Me")]
            (fun (given, expected) -> given |> Should Contain expected)

        Fact "There's no I in Fail"("Fail" |> Wont Contain "I")
]