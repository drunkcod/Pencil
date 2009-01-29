#light

namespace Pencil.Test.Unit

open System
open Pencil.Unit
open NUnit.Framework

[<NUnitFixture>]
type ContainMatcherTests() =
    [<Test>]
    member this.Should_match_strings() =
        Contain.Match("Bar","FooBarBaz") |> Should Equal true

    [<Test>]
    member this.Match_should_return_false_if_not_present() =
        Contain.Match("expected", "actual") |> Should Equal false

    [<Test>]
    member this.Format_should_have_sensible_message() =
        Contain.Format("expected", "actual")
        |> Should Equal "\"actual\" doesn't contain \"expected\""
