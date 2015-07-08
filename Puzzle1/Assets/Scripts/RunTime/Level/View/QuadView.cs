using Core;
using UnityEngine;
using DG.Tweening;

public class QuadView : MonoBehaviour
{
    public const float QuadSize = 1;
    public const float TouchScale = 0.7f;
    public const float TouchScaleDuration = 0.2f;

    private MeshFilter _meshFilter;

    private Material[] _materials;

    private MeshRenderer _meshRenderer;

    private BoxCollider _boxCollider;

    private IQuad _quad;

    private Vector3 _localEulerAngles;

    private Tweener _scaleTween;

    private void Awake()
    {
        _localEulerAngles = transform.localEulerAngles;
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshFilter.mesh = LevelViewUtil.GetQuadMesh(QuadSize);
        Shader shader = LevelViewUtil.GetQuadShader();
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
        Color color1 = new Color(1, 0.5f, 0);
        Color color2 = new Color(0, 0.5f, 1);
        _materials[0].color = _quad.value == 1 ? color1 : color2;
        _materials[1].color = _quad.value == 1 ? color2 : color1;
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

    public void TouchStart()
    {
        KillScaleTween();
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
        KillScaleTween();
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
        KillScaleTween();
        _scaleTween = DOTween.To(() => transform.localScale, x =>
            {
                transform.localScale = x;
                _boxCollider.size = new Vector3(1 / x.x, 1 / x.y, _boxCollider.size.z);
            },
            Vector3.one,
            TouchScaleDuration * 2
        ).SetEase(Ease.OutBack);
    }

    private void KillScaleTween()
    {
        if (_scaleTween != null)
        {
            _scaleTween.Kill();
            _scaleTween = null;
        }
    }
}
