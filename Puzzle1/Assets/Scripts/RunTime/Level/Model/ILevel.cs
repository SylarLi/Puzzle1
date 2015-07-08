using Core;

/// <summary>
/// 关卡
/// </summary>
public interface ILevel : IEventDispatcher
{
    /// <summary>
    /// 谜题
    /// </summary>
    IPuzzle puzzle { get; }

    /// <summary>
    /// 操作记录和控制
    /// </summary>
    IRecord record { get; }

    /// <summary>
    /// 解析器：描述操作对谜题的影响，以及过关的判断
    /// </summary>
    IResolver resolver { get; }
}
