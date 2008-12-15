namespace Pencil.Test.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;
    using Pencil.Core;
    using Pencil.Test.Stubs;

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
        [Test]
        public void Should_visit_all_types()
        {
            var visited = new List<string>();
            var handler = new HandlerStub();
            var reader = new AssemblyReader(handler);

            handler.BeginTypeHandler = x => visited.Add(x.Name);
            reader.Read(GetAssemblyWithTypes(new TypeStub("Type1"), new TypeStub("Type2")));

            Assert.That(visited, Is.EquivalentTo(new[]{ "Type1", "Type2" }));
        }

		static IAssembly GetSingleTypeAssembly()
		{
			var type = new TypeStub("MyType");
			var method = new MethodStub("DoStuff");
			type.GetMethodsHandler = () => new[]{ method };

			return GetAssemblyWithTypes(type);
		}

        static IAssembly GetAssemblyWithTypes(params IType[] types)
        {
            var assembly = new AssemblyStub("Test.Assembly.dll");
            var module = new ModuleStub("Module1");
            assembly.GetModulesHandler = () => new[] { module };
            module.GetTypesHandler = () => types;
            return assembly;
        }
	}
}