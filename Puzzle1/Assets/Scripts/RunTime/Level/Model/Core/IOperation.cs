public interface IOperation : IStream
{
    OpType type { get; }

    int row { get; }

    int column { get; }
}
