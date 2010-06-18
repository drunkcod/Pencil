namespace Pencil.Core
{
    public struct Edge
    {
        public readonly string From, To;

        internal Edge(string from, string to) {
            this.From = from;
            this.To = to;
        }

        public override string ToString() {
            return "{0}->{1}".InvariantFormat(From, To);
        }
    }
}