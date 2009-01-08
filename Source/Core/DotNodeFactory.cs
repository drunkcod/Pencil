namespace Pencil.Core
{
	class DotNodeFactory : INodeFactory
	{
        int nextNodeId;

		public Node Create()
		{
			return new DotNode(nextNodeId++);
		}
	}
}