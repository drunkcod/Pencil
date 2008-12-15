#light

#r "..\Build\Debug\Pencil.dll"

open System
open Pencil.Core

let handler = { 
    new obj() 
        interface IHandler with
            member self.BeginAssembly x = ()
            member self.EndAssembly() = ()
            member self.BeginModule x = ()
            member self.EndModule() = ()
            member self.BeginType x = ()
            member self.EndType() = ()
            member self.BeginMethod x = Console.WriteLine("{0}", x.Name)
            member self.EndMethod() = ()
 }
    
let reader = AssemblyReader(handler)

reader.Read(AssemblyLoader.LoadFrom("..\Build\Debug\Pencil.dll"))