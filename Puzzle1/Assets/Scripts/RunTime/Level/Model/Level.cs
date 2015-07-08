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
        do
        {
            for (int i = 0; i < 10; i++)
            {
                IOperation operation = new Operation(random.Next(_puzzle.rows), random.Next(_puzzle.columns));
                _resolver.Apply(_puzzle, operation);
            }
        }
        while (_resolver.IsWin(_puzzle));
        _record.Clear();
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
