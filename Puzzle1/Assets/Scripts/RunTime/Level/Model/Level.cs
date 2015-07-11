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
        for (i = 0; i < _puzzle.rows; i++)
        {
            for (int j = 0; j < _puzzle.columns; j++)
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

        if (pp.arrow > 0)
        {
            // QuadValue.Left | QuadValue.Up, QuadValue.Right | QuadValue.Up, QuadValue.Left | QuadValue.Down, QuadValue.Right | QuadValue.Down
            QuadValue[] diretions = new QuadValue[] { QuadValue.Left, QuadValue.Right, QuadValue.Up, QuadValue.Down };
            do
            {
                for (i = 0; i < _puzzle.rows; i++)
                {
                    for (int j = 0; j < _puzzle.columns; j++)
                    {
                        if ((_puzzle[i, j].value & (QuadValue.Left | QuadValue.Right | QuadValue.Up | QuadValue.Down)) > 0)
                        {
                            _puzzle[i, j] = new Quad(i, j, QuadValue.Front);
                        }
                    }
                }
                i = pp.arrow;
                while (i > 0)
                {
                    int row = random.Next(_puzzle.rows);
                    int column = random.Next(_puzzle.columns);
                    if (_puzzle[row, column].value == QuadValue.Front || _puzzle[row, column].value == QuadValue.Back)
                    {
                        _puzzle[row, column].value = diretions[random.Next(diretions.Length)];
                        i--;
                    }
                }
            }
            while (_resolver.ResolveIsLoop(puzzle));
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

    public void MakePuzzle(QuadValue[,] values)
    {
        _record.Clear();
        _puzzle = new Puzzle(values.GetLength(0), values.GetLength(1));
        for (int i = 0; i < _puzzle.rows; i++)
        {
            for (int j = 0; j < _puzzle.columns; j++)
            {
                _puzzle[i, j] = new Quad(i, j, values[i, j]);
            }
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
