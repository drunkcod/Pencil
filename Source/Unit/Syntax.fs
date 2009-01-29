#light

namespace Pencil.Unit

open System
open System.Diagnostics
open System.Collections.Generic

type IMatcher<'a> =
    abstract Match : 'a * 'a -> bool
    abstract Format : 'a * 'a -> string

type Error = { Location: string; Message: string }

type ITestResult =
    abstract Success : unit -> unit
    abstract Failiure : Error -> unit

type DefaultTestResult() =
    let mutable Count = 0
    let Errors = List<Error>()

    let Tally (x:char) =
        Count <- Count + 1
        Console.Write(x)

    member this.ShowSummary() =
        Console.WriteLine("{2}{2}{0} tests run, {1} failed.", Count, Errors.Count, Environment.NewLine)
        if Errors.Count <> 0 then
            Errors |> Seq.iter (fun e -> Console.WriteLine("{0} {1}", e.Location, e.Message))

    interface ITestResult with
        member this.Success() =
            Tally '.'
        member this.Failiure e =
            Tally 'F'
            Errors.Add(e)

[<AutoOpen>]
module Syntax =
    let (@@) (format:string) (arg0,arg1) = String.Format(format, arg0, arg1)
    let (|>) x f = let r = f x in r |> ignore; r //work-around for broken debug info.

    let Result = DefaultTestResult()
    let mutable TestResult = Result :> ITestResult

    let Should (matcher:IMatcher<'a>) = fun e a ->
        if matcher.Match(e,a) then
            TestResult.Success()
        else
            let frame = StackTrace(true).GetFrame(1)
            let trace = "{0}:{1}" @@ (frame.GetFileName(), frame.GetFileLineNumber())
            TestResult.Failiure({Location = trace; Message = matcher.Format(e,a)})

    let Equal<'a> = {new IMatcher<'a> with
            member x.Match (e, a) = a.Equals(e)
            member x.Format (e, a) = "Expected:{0}, Actual:{1}" @@ (e,a)}

    let Contain = {new IMatcher<string> with
        member x.Match(e,a) = a.Contains(e)
        member x.Format(e,a) = "\"{1}\" doesn't contain \"{0}\"" @@ (e,a)}