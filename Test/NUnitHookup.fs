#light
namespace Pencil.Unit

open System
open NUnit.Framework

[<AutoOpen>]
module NUnitHookup =
    let NUnitResult = {new ITestResult with
        member this.Begin test = this
        member this.Success() = this
        member this.Failiure e = 
            Assert.Fail(e)
            this}
    let Should m e a = Should m e a NUnitResult |> ignore

type NUnitFixtureAttribute() =
    inherit TestFixtureAttribute()
