namespace Pencil.Test.Scenarios
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using Microsoft.CSharp;
    using NUnit.Framework;

    [TestFixture]
    public class Scenario1Tests
    {
        [Test]
        public void Should_compile_given_Pencil_Core()
        {
            var codeProvider = new CSharpCodeProvider(new Dictionary<string,string>(){{"CompilerVersion", "v3.5"}});
            var options = new CompilerParameters();
            options.GenerateExecutable = false;
            options.GenerateInMemory = true;
            options.ReferencedAssemblies.Add("Pencil.dll");

            var result = codeProvider.CompileAssemblyFromFile(options, @"..\..\Scenarios\PrintMethodDependencies.cs");
            Assert.AreEqual(0, result.NativeCompilerReturnValue);
        }
    }
}
