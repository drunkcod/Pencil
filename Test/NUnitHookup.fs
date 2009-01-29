#light
namespace Pencil.Unit

open System
open NUnit.Framework

Syntax.TestResult <- {new ITestResult with
    member this.Success() = ()
    member this.Failiure e = Assert.Fail(e.Message + " in " + e.Location)}

type NUnitFixtureAttribute() =
    inherit TestFixtureAttribute()
