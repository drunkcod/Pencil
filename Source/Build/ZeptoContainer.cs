namespace Pencil.Build
{
	using System;
	using System.Collections.Generic;
	class ZeptoContainer
	{
		Dictionary<Type, object> things = new Dictionary<Type, object>();

		public void Register(Type type, object resolve){ things.Add(type, resolve); }
		public T Get<T>()
		{
			var instance = Resolve(typeof(T));
			if(instance != null)
				return (T)instance;
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
			object instance;
			if(things.TryGetValue(type, out instance))
				return instance;
			return null;
		}
	}
}