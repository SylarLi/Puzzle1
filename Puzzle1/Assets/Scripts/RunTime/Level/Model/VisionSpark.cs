public enum VisionSparkType
{
    Sprinkle,
}

public class VisionSpark
{
    private VisionSparkType _type;

    private float _delay;

    public VisionSpark(VisionSparkType type)
    {
        _type = type;
    }

    public VisionSparkType type
    {
        get
        {
            return _type;
        }
    }

    public float delay
    { 
        get
        {
            return _delay;
        }
        set
        {
            _delay = value;
        }
    }
}
