using System.IO;

public class Operation : IOperation
{
    private OpType _type;

    private int _row;

    private int _column;

    public Operation() : base()
    {

    }

    public Operation(OpType type, int row, int column)
    {
        _type = type;
        _row = row;
        _column = column;
    }

    public void WriteIn(BinaryWriter writer)
    {
        writer.Write((int)_type);
        writer.Write(_row);
        writer.Write(_column);
    }

    public void ReadOut(BinaryReader reader)
    {
        _type = (OpType)reader.ReadInt32();
        _row = reader.ReadInt32();
        _column = reader.ReadInt32();
    }

    public OpType type
    {
        get
        {
            return _type;
        }
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
}
