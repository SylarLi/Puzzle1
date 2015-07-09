using Core;
using UnityEngine;
using DG.Tweening;

public class QuadView : MonoBehaviour
{
    public const float QuadSize = 1;
    public const float RollAngle = 180f;
    public const float RollDuration = 0.5f;
    public const float TouchScale = 0.7f;
    public const float TouchScaleDuration = 0.2f;
    public const float ShakeAngle = 45f;
    public const float ShakeDuration = 0.25f;

    private MeshFilter _meshFilter;

    private Material[] _materials;

    private MeshRenderer _meshRenderer;

    private BoxCollider _boxCollider;

    private IQuad _quad;

    private Vector3 _localEulerAngles;

    private Tweener _rollTween;

    private Tweener _scaleTween;

    private Sequence _shakeSeq;

    private void Awake()
    {
        _localEulerAngles = transform.localEulerAngles;
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshFilter.mesh = QuadViewUtil.GetQuadMesh(QuadSize);
        Shader shader = QuadViewUtil.GetQuadShader();
        _materials = new Material[2]
        {
            new Material(shader) { color = Color.white },
            new Material(shader) { color = Color.white },
        };
        _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        _meshRenderer.materials = _materials;
        _boxCollider = gameObject.AddComponent<BoxCollider>();
        _boxCollider.center = Vector3.zero;
        _boxCollider.size = new Vector3(1, 1, 0.001f);
    }

    public IQuad quad
    {
        get
        {
            return _quad;
        }
        set
        {
            _quad = value;
            Trigger();
            Listen();
        }
    }

    private void Listen()
    {
        _quad.AddEventListener(VisionEvent.AlphaChange, AlphaChangeHandler);
        _quad.AddEventListener(VisionEvent.LocalPositionChange, LocalPositionChangeHandler);
        _quad.AddEventListener(VisionEvent.LocalEulerAnglesChange, LocalEulerAnglesChangeHandler);
        _quad.AddEventListener(VisionEvent.LocalScaleChange, LocalScaleChangeHandler);
        _quad.AddEventListener(QuadEvent.QuadValueChange, QuadValueChangeHandler);
    }

    private void Trigger()
    {
        AlphaChangeHandler(null);
        LocalPositionChangeHandler(null);
        LocalEulerAnglesChangeHandler(null);
        LocalScaleChangeHandler(null);
        QuadValueChangeHandler(null);

    }

    private void AlphaChangeHandler(IEvent e)
    {
        for (int i = _materials.Length - 1; i >= 0; i--)
        {
            Color color = _materials[i].color;
            color.a = _quad.alpha;
            _materials[i].color = color;
        }
    }

    private void LocalPositionChangeHandler(IEvent e)
    {
        transform.localPosition = _quad.localPosition;
    }

    private void LocalEulerAnglesChangeHandler(IEvent e)
    {
        transform.localEulerAngles = _quad.localEulerAngles;
    }

    private void LocalScaleChangeHandler(IEvent e)
    {
        transform.localScale = _quad.localScale;
    }

    private void QuadValueChangeHandler(IEvent e)
    {
        switch (_quad.value)
        {
            case QuadValue.Front:
                {
                    _materials[0].color = QuadViewUtil.GetColor(QuadValue.Front);
                    _materials[1].color = QuadViewUtil.GetColor(QuadValue.Back);
                    _boxCollider.enabled = true;
                    _quad.localEulerAngles = Vector3.zero;
                    break;
                }
            case QuadValue.Back:
                {
                    _materials[0].color = QuadViewUtil.GetColor(QuadValue.Front);
                    _materials[1].color = QuadViewUtil.GetColor(QuadValue.Back);
                    _boxCollider.enabled = true;
                    _quad.localEulerAngles = new Vector3(180, 0, 0);
                    break;
                }
            case QuadValue.Block:
                {
                    _materials[0].color = QuadViewUtil.GetColor(QuadValue.Block);
                    _materials[1].color = QuadViewUtil.GetColor(QuadValue.Block);
                    _boxCollider.enabled = false;
                    break;
                }
        }
    }

    public void QuadRoll(Vector3 deltaEulerAngles, float delay)
    {
        KillQuadRoll();
        _rollTween = DOTween.To(() => localEulerAngles, x => localEulerAngles = x, localEulerAngles + deltaEulerAngles, RollDuration)
            .SetDelay(delay)
            .SetEase(Ease.OutBack);
    }

    private void KillQuadRoll()
    {
        if (_rollTween != null)
        {
            _rollTween.Kill();
            _rollTween = null;
        }
    }

    public void TouchStart()
    {
        KillTouchScale();
        _scaleTween = DOTween.To(() => transform.localScale, x => 
            {
                transform.localScale = x;
                _boxCollider.size = new Vector3(1 / x.x, 1 / x.y, _boxCollider.size.z);
            },
            new Vector3(TouchScale, TouchScale, 1), 
            TouchScaleDuration
        );
    }

    public void TouchEnd()
    {
        KillTouchScale();
        _scaleTween = DOTween.To(() => transform.localScale, x =>
            {
                transform.localScale = x;
                _boxCollider.size = new Vector3(1 / x.x, 1 / x.y, _boxCollider.size.z);
            },
            Vector3.one,
            TouchScaleDuration
        );
    }

    public void TouchClick()
    {
        KillTouchScale();
        _scaleTween = DOTween.To(() => transform.localScale, x =>
            {
                transform.localScale = x;
                _boxCollider.size = new Vector3(1 / x.x, 1 / x.y, _boxCollider.size.z);
            },
            Vector3.one,
            TouchScaleDuration * 2
        ).SetEase(Ease.OutBack);
    }

    private void KillTouchScale()
    {
        if (_scaleTween != null)
        {
            _scaleTween.Kill();
            _scaleTween = null;
        }
    }

    public void BlockShake(Vector3 deltaEulerAngles, float delay)
    {
        KillBlockShake();
        _shakeSeq = DOTween.Sequence();
        _shakeSeq.AppendInterval(delay)
            .Append(DOTween.To(() => localEulerAngles, x => localEulerAngles = x, localEulerAngles + deltaEulerAngles, ShakeDuration * 0.25f))
            .Append(DOTween.To(() => localEulerAngles, x => localEulerAngles = x, localEulerAngles - deltaEulerAngles, ShakeDuration * 0.25f))
            .Append(DOTween.To(() => localEulerAngles, x => localEulerAngles = x, localEulerAngles, ShakeDuration * 0.5f).SetEase(Ease.OutBack));
    }

    private void KillBlockShake()
    {
        if (_shakeSeq != null)
        {
            _shakeSeq.Kill();
            _shakeSeq = null;
        }
    }
}
