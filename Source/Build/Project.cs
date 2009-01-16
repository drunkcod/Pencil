namespace Pencil.Build
{
	using System;
	using System.Collections.Generic;

	public class Project : IProject
	{
		readonly Dictionary<string,Target> targets;
		readonly HashSet<string> done = new HashSet<string>();
		internal Logger logger = new Logger(System.IO.TextWriter.Null);
		readonly ZeptoContainer container = new ZeptoContainer();

		public Project()
		{
			targets = new MethodTargetExtractor().GetTargets(this);
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
			if(done.Contains(targetName))
				return;
			logger.Write("{0}:", targetName);
			using(logger.Indent())
			{
				RunCore(targetName);
				done.Add(targetName);
			}
		}

		protected virtual void RunCore(string targetName)
		{
			targets[targetName].Execute();
		}

		public void Register<T>(T instance){ container.Register(typeof(T), instance); }
	}
}