using Core;

public class QuadEvent : Event
{
    public const string QuadValueChange = "QuadValueChange";

    public QuadEvent(string type, object data = null) : base(type, data)
    {

    }
}
