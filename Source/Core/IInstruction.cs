namespace Pencil.Core
{
    public interface IInstruction
    {
        bool IsCall { get; }
        Opcode Opcode { get; }
        object Operand { get; }
    }
}
