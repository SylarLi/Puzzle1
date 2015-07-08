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
            UpdateQuadView();
        }
    }

    private void UpdateQuadView()
    {
        switch (_quad.value)
        {
            case QuadValue.Front:
                {
                    _materials[0].color = QuadViewUtil.GetColor(QuadValue.Front);
                    _materials[1].color = QuadViewUtil.GetColor(QuadValue.Back);
                    _boxCollider.enabled = true;
                    break;
                }
            case QuadValue.Back:
                {
                    _materials[0].color = QuadViewUtil.GetColor(QuadValue.Back);
                    _materials[1].color = QuadViewUtil.GetColor(QuadValue.Front);
                    _boxCollider.enabled = true;
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

    public Vector3 localEulerAngles
    {
        get
        {
            return _localEulerAngles;
        }
        set
        {
            _localEulerAngles = value;
            transform.localEulerAngles = _localEulerAngles;
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
