namespace Pencil.Core
{
    public class Edge
    {
        Node from, to;

        internal Edge(Node from, Node to)
        {
            this.from = from;
            this.to = to;
        }

        public override string ToString()
        {
            return "{0}->{1}".InvariantFormat(from.Id, to.Id);
        }
    }
}