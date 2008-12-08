namespace Pencil.Test.Stubs
{
    using Pencil.Build;
    using System;

    class ProjectStub : IProject
    {
        public Action<string> RunHandler;

        public bool HasTarget(string name) { return false; }
        public void Run(string target) { RunHandler(target); }
    }
}