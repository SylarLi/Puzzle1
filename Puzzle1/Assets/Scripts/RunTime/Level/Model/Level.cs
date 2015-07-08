using Core;

public class Level : EventDispatcher, ILevel
{
    private IPuzzle _puzzle;

    private IRecord _record;

    private IResolver _resolver;

    public Level(int rows, int columns)
    {
        _puzzle = new Puzzle(rows, columns);
        _record = new Record();
        _resolver = new Resolver();
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
