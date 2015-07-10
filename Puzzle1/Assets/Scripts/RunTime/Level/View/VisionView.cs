using Core;
using UnityEngine;

public class VisisonView<T> : MonoBehaviour where T : IVision
{
    private T _data;

    public T data
    {
        get
        {
            return _data;
        }
        set
        {
            _data = value;
            Listen();
            Trigger();
        }
    }

    protected virtual void Listen()
    {
        _data.AddEventListener(VisionEvent.AlphaChange, AlphaChangeHandler);
        _data.AddEventListener(VisionEvent.LocalPositionChange, LocalPositionChangeHandler);
        _data.AddEventListener(VisionEvent.LocalEulerAnglesChange, LocalEulerAnglesChangeHandler);
        _data.AddEventListener(VisionEvent.LocalScaleChange, LocalScaleChangeHandler);
        _data.AddEventListener(VisionEvent.DoSpark, DoSparkHandler);
    }

    protected virtual void Trigger()
    {
        AlphaChangeHandler(null);
        LocalPositionChangeHandler(null);
        LocalEulerAnglesChangeHandler(null);
        LocalScaleChangeHandler(null);
    }

    protected virtual void AlphaChangeHandler(IEvent e)
    {
        
    }

    protected virtual void LocalPositionChangeHandler(IEvent e)
    {
        transform.localPosition = _data.localPosition;
    }

    protected virtual void LocalEulerAnglesChangeHandler(IEvent e)
    {
        transform.localEulerAngles = _data.localEulerAngles;
    }

    protected virtual void LocalScaleChangeHandler(IEvent e)
    {
        transform.localScale = _data.localScale;
    }

    protected virtual void DoSparkHandler(IEvent e)
    {

    }
}
