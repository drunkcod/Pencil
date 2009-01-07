namespace Pencil.Build
{
	using System;
	using System.Collections.Generic;

	public class Project : IProject
	{
		Dictionary<string,Target> targets = new Dictionary<string,Target>();
		internal Logger logger;
		readonly ZeptoContainer container = new ZeptoContainer();

		public Project()
		{
			foreach(var m in GetType().GetMethods())
			if(m.DeclaringType != typeof(object))
				targets.Add(m.Name, new MethodTarget(this, m));
			container.Register(typeof(IFileSystem), () => FileSystem);
			container.Register(typeof(IExecutionEnvironment), () => ExecutionEnvironment);
		}

		public T New<T>()
		{
			return container.Get<T>();
		}

		public bool HasTarget(string name)
		{
			return targets.ContainsKey(name);
		}

		public void Run(string targetName)
		{
			logger.Write("{0}:", targetName);
			using(logger.Indent())
				targets[targetName].Execute();
		}

		public IFileSystem FileSystem { get; set;  }
		public IExecutionEnvironment ExecutionEnvironment { get; set; }
	}
}