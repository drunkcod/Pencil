#light

namespace Pencil.Test.Unit

open System
open Pencil.Unit
open NUnit.Framework

[<NUnitFixture>]
type ContainMatcherTests() =
    [<Test>]
    member this.Should_match_strings() =
        (Contain "Bar" "FooBarBaz").IsMatch |> Should Be true

    [<Test>]
    member this.Match_should_return_false_if_not_present() =
        (Contain "expected" "actual").IsMatch |> Should Be false

    [<Test>]
    member this.Format_should_have_sensible_message() =
        (Contain "expected" "actual").Message
        |> Should Be "\"actual\" doesn't contain \"expected\""
