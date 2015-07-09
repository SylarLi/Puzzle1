public interface IOperation : IStream
{
    /// <summary>
    /// 操作类型
    /// </summary>
    OpType type { get; }

    /// <summary>
    /// 影响的行号
    /// </summary>
    int row { get; }

    /// <summary>
    /// 影响的列号
    /// </summary>
    int column { get; }

    /// <summary>
    /// 对哪些方向有影响
    /// </summary>
    QuadValue direction { get; }
}
