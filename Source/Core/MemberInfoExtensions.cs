namespace Pencil.Core
{
	using System.Reflection;
	static class MemberInfoExtensions
	{
		public static bool IsGenerated(this MemberInfo member)
		{
			return member.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false).Length != 0;
		}
	}
}