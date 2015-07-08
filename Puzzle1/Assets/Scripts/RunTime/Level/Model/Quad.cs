using Core;
using System.IO;

public class Quad : EventDispatcher, IQuad
{
    private int _row;

    private int _column;

    private int _value;

    public Quad() : base()
    {

    }

    public Quad(int row, int column, int value = 0)
    {
        _row = row;
        _column = column;
        _value = value;
    }

    public int row
    {
        get
        {
            return _row;
        }
    }

    public int column
    {
        get
        {
            return _column;
        }
    }

    public int value
    {
        get
        {
            return _value;
        }
        set
        {
            if (_value != value)
            {
                _value = value;
                DispatchEvent(new QuadEvent(QuadEvent.QuadValueChange));
            }
        }
    }

    public IQuad Clone()
    {
        return new Quad(row, column, value);
    }

    public void WriteIn(BinaryWriter writer)
    {
        writer.Write(_row);
        writer.Write(_column);
        writer.Write(_value);
    }

    public void ReadOut(BinaryReader reader)
    {
        _row = reader.ReadInt32();
        _column = reader.ReadInt32();
        _value = reader.ReadInt32();
    }
}
