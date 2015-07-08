using System.IO;
using System.Collections.Generic;

public class Record : List<IOperation>, IRecord
{
    public Record() : base()
    {

    }

    public void Push(params IOperation[] ops)
    {
        AddRange(ops);
    }

    public IOperation Pop()
    {
        IOperation result = null;
        if (Count > 0)
        {
            result = this[Count - 1];
            RemoveAt(Count - 1);
        }
        return result;
    }

    public void WriteIn(BinaryWriter writer)
    {
        writer.Write(Count);
        for (int i = 0; i < Count; i++)
        {
            this[i].WriteIn(writer);
        }
    }

    public void ReadOut(BinaryReader reader)
    {
        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            Operation op = new Operation();
            op.ReadOut(reader);
            Push(op);
        }
    }
}
