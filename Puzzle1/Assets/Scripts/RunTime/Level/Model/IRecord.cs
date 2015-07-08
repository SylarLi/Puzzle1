using System.Collections.Generic;

public interface IRecord : IList<IOperation>, IEnumerable<IOperation>, IStream
{
    void Push(params IOperation[] ops);

    IOperation Pop();

    void Reverse();
}

