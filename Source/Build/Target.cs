namespace Pencil.Build
{
	using System.Collections.Generic;
	using System.Reflection;

	public class Target
	{
		protected IProject project;

		protected Target(IProject project)
		{
			this.project = project;
		}

		public void Execute()
		{
			SatisfyDependencies();
			ExecuteCore();
		}

		private void SatisfyDependencies()
		{
			foreach(var item in GetDependencies())
				project.Run(item);
		}

		public virtual IEnumerable<string> GetDependencies()
		{
			return new string[0];
		}

		protected virtual void ExecuteCore(){}
	}
}