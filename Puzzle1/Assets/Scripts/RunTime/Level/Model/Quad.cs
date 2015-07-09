using Core;
using System.IO;

public class Quad : Vision, IQuad
{
    private int _row;

    private int _column;

    private QuadValue _value;

    public Quad() : base()
    {

    }

    public Quad(int row, int column, QuadValue value = QuadValue.Block)
    {
        _row = row;
        _column = column;
        _value = value;
    }

    public void WriteIn(BinaryWriter writer)
    {
        writer.Write(_row);
        writer.Write(_column);
        writer.Write((int)_value);
    }

    public void ReadOut(BinaryReader reader)
    {
        _row = reader.ReadInt32();
        _column = reader.ReadInt32();
        _value = (QuadValue)reader.ReadInt32();
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

    public QuadValue value
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
}
