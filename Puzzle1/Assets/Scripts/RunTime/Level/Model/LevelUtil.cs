using System;
using System.Collections.Generic;
using System.IO;

public static class LevelUtil
{
    public static string Serialize<T>(T info) where T : IStream
    {
        string serial = "";
        try
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                info.WriteIn(writer);
                serial = Convert.ToBase64String(stream.ToArray());
            }
        }
        catch (Exception)
        {
            serial = "";
        }
        return serial;
    }

    public static T Deserialize<T>(string serial) where T : class, IStream
    {
        T info = default(T);
        try
        {
            byte[] bytes = Convert.FromBase64String(serial);
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;
                stream.Flush();
                BinaryReader reader = new BinaryReader(stream);
                info = TypeToObject(typeof(T)) as T;
                info.ReadOut(reader);
            }
        }
        catch (Exception)
        {
            info = null;
        }
        return info;
    }

    public static IStream TypeToObject(Type type)
    {
        IStream result = null;
        switch (type.Name)
        {
            case "IQuad":
                {
                    result = new Quad();
                    break;
                }
            case "IPuzzle":
                {
                    result = new Puzzle();
                    break;
                }
            case "IOperation":
                {
                    result = new Operation();
                    break;
                }
            case "IRecord":
                {
                    result = new Record();
                    break;
                }
            default:
                {
                    throw new NotSupportedException();
                }
        }
        return result;
    }
}
