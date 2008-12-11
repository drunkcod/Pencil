#light

#r "..\Build\Debug\Pencil.dll"

open System
open System.IO
open System.Reflection
open System.Xml.Serialization
open Pencil.Core
open Pencil.NMeter

let includeInGraph =
    let path = Path.Combine(__SOURCE_DIRECTORY__, "Ignore.xml")
    let configuration = XmlSerializer(typeof<IgnoreFilterConfiguration>).Deserialize(File.OpenRead(path)) :?> IgnoreFilterConfiguration
    IgnoreFilter.From(configuration).Include

let isWpfAssembly name =
    name = "WindowsBase"
    || name = "PresentationCore"
    || name = "PresentationFramework"
    || name = "WindowsFormsIntegration"

let IsAssembly fileName =
    let ext = Path.GetExtension(fileName)
    let file = Path.GetFileName(fileName)
    ext = ".dll" || ext = ".exe"

let digraph = new DirectedGraph()
let loader = AssemblyLoader()
let dependencies = new AssemblyDependencyGraph(digraph, loader, Predicate(includeInGraph))

Directory.GetFiles(".", "*.*")
|> Seq.filter IsAssembly
|> Seq.filter (fun x -> not (x.Contains("tests")))
|> Seq.iter (fun file -> dependencies.Add(loader.LoadFrom(file)))

Console.WriteLine((new DotBuilder()).ToString(digraph))
