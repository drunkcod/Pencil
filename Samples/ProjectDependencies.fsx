(* Find VisualStudio project dependencies *)
#r "..\Build\Pencil.dll"

open System
open System.Reflection
open System.Collections.Generic
open System.IO
open System.Xml
open System.Xml.XPath
open Pencil.Core
open Pencil.Dot

let MakeList (collection:IEnumerable<'a>) = List<'a>(collection) 

let tryMap f error =
    fun x ->
        try Some(f x)
        with ex ->
            error x
            None

type VisualStudioProject(fileName) =
    static let loadedProjects = Dictionary<string, VisualStudioProject>(StringComparer.InvariantCultureIgnoreCase)
    
    let xpath = XPathDocument(fileName : string).CreateNavigator()
    let ns =  XmlNamespaceManager(xpath.NameTable)
    do ns.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003")
    
    static member Load (fileName:string) =
        let fileName = Path.GetFullPath(fileName)
        match loadedProjects.TryGetValue(fileName) with
        | (true, project) -> project
        | (false, _) ->
            let project = VisualStudioProject(fileName)
            loadedProjects.Add(fileName, project)
            project

    member private this.Select query map =
        let items = xpath.Select(query, ns)
        seq { while items.MoveNext() do 
                yield map items.Current.Value }
   
    member this.AssemblyName = this.SelectSingleValue "//x:AssemblyName"
    member this.Name = fileName
    member this.Id = Guid(this.SelectSingleValue "//x:ProjectGuid" : string)
    member this.AssemblyReferences = this.Select "//x:ItemGroup/x:Reference/@Include" (fun x -> AssemblyName(x).Name)

    member this.ProjectReferences = 
        let tryLoad = tryMap (fun x -> VisualStudioProject.Load(this.Resolve x)) (fun x -> Console.Error.WriteLine("WARNING: {0} references missing project {1}", fileName, x))
        this.Select "//x:ItemGroup/x:ProjectReference/@Include" tryLoad
        |> Seq.choose id

    member private this.SelectSingleValue query = xpath.SelectSingleNode(query, ns).Value

    member private this.Resolve path = Path.Combine(Path.GetDirectoryName(fileName), path.Replace('\\', Path.DirectorySeparatorChar))

let root = fsi.CommandLineArgs.[1]

let projects =
    Directory.GetFiles(root, "*.??proj", SearchOption.AllDirectories)
    |> Seq.filter (fun x -> x.EndsWith("csproj") || x.EndsWith("vbproj"))
    |> Seq.map VisualStudioProject.Load

let projectLookup = Dictionary<string, VisualStudioProject>(StringComparer.InvariantCultureIgnoreCase)

projects |> Seq.iter (fun x -> projectLookup.Add(x.AssemblyName, x))

type ProjectDependencyGraph = 
    inherit DependencyGraph<VisualStudioProject>
        override this.GetLabel item = Path.GetFileNameWithoutExtension(item.Name) 
        override this.GetDependencies item =
            item.AssemblyReferences
            |> Seq.map projectLookup.TryGetValue
            |> Seq.filter fst
            |> Seq.map snd
            |> Seq.append item.ProjectReferences            
        override this.ShouldAddCore x = projectLookup.ContainsKey(x.AssemblyName)
    new(graph) = {inherit DependencyGraph<VisualStudioProject>(graph)}

let digraph = DirectedGraph(DotNodeFactory())
let dependencies = ProjectDependencyGraph(digraph)      
projects |> Seq.iter dependencies.Add

let dot = DotBuilder(Console.Out)
dot.RankDirection <- RankDirection.LeftRight
dot.NodeStyle <- DotNodeStyle(FontSize = 9, Shape = NodeShape.Box)
dot.EmitDigraphDefinition <- false
dot.Write(digraph)