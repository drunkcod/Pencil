﻿namespace Pencil.Core
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using SystemType = System.Type;

    public class Type : IType
    {
        const BindingFlags AllMethods = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
        SystemType type;

        public static IType Wrap(SystemType type)
        {
            return new Type(type);
        }

        private Type(SystemType type)
        {
            this.type = type;
        }

        public string Name { get { return type.Name; } }
        public IEnumerable<IMethod> Methods { get { return type.GetMethods(AllMethods).Map<MethodInfo, IMethod>(Method.Wrap); } }

		public bool IsGenerated
		{
			get
			{
				var parent = type.DeclaringType;
				return type.IsGenerated() || (parent != null && parent.IsGenerated());
			}
		}

		public override string ToString(){ return Name; }

		public override bool Equals(object obj)
		{
			Type other = obj as Type;
			return other != null && other.type == type;
		}

		public override int GetHashCode()
		{
			return type.GetHashCode();
		}
    }
}