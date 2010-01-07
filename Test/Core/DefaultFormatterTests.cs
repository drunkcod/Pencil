using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Linq.Expressions;
using System.Reflection;

namespace Pencil.Test.Core
{
    class MyClass
    {
        public void Action(){}
    }

    class DefaultFormatter
    {
        public string Format(MethodInfo method) {
            return string.Format("{0} {1}::{2}()", method.ReturnType.FullName, method.DeclaringType.FullName, method.Name);
        }
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
            return Tests(CheckFormat<MyClass>(x => x.Action(), "System.Void Pencil.Test.Core.MyClass::Action()"));
        }

        T[] Tests<T>(params T[] tests) { return tests; }

        TestCaseData CheckFormat<T>(Expression<Action<T>> func, string expected){
            
            return new TestCaseData((func.Body as MethodCallExpression).Method, expected);
        }
    }
}
