using System.Collections.Generic;

public interface IRecord : IList<IOperation>, IEnumerable<IOperation>, IStream
{
    bool opValid { get; set; }

    void Push(params IOperation[] ops);

    IOperation Pop();

    void Reverse();
}

