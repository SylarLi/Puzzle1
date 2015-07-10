public interface IResolver
{
    /// <summary>
    /// 同时影响谜题数据和表现
    /// </summary>
    /// <param name="puzzle"></param>
    /// <param name="op"></param>
    void ResolveTouch(IPuzzle puzzle, IOperation op);

    /// <summary>
    /// 只影响谜题数据
    /// </summary>
    /// <param name="puzzle"></param>
    /// <param name="op"></param>
    void ResolveTouchData(IPuzzle puzzle, IOperation op);

    /// <summary>
    /// 是否死循环
    /// </summary>
    /// <param name="puzzle"></param>
    /// <param name="op"></param>
    /// <returns></returns>
    bool ResolveIsLoop(IPuzzle puzzle);

    /// <summary>
    /// Solved???
    /// </summary>
    /// <param name="puzzle"></param>
    /// <returns></returns>
    bool IsSolved(IPuzzle puzzle);
}

