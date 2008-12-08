namespace Pencil.Test.Stubs
{
    using System.Collections.Generic;
    using System;
    using Pencil.Build;

    class TargetStub : Target
    {
        IEnumerable<string> dependencies;

        public Action ExecuteHandler;

        public TargetStub(IProject project, IEnumerable<string> dependencies)
            : base(project)
        {
            this.dependencies = dependencies;
        }

        public override IEnumerable<string> GetDependencies()
        {
            return dependencies;
        }

        protected override void ExecuteCore() { ExecuteHandler(); }
    }
}