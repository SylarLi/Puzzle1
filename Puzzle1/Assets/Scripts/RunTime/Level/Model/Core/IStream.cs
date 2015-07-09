using System.IO;

public interface IStream
{
    void WriteIn(BinaryWriter writer);

    void ReadOut(BinaryReader reader);
}
