namespace Pencil.Core
{
	using System;

	public class DotNodeFactory : INodeFactory
	{
        int nextNodeId;

		public event EventHandler<NodeCreatedEventArgs<DotNode>> NodeCreated;

		public Node Create()
		{
			var node = new DotNode(nextNodeId++);
			var tmp = NodeCreated;
			if(tmp != null)
				tmp(this, new NodeCreatedEventArgs<DotNode>(node));
			return node;
		}
	}
}