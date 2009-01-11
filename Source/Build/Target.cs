namespace Pencil.Build
{
	using System.Collections.Generic;
	using System.Reflection;

	public abstract class Target
	{
		public void Execute()
		{
			SatisfyDependencies();
			ExecuteCore();
		}

		private void SatisfyDependencies()
		{
			foreach(var item in GetDependencies())
				GetProjectCore().Run(item);
		}

		public virtual IEnumerable<string> GetDependencies()
		{
			return new string[0];
		}

		protected abstract IProject GetProjectCore();

		protected virtual void ExecuteCore(){}
	}
}