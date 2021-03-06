namespace Pencil.Core
{
	using System;
	using System.Collections.Generic;

	public abstract class DependencyGraph<T>
	{
		DirectedGraph graph;
		Dictionary<string, Node> nodes = new Dictionary<string, Node>();


		public DependencyGraph(DirectedGraph graph)
		{
			this.graph = graph;
		}

		public event EventHandler<NodeCreatedEventArgs<T>> NodeCreated;

		public void Add(T item)
		{
			if(ShouldAdd(item))
				AddChildren(CreateNode(item).Item, item);
		}

		void AddChildren(Node current, T parent)
		{
			foreach(var item in GetDependencies(parent))
				if(ShouldAdd(item))
				{
					var node = CreateNode(item);
                    graph.Connect(current, node.Item);
                    if(node.Created && Recursive)
						AddChildren(node.Item, item);
				}
		}

		bool ShouldAdd(T item){ return ShouldAddCore(item); }

		protected virtual bool Recursive { get { return true; } }
		protected abstract string GetLabel(T item);
		protected virtual string GetId(T item){ return GetLabel(item); }
		protected abstract IEnumerable<T> GetDependencies(T item);
		protected abstract bool ShouldAddCore(T item);

		struct CreateResult
        {
            public bool Created;
            public Node Item;
        }

		CreateResult CreateNode(T item)
		{
            var result = new CreateResult();
			var label = GetLabel(item);
			var id = GetId(item);
            result.Created = !nodes.TryGetValue(id, out result.Item);
            if(result.Created)
            {
                result.Item = AddNode(label);
				var tmp = NodeCreated;
				if(tmp != null)
					tmp(this, new NodeCreatedEventArgs<T>(item));
                nodes.Add(id, result.Item);
            }
            return result;
		}

		Node AddNode(string label)
		{
			return graph.AddNode(label);
		}
	}
}