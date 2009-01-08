namespace Pencil.Core
{
	public class DotNode : Node
	{
		int id;

		public DotNode(int id)
		{
			this.id = id;
		}

		public override string Id { get { return id.ToString(); } }

		public override string ToString()
		{
		    return "{0}[label=\"{1}\"]".InvariantFormat(Id, Label);
		}
	}
}