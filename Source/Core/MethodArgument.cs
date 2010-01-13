namespace Pencil.Core
{
    using ParameterInfo = System.Reflection.ParameterInfo;

    class MethodArgument : IMethodArgument
    {
        readonly string name;
        readonly IType type;

		public string Name { get { return name; } }
        public IType Type { get { return type; } }

        internal MethodArgument(string name, IType type)
        {
            this.name = name;
            this.type = type;
        }
    }
}
