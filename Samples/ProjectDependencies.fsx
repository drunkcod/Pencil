(* Find VisualStudio project dependencies *)
#light

open System
open System.Reflection
open System.Collections.Generic
open System.IO
open System.Xml
open System.Xml.XPath
open Pencil.Core

let MakeList (collection:IEnumerable<'a>) = List<'a>(collection) 

type VisualStudioProject =
    val mutable name : string
    val mutable id : Guid
    val mutable assemblyReferences : List<string>
    val mutable projectReferences : List<VisualStudioProject>
       
    static member Load (fileName:string) =
        let ResolvePath (x:string) = Path.Combine(Path.GetDirectoryName(fileName), x.Replace('\\', Path.DirectorySeparatorChar))
        VisualStudioProject(XPathDocument(fileName).CreateNavigator(), ResolvePath)
   
    member this.Name = this.name
    member this.Id = this.id
    member this.AssemblyReferences = this.assemblyReferences
    member this.ProjectReferences = this.projectReferences
        
    private new((xpath:XPathNavigator), resolve) =
        let ns = XmlNamespaceManager(xpath.NameTable)
        ns.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003")
        let SelectSingleValue query = xpath.SelectSingleNode(query, ns).Value
        let SelectList query f =
            let items = xpath.Select(query, ns)
            seq { while items.MoveNext() do yield f items.Current.Value } 
            |> MakeList
        { name = SelectSingleValue "//x:AssemblyName";
          id = Guid(SelectSingleValue "//x:ProjectGuid");
          assemblyReferences = SelectList "//x:ItemGroup/x:Reference/@Include" (fun x -> AssemblyName(x).Name)
          projectReferences = SelectList "//x:ItemGroup/x:ProjectReference/@Include" 
            (fun x -> VisualStudioProject.Load (resolve x))}

let root = ".."

let projects =
    Directory.GetFiles(root, "*.csproj", SearchOption.AllDirectories)
    |> Seq.map VisualStudioProject.Load

let projectLookup = Dictionary<string, VisualStudioProject>()

projects |> Seq.iter (fun x -> projectLookup.Add(x.Name, x))

type ProjectDependencyGraph = 
    inherit DependencyGraph<VisualStudioProject>
        override this.GetLabel item = item.Name 
        override this.GetDependencies item =
            item.AssemblyReferences
            |> Seq.map projectLookup.TryGetValue
            |> Seq.filter fst
            |> Seq.map snd
            |> Seq.append item.ProjectReferences            
        override this.ShouldAddCore x = projectLookup.ContainsKey(x.Name)
    new(graph) = 
        {inherit DependencyGraph<VisualStudioProject>(graph)}

let digraph = DirectedGraph()
let dependencies = ProjectDependencyGraph(digraph)      
projects |> Seq.iter dependencies.Add

let dot = DotBuilder(Console.Out)
dot.Write(digraph)
