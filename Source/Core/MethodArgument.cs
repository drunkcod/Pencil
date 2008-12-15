namespace Pencil.Core
{
    using ParameterInfo = System.Reflection.ParameterInfo;

    class MethodArgument : IMethodArgument
    {
        ParameterInfo parameter;

        public static IMethodArgument Wrap(ParameterInfo parameter)
        {
            return new MethodArgument(parameter);
        }

        public IType Type { get { return Pencil.Core.Type.Wrap(parameter.ParameterType); } }

        MethodArgument(ParameterInfo parameter)
        {
            this.parameter = parameter;
        }
    }
}
