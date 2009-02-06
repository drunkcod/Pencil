#light

open System
open System.Collections.Generic
open System.IO
open Pencil.Core
open Pencil.Unit

let inline Types (x:^src) = (^src:(member Types: ^a)(x))
let inline Methods (x:^src) = (^src:(member Methods: ^a)(x))

let IsSuite (m:IMethod) = m.ReturnType.Equals(typeof<ISuite>) && m.Arguments.Count = 0

type Error = {Test: string; Message: string}

type TextTestResult (target:TextWriter, started:DateTime) =
    let mutable test = ""
    let mutable count = 0
    let failures = List<Error>()
    interface ITestResult with
        member this.Begin t =
            test <- t
            this :> ITestResult

        member this.Success() =
            count <- count + 1
            target.Write('.')
            this :> ITestResult

        member this.Failiure e =
            count <- count + 1
            failures.Add {Test = test; Message = e}
            this :> ITestResult

    member this.ShowReport() =
        let time = DateTime.Now - started
        let (data:obj array) = [|
            box Environment.NewLine
            box count
            box failures.Count
            box time.TotalSeconds|]
        target.WriteLine("{0}Tests run: {1}, Failures: {2}, Time: {3:F3} seconds", data)
        failures |> Seq.iteri (fun n e -> target.WriteLine("    {0}) \"{1}\" failed with {2}.", n + 1, e.Test, e.Message))

let result = TextTestResult(Console.Out, DateTime.Now)
Console.WriteLine()

let target = "FactAndTheory.dll"//fsi.CommandLineArgs.[1]
AssemblyLoader.LoadFrom(target).Modules
|> Seq.map Types |> Seq.concat
|> Seq.filter (fun x -> x.IsPublic)
|> Seq.map Methods |> Seq.concat
|> Seq.filter IsSuite
|> Seq.map (fun x -> x.Invoke(null, null) :?> ISuite)
|> Seq.map (fun x -> x.Tests)
|> Seq.concat
|> Seq.fold (fun result t -> t result) (result :> ITestResult)
|> ignore

result.ShowReport()