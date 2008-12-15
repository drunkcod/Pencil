namespace Pencil.Test.Core
{
	using System.Text;
	using NUnit.Framework;
	using Pencil.Core;
	using Pencil.Test.Stubs;

	interface IHandler
	{
		void BeginAssembly(IAssembly assembly);
		void EndAssembly();

		void BeginModule(IModule module);
		void EndModule();

		void BeginType(IType type);
		void EndType();

		void BeginMethod(IMethod method);
		void EndMethod();
	}

	class HandlerStub : IHandler
	{
		StringBuilder result = new StringBuilder();

		public void BeginAssembly(IAssembly assembly){ Append("Assembly"); }
		public void EndAssembly(){ Append(" ~Assembly"); }
		public void BeginModule(IModule module){ Append(" Module"); }
		public void EndModule(){ Append(" ~Module"); }
		public void BeginType(IType type){ Append(" Type"); }
		public void EndType(){ Append(" ~Type"); }
		public void BeginMethod(IMethod method){ Append(" Method"); }
		public void EndMethod(){ Append(" ~Method"); }

		public string Result { get { return result.ToString(); } }

		void Append(string s)
		{
			result.Append(s);
		}
	}

	class AssemblyReader
	{
		IHandler handler;

		public AssemblyReader(IHandler handler)
		{
			this.handler = handler;
		}

		public void Read(IAssembly assembly)
		{
			handler.BeginAssembly(assembly);
			assembly.Modules.ForEach(Read);
			handler.EndAssembly();
		}

		void Read(IModule module)
		{
			handler.BeginModule(module);
			module.Types.ForEach(Read);
			handler.EndModule();
		}

		void Read(IType type)
		{
			handler.BeginType(type);
			type.Methods.ForEach(Read);
			handler.EndType();
		}

		void Read(IMethod method)
		{
			handler.BeginMethod(method);
			handler.EndMethod();
		}
	}


	[TestFixture]
	public class AssemblyReaderTests
	{
		[Test]
		public void Should_visit_in_descending_order()
		{
			var handler = new HandlerStub();
			var reader = new AssemblyReader(handler);
			reader.Read(GetSingleTypeAssembly());
			handler.Result.ShouldEqual("Assembly Module Type Method ~Method ~Type ~Module ~Assembly");
		}

		static IAssembly GetSingleTypeAssembly()
		{
			var assembly = new AssemblyStub("Small.Assembly.dll");
			var module = new ModuleStub();
			var type = new TypeStub();
			var method = new MethodStub("DoStuff");

			assembly.GetModulesHandler = () => new[]{ module };
			module.GetTypesHandler = () => new[]{ type };
			type.GetMethodsHandler = () => new[]{ method };

			return assembly;
		}
	}
}