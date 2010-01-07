using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Linq.Expressions;
using Pencil.Core;
using System.Reflection;

namespace Pencil.Test.Core
{
    class MyClass
    {
        public void Action(){}
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
                CheckFormat<MyClass>(x => x.Action(42), "System.Void Pencil.Test.Core.MyClass::Action(System.Int32)"),
                CheckFormat<MyClass>(x => x.Action(42, ""), "System.Void Pencil.Test.Core.MyClass::Action(System.Int32, System.String)"));
        }

        T[] Tests<T>(params T[] tests) { return tests; }

        TestCaseData CheckFormat<T>(Expression<Action<T>> func, string expected){
            var method = (func.Body as MethodCallExpression).Method;
            var data = new TestCaseData(method, expected);
            return data.SetName("\"" + method + "\"");
        }
    }
}
