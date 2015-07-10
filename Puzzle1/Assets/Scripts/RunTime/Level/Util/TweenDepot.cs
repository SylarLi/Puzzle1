using DG.Tweening;
using System.Collections.Generic;

public class TweenDepot : List<TweenInfo>
{
    public Sequence ToSequence()
    {
        Sequence sequence = DOTween.Sequence();
        ForEach((TweenInfo each) => sequence.Insert(each.atPosition, each.tween));
        return sequence;
    }

    public List<TweenInfo> Get(string id)
    {
        return FindAll((TweenInfo each) => each.id != null && each.id.Equals(id));
    }

    public TweenInfo GetFirst(string id)
    {
        List<TweenInfo> infos = Get(id);
        return infos.Count > 0 ? infos[0] : null;
    }

    public void Add(float atPosition, Tween tween)
    {
        Add(new TweenInfo(tween.id as string, atPosition, tween));
    }

    public void Kill(string id, bool complete = false)
    {
        Get(id).ForEach((TweenInfo each) => 
        {
            each.Kill(complete);
            Remove(each);
        });
    }
}

public class TweenInfo
{
    private string _id;

    private float _atPosition;

    private Tween _tween;

    public TweenInfo(string id, float atPosition, Tween tween)
    {
        _id = id;
        _atPosition = atPosition;
        _tween = tween;
    }

    public string id
    {
        get
        {
            return _id;
        }
    }

    public float atPosition
    {
        get
        {
            return _atPosition;
        }
    }

    public Tween tween
    {
        get
        {
            return _tween;
        }
    }

    public void Kill(bool complete = false)
    {
        _tween.Kill(complete);
    }
}
