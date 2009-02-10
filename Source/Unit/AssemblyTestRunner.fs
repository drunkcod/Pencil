#light

namespace Pencil.Unit

open System
open Pencil.Core
open Pencil.Unit
open Pencil.Unit.Suite

module AssemblyTestRunner =
    let inline private Types (x:^src) = (^src:(member Types: ^a)(x))
    let inline private Methods (x:^src) = (^src:(member Methods: ^a)(x))
    let Invoke (m:IMethod) =
        try
            m.Invoke(null, null) :?> ISuite
        with e ->
            Console.WriteLine(e.InnerException)
            {new ISuite with member this.Tests = []}

    let Run (testAssemblyPath:string, testRunner:ITestRunner) =
        AssemblyLoader.LoadFrom(testAssemblyPath).Modules
        |> Seq.map_concat Types
        |> FilterMap (fun x -> x.IsPublic) Methods
        |> Seq.concat
        |> FilterMap IsSuite Invoke
        |> testRunner.Run
