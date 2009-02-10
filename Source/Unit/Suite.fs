#light

namespace Pencil.Unit

open Pencil.Core

type ISuite =
    abstract Tests : (ITestResult -> ITestResult) list

module Suite =
    let Fact m f = fun (result:ITestResult) ->
        (f()) (result.Begin m)

    let Theory m inputs f = fun (result:ITestResult) ->
        inputs |> Seq.fold (fun result x -> f x result) (result.Begin m)

    let Suite x = {new ISuite with member this.Tests = x }

    let IsSuite (m:IMethod) = m.ReturnType.Equals(typeof<ISuite>) && m.Arguments.Count = 0
