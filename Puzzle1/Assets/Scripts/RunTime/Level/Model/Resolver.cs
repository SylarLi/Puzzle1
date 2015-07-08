public class Resolver : IResolver
{
    public void Apply(IPuzzle puzzle, IOperation op)
    {
        IQuad[] rowQuads = puzzle.GetRowQuads(op.row);
        for (int i = op.column - 1; i >= 0; i--)
        {
            if (rowQuads[i].value == QuadValue.Block)
            {
                break;
            }
            else
            {
                rowQuads[i].value = (QuadValue)(QuadValue.Back - rowQuads[i].value);
            }
        }
        for (int i = op.column + 1, len = rowQuads.Length; i < len; i++)
        {
            if (rowQuads[i].value == QuadValue.Block)
            {
                break;
            }
            else
            {
                rowQuads[i].value = (QuadValue)(QuadValue.Back - rowQuads[i].value);
            }
        }
        IQuad[] columnQuads = puzzle.GetColumnQuads(op.column);
        for (int i = op.row - 1; i >= 0; i--)
        {
            if (columnQuads[i].value == QuadValue.Block)
            {
                break;
            }
            else
            {
                columnQuads[i].value = (QuadValue)(QuadValue.Back - columnQuads[i].value);
            }
        }
        for (int i = op.row + 1, len = columnQuads.Length; i < len; i++)
        {
            if (columnQuads[i].value == QuadValue.Block)
            {
                break;
            }
            else
            {
                columnQuads[i].value = (QuadValue)(QuadValue.Back - columnQuads[i].value);
            }
        }
    }

    public void Reverse(IPuzzle puzzle, IOperation op)
    {
        Apply(puzzle, op);
    }

    public bool IsWin(IPuzzle puzzle)
    {
        bool pass = true;
        QuadValue value = QuadValue.Block;
        for (int i = 0; i < puzzle.rows; i++)
        {
            for (int j = 0; j < puzzle.columns; j++)
            {
                if (value == QuadValue.Block && 
                    (puzzle[i, j].value == QuadValue.Front || puzzle[i, j].value == QuadValue.Back))
                {
                    value = puzzle[i, j].value;
                }
                if ((value == QuadValue.Front || value == QuadValue.Back) &&
                    (puzzle[i, j].value == QuadValue.Front || puzzle[i, j].value == QuadValue.Back) &&
                    value != puzzle[i, j].value)
                {
                    pass = false;
                    break;
                }
            }
        }
        return pass;
    }

    public bool IsLose(IPuzzle puzzle)
    {
        return false;
    }
}
