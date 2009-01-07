namespace Pencil.Build
{
	using System;
	using System.Collections.Generic;
	class ZeptoContainer
	{
		Dictionary<Type, Func<object>> things = new Dictionary<Type, Func<object>>();

		public void Register(Type type, Func<object> resolve){ things.Add(type, resolve); }
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
			Func<object> resolve;
			if(things.TryGetValue(type, out resolve))
				return resolve();
			return null;
		}
	}
}