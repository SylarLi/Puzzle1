using Core;

public interface IQuad : IVision, IStream
{
    int row { get; }

    int column { get; }

    QuadValue value { get; set; }

    IQuad Clone();
}
