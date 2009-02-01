#light

namespace Pencil.Test.Unit

open System
open Pencil.Unit
open NUnit.Framework

[<NUnitFixture>]
type ContainMatcherTests() =
    [<Test>]
    member this.Should_match_strings() =
        (Contain "Bar" "FooBarBaz").IsMatch |> Should Equal true

    [<Test>]
    member this.Match_should_return_false_if_not_present() =
        (Contain "expected" "actual").IsMatch |> Should Equal false

    [<Test>]
    member this.Format_should_have_sensible_message() =
        (Contain "expected" "actual").Message
        |> Should Equal "\"actual\" doesn't contain \"expected\""
