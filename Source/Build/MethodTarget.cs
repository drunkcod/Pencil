namespace Pencil.Build
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	public class MethodTarget : Target
	{
		IProject project;
		MethodInfo method;

		public MethodTarget(IProject project, MethodInfo method)
		{
			this.project = project;
			this.method = method;
		}

		public override IEnumerable<string> GetDependencies()
		{
			foreach(DependsOnAttribute item in method.GetCustomAttributes(typeof(DependsOnAttribute), false))
				yield return item.Name;
		}

		protected override IProject GetProjectCore (){ return project; }

		protected override void ExecuteCore()
		{
			try
			{
				method.Invoke(project, null);
			}
			catch(TargetInvocationException e)
			{
				throw new TargetFailedException(e.InnerException);
			}
		}
	}
}