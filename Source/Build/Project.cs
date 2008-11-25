namespace Pencil.Build
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	public class Project : IProject
	{
		Dictionary<string,MethodInfo> targets = new Dictionary<string,MethodInfo>();
		public Project()
		{
			foreach(var m in GetType().GetMethods())
			if(m.DeclaringType != typeof(object))
				targets.Add(m.Name, m);
		}

		protected T New<T>()
		{
			return (T)typeof(T).GetConstructor(Type.EmptyTypes).Invoke(null);
		}

		public bool HasTarget(string name)
		{
			return targets.ContainsKey(name);
		}

		public void Run(string targetName)
		{
			targets[targetName].Invoke(this, null);
		}
	}
}