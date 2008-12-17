namespace Pencil.Core
{
	using System.Reflection;
	static class MemberInfoExtensions
	{
		public static bool IsGenerated(this MemberInfo member)
		{
			return member.HasAttribute(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute));
		}

		static bool HasAttribute(this MemberInfo member, System.Type attributeType)
		{
			return member.GetCustomAttributes(attributeType, false).Length != 0;
		}
	}
}