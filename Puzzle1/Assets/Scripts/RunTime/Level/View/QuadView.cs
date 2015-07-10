﻿using Core;
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
        _materials = new Material[2]
        {
            Style.GetQuadMaterial(),
            Style.GetQuadMaterial(),
        };
        _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        _meshRenderer.sharedMaterials = _materials;
        _boxCollider = gameObject.AddComponent<BoxCollider>();
        _boxCollider.center = Vector3.zero;
        _boxCollider.size = new Vector3(1, 1, 0.001f);
    }

    protected override void AlphaChangeHandler(IEvent e)
    {
        base.AlphaChangeHandler(e);
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

    protected override void LocalScaleChangeHandler(IEvent e)
    {
        base.LocalScaleChangeHandler(e);
        _boxCollider.size = new Vector3(1 / data.localScale.x, 1 / data.localScale.y, _boxCollider.size.z);
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
            IQuad quad = data.Clone();
            quad.localAlpha = data.localAlpha;
            quad.parentAlpha = data.parentAlpha;
            quad.localPosition = data.localPosition + new Vector3(0, 0, -0.001f);
            quad.localEulerAngles = data.localEulerAngles;
            quad.localScale = data.localScale;
            quad.touchEnable = false;
            quadView.data = quad;
            Tween t1 = DOTween.To(() => quad.localScale, x => quad.localScale = x, quad.localScale * Style.QuadSprinkleScale, Style.QuadSprinkleDuration).SetEase(Ease.OutCubic);
            Tween t2 = DOTween.To(() => quad.localAlpha, x => quad.localAlpha = x, 0f, Style.QuadSprinkleDuration).SetEase(Ease.OutCubic);
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(spark.delay)
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
        data.localEulerAngles = Style.GetAngles(data.value);
        Color color1 = Color.white;
        Color color2 = Color.white;
        switch (data.value)
        {
            case QuadValue.Front:
                {
                    _meshType = SimplePoolItemType.QuadMesh;
                    color1 = Style.GetColor(QuadValue.Front, data.alpha);
                    color2 = Style.GetColor(QuadValue.Back, data.alpha);
                    break;
                }
            case QuadValue.Back:
                {
                    _meshType = SimplePoolItemType.QuadMesh;
                    color1 = Style.GetColor(QuadValue.Back, data.alpha);
                    color2 = Style.GetColor(QuadValue.Front, data.alpha);
                    break;
                }
            case QuadValue.Block:
                {
                    data.localAlpha = 0;
                    _meshType = SimplePoolItemType.QuadMesh;
                    color1 = color2 = Style.GetColor(QuadValue.Block, data.alpha);
                    break;
                }
            default:
                {
                    if ((data.value & (QuadValue.Left | QuadValue.Right | QuadValue.Up | QuadValue.Down)) > 0)
                    {
                        _meshType = SimplePoolItemType.ArrowMesh;
                        color1 = color2 = Style.GetColor(data.value, data.alpha);
                    }
                    break;
                }
        }
        Mesh mesh = SimplePool.inst.Get<Mesh>(_meshType);
        Color32[] colors = new Color32[mesh.colors32.Length];
        for (int i = colors.Length - 1; i >= 0; i--)
        {
            colors[i] = i < 4 ? color1 : color2;
        }
        mesh.colors32 = colors;
        _meshFilter.mesh = mesh;
    }
}
