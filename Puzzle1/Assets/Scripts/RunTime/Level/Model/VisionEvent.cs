using Core;

public class VisionEvent : Event
{
    public const string AlphaChange = "AlphaChange";
    public const string LocalPositionChange = "LocalPositionChange";
    public const string LocalEulerAnglesChange = "LocalEulerAnglesChange";
    public const string LocalScaleChange = "LocalScaleChange";

    public VisionEvent(string type, object data = null) : base(type, data = null)
    {

    }
}
