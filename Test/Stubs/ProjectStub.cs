namespace Pencil.Test.Stubs
{
    using Pencil.Build;
    using System;

    class ProjectStub : IProject
    {
        public Action<string> RunHandler = x => {};
		public Predicate<string> HasTargetHandler;

        public bool HasTarget(string name) { return HasTargetHandler(name); }
        public void Run(string target) { RunHandler(target); }
		public void Register<T>(T instance){}
    }
}