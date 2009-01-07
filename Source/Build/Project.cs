namespace Pencil.Build
{
	using System;
	using System.IO;
	using System.Collections.Generic;

	public class Project : IProject
	{
		Dictionary<string,Target> targets = new Dictionary<string,Target>();
		readonly IFileSystem fileSystem = new FileSystem();
		readonly IExecutionEnvironment environment = new ExecutionEnvironment();
		internal Logger logger;

		public Project()
		{
			foreach(var m in GetType().GetMethods())
			if(m.DeclaringType != typeof(object))
				targets.Add(m.Name, new MethodTarget(this, m));
		}

		public T New<T>()
		{
			foreach(var ctor in typeof(T).GetConstructors())
			{
				var parameters = ctor.GetParameters();
				var args = new object[parameters.Length];
				for(int i = 0; i != parameters.Length; ++i)
					args[i] = Resolve(parameters[i].ParameterType);
				return (T)ctor.Invoke(args);
			}
			return default(T);
		}

		object Resolve(Type type)
		{
			if(type == typeof(IFileSystem))
				return FileSystem;
			if(type == typeof(IExecutionEnvironment))
				return environment;
			return null;
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

		public IFileSystem FileSystem { get { return fileSystem; } }
	}
}