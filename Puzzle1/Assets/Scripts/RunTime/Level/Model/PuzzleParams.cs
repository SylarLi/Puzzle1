public class PuzzleParams
{
    /// <summary>
    /// 行数
    /// </summary>
    public int rows = 3;

    /// <summary>
    /// 列数
    /// </summary>
    public int columns = 3;

    /// <summary>
    /// 障碍物数量
    /// </summary>
    public int block = 0;

    /// <summary>
    /// 根据难度生成谜题参数
    /// </summary>
    /// <param name="rank"></param>
    public static PuzzleParams GetPuzzleParamsByRank(int rank)
    {
        PuzzleParams pp = new PuzzleParams();
        if (rank <= 5)
        {
            pp.rows = pp.columns = rank + 2;
            pp.block = 0;
        }
        else
        {
            pp.rows = pp.columns = 7;
            pp.block = System.Math.Min(rank - 5, 4);
        }
        return pp;
    }
}
