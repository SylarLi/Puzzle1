public interface IResolver
{
    void Apply(IPuzzle puzzle, IOperation op);

    bool IsWin(IPuzzle puzzle);

    bool IsLose(IPuzzle puzzle);
}

