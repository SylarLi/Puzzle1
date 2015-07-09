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
    /// 箭头数量
    /// </summary>
    public int arrow = 0;

    /// <summary>
    /// 根据难度随机生成谜题参数
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
        else if (rank <= 10)
        {
            pp.rows = pp.columns = 7;
            pp.block = rank - 5;
        }
        else if (rank <= 15)
        {
            pp.rows = pp.columns = 7;
            pp.block = 0;
            pp.arrow = rank - 10;
        }
        else
        {
            pp.rows = pp.columns = 7;
            pp.block = new System.Random().Next(10);
            pp.arrow = 10 - pp.block;
        }
        return pp;
    }
}
