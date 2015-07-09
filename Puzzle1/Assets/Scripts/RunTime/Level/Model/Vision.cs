using Core;
using UnityEngine;

public abstract class Vision : EventDispatcher, IVision
{
    private float _parentAlpha;

    private float _localAlpha;

    private Vector3 _localPosition;

    private Vector3 _localEulerAngles;

    private Vector3 _localScale;

    private bool _touchEnable;

    public Vision()
    {
        _parentAlpha = 1;
        _localAlpha = 1;
        _localPosition = Vector3.zero;
        _localEulerAngles = Vector3.zero;
        _localScale = Vector3.one;
    }

    public float alpha
    {
        get
        {
            return parentAlpha * localAlpha;
        }
    }

    public float parentAlpha
    {
        get
        {
            return _parentAlpha;
        }
        set
        {
            if (_parentAlpha != value)
            {
                _parentAlpha = value;
                UpdateChildrenAlpha();
                DispatchEvent(new VisionEvent(VisionEvent.AlphaChange));
            }
        }
    }

    public float localAlpha
    {
        get
        {
            return _localAlpha;
        }
        set
        {
            if (_localAlpha != value)
            {
                _localAlpha = value;
                UpdateChildrenAlpha();
                DispatchEvent(new VisionEvent(VisionEvent.AlphaChange));
            }
        }
    }

    protected virtual void UpdateChildrenAlpha()
    {

    }

    public Vector3 localPosition
    {
        get
        {
            return _localPosition;
        }
        set
        {
            if (_localPosition != value)
            {
                _localPosition = value;
                DispatchEvent(new VisionEvent(VisionEvent.LocalPositionChange));
            }
        }
    }

    public Vector3 localEulerAngles
    {
        get
        {
            return _localEulerAngles;
        }
        set
        {
            if (_localEulerAngles != value)
            {
                _localEulerAngles = value;
                DispatchEvent(new VisionEvent(VisionEvent.LocalEulerAnglesChange));
            }
        }
    }

    public Vector3 localScale
    {
        get
        {
            return _localScale;
        }
        set
        {
            if (_localScale != value)
            {
                _localScale = value;
                DispatchEvent(new VisionEvent(VisionEvent.LocalScaleChange));
            }
        }
    }

    public bool touchEnable
    {
        get
        {
            return _touchEnable;
        }
        set
        {
            if (_touchEnable != value)
            {
                _touchEnable = value;
                DispatchEvent(new VisionEvent(VisionEvent.TouchEnableChange));
            }
        }
    }
}
