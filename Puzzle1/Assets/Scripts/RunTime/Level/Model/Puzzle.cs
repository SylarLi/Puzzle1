using Core;
using System.IO;
using System.Collections.Generic;

public class Puzzle : EventDispatcher, IPuzzle
{
    private int _rows;

    private int _columns;

    private IQuad[,] _values;

    public Puzzle() : base()
    {
        
    }

    public Puzzle(int rows, int columns)
    {
        _rows = rows;
        _columns = columns;
        _values = new IQuad[_rows, _columns];
    }

    public int rows
    {
        get
        {
            return _rows;
        }
    }

    public int columns
    {
        get
        {
            return _columns;
        }
    }

    public IQuad this[int row, int column]
    {
        get
        {
            return _values[row, column];
        }
        set
        {
            if (_values[row, column] == null)
            {
                _values[row, column] = value;
            }
            else
            {
                _values[row, column].value = value.value;
            }
        }
    }

    public IQuad[] GetRowQuads(int row)
    {
        IQuad[] quads = new IQuad[columns];
        for (int i = 0, len = quads.Length; i < len; i++)
        {
            quads[i] = _values[row, i];
        }
        return quads;
    }

    public IQuad[] GetColumnQuads(int column)
    {
        IQuad[] quads = new IQuad[rows];
        for (int i = 0, len = quads.Length; i < len; i++)
        {
            quads[i] = _values[i, column];
        }
        return quads;
    }

    public IPuzzle Clone()
    {
        IPuzzle puzzle = new Puzzle(rows, columns);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                puzzle[i, j] = this[i, j].Clone();
            }
        }
        return puzzle;
    }

    public void WriteIn(BinaryWriter writer)
    {
        writer.Write(_rows);
        writer.Write(_columns);
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                this[i, j].WriteIn(writer);
            }
        }
    }

    public void ReadOut(BinaryReader reader)
    {
        _rows = reader.ReadInt32();
        _columns = reader.ReadInt32();
        _values = new Quad[_rows, _columns];
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                this[i, j] = new Quad();
                this[i, j].ReadOut(reader);
            }
        }
    }
}
