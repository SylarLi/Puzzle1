using Core;

public class PuzzleEvent : Event
{
    public const string SolveChange = "SolveChange";

    public PuzzleEvent(string type, object data = null) : base(type, data)
    {

    }
}
