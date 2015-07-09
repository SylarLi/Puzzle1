using Core;

public class VisionEvent : Event
{
    public const string AlphaChange = "AlphaChange";
    public const string LocalPositionChange = "LocalPositionChange";
    public const string LocalEulerAnglesChange = "LocalEulerAnglesChange";
    public const string LocalScaleChange = "LocalScaleChange";
    public const string TouchEnableChange = "TouchEnableChange";

    public VisionEvent(string type, object data = null) : base(type, data = null)
    {

    }
}
