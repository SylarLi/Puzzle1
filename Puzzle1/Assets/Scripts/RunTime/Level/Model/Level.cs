using Core;

public class Level : EventDispatcher, ILevel
{
    private IPuzzle _puzzle;

    private IRecord _record;

    private IResolver _resolver;

    public Level()
    {
        _record = new Record();
        _resolver = new Resolver();
    }

    public void MakePuzzle(PuzzleParams pp)
    {
        _record.Clear();
        _puzzle = new Puzzle(pp.rows, pp.columns);
        int i = 0;
        for (i = 0; i < puzzle.rows; i++)
        {
            for (int j = 0; j < puzzle.columns; j++)
            {
                _puzzle[i, j] = new Quad(i, j, QuadValue.Front);
            }
        }
        System.Random random = new System.Random();
        i = pp.block;
        while (i > 0)
        {
            int row = random.Next(_puzzle.rows);
            int column = random.Next(_puzzle.columns);
            _puzzle[row, column].value = QuadValue.Block;
            i--;
        }
        i = pp.arrow;
        while (i > 0)
        {
            int row = random.Next(_puzzle.rows);
            int column = random.Next(_puzzle.columns);
            if (_puzzle[row, column].value != QuadValue.Block)
            {
                _puzzle[row, column].value = QuadValue.Left;
                i--;
            }
        }
        i = 10;
        while (i > 0 || _resolver.IsSolved(_puzzle))
        {
            IOperation op = new Operation(OpType.TouchClick, random.Next(_puzzle.rows), random.Next(_puzzle.columns));
            _record.Push(op);
            _resolver.ResolveTouchData(_puzzle, op);
            i--;
        }
    }

    public IPuzzle puzzle
    {
        get
        {
            return _puzzle;
        }
    }

    public IRecord record
    {
        get
        {
            return _record;
        }
    }

    public IResolver resolver
    {
        get
        {
            return _resolver;
        }
    }
}
