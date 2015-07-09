using Core;
using UnityEngine;

public class QuadView : VisisonView<IQuad>
{
    private MeshFilter _meshFilter;

    private Material[] _materials;

    private MeshRenderer _meshRenderer;

    private BoxCollider _boxCollider;

    private void Awake()
    {
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshFilter.mesh = Style.GetQuadMesh(Style.QuadSize);
        Shader shader = Style.GetQuadShader();
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

    protected override void Listen()
    {
        base.Listen();
        data.AddEventListener(QuadEvent.QuadValueChange, QuadValueChangeHandler);
    }

    protected override void Trigger()
    {
        base.Trigger();
        QuadValueChangeHandler(null);

    }

    protected override void AlphaChangeHandler(IEvent e)
    {
        for (int i = _materials.Length - 1; i >= 0; i--)
        {
            Color color = _materials[i].color;
            color.a = data.alpha;
            _materials[i].color = color;
        }
    }

    protected override void LocalScaleChangeHandler(IEvent e)
    {
        base.LocalScaleChangeHandler(e);
        _boxCollider.size = new Vector3(1 / data.localScale.x, 1 / data.localScale.y, _boxCollider.size.z);
    }

    private void QuadValueChangeHandler(IEvent e)
    {
        switch (data.value)
        {
            case QuadValue.Front:
                {
                    _materials[0].color = Style.GetColor(QuadValue.Front, data.alpha);
                    _materials[1].color = Style.GetColor(QuadValue.Back, data.alpha);
                    _boxCollider.enabled = true;
                    data.localEulerAngles = Vector3.zero;
                    break;
                }
            case QuadValue.Back:
                {
                    _materials[0].color = Style.GetColor(QuadValue.Back, data.alpha);
                    _materials[1].color = Style.GetColor(QuadValue.Front, data.alpha);
                    _boxCollider.enabled = true;
                    data.localEulerAngles = Vector3.zero;
                    break;
                }
            case QuadValue.Block:
                {
                    _materials[0].color = Style.GetColor(QuadValue.Block, data.alpha);
                    _materials[1].color = Style.GetColor(QuadValue.Block, data.alpha);
                    _boxCollider.enabled = false;
                    data.localEulerAngles = Vector3.zero;
                    data.localAlpha = 0;
                    break;
                }
        }
    }
}
