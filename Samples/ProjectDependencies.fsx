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
        try
            Some(f x)
        with ex ->
            error x
            None

type VisualStudioProject =
    val mutable fileName : string
    val mutable assemblyName : string
    val mutable id : Guid
    val mutable assemblyReferences : List<string>
    val mutable projectReferences : List<VisualStudioProject>
       
    static member Load (fileName:string) =
        let ResolvePath (x:string) = Path.Combine(Path.GetDirectoryName(fileName), x.Replace('\\', Path.DirectorySeparatorChar))
        VisualStudioProject(fileName, XPathDocument(fileName).CreateNavigator(), ResolvePath)
   
    member this.AssemblyName = this.assemblyName
    member this.Name = this.fileName
    member this.Id = this.id
    member this.AssemblyReferences = this.assemblyReferences
    member this.ProjectReferences = this.projectReferences
        
    private new(fileName, (xpath:XPathNavigator), resolve) =
        let ns = XmlNamespaceManager(xpath.NameTable)
        ns.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003")
        let SelectSingleValue query = xpath.SelectSingleNode(query, ns).Value
        let SelectList query f =
            let items = xpath.Select(query, ns)
            let tryMap = tryMap f (fun x -> Console.Error.WriteLine("WARNING: {0} references missing project {1}", fileName, x))
            seq { 
                while items.MoveNext() do 
                    match tryMap items.Current.Value with
                    | None -> ()
                    | Some(x) -> yield x }
            |> MakeList
        { assemblyName = SelectSingleValue "//x:AssemblyName"
          fileName = fileName
          id = Guid(SelectSingleValue "//x:ProjectGuid")
          assemblyReferences = SelectList "//x:ItemGroup/x:Reference/@Include" (fun x -> AssemblyName(x).Name)
          projectReferences = SelectList "//x:ItemGroup/x:ProjectReference/@Include" (fun x -> VisualStudioProject.Load (resolve x))}

let root = fsi.CommandLineArgs.[1]

let projects =
    Directory.GetFiles(root, "*.*proj", SearchOption.AllDirectories)
    |> Seq.filter (fun x -> x.EndsWith("csproj") || x.EndsWith("vbproj"))
    |> Seq.map VisualStudioProject.Load

let projectLookup = Dictionary<string, VisualStudioProject>()

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
    new(graph) = 
        {inherit DependencyGraph<VisualStudioProject>(graph)}

let digraph = DirectedGraph(DotNodeFactory())
let dependencies = ProjectDependencyGraph(digraph)      
projects |> Seq.iter dependencies.Add

let dot = DotBuilder(Console.Out)
dot.RankDirection <- RankDirection.LeftRight
dot.NodeStyle <- DotNodeStyle(FontSize = 9, Shape = NodeShape.Box)
//dot.EmitDigraphDefinition <- false
dot.Write(digraph)
