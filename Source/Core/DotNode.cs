namespace Pencil.Core
{
	using System.Text;

	public class DotNode : Node
	{
		readonly int id;

		public DotNode(int id)
		{
			this.id = id;
			Style = new DotNodeStyle();
		}
		
		public DotNodeStyle Style { get ; set; }

		public override string Id { get { return id.ToString(); } }

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendFormat("{0}[label=\"{1}\"", Id, Label);
			if(!Style.IsEmpty)
				Style.AppendTo(builder.Append(' '));
			return builder.Append(']').ToString();
		}
	}
}
