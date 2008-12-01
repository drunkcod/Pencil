namespace Pencil.Build
{
	using System;

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class DependsOnAttribute : Attribute
	{
		string name;
		public DependsOnAttribute(string name)
		{
			this.name = name;
		}

		public string Name { get { return name; } }
	}
}