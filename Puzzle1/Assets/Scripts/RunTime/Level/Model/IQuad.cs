using Core;

public interface IQuad : IEventDispatcher, IStream
{
    int row { get; }

    int column { get; }

    QuadValue value { get; set; }

    IQuad Clone();
}
