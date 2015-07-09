using Core;

public interface IPuzzle : IVision, IStream
{
    int rows { get; }

    int columns { get; }

    IQuad this[int row, int column] { get; set; }

    IQuad[] GetRowQuads(int row);

    IQuad[] GetColumnQuads(int column);

    IPuzzle Clone();

    bool solved { get; set; }
}
