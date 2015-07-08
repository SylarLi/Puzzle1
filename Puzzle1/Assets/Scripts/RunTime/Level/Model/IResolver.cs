public interface IResolver
{
    void Apply(IPuzzle puzzle, IOperation op);

    void Reverse(IPuzzle puzzle, IOperation op);

    bool IsWin(IPuzzle puzzle);

    bool IsLose(IPuzzle puzzle);
}

