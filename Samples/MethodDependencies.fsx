#r "..\Build\Pencil.dll"

open System
open System.Text
open System.Collections.Generic
open Pencil.Core

type MyType() =
    let foo = List()
    let bar = StringBuilder()

    member this.SayHello() = Console.WriteLine("Hello World!")

let loader = DefaultTypeLoader()

let shouldFollowMethod (m:IMethod) = not(m.DeclaringType.FullName.StartsWith("System."))

let printTrace (m:IMethod) =
    let rec loop (prefix:string) (m:IMethod) =
        Console.WriteLine("{0}{1}", prefix, m)
        System.Threading.Thread.Sleep(250)
        let prefix' = prefix + "\t"
        m.Body |> Seq.iter (fun il -> 
            Console.WriteLine("{0}{1}", prefix', il)
            if il.IsCall then
                let next = il.Operand :?> IMethod
                if shouldFollowMethod next then
                    loop prefix' next)
    loop "" m 

let x = typeof<MyType>.GetConstructors().[0]

x.GetMethodBody().GetILAsByteArray()
|> Seq.iter (fun x -> Console.WriteLine("{0:x2}", x))

loader.FromNative(x)
|> printTrace