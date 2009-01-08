namespace Pencil.Core
{
    using System.Collections.Generic;

    public class Node
    {
        List<Node> edges = new List<Node>();

        internal IEnumerable<Edge> Edges
        {
            get
            {
                foreach(var to in edges)
                    yield return new Edge(this, to);
            }
        }

        public string Label { get; set; }
		public virtual string Id { get { return Label; } }

        public void ConnectTo(Node to)
        {
            if(!edges.Contains(to))
                edges.Add(to);
        }

        internal bool IsEmpty { get { return Label.IsNullOrEmpty(); } }

    }
}