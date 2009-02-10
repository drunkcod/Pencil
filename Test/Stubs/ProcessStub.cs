namespace Pencil.Test.Stubs
{
    using System;
    using Pencil.Build;

    class ProcessStub : IProcess
    {
        public Action WaitForExitHandler = () => {};
        public Func<int> GetExitCodeHandler = () => 0; 
        public bool HasExited { get { return true; } }

        public int ExitCode { get { return GetExitCodeHandler(); } }

        public System.IO.TextReader StandardOutput
        {
            get { throw new NotImplementedException(); }
        }
        
        public void WaitForExit(){ WaitForExitHandler(); }
    }
}
