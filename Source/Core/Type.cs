namespace Pencil.Core
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using SystemType = System.Type;

    public class Type : IType
    {
        const BindingFlags AllMethods = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
        SystemType type;

		class DependencyCollection
		{
			IType parent;
			Dictionary<string, IType> types = new Dictionary<string, IType>();

			public DependencyCollection(IType parent)
			{
				this.parent = parent;
			}

			public void Add(IType type)
			{
				if(type.Equals(parent)
				|| type.Equals(typeof(object))
				|| type.Equals(typeof(void))
				|| type.IsGenericParameter
				|| type.Equals(typeof(System.Runtime.InteropServices._Attribute)))
					return;
				var key = type.Name;
				if(!types.ContainsKey(key))
					types.Add(key, type);
			}

			public ICollection<IType> Types { get { return types.Values; } }
		}

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
		public ICollection<IType> DependsOn
		{
			get
			{
				if(type.IsEnum)
					return new IType[0];
				var dependsOn = new DependencyCollection(this);
				dependsOn.Add(Type.Wrap(type.BaseType));
				type.GetInterfaces().ForEach(x => dependsOn.Add(Type.Wrap(x)));
				EachOwnMethod(m => m.Arguments.Map(a => a.Type).ForEach(dependsOn.Add));
				EachOwnMethod(m => dependsOn.Add(m.ReturnType));
				EachOwnMethod(m => m.Calls.ForEach(x =>{ dependsOn.Add(x.DeclaringType);}));
				return dependsOn.Types;
			}
		}

		void EachOwnMethod(Action<IMethod> action)
		{
			foreach(var method in Methods)
				if(method.DeclaringType.Equals(type))
					action(method);
		}

		public bool IsGenerated
		{
			get
			{
				var parent = type.DeclaringType;
				return type.IsGenerated() || (parent != null && parent.IsGenerated());
			}
		}

		public bool IsGenericParameter { get { return type.IsGenericParameter; } }

		public override string ToString(){ return Name; }

		public override bool Equals(object obj)
		{
			Type other = obj as Type;
			return (other != null && other.type == type) || (obj as SystemType == type);
		}

		public override int GetHashCode()
		{
			return type.GetHashCode();
		}
    }
}