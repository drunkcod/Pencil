namespace Pencil.Build
{
	using System;
	using System.Collections.Generic;
	class ZeptoContainer
	{
		Dictionary<Type, object> things = new Dictionary<Type, object>();

		public void Register(Type type, object instance){ things.Add(type, instance); }
		public T Get<T>()
		{
			foreach(var ctor in typeof(T).GetConstructors())
			{
				var parameters = ctor.GetParameters();
				var args = new object[parameters.Length];
				for(int i = 0; i != parameters.Length; ++i)
					args[i] = Resolve(parameters[i].ParameterType);
				return (T)ctor.Invoke(args);
			}
			return default(T);
		}

		object Resolve(Type type)
		{
			object thing;
			things.TryGetValue(type, out thing);
			return thing;
		}
	}
}