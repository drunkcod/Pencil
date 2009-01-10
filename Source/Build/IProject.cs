namespace Pencil.Build
{
    public interface IProject
    {
        bool HasTarget(string name);
        void Run(string target);
		void Register<T>(T instance);
    }
}