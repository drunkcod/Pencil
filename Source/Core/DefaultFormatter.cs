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
                if(item.ParameterType.IsGenericType)
                    AppendGeneric(signature, format, item.ParameterType);
                else
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

        void AppendGeneric(StringBuilder signature, string preFormat, System.Type type) {
            var format = string.Format(preFormat, type.Name.Substring(0, type.Name.IndexOf('`')) +  "<{0}");            
            AppendTypes(signature, format, type.GetGenericArguments())
                .Append('>');
        }

        StringBuilder AppendTypes(StringBuilder signature, string format, System.Type[] types) {
            foreach(var item in types) {
                signature.AppendFormat(format, Format(item));
                format = ", {0}";
            }
            return signature;
        }

        void AppenGenericArguments(StringBuilder signature, MethodInfo method) {
            if(method.IsGenericMethod) {
                var format = "<{0}";
                AppendTypes(signature, format, method.GetGenericArguments())
                    .Append('>');
            }
        }
    }
}
