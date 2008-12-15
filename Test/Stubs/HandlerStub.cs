namespace Pencil.Test.Stubs
{
    using System;
    using System.Text;
    using Pencil.Core;

    class HandlerStub : IHandler
    {
        StringBuilder result = new StringBuilder();

        public Action<IType> BeginTypeHandler;

        public HandlerStub()
        {
            BeginTypeHandler = x => Append(" Type");
        }

        public void BeginAssembly(IAssembly assembly) { Append("Assembly"); }
        public void EndAssembly() { Append(" ~Assembly"); }
        public void BeginModule(IModule module) { Append(" Module"); }
        public void EndModule() { Append(" ~Module"); }
        public void BeginType(IType type) { BeginTypeHandler(type); }
        public void EndType() { Append(" ~Type"); }
        public void BeginMethod(IMethod method) { Append(" Method"); }
        public void EndMethod() { Append(" ~Method"); }

        public string Result { get { return result.ToString(); } }

        void Append(string s)
        {
            result.Append(s);
        }
    }
}
