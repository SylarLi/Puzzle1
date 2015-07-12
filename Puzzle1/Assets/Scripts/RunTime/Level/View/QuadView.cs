using Core;
using DG.Tweening;
using UnityEngine;

public class QuadView : VisisonView<IQuad>
{
    private SimplePoolItemType _meshType;

    private MeshFilter _meshFilter;

    private Material[] _materials;

    private MeshRenderer _meshRenderer;

    private BoxCollider _boxCollider;

    protected override void Listen()
    {
        base.Listen();
        data.AddEventListener(QuadEvent.QuadValueChange, QuadValueChangeHandler);
    }

    protected override void Trigger()
    {
        InitQuadView();
        base.Trigger();
        QuadValueChangeHandler(null);
    }

    private void InitQuadView()
    {
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        _boxCollider = gameObject.AddComponent<BoxCollider>();
        _boxCollider.center = Vector3.zero;
        _boxCollider.size = new Vector3(1, 1, 0.001f);
    }

    protected override void AlphaChangeHandler(IEvent e)
    {
        base.AlphaChangeHandler(e);
        if (_meshType != SimplePoolItemType.None)
        {
            Color32[] source = _meshFilter.mesh.colors32;
            Color32[] colors = new Color32[source.Length];
            for (int i = colors.Length - 1; i >= 0; i--)
            {
                Color color = source[i];
                color.a = data.alpha;
                colors[i] = color;
            }
            _meshFilter.mesh.colors32 = colors;
        }
    }

    protected override void LocalScaleChangeHandler(IEvent e)
    {
        base.LocalScaleChangeHandler(e);
        _boxCollider.size = new Vector3(1 / data.localScale.x, 1 / data.localScale.y, _boxCollider.size.z);
    }

    protected override void MaterialChangeHandler(IEvent e)
    {
        base.MaterialChangeHandler(e);
        Material material = Style.GetQuadMaterial(data.material);
        _meshRenderer.sharedMaterials = new Material[2] { material, material };
    }

    protected override void UVOffsetsChangeHandler(IEvent e)
    {
        base.UVOffsetsChangeHandler(e);
        if (_meshType != SimplePoolItemType.None)
        {
            Vector2[] uvs = new Vector2[Style.QuadUV.Length];
            for (int i = uvs.Length - 1; i >= 0; i--)
            {
                Vector2 uv = Style.QuadUV[i];
                int index = i / 4;
                uv.x = data.uvOffsets[index].x + uv.x * Style.QuadUVTilling.x;
                uv.y = data.uvOffsets[index].y + uv.y * Style.QuadUVTilling.y;
                uvs[i] = uv;
            }
            _meshFilter.mesh.uv = uvs;
        }
    }

    protected override void TouchEnableChangeHandler(IEvent e)
    {
        base.TouchEnableChangeHandler(e);
        _boxCollider.enabled = data.touchEnable;
    }

    protected override void DoSparkHandler(IEvent e)
    {
        base.DoSparkHandler(e);
        VisionSpark spark = e.data as VisionSpark;
        if (spark.type == VisionSparkType.Sprinkle)
        {
            GameObject quadgo = new GameObject(string.Format("quad_{0}_{1}_sprinkle", data.row, data.column));
            quadgo.transform.parent = transform.parent;
            quadgo.layer = Layer.Effect;
            QuadView quadView = quadgo.AddComponent<QuadView>();
            quadView.data = data.Clone();
            quadView.data.localAlpha = data.localAlpha;
            quadView.data.parentAlpha = data.parentAlpha;
            quadView.data.localPosition = data.localPosition + new Vector3(0, 0, -0.001f);
            quadView.data.localEulerAngles = data.localEulerAngles;
            quadView.data.localScale = data.localScale;
            quadView.data.touchEnable = false;
            quadView.data.material = VisionMaterial.Unlit;
            quadView.data.uvOffsets = new Vector2[] { new Vector2(0, 0.25f), new Vector2(0, 0.25f) };
            quadgo.SetActive(false);
            Tween t1 = DOTween.To(() => quadView.data.localScale, x => quadView.data.localScale = x, quadView.data.localScale * Style.QuadSprinkleScale, Style.QuadSprinkleDuration).SetEase(Ease.OutCubic);
            Tween t2 = DOTween.To(() => quadView.data.localAlpha, x => quadView.data.localAlpha = x, 0f, Style.QuadSprinkleDuration).SetEase(Ease.OutCubic);
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(spark.delay)
                    .AppendCallback(() => quadgo.SetActive(true))
                    .Append(t1)
                    .Join(t2)
                    .OnComplete(() => GameObject.Destroy(quadView.gameObject));
        }
    }

    private void QuadValueChangeHandler(IEvent e)
    {
        if (_meshType != SimplePoolItemType.None)
        {
            SimplePool.inst.Return(_meshType, _meshFilter.mesh);
        }
        _meshType = SimplePoolItemType.QuadMesh;
        _meshFilter.mesh = SimplePool.inst.Get<Mesh>(_meshType);
        data.localEulerAngles = Style.GetAngles(data.value);
        data.uvOffsets = Style.GetQuadUVOffsets(data.value);
    }
}
