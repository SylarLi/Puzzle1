using Core;

/// <summary>
/// 关卡
/// </summary>
public interface ILevel : IEventDispatcher
{
    /// <summary>
    /// 根据参数生成谜题
    /// </summary>
    /// <param name="pp"></param>
    void MakePuzzle(PuzzleParams pp);

    /// <summary>
    /// 谜题
    /// </summary>
    IPuzzle puzzle { get; }

    /// <summary>
    /// 玩家操作记录
    /// </summary>
    IRecord record { get; }

    /// <summary>
    /// 解析器：描述操作对谜题的影响，以及过关的判断
    /// </summary>
    IResolver resolver { get; }
}
