namespace Pencil.Core
{
    public interface IInstruction
    {
        bool IsCall { get; }
        object Operand { get; }
    }
}
