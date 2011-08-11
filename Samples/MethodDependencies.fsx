#r "..\Build\Pencil.dll"
#r @"R:\Cint\main\build\bin\Cint.dll"
#r @"R:\Cint\main\build\bin\Cint.Tests.dll"

open System
open System.Text
open System.Collections.Generic
open Pencil.Core

type MyType() =
    let foo = DefaultTypeLoader()
    let bar = StringBuilder()

    member this.SayHello() = Console.WriteLine("Hello World!")

let loader = DefaultTypeLoader()

let shouldFollowMethod (m:IMethod) = 
    let fullName = m.DeclaringType.FullName
    not (["System."; "Rhino."] |> Seq.exists (fun x -> fullName.StartsWith(x)))

let printTrace (m:IMethod) =
    Console.WriteLine("{0}", m)
    let rec loop (prefix:string) (m:IMethod) =
        let prefix' = prefix + "\t"
        m.Body |> Seq.iter (fun il -> 
            if il.IsCall then
                let next = il.Operand :?> IMethod
                if il.Opcode = Opcode.FromName("newobj") && next.DeclaringType.FullName.StartsWith("Cint.") then
                    Console.WriteLine("{0}{1}", prefix', il)
                if shouldFollowMethod next then
                    loop prefix' next)
    loop "" m 

let x = typeof<Cint.Sites.Cpx.Controllers.OrderControllerBuilder>.GetConstructors().[0]

loader.FromNative(x)
|> printTrace