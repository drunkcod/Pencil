namespace Pencil.Test.Stubs
{
    using System;
    using Pencil.Build;

    class ProcessStub : IProcess
    {
        public bool HasExited { get { return true; } }

        public int ExitCode { get { return 0; } }

        public System.IO.TextReader StandardOutput
        {
            get { throw new NotImplementedException(); }
        }
    }
}
