#light
#r "..\Build\Debug\Pencil.Unit.dll"

open Pencil.Unit

Fact "Pizza should have cheese." ("Pizza" |> Should (Contain "Cheese"))