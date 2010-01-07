using System;
using System.Text;
using System.Reflection;

namespace Pencil.Core
{
    public class DefaultFormatter
    {
        public string Format(MethodInfo method) {
            var signature = new StringBuilder(Format(method.ReturnType));
            signature.AppendFormat(" {0}::{1}", Format(method.DeclaringType), method.Name);
            AppenGenericArguments(signature, method);
            signature.Append('(');
            var format = "{0}";
            foreach(var item in method.GetParameters()) {
                signature.AppendFormat(format, Format(item.ParameterType));
                format = ", {0}";
            }
            return signature.Append(')').ToString();
        }

        string Format(System.Type type){
            var fullName = type.FullName;
            if(fullName == null)
                return type.Name;
            return fullName; 
        }

        void AppenGenericArguments(StringBuilder signature, MethodInfo method) {
            if(method.IsGenericMethod) {
                var format = "<{0}";
                foreach(var item in method.GetGenericArguments()) {
                    signature.AppendFormat(format, Format(item));
                    format = ", {0}";
                }
                signature.Append('>');
            }
        }
    }
}
