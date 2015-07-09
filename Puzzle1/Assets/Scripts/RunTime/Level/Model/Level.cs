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
        do
        {
            _record.Clear();
            _puzzle = new Puzzle(pp.rows, pp.columns);
            for (int i = 0; i < puzzle.rows; i++)
            {
                for (int j = 0; j < puzzle.columns; j++)
                {
                    _puzzle[i, j] = new Quad(i, j, QuadValue.Front);
                }
            }
            System.Random random = new System.Random();
            for (int i = 0; i < pp.block; i++)
            {
                _puzzle[random.Next(_puzzle.rows), random.Next(_puzzle.columns)].value = QuadValue.Block;
            }
            for (int i = 0; i < 10; i++)
            {
                IOperation op = new Operation(OpType.TouchClick, random.Next(_puzzle.rows), random.Next(_puzzle.columns));
                _record.Push(op);
                _resolver.ResolveData(_puzzle, op);
            }
        }
        while (_resolver.IsSolved(_puzzle));
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
