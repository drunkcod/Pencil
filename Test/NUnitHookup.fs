#light
namespace Pencil.Unit

open System
open NUnit.Framework

[<AutoOpen>]
module NUnitHookup =
    let NUnitResult = {new ITestResult with
        member this.Success() = ()
        member this.Failiure e = Assert.Fail(e.Message)}
    let Should m e a = Should m e a NUnitResult

type NUnitFixtureAttribute() =
    inherit TestFixtureAttribute()
