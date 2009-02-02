#light

namespace Pencil.Unit

open System
open System.Text
open System.Diagnostics
open System.Collections.Generic

type IMatcher =
    abstract IsMatch : bool
    abstract Message : string
    abstract MatchMessage : string

type Error = { Message: string }

type ITestResult =
    abstract Success : unit -> unit
    abstract Failiure : Error -> unit

[<AutoOpen>]
module Syntax =
    let (@@) (format:string) (arg0,arg1) = String.Format(format, arg0, arg1)

    let private Format (s:seq<'a>) =
        let m =
            s |> Seq.fold
                (fun (m:StringBuilder) x -> m.AppendFormat("{0}; ", box x))
                (StringBuilder().Append('['))
        if m.Length > 1 then
            do m.Length <- m.Length - 2
        m.Append(']')

    let mutable TestResult = {new ITestResult with
        member this.Success() = ()
        member this.Failiure e = ()}

    let Fact m f = f {new ITestResult with
        member this.Success() = ()
        member this.Failiure e = Console.WriteLine("{0} Failed with {1}.", m, e.Message)}

    let Theory m inputs f =
        let failed = List<_>()
        let test (f, a) = f {new ITestResult with
            member this.Success() = ()
            member this.Failiure e = failed.Add(a)}

        inputs |> Seq.map (fun a -> (f a, a)) |> Seq.iter test
        if failed.Count > 0 then
            Console.WriteLine("{0}", box m)
            failed |> Seq.iter (fun x -> Console.WriteLine("\tfailed for {0}", box x))

    let Should m e a =
        let (m:IMatcher) = m e a
        fun (result:ITestResult) ->
            if m.IsMatch then
                result.Success()
            else
                result.Failiure({Message = m.Message})

    let Wont m e a =
        let (m:IMatcher) = m e a
        fun (result:ITestResult) ->
            if not m.IsMatch then
                result.Success()
            else
                result.Failiure({Message = m.MatchMessage})

    let Equal e a = {new IMatcher with
            member x.IsMatch = a.Equals(e)
            member x.Message = "Expected:{0}, Actual:{1}" @@ (e,a)
            member x.MatchMessage = String.Format("Actual was {0}", box a)}
    let Contain e a =
        match box e, box a with
        | (:? string as e),(:? string as a) -> {new IMatcher with
            member x.IsMatch = a.Contains(e)
            member x.Message = "\"{1}\" doesn't contain \"{0}\"" @@ (e,a)
            member x.MatchMessage = "\"{0}\" found in \"{1\"" @@ (e,a)}
        | _ -> {new IMatcher with
            member x.IsMatch = a |> Seq.exists e.Equals
            member x.Message = string (Format(a).AppendFormat(" doesn't contain {0}", box e))
            member x.MatchMessage = string (Format(a).AppendFormat(" contains {0}", box e))}
