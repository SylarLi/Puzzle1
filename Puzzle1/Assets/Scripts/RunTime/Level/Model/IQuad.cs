using Core;

public interface IQuad : IEventDispatcher, IStream
{
    int row { get; }

    int column { get; }

    int value { get; set; }

    IQuad Clone();
}
