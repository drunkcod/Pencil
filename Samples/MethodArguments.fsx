#light

#r "..\Build\Debug\Pencil.dll"

open System
open Pencil.Core

type CustomHandler() =
    inherit DefaultHandler()
    member x.InUserMethod =
        let m = x.Method
        not (x.Type.IsGenerated || m.IsSpecialName || m.IsGenerated) && m.DeclaringType = x.Type

    override x.BeginTypeCore() =
        if not x.Type.IsGenerated then
            Console.WriteLine("{0}", x.Type.Name)

    override x.BeginMethodCore() =
        let m = x.Method;
        if x.InUserMethod then
            Console.WriteLine("\t{0}({1})", m.Name, m.Arguments.Count)

AssemblyReader(CustomHandler()).Read(AssemblyLoader.LoadFrom("..\Build\Debug\Pencil.dll"))