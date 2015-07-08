public class Resolver : IResolver
{
    public void Apply(IPuzzle puzzle, IOperation op)
    {
        IQuad[] rowQuads = puzzle.GetRowQuads(op.row);
        foreach (IQuad quad in rowQuads)
        {
            if (quad.column != op.column)
            {
                quad.value = 1 - quad.value;
            }
        }
        IQuad[] columnQuads = puzzle.GetColumnQuads(op.column);
        foreach (IQuad quad in columnQuads)
        {
            if (quad.row != op.row)
            {
                quad.value = 1 - quad.value;
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
        for (int i = 0; i < puzzle.rows; i++)
        {
            for (int j = 1; j < puzzle.columns; j++)
            {
                if (puzzle[i, j].value != puzzle[0, 0].value)
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
