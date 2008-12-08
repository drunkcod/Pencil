namespace Pencil.Core
{
    class Edge
    {
        Node from, to;

        public Edge(Node from, Node to)
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
