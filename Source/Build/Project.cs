namespace Pencil.Build
{
	using System;
	using System.IO;
	using System.Collections.Generic;

	public class Project : IProject
	{
		Dictionary<string,Target> targets = new Dictionary<string,Target>();
		internal TextWriter logger;
		public Project()
		{
			foreach(var m in GetType().GetMethods())
			if(m.DeclaringType != typeof(object))
				targets.Add(m.Name, new MethodTarget(this, m));
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
			logger.WriteLine("{0}:", targetName);
			targets[targetName].Execute();
		}
	}
}