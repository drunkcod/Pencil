namespace Pencil.Build
{
	using System;
	using System.Collections.Generic;

	public class Project : IProject
	{
		readonly Dictionary<string,Target> targets;
		internal Logger logger;
		readonly ZeptoContainer container = new ZeptoContainer();

		public Project()
		{
			targets = new MethodTargetExtractor().GetTargets(this);
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