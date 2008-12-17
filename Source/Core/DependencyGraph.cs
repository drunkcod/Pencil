namespace Pencil.Core
{
	using System.Collections.Generic;

	public abstract class DependencyGraph<T>
	{
		DirectedGraph graph;
		Dictionary<string, Node> nodes = new Dictionary<string, Node>();


		public DependencyGraph(DirectedGraph graph)
		{
			this.graph = graph;
		}

		public void Add(T item)
		{
			if(ShouldAdd(item))
				AddChildren(CreateNode(item).Item, item);
		}

		protected Node AddNode(string label)
		{
			return graph.AddNode(label);
		}

		void AddChildren(Node current, T parent)
		{
			foreach(var item in GetDependencies(parent))
			{
				var node = CreateNode(item);
				current.ConnectTo(node.Item);
				if(node.Created && Recursive)
					AddChildren(node.Item, item);
			}
		}

		protected virtual bool Recursive { get { return true; } }
		protected abstract string GetLabel(T item);
		protected abstract IEnumerable<T> GetDependencies(T item);
		protected virtual bool ShouldAdd(T item){ return !nodes.ContainsKey(GetLabel(item)); }

		struct CreateResult
        {
            public bool Created;
            public Node Item;
        }

		CreateResult CreateNode(T item)
		{
            var result = new CreateResult();
			var label = GetLabel(item);
            result.Created = !nodes.TryGetValue(label, out result.Item);
            if(result.Created)
            {
                result.Item = AddNode(label);
                nodes.Add(label, result.Item);
            }
            return result;
		}
	}
}