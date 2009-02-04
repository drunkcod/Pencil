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

			public void Add(IType item)
			{
				var element = item.ElementType;
				if(element.Equals(parent)
				|| element.Equals(typeof(object))
				|| element.Equals(typeof(void))
				|| element.IsGenericParameter
				|| element.Equals(typeof(System.Runtime.InteropServices._Attribute)))
					return;
				var key = item.Name;
				if(!types.ContainsKey(key))
					types.Add(key, item);
			}

			public ICollection<IType> Types { get { return types.Values; } }
		}

		public static IType Wrap(SystemType type)
        {
            return new Type(type);
        }

        private Type(SystemType type)
        {
			if(type == null)
				throw new ArgumentNullException();
            this.type = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        }

        public string Name { get { return type.Name; } }
		public string FullName { get { return type.FullName; } }
        public IType ElementType
		{
			get
			{
				var elementType = type.GetElementType();
				return elementType == null ? this : Type.Wrap(elementType);
			}
		}
        public IEnumerable<IMethod> Methods { get { return type.GetMethods(AllMethods).Map<MethodInfo, IMethod>(Method.Wrap); } }
		public ICollection<IType> DependsOn
		{
			get
			{
				if(type.IsEnum)
					return new IType[0];
				var dependsOn = new DependencyCollection(this);
				var baseType = type.BaseType;
				if(baseType != null)
					dependsOn.Add(Type.Wrap(baseType));
				type.GetInterfaces().ForEach(x =>
				{
					if(Implements(x))
						dependsOn.Add(Type.Wrap(x));
				});
                type.GetFields(AllMethods).ForEach(x => { if(x.DeclaringType == type) dependsOn.Add(Type.Wrap(x.FieldType)); });
				EachOwnMethod(m => m.Arguments.Map(a => a.Type).ForEach(dependsOn.Add));
				EachOwnMethod(m => dependsOn.Add(m.ReturnType));
				EachOwnMethod(m => m.Calls.ForEach(x =>{ dependsOn.Add(x.DeclaringType);}));
				return dependsOn.Types;
			}
		}
		public IEnumerable<IType> NestedTypes
		{
			get
			{
				return type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
				.Map(x => Wrap(x));
			}
		}

		bool Implements(System.Type interfaceType)
		{
			var baseType = type.BaseType;
			return baseType == null || !interfaceType.IsAssignableFrom(baseType);
		}

		void EachOwnMethod(Action<IMethod> action)
		{
			foreach(var method in Methods)
				if(method.DeclaringType.Equals(type))
					action(method);
            foreach(var ctor in type.GetConstructors(AllMethods))
                action(Method.Wrap(ctor));
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

		public bool IsA<T>(){ return typeof(T).IsAssignableFrom(type); }

		public bool IsPublic { get {return type.IsPublic || type.IsNestedPublic; } }

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