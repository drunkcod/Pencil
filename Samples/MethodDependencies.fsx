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

let trace (m:IMethod) =
    Console.WriteLine("{0}", m)
    let rec loop (prefix:string) (m:IMethod) =
        let prefix' = prefix + "\t"
        m.Body |> Seq.collect (fun il -> seq {
            if il.IsCall then
                let next = il.Operand :?> IMethod
                if il.Opcode = Opcode.FromName("newobj") && next.DeclaringType.FullName.StartsWith("Cint.") then
                    yield il
                if shouldFollowMethod next then
                    yield! loop prefix' next })
    loop "" m 

let x = typeof<Cint.Sites.Cpx.Controllers.OrderControllerBuilder>.GetConstructors().[0]

loader.FromNative(x)
|> trace
|> Seq.countBy string
|> Seq.sortBy snd
|> Seq.iter (fun (il, count) -> Console.WriteLine("#{0} {1}", count, il))