#light

namespace Pencil.Unit

open System
open System.Diagnostics
open System.Collections.Generic

type IMatcher =
    abstract Match<'a> : 'a * 'a -> bool
    abstract Format<'a> : 'a * 'a -> string
    
type Error = { Location: string; Message: string }

type ITestResult =
    abstract Success : unit -> unit
    abstract Failiure : Error -> unit

[<AutoOpen>]
module Syntax =
    let (@@) (format:string) (arg0,arg1) = String.Format(format, arg0, arg1)
    let (|>) x f = let r = f x in r |> ignore; r //work-around for broken debug info.
    let mutable Count = 0
    let Errors = List<Error>()
    
    let mutable Result = {new ITestResult with
        member this.Success() =
            Console.Write('.')
        member this.Failiure e = 
            Console.Write('F')
            Errors.Add(e)}
            
    let Should (matcher:IMatcher) = fun e a ->
        Count <- Count + 1
        if matcher.Match(e,a) then
            Result.Success()
        else
            let frame = StackTrace(true).GetFrame(1)
            let trace = "{0}:{1}" @@ (frame.GetFileName(), frame.GetFileLineNumber())
            Result.Failiure({Location = trace; Message = matcher.Format(e,a)})

    let Equal = {new IMatcher with
            member x.Match (e, a) = a.Equals(e)
            member x.Format (e, a) = "Expected:{0}, Actual:{1}" @@ (e,a) }
