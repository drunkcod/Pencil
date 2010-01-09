using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Linq.Expressions;
using Pencil.Core;
using System.Reflection;
using System.Reflection.Emit;

namespace Pencil.Test.Core
{
    class MethodFactory
    {
        static public MethodInfo CreateMethod(string name, System.Type returnType, System.Type[] arguments, Action<ILGenerator> createIL) {
            const string moduleName = "NMeter.Generated";
            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(moduleName), AssemblyBuilderAccess.ReflectionOnly);
            var module = assembly.DefineDynamicModule(moduleName);
            var method = module.DefineGlobalMethod(name, MethodAttributes.Static | MethodAttributes.Public, returnType, arguments);
            createIL(method.GetILGenerator());
            module.CreateGlobalFunctions();
            return module.GetMethod(method.Name, arguments);
        }
    }

    class MyClass
    {
        public void Generic<T>() { }
        public void GenericFunc<T>(Func<T> func) { }
        public void GenericFunc2<T0,T1>(Func<T0> func,Predicate<T1> predicate) { }
        public void Generic2<T0, T1>() { }
        public void Action() { }
        public void Action<T>(T item) { }
        public void Action<T0,T1>(T0 arg0, T1 arg1) { }
    }

    [TestFixture]
    public class DefaultFormatterTests
    {
        DefaultFormatter formatter = new DefaultFormatter();

        [TestCaseSource("MethodTests")]
        public void MethodFormatting(MethodInfo method, string expected){
            Assert.That(formatter.Format(method), Is.EqualTo(expected));
        }

        public IEnumerable<TestCaseData> MethodTests() {
            return Tests(
                CheckFormat<MyClass>(x => x.Action(), "System.Void Pencil.Test.Core.MyClass::Action()"),
                CheckFormat<MyClass>(x => x.Action(42), "System.Void Pencil.Test.Core.MyClass::Action<System.Int32>(System.Int32)"),
                CheckFormat<MyClass>(x => x.Action(42, ""), "System.Void Pencil.Test.Core.MyClass::Action<System.Int32, System.String>(System.Int32, System.String)"),
                CheckFormat(typeof(MyClass).GetMethod("Generic"), "System.Void Pencil.Test.Core.MyClass::Generic<T>()"),
                CheckFormat(typeof(MyClass).GetMethod("Generic2"), "System.Void Pencil.Test.Core.MyClass::Generic2<T0, T1>()"),
                CheckFormat(typeof(MyClass).GetMethod("GenericFunc"), "System.Void Pencil.Test.Core.MyClass::GenericFunc<T>(Func<T>)"),
                CheckFormat(typeof(MyClass).GetMethod("GenericFunc2"), "System.Void Pencil.Test.Core.MyClass::GenericFunc2<T0, T1>(Func<T0>, Predicate<T1>)"),
                CheckFormat(MethodFactory.CreateMethod("GlobalMethod", typeof(int), System.Type.EmptyTypes, il =>
                {
                    il.Emit(OpCodes.Ldc_I4, 42);
                    il.Emit(OpCodes.Ret);
                }), "System.Int32 GlobalMethod()"));
        }

        T[] Tests<T>(params T[] tests) { return tests; }

        TestCaseData CheckFormat<T>(Expression<Action<T>> func, string expected){
            var method = (func.Body as MethodCallExpression).Method;
            return CheckFormat(method, expected);
        }

        TestCaseData CheckFormat(MethodInfo method, string expected) {
            var data = new TestCaseData(method, expected);
            return data.SetName("\"" + method + "\"");
        }

    }
}
