using System.IO;

public class Operation : IOperation
{
    private int _row;

    private int _column;

    public Operation() : base()
    {

    }

    public Operation(int row, int column)
    {
        _row = row;
        _column = column;
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

    public void WriteIn(BinaryWriter writer)
    {
        writer.Write(_row);
        writer.Write(_column);
    }

    public void ReadOut(BinaryReader reader)
    {
        _row = reader.ReadInt32();
        _column = reader.ReadInt32();
    }
}
