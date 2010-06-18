namespace Pencil.Core
{
    public class Node
    {
        public string Label { get; set; }
		public virtual string Id { get { return Label; } }
        internal bool IsEmpty { get { return Label.IsNullOrEmpty(); } }
    }
}