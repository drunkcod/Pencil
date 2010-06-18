namespace Pencil.Core
{
    public class Edge
    {
        string from, to;

        internal Edge(string from, string to)
        {
            this.from = from;
            this.to = to;
        }

        public override string ToString()
        {
            return "{0}->{1}".InvariantFormat(from, to);
        }
    }
}