using Core;

public class VisionEvent : Event
{
    public const string AlphaChange = "AlphaChange";
    public const string LocalPositionChange = "LocalPositionChange";
    public const string LocalEulerAnglesChange = "LocalEulerAnglesChange";
    public const string LocalScaleChange = "LocalScaleChange";
    public const string MaterialChange = "MaterialChange";
    public const string UVOffsetsChange = "UVOffsetsChange";
    public const string TouchEnableChange = "TouchEnableChange";

    public const string DoSpark = "DoSpark";

    public VisionEvent(string type, object data = null) : base(type, data)
    {

    }
}
