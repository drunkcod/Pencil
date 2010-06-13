namespace Pencil.Test.Core
{
    using Pencil.Core;
    using NUnit.Framework;
    using System;
	using System.Collections.Generic;
    using Pencil.Test.Stubs;
    using System.Reflection.Emit;
    using System.Reflection;

    [TestFixture]
    public partial class DisassemblerTests : ITokenResolver
    {
		[Test]
		public void Decode_should_parse_whole_stream()
		{
			var il = new byte[]{ 0, 1, 20 };
			var expected = new[]{ "nop", "break", "ldnull" };
            var ir = new InstructionReader(this, il);
            var result = new List<Instruction>(ir.ReadToEnd());
            Assert.That(result.Map(x => x.ToString()).ToList(), Is.EquivalentTo(expected));
		}

		void CheckDecode(string expected, params byte[] il)
        {
            var ir = new InstructionReader(this, il);
            var result = new List<Instruction>(ir.ReadToEnd());
            Assert.AreEqual(expected, result[0].ToString());
        }

        [Test]
        public void decode_method() {
            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("Pencil.Generated"), AssemblyBuilderAccess.ReflectionOnly);
            var module = assembly.DefineDynamicModule("Pencil.Generated");
            var method = module.DefineGlobalMethod("Return42", MethodAttributes.Static | MethodAttributes.Public, typeof(int), System.Type.EmptyTypes);
            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldc_I4, 42);
            il.Emit(OpCodes.Ret);
            module.CreateGlobalFunctions();
            var expected = new[] { "ldc.i4 42", "ret" };

            var body = new PencilMethodBody(module.GetMethod("Return42"));
            var actual = body.DecodeBody(new DefaultTypeLoader());

            Assert.That(actual.Map(x => x.ToString()).ToList(), Is.EquivalentTo(expected));
        }

        void SetResolveToken(string token)
        {
            resolveTokenResult = token;
        }

        string ITokenResolver.ResolveString(int token)
        {
            return resolveTokenResult;
        }

		IMethod ITokenResolver.ResolveMethod(int token)
        {
            return new MethodStub(resolveTokenResult);
        }

        string resolveTokenResult;

        #region ITokenResolver Members


        IType ITokenResolver.ResolveType(int token) {
            return new TypeStub(resolveTokenResult);
        }

        class FieldStub : IField
        {
            readonly string name;

            public FieldStub(string name) {
                this.name = name;
            }

            public override string ToString() {
                return Name;
            }

            #region IMember Members

            public IType DeclaringType {
                get { throw new NotImplementedException(); }
            }

            public string Name {
                get { return name; }
            }

            #endregion
        }

        IField ITokenResolver.ResolveField(int token) {
            return new FieldStub(resolveTokenResult);
        }

        #endregion
    }
}
