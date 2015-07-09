public interface IOperation : IStream
{
    OperationType type { get; }

    int row { get; }

    int column { get; }
}
