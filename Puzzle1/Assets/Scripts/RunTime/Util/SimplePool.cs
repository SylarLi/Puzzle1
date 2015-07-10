using System;
using System.Collections;
using System.Collections.Generic;

public class SimplePool : Singleton<SimplePool>
{
    private Dictionary<SimplePoolItemType, Queue> _caches;

    private Dictionary<SimplePoolItemType, Func<object>> _factories;

    public SimplePool()
    {
        _caches = new Dictionary<SimplePoolItemType, Queue>();
        _factories = new Dictionary<SimplePoolItemType, Func<object>>();
    }

    public int Count
    {
        get
        {
            int sums = 0;
            foreach (Queue q in _caches.Values)
            {
                sums += q.Count;
            }
            return sums;
        }
    }

    public void Bind<T>(SimplePoolItemType key, Func<object> factory)
    {
        string key1 = factory.Method.ReturnType.Name;
        string key2 = typeof(T).Name;
        if (key1 == key2)
        {
            _factories[key] = factory;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public T Get<T>(SimplePoolItemType key)
    {
        T result = default(T);
        if (!_caches.ContainsKey(key))
        {
            _caches[key] = new Queue();
        }
        if (_caches[key].Count > 0)
        {
            result = (T)_caches[key].Dequeue();
        }
        else
        {
            result = (T)_factories[key]();
        }
        return result;
    }

    public void Return(SimplePoolItemType key, object obj)
    {
        if (!_caches.ContainsKey(key))
        {
            _caches[key] = new Queue();
        }
        _caches[key].Enqueue(obj);
    }
}
