namespace Pencil.Core
{
    using System.Collections.Generic;

    public class Node
    {
        int id;
        List<Node> edges = new List<Node>();

        internal Node(int id)
        {
            this.id = id;
        }

        internal int Id { get { return id; } }
        internal IEnumerable<Edge> Edges
        {
            get
            {
                foreach(var to in edges)
                    yield return new Edge(this, to);
            }
        }

        public string Label { get; set; }

        public void ConnectTo(Node to) 
        {
            if(!edges.Contains(to))
                edges.Add(to); 
        }

        internal bool IsEmpty { get { return Label.IsNullOrEmpty(); } }

        public override string ToString()
        {
            return "{0}[label=\"{1}\"]".InvariantFormat(Id, Label);
        }
    }
}
