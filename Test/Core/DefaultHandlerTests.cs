namespace Pencil.Test.Core
{
	using System;
	using NUnit.Framework;
	using Pencil.Core;
	using Pencil.Test.Stubs;

	[TestFixture]
	public class DefaultHandlerTests
	{
		class TestHandler : DefaultHandler
		{
			public Action<IAssembly> OnBeginAssembly = x => {};
			public Action<IModule> OnBeginModule = x => {};
			public Action<IType> OnBeginType = x => {};
			public Action<IMethod> OnBeginMethod = x => {};

			protected override void BeginAssemblyCore(){ OnBeginAssembly(Assembly); }
			protected override void BeginModuleCore(){ OnBeginModule(Module); }
			protected override void BeginTypeCore(){ OnBeginType(Type); }
			protected override void BeginMethodCore(){ OnBeginMethod(Method);}
		}

		[Test]
		public void Should_remember_current_assembly()
		{
			var handler = new TestHandler();
			IAssembly activeAssembly = null;
			handler.OnBeginAssembly = x => activeAssembly = x;

			var assembly = new AssemblyStub("Test.Assembly");
			handler.BeginAssembly(assembly);

			activeAssembly.ShouldBeSameAs(assembly);
		}
		[Test]
		public void Should_remember_current_module()
		{
			var handler = new TestHandler();
			IModule activeModule = null;
			handler.OnBeginModule = x => activeModule = x;

			var module = new ModuleStub("TestModule");
			handler.BeginModule(module);

			activeModule.ShouldBeSameAs(module);
		}
		[Test]
		public void Should_remember_current_type()
		{
			var handler = new TestHandler();
			IType activeType = null;
			handler.OnBeginType = x => activeType = x;

			var type = new TypeStub("TestType");
			handler.BeginType(type);

			activeType.ShouldBeSameAs(type);
		}
		[Test]
		public void Should_remember_current_method()
		{
			var handler = new TestHandler();
			IMethod activeMethod = null;
			handler.OnBeginMethod = x => activeMethod = x;

			var method = new MethodStub("DoStuff");
			handler.BeginMethod(method);

			activeMethod.ShouldBeSameAs(method);
		}
	}
}