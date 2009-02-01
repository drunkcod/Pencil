#light

namespace Pencil.Unit

open System
open System.Text
open System.Diagnostics
open System.Collections.Generic

type IMatcher =
    abstract IsMatch : bool
    abstract Message : string

type Error = { Message: string }

type ITestResult =
    abstract Success : unit -> unit
    abstract Failiure : Error -> unit

[<AutoOpen>]
module Syntax =
    let (@@) (format:string) (arg0,arg1) = String.Format(format, arg0, arg1)

    let mutable TestResult = {new ITestResult with
        member this.Success() = ()
        member this.Failiure e = ()}

    let mutable SkipFrames = 1

    let Fact m f = f  {new ITestResult with
        member this.Success() = ()
        member this.Failiure e = Console.WriteLine("{0} Failed with {1}.", m, e.Message)}

    let Should (e:'a -> IMatcher) a =
        fun (result:ITestResult) ->
            let matcher = e a
            if matcher.IsMatch then
                result.Success()
            else
                result.Failiure({Message = matcher.Message})

    let Equal e a = {new IMatcher with
            member x.IsMatch = a.Equals(e)
            member x.Message = "Expected:{0}, Actual:{1}" @@ (e,a)}

    let Contain e a = 
        match box e, box a with
        | (:? string as e),(:? string as a) -> {new IMatcher with
            member x.IsMatch = a.Contains(e)
            member x.Message = "\"{1}\" doesn't contain \"{0}\"" @@ (e,a)}
        | _ -> {new IMatcher with
            member x.IsMatch = a |> Seq.exists e.Equals
            member x.Message = 
                let message = StringBuilder()
                message.Append('[') |> ignore
                a |> Seq.iter (fun x -> message.AppendFormat("{0}; ", box x) |> ignore)
                message.Length <- message.Length - 2
                string (message.AppendFormat("] doesn't contain {0}", box e))}

