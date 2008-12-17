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
			Dictionary<string, IType> types = new Dictionary<string, IType>();

			public void Add(IType type)
			{
				if(type.Equals(typeof(object)) || type.Equals(typeof(void)))
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
				var dependsOn = new DependencyCollection();
				EachNonObjectMethod(m => m.Arguments.Map(a => a.Type).ForEach(dependsOn.Add));
				EachNonObjectMethod(m => dependsOn.Add(m.ReturnType));
				EachNonObjectMethod(m => m.Calls.ForEach(x =>{ dependsOn.Add(x.DeclaringType);}));
				return dependsOn.Types;
			}
		}

		void EachNonObjectMethod(Action<IMethod> action)
		{
			foreach(var method in Methods)
				if(!method.DeclaringType.Equals(typeof(object)))
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