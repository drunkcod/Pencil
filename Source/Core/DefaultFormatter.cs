using System;
using System.Text;
using System.Reflection;

namespace Pencil.Core
{
    public class DefaultFormatter
    {
        public string Format(MethodInfo method) {
            var signature = new StringBuilder();
            AppendFormat(signature, method.ReturnType);
            signature.Append(" ");
            AppendFormat(signature, method.DeclaringType);
            signature.AppendFormat("::{0}", method.Name);
            AppenGenericArguments(signature, method);
            signature.Append('(');
            var separator = "";
            foreach(var item in method.GetParameters()) {
                signature.Append(separator);
                var parameter = item.ParameterType;
                if(parameter.IsGenericType)
                    AppendGeneric(signature, parameter);
                else {
                    AppendFormat(signature, parameter);
                }
                separator = ", ";
            }
            return signature.Append(')').ToString();
        }

        static void AppendFormat(StringBuilder signature, System.Type type){
            var fullName = type.FullName;
            signature.AppendFormat(fullName == null ? type.Name : fullName);
        }

        static void AppendGeneric(StringBuilder signature, System.Type type) {
            var separator =  type.Name.Substring(0, type.Name.IndexOf('`'));
            AppendGenericTypes(signature, separator, type.GetGenericArguments());
        }

        static StringBuilder AppendGenericTypes(StringBuilder signature, string separator, System.Type[] types) {
            separator += '<';                
            foreach(var item in types) {
                signature.Append(separator);
                AppendFormat(signature, item);
                separator = ", ";
            }
            return signature.Append('>');
        }

        static void AppenGenericArguments(StringBuilder signature, MethodInfo method) {
            if(method.IsGenericMethod) {
                AppendGenericTypes(signature, "", method.GetGenericArguments());
            }
        }
    }
}
