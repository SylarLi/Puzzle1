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
                    _meshFilter.mesh = Style.GetQuadMesh();
                    _materials[0].color = Style.GetColor(QuadValue.Front, data.alpha);
                    _materials[1].color = Style.GetColor(QuadValue.Back, data.alpha);
                    break;
                }
            case QuadValue.Back:
                {
                    _meshFilter.mesh = Style.GetQuadMesh();
                    _materials[0].color = Style.GetColor(QuadValue.Back, data.alpha);
                    _materials[1].color = Style.GetColor(QuadValue.Front, data.alpha);
                    break;
                }
            case QuadValue.Block:
                {
                    _meshFilter.mesh = Style.GetQuadMesh();
                    _materials[1].color = _materials[0].color = Style.GetColor(QuadValue.Block, data.alpha);
                    data.localAlpha = 0;
                    break;
                }
            default:
                {
                    if ((data.value & (QuadValue.Left | QuadValue.Right | QuadValue.Up | QuadValue.Down)) > 0)
                    {
                        _meshFilter.mesh = Style.GetArrowMesh();
                        _materials[1].color = _materials[0].color = Style.GetColor(data.value, data.alpha);
                    }
                    break;
                }
        }
        data.localEulerAngles = Style.GetAngles(data.value);
    }
}
